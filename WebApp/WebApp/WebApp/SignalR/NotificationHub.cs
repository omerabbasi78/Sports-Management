using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using WebApp.HelperClass;

namespace WebApp.SignalR
{
    public class NotificationHub : Hub
    {
        static List<UserConnection> uList = new List<UserConnection>();
        public static void SendNotification(string text, string icon, string url)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            List<string> usersList = uList.Where(w => w.CompanyId == Common.CurrentUser.CompanyId).Select(s => s.ConnectionID).ToList();
            context.Clients.Clients(usersList).broadcastMessage(text, icon, url);
        }

        public override Task OnConnected()
        {
            //Get the UserId
            var us = new UserConnection();
            us.CompanyId = Common.CurrentUser.CompanyId;
            if (role.ToLower().Contains("owner"))
            {
                us.isAdmin = true;
            }
            us.ConnectionID = Context.ConnectionId;
            uList.Add(us);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            uList.RemoveAt(uList.FindIndex(u => u.ConnectionID == Context.ConnectionId));
            return base.OnDisconnected(stopCalled);
        }
    }

    class UserConnection
    {
        public long CompanyId { set; get; }
        public bool isAdmin { set; get; }
        public string ConnectionID { set; get; }
        public string role { set; get; }
        public long userId { set; get; }
        public long invoiceId { set; get; }
    }
}