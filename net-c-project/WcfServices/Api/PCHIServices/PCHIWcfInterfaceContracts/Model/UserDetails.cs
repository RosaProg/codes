using PCHI.Model.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model
{
    /// <summary>
    /// Defines the user details
    /// </summary>
    public class UserDetails
    {
        /// <summary>
        /// Gets or sets the user Id
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the UserName
        /// </summary>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the Title of the user
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the User's first name
        /// </summary>
        [DataMember]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the User's last name
        /// </summary>
        [DataMember]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email
        /// </summary>
        [DataMember]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the provider used for two factor authentication
        /// </summary>
        [DataMember]
        public string TwoFactorProvider { get; set; }

        /// <summary>
        /// Gets or sets the user phonenumber
        /// </summary>
        [DataMember]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the external Id of hte user
        /// </summary>
        [DataMember]
        public string ExternalId { get; set; }

        /// <summary>
        /// Gets or sets the security question
        /// </summary>
        [DataMember]
        public string SecurityQuestion { get; set; }

        /// <summary>
        /// Gets or sets the security Answer
        /// </summary>
        [DataMember]
        public string SecurityAnswer { get; set; }

        /// <summary>
        /// Holds the private list of Roles
        /// </summary>
        [DataMember]
        private List<string> roles = new List<string>();

        /// <summary>
        /// Gets or sets a value indicating whether the user is an external user and has to be logged in via external means such as Active Directory
        /// </summary>
        [DataMember]
        public bool IsExternalUser { get; set; }

        /// <summary>
        /// Gets the list of roles that are assigned to the User.
        /// This list is for reading only and cannot be used for updating the user roles
        /// </summary>
        public List<string> Roles { get { return this.roles == null ? new List<string>() : this.roles; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDetails"/> class
        /// </summary>
        public UserDetails()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDetails"/> class
        /// </summary>
        /// <param name="user">The user to get the data from</param>
        public UserDetails(User user)
        {
            this.Id = user.Id;
            this.UserName = user.UserName;
            this.Title = user.Title;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;

            this.Email = user.Email;
            this.TwoFactorProvider = user.TwoFactorAuthenticationProvider;
            this.PhoneNumber = user.PhoneNumber;
            this.ExternalId = user.ExternalId;
            this.SecurityQuestion = user.SecurityQuestion;
            this.SecurityAnswer = user.SecurityAnswer;
            this.roles = user.RoleNames.ToList();
            this.IsExternalUser = user.IsExternalUser;
        }

        /// <summary>
        /// Updates the user with data container in this class.
        /// If the userID doesn't match the Id in this class an error is thrown
        /// </summary>
        /// <param name="user">The user to update</param>
        public void UpdateUser(User user)
        {
            if (user.Id != this.Id) throw new Exception("Id of provided User doesn't match this Id");
            user.UserName = string.IsNullOrWhiteSpace(this.UserName) ? user.UserName : this.UserName;
            user.Title = this.Title == null ? user.Title : this.Title;
            user.FirstName = this.FirstName == null ? user.FirstName : this.FirstName;
            user.LastName = this.LastName == null ? user.LastName : this.LastName;

            user.Email = this.Email == null ? user.Email : this.Email;
            user.TwoFactorAuthenticationProvider = this.TwoFactorProvider == null ? user.TwoFactorAuthenticationProvider : this.TwoFactorProvider;
            user.PhoneNumber = this.PhoneNumber == null ? user.PhoneNumber : this.PhoneNumber;
            user.ExternalId = this.ExternalId == null ? user.ExternalId : this.ExternalId;
            user.SecurityAnswer = this.SecurityAnswer == null ? user.SecurityAnswer : this.SecurityAnswer;
            user.SecurityQuestion = this.SecurityQuestion == null ? user.SecurityQuestion : this.SecurityQuestion;
            user.IsExternalUser = this.IsExternalUser;
        }
    }
}
