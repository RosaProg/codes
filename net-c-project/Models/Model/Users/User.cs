using DSPrima.WcfUserSession.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using PCHI.Model.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Users
{
    /// <summary>
    /// Defines the User entity
    /// </summary>
    public class User : IdentityUser, DSPrima.WcfUserSession.Interfaces.IUser
    {
        /// <summary>
        /// Gets or sets the Title of the user
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the User's first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the User's last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the external Id of the User
        /// </summary>
        [Index, MaxLength(128)]
        public string ExternalId { get; set; }

        /// <summary>
        /// Gets or sets the provider to be used for TwoFactorAuthentication
        /// </summary>
        public string TwoFactorAuthenticationProvider { get; set; }

        /// <summary>
        /// Gets or sets the security question for the user
        /// </summary>
        [MaxLength(500)]
        public string SecurityQuestion { get; set; }

        /// <summary>
        /// Gets or sets the security answer for the user
        /// This should probably be encrypted
        /// </summary>
        [MaxLength(500)]
        public string SecurityAnswer { get; set; }

        /// <summary>
        /// Gets or sets the confirmation token for the user
        /// </summary>
        public string RegistrationConfirmationToken { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether lockout is enabled for this user
        /// </summary>
        public override bool LockoutEnabled
        {
            get
            {
                return base.LockoutEnabled;
            }

            set
            {
                base.LockoutEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user is from an external source such as Active Directory
        /// </summary>
        public bool IsExternalUser { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this user has enabled two (or more) factor authentication (True) or not (false)
        /// </summary>
        [NotMapped]
        public bool MultiStepVerificationEnabled
        {
            get
            {
                return this.TwoFactorEnabled;
            }

            set
            {
                this.TwoFactorEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets the Display Name of the user
        /// Note: Nothing is actually set. Just complying with the IUser Interface.
        /// </summary>
        [NotMapped]
        public string DisplayName
        {
            get { return this.Title + " " + this.FirstName + " " + this.LastName; }
            set { }
        }

        /// <summary>
        /// Gets or sets the names of the roles the user belongs to
        /// This will be used for available Functionality
        /// </summary>
        public IEnumerable<string> RoleNames
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Permissions the user has access to
        /// </summary>
        public IEnumerable<Permission> Permissions { get; set; }

        /// <summary>
        /// Gets or sets the UserEntities that belong to this User
        /// </summary>
        public virtual ICollection<ProxyUserPatientMap> ProxyUserPatientMap { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class
        /// </summary>
        public User()
        {
            this.ProxyUserPatientMap = new List<ProxyUserPatientMap>();
            this.RoleNames = new List<string>();
        }        
    }
}
