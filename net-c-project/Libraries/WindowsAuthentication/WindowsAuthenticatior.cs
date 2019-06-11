using PCHI.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsAuthentication
{
    /// <summary>
    /// Authenticates a username and password against active directory
    /// </summary>
    public class WindowsAuthenticatior : IExternalAuthenticator
    {
        /// <summary>
        /// Verifies if the given username and password is correct against the Active Directory
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        /// <returns>A boolean indicating the username and password are correct (true) or not (false)</returns>
        public bool VerifyUsernameAndPassword(string username, string password)
        {            
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain))
            {
                // validate the credentials
                return pc.ValidateCredentials(username, password);
            }
        }
    }
}
