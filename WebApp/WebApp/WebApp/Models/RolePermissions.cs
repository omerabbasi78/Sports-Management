using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class RolePermissions : Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RolePermissionId { get; set; }
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Roles Roles { get; set; }
        public int PagePermissionId { get; set; }
        [ForeignKey("PagePermissionId")]
        public PagePermissions PagePermission { get; set; }
    }
}