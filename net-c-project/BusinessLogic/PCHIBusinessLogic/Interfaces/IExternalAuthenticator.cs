using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.BusinessLogic.Interfaces
{
    /// <summary>
    /// Defines the interface which external authentication mechanisms (such as against against Active Directory) must support
    /// </summary>
    public interface IExternalAuthenticator
    {
        /// <summary>
        /// Verifies if the given username and password is correct for the external method of Authentication
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        /// <returns>A boolean indicating the username and password are correct (true) or not (false)</returns>
        bool VerifyUsernameAndPassword(string username, string password);
    }
}
