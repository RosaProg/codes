using DSPrima.WcfUserSession.Interfaces;
using DSPrima.WcfUserSession.Model;
using DSPrima.WcfUserSession.SecurityHandlers;
using Microsoft.AspNet.Identity;
using PCHI.BusinessLogic.Interfaces;
using PCHI.BusinessLogic.Managers;
using PCHI.BusinessLogic.Properties;
using PCHI.BusinessLogic.Security;
using PCHI.DataAccessLibrary;
using PCHI.Model.Security;
using PCHI.Model.Users;
using System.Collections.Generic;
using System.Linq;

namespace PCHI.BusinessLogic
{
    /// <summary>
    /// Implementes the IUserManager logic for the WcfUserSessions
    /// </summary>
    public class WcfUserSessionUserManager : IUserManager
    {
        /// <summary>
        /// Holds the external authenticator instance
        /// </summary>
        public static IExternalAuthenticator ExternalAuthenticathor;

        /// <summary>
        /// Finds a user by userName and password
        /// </summary>
        /// <param name="username">The username of the user to find</param>
        /// <param name="password">The password of the user</param>
        /// <param name="u">The reference of the user to fill</param>
        /// <returns>The user if found or null otherwise</returns>
        public LoginResult Find(string username, string password, ref DSPrima.WcfUserSession.Interfaces.IUser u)
        {
            if (string.IsNullOrWhiteSpace(username)) return LoginResult.Failed;

            ManagerHandler m = new ManagerHandler();
            User user = m.UserManager.FindByName(username);

            if (user == null)
            {
                return LoginResult.Failed;
            }

            if (!user.EmailConfirmed)
            {
                return LoginResult.RegistrationNotCompleted;
            }

            if (m.UserManager.IsLockedOut(user.Id))
            {
                return LoginResult.UserIsLockedOut;
            }

            if (user.IsExternalUser && !string.IsNullOrWhiteSpace(password))
            {
                bool success = WcfUserSessionUserManager.ExternalAuthenticathor.VerifyUsernameAndPassword(username, password);
                if (!success) return LoginResult.Failed;
            }
            else if (user.IsExternalUser && !WcfUserSessionSecurity.Current.RequestHeader.ClientName.Equals(Settings.Default.SecuredAccessServerName, System.StringComparison.CurrentCultureIgnoreCase))
            {
                return LoginResult.Failed;
            }
            else if (!user.IsExternalUser && !m.UserManager.CheckPassword(user, password))
            {
                if (user.LockoutEnabled)
                {
                    m.UserManager.AccessFailed(user.Id);
                }

                return LoginResult.Failed;
            }

            m.UserManager.ResetAccessFailedCount(user.Id);

            user.ProxyUserPatientMap = (from p in m.UserManager.GetPatientsForUserInSession(user.Id) select new ProxyUserPatientMap(user, p)).ToList();
            user.Permissions = m.UserManager.GetPermissions(user.Id);
            user.RoleNames = m.UserManager.GetRoles(user.Id);

            u = user;
            return LoginResult.Success;
        }

        /// <summary>
        /// Gets the user with the given Id
        /// </summary>
        /// <param name="userId">The Id of the user to retrieve</param>
        /// <returns>The User if found or null otherwise</returns>
        public DSPrima.WcfUserSession.Interfaces.IUser Find(string userId)
        {
            using (UserManager manager = new UserManager(new AccessHandlerManager()))
            {
                User user = manager.FindById(userId);
                user.Permissions = manager.GetPermissions(user.Id);
                user.ProxyUserPatientMap = (from p in manager.GetPatientsForUserInSession(user.Id) select new ProxyUserPatientMap(user, p)).ToList();

                user.RoleNames = manager.GetRoles(user.Id);
                return user;
            }
        }

        /// <summary>
        /// Sets the CurrentSession of the Business Logic to the WcfUserSessionSecurity current session
        /// </summary>
        /// <param name="sessionId">The Id of the current session</param>
        /// <param name="user">The user to set it to</param>
        /// <param name="header">The Request header that was part of the message</param>
        public static void WcfUserSessionSecurity_SessionUpdated(string sessionId, DSPrima.WcfUserSession.Interfaces.IUser user, RequestHeader header)
        {
            SecuritySession.SetCurrentSession(sessionId, (User)user);
            WcfUserSessionUserManager.SetSessionPermissions();
        }

        /// <summary>
        /// Sets the functionalities available for the currently selected role to the current Session
        /// </summary>
        public static void SetSessionPermissions()
        {
            ClientSessionDetails details = new ClientSessionDetails();
            details.SelectedRole = SecuritySession.Current.Role;

            if (WcfUserSessionSecurity.Current.User != null)
            {
                User user = (User)WcfUserSessionSecurity.Current.User;
                details.AvailableRoles = user.RoleNames.ToList();
                details.AvailablePatients = user.ProxyUserPatientMap.Select(m => m.Patient).ToDictionary(p => p.Id, p => p.DisplayName);

                if (SecuritySession.Current.Role != null)
                {
                    details.SelectedRole = SecuritySession.Current.Role;

                    /*if (SecuritySession.Current.Role == "PatientProxy") details.Permissions = new List<Permission>();
                    else*/
                    details.Permissions = new ManagerHandler().UserManager.GetPermissionsForRole(SecuritySession.Current.Role);
                }
            }

            WcfUserSessionSecurity.Current.AddSessionInformation(details);
        }
    }
}