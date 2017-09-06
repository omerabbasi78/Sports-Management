
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class UserController : BaseController
    {
        // GET: User
        public ActionResult EditProfile()
        {
            //get user id from cookie and show data in view using layout
            return View();
        }

        [HttpPost]
        public ActionResult EditProfile(WebApp.Models.RegisterViewModel registerModel)
        {
            return View();
        }
    }
}