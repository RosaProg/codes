using Microsoft.AspNet.Identity;
using PCHI.DataAccessLibrary;
using PCHI.Model.Security;
using PCHI.Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.BusinessLogic.TokenProviders
{
    /// <summary>
    /// A implementation of the IUserTokenProvider for easy extending
    /// </summary>
    public abstract class UserTokenProvider : IUserTokenProvider<User, string>
    {
        /// <summary>
        /// Generates a security Token and stores it in the DB
        /// </summary>
        /// <param name="purpose">The purpose of the token</param>
        /// <param name="manager">The user manager instance that calls this</param>
        /// <param name="user">The user to generate the token for</param>
        /// <returns>The task that is generating the Token</returns>
        public Task<string> GenerateAsync(string purpose, UserManager<User, string> manager, User user)
        {
            return Task.FromResult<string>(this.GenerateCode(purpose, user));
        }

        /// <summary>
        /// Validates if the provider is valid for the user
        /// </summary>
        /// <param name="manager">The manager that calls this</param>
        /// <param name="user">The user to validate for</param>
        /// <returns>True if valid false otherwise</returns>
        public Task<bool> IsValidProviderForUserAsync(UserManager<User, string> manager, User user)
        {
            return Task.FromResult<bool>(manager.SupportsUserSecurityStamp);
        }

        /// <summary>
        /// Notifies the user
        /// </summary>
        /// <param name="token">The token to send</param>
        /// <param name="manager">The manager that calls this</param>
        /// <param name="user">The user to send the token to</param>
        /// <returns>A task that executes this</returns>
        public abstract Task NotifyAsync(string token, UserManager<User, string> manager, User user);

        /// <summary>
        /// Validates the token
        /// </summary>
        /// <param name="purpose">The purpose of the message to validate </param>
        /// <param name="token">The token to validate</param>
        /// <param name="manager">The mangager that calls this</param>
        /// <param name="user">The user to validate for</param>
        /// <returns>A task that executes this with True as a result indicating success</returns>
        public Task<bool> ValidateAsync(string purpose, string token, UserManager<User, string> manager, User user)
        {
            return Task.FromResult<bool>(this.Validate(purpose, token, user));
        }

        /// <summary>
        /// Generates the code
        /// </summary>
        /// <param name="purpose">The purpose to generate it for</param>
        /// <param name="user">The user to generate the token for</param>
        /// <returns>A token generated for the user</returns>
        protected string GenerateCode(string purpose, User user)
        {
            AccessHandlerManager ahm = new AccessHandlerManager();
            UserSecurityCode code = ahm.UserAccessHandler.GetSecurityCode(user.Id, purpose);
            if (code == null || code.ExpiresAt < DateTime.Now)
            {
                code = UserSecurityCode.CreateSecurityCode(user, purpose);
            }
            else
            {
                code.ResetExpiry();
            }

            new AccessHandlerManager().UserAccessHandler.StoreSecurityCode(code);

            return code.Code;
        }

        /// <summary>
        /// Validates the token
        /// </summary>
        /// <param name="purpose">The purpose of the token</param>
        /// <param name="token">The token to validate</param>
        /// <param name="user">The user to validate the token for</param>
        /// <returns>True if the token matches, false otherwise</returns>
        private bool Validate(string purpose, string token, User user)
        {
            AccessHandlerManager ahm = new AccessHandlerManager();
            var code = ahm.UserAccessHandler.GetSecurityCode(user.Id, purpose);

            if (code != null && code.Code.Equals(token, StringComparison.CurrentCultureIgnoreCase))
            {
                ahm.UserAccessHandler.DeleteSecurityCode(user.Id, purpose);
                if (code.ExpiresAt >= DateTime.Now) return true;
            }

            return false;
        }
    }
}
