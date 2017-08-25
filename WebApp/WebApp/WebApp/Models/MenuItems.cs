using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class MenuItems : Entity
    {
        public MenuItems()
        {
            MenuItemsList = new HashSet<MenuItems>();
            PagePermissions = new HashSet<PagePermissions>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; }
        public bool IsParent { get; set; }
        public int? GroupId { get; set; }
        public string IconClass { get; set; }
        public ICollection<MenuItems> MenuItemsList { get; set; }
        [ForeignKey("GroupId")]
        public virtual MenuItems MenuItem { get; set; }
        public virtual ICollection<PagePermissions> PagePermissions { get; set; }
        public int SortOrder { get; set; }
    }
}