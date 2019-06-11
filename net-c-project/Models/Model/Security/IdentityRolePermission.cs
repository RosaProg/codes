using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Security
{
    /// <summary>
    /// Defines a permission assigned to a role
    /// </summary>
    public class IdentityRolePermission
    {
        /// <summary>
        /// Gets or sets the Id of the role
        /// </summary>
        [Key, ForeignKey("Role"), Column(Order = 0)]
        public string RoleId { get; set; }

        /// <summary>
        /// Gets or sets the Role that has a permission
        /// </summary>
        public IdentityRole Role { get; set; }

        /// <summary>
        /// Gets or sets the Text representation of the Permission.
        /// This is the ToString() version of "Permission"
        /// </summary>
        public string PermissionString { get; set; }

        /// <summary>
        /// Gets or sets the actual permission
        /// </summary>
        [Key, Column(Order = 1)]
        public Permission Permission { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityRolePermission"/> class
        /// </summary>
        public IdentityRolePermission()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityRolePermission"/> class
        /// </summary>
        /// <param name="role">The role</param>
        /// <param name="permission">The permission</param>
        public IdentityRolePermission(IdentityRole role, Permission permission)
        {
            this.RoleId = role.Id;
            this.Role = role;
            this.PermissionString = permission.ToString();
            this.Permission = permission;
        }
    }
}
