using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class PagePermissions : Entity
    {
        public PagePermissions()
        {
            RolePermissions = new HashSet<RolePermissions>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PagePermissionId { get; set; }
        public int PermissionId { get; set; }
        public string PermissionText { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string PageUrl { get; set; }
        public int MenuItemId { get; set; }
        [ForeignKey("MenuItemId")]
        public MenuItems MenuItem { get; set; }
        public virtual ICollection<RolePermissions> RolePermissions { get; set; }
    }
}