using DSPrima.WcfUserSession.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPrima.WcfUserSession.Interfaces
{
    /// <summary>
    /// Interface defining functionality for user finding
    /// </summary>
    public interface IUserManager
    {
        /// <summary>
        /// Finds a user by userName and password
        /// </summary>
        /// <param name="username">The username of the user to find</param>
        /// <param name="password">The password of the user</param>
        /// <param name="user">If the login was successful, the user must be filled in here</param>
        /// <returns>The user if found or null otherwise</returns>
        LoginResult Find(string username, string password, ref IUser user);

        /// <summary>
        /// Gets the user with the given Id
        /// </summary>
        /// <param name="userId">The Id of the user to retrieve</param>
        /// <returns>The User if found or null otherwise</returns>
        IUser Find(string userId);
    }
}
