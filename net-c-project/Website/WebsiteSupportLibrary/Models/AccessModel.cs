using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebsiteSupportLibrary.Models
{
    public class LoginModel
    {
        /// <summary>
        /// User name of the account to reset the password
        /// </summary>
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        /// <summary>
        /// Current password of the account
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Indicates if the user wants to be remembered by the website
        /// </summary>
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class TwoFactorModel
    {
        /// <summary>
        /// User name of the account to reset the password
        /// </summary>
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        /// <summary>
        /// Current password of the account
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Security Code")]
        public string TwoFactorCode { get; set; }
    }
    /// <summary>
    /// Class containing all the necessary elements to let the user reset his password
    /// </summary>
    public class ResetPassword
    {
        /// <summary>
        /// User name of the account to reset the password
        /// </summary>
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        /// <summary>
        /// New password to assign to the account
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        /// <summary>
        /// Confirmation of the given new password for the account
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Token sent by email as an extra security factor
        /// </summary>
        [Required]
        [Display(Name = "Token")]
        public string Token { get; set; }

        /// <summary>
        /// The security question that the user previously defined
        /// </summary>
        [Required]
        [Display(Name = "Security question")]
        public string SecurityQuestion { get; set; }

        /// <summary>
        /// The answer to the security question that the user previously defined
        /// </summary>
        [Required]
        [Display(Name = "Security answer")]
        public string SecurityAnswer { get; set; }
    }
}