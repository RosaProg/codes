using DSPrima.Security;
using PCHI.Model.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Security
{
    /// <summary>
    /// Defines a security code for a given user
    /// </summary>
    public class UserSecurityCode
    {
        /// <summary>
        /// Gets or sets the User Id
        /// </summary>
        [Key, MaxLength(128), Column(Order = 0), ForeignKey("User")]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the User the security code is for
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Gets or sets the type of code it is
        /// </summary>
        [Key, MaxLength(100), Column(Order = 1)]
        public string SecurityCodePurpose { get; set; }

        /// <summary>
        /// Gets or sets the encrypted version of the code
        /// </summary>
        public string EncryptedCode { get; set; }

        /// <summary>
        /// Gets or sets the code itself
        /// </summary>
        [NotMapped]
        public string Code
        {
            get
            {
                if (this.User == null) throw new Exception("User doesn't exist");
                return MachineKeyEncryption.Decrypt(this.EncryptedCode, User.Id);
            }

            set
            {
                if (this.User == null) throw new Exception("User must be set before the code can be set");
                this.EncryptedCode = MachineKeyEncryption.Encrypt(value, User.Id);
            }
        }

        /// <summary>
        /// Gets or sets the date and time the code expires
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSecurityCode"/> class
        /// </summary>
        internal UserSecurityCode()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSecurityCode"/> class
        /// Sets the encrypted code so it can be decrypted for verification purposes
        /// </summary>
        /// <param name="user">The User</param>
        /// <param name="purpose">The purpose</param>
        /// <param name="encryptedCode">The encrypted code</param>
        public UserSecurityCode(User user, string purpose, string encryptedCode)
        {
            this.User = user;
            this.User.Id = user.Id;
            this.SecurityCodePurpose = purpose;
            this.EncryptedCode = encryptedCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSecurityCode"/> class
        /// </summary>
        /// <param name="user">The user the code is for</param>
        /// <param name="purpose">The purpose of the code it is for</param>
        /// <returns>A security code for the given type and user</returns>
        public static UserSecurityCode CreateSecurityCode(User user, string purpose)
        {
            UserSecurityCode code = new UserSecurityCode();
            code.User = user;
            code.User.Id = user.Id;
            code.SecurityCodePurpose = purpose;
            code.Code = UserSecurityCode.GenerateCode();
            code.ResetExpiry();

            return code;
        }

        /// <summary>
        /// Holds the list of allowed characters for the code
        /// </summary>
        private static readonly string[] AllowedCharacters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };

        /// <summary>
        /// Generates a code of 10 characters
        /// </summary>
        /// <returns>The generated code</returns>
        private static string GenerateCode()
        {
            Random r = new Random((int)DateTime.Now.Ticks / 10000);
            StringBuilder code = new StringBuilder();
            for (int i = 0; i < 10; i++)
            {
                if (i == 5) code.Append("-");
                code.Append(UserSecurityCode.AllowedCharacters[r.Next(0, UserSecurityCode.AllowedCharacters.Length - 1)]);
            }

            return code.ToString();
        }

        /// <summary>
        /// Resets the expiry time for this code
        /// </summary>
        public void ResetExpiry()
        {
            this.SetExpire();
        }

        /// <summary>
        /// Sets the expiry time for this code to Now + 30 minutes
        /// </summary>
        private void SetExpire()
        {
            this.ExpiresAt = DateTime.Now.AddMinutes(30);
        }
    }
}
