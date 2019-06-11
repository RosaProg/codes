using DSPrima.WcfUserSession.Behaviours;
using DSPrima.WcfUserSession.Model;
using DSPrima.WcfUserSession.SecurityHandlers;
using Microsoft.AspNet.Identity;
using PCHI.BusinessLogic;
using PCHI.BusinessLogic.Security;
using PCHI.BusinessLogic.Utilities;
using PCHI.BusinessLogic.Utilities.Model;
using PCHI.Model.Messages;
using PCHI.Model.Security;
using PCHI.Model.Users;
using PCHI.WcfServices.API.PCHIServices.InterfaceClients.Base;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Users;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.WcfServices.API.PCHIServices.InterfaceClients.Users
{
    /// <summary>
    /// The server site implementation of the IUserService
    /// </summary>
    [WcfUserSessionBehaviour]
    public class UserService : BaseService, IUserService
    {
        /// <summary>
        /// Logs the user in
        /// If Two factor authentication is required the authentication code for that is automatically send to the user
        /// </summary>
        /// <param name="userName">The username of the user</param>
        /// <param name="password">The password of the user</param>
        /// <returns>An operation indicating success with the Data variable indicating if Two factor authentication is required (true) or not (false)</returns>
        public OperationResultAsBool Login(string userName, string password)
        {
            Logger.Audit(new Audit(Model.Security.Actions.LOGIN_STARTED, AuditEventType.READ, typeof(User), "UserName", userName));
            LoginResult result = WcfUserSessionSecurity.Login(userName, password);
            if (result == LoginResult.Success)
            {
                User u = this.handler.UserManager.FindByName(userName);
                if (u.TwoFactorEnabled && u.TwoFactorAuthenticationProvider != null && this.handler.UserManager.TwoFactorProviders.ContainsKey(u.TwoFactorAuthenticationProvider))
                {
                    string token = this.handler.UserManager.GenerateTwoFactorToken(u.Id, u.TwoFactorAuthenticationProvider);
                    this.handler.UserManager.NotifyTwoFactorToken(u.Id, u.TwoFactorAuthenticationProvider, token);
                    return new OperationResultAsBool(null, true);
                }
                else
                {
                    Logger.Audit(new Audit(Model.Security.Actions.LOGIN_COMPLETED, AuditEventType.READ, typeof(User), "UserName", userName));
                }
            }
            else
            {
                Logger.Audit(new Audit(Model.Security.Actions.LOGIN_COMPLETED, AuditEventType.READ, typeof(User), "UserName", userName, false));
            }

            PCHIError err = null;
            if (result == LoginResult.Failed) err = this.handler.MessageManager.GetError(ErrorCodes.LOGIN_FAILED);
            if (result == LoginResult.UserIsLockedOut) err = this.handler.MessageManager.GetError(ErrorCodes.USER_IS_LOCKEDOUT);
            if (result == LoginResult.RegistrationNotCompleted) err = this.handler.MessageManager.GetError(ErrorCodes.REGISTRATION_NOT_COMPLETED);
            return new OperationResultAsBool(err, false);
        }

        /// <summary>
        /// Authenticates the second step in the authentication process
        /// </summary>
        /// <param name="userName">The username</param>
        /// <param name="token">The authentication code</param>
        /// <returns>True if success, false otherwise</returns>
        public bool TwoFactorAuthenticate(string userName, string token)
        {
            User u = this.handler.UserManager.FindByName(userName);
            string userId = u.Id;
            bool result = this.handler.UserManager.VerifyTwoFactorToken(userId, u.TwoFactorAuthenticationProvider, token);
            if (result)
            {
                Logger.Audit(new Audit(Model.Security.Actions.LOGIN_COMPLETED, AuditEventType.READ, typeof(User), "UserName", userName));
                //WcfUserSessionSecurity.Current.MultiStepVerificationCompleted(userName);
            }

            return result;
        }

        /// <summary>
        /// Checks if the user has multiple roles and/or patients.
        /// If the user has 1 or more Patients and no additional roles, the PatientProxy role is automatically selected and Authentication/Login is automatically completed. It is up to the website to determine which Patient the user is using/wants to use.
        /// If the user has only 1 Role, this Role is automatically selected for the user and Authentication/Login is automatically completed.
        /// If the user has more then 1 Role or 1 Role in addition to the patients. These roles are in the Strings variable and the User has to select which Role to use.
        /// </summary>
        /// <param name="userName">The username of the user. If the username is null, the current authenticated User for the session is used instead.</param>
        /// <returns>An operationResult inidicating success or failure. The Strings variable is filled with Role Name(s) the user has access to.</returns>
        public OperationResultAsLists UserHasMultipleRoles(string userName)
        {
            try
            {
                userName = userName == null ? WcfUserSessionSecurity.Current.User.UserName : userName;
                User user = this.handler.UserManager.FindByName(userName == null ? WcfUserSessionSecurity.Current.User.UserName : userName);
                if (user == null) throw this.handler.MessageManager.GetError(ErrorCodes.USER_SESSION_EXPIRED);
                if (WcfUserSessionSecurity.Current.User != null && !WcfUserSessionSecurity.Current.VerifyNameOrIdWithSession(userName: userName)) throw this.handler.MessageManager.GetError(ErrorCodes.USER_UNKNOWN);

                SecuritySession.SetCurrentSession(WcfUserSessionSecurity.Current.SessionId, user);

                List<string> userRoles = this.handler.UserManager.GetRoles(user.Id).ToList();
                List<Patient> patients = this.handler.UserManager.GetPatientsForUser(userId: user.Id);
                Dictionary<string, string> roles = new Dictionary<string, string>();
                if (userRoles.Count == 1 /*&& patients.Count == 0*/)
                {
                    var result = this.SelectRole(user.UserName, userRoles[0]);
                    if (!result.Succeeded) throw new PCHIError(result.ErrorCode, result.ErrorMessages);
                }
                /*else if (patients.Count > 0 && userRoles.Count == 0)
                {
                    var result = this.SelectRole(user.UserName, "PatientProxy");
                    if (!result.Succeeded) throw new PCHIError(result.ErrorCode, result.ErrorMessages);
                }*/

                /*
                if (patients.Count > 0)
                {
                    userRoles.Add("PatientProxy");
                }*/

                return new OperationResultAsLists(null) { Strings = userRoles };
            }
            catch (Exception ex)
            {
                return new OperationResultAsLists(ex);
            }
        }

        /// <summary>
        /// Selects the Role to be used
        /// </summary>
        /// <param name="userName">The name of the user to set the Patient for</param>
        /// <param name="role">The role name the user has selected</param>
        /// <returns>An operation result indicating success or failure</returns>        
        public OperationResult SelectRole(string userName, string role)
        {
            if (!WcfUserSessionSecurity.Current.VerifyNameOrIdWithSession(userName: userName)) throw this.handler.MessageManager.GetError(ErrorCodes.USER_UNKNOWN);

            try
            {
                SecuritySession.Current.SetUserSelectedRole(WcfUserSessionSecurity.Current.SessionId, role);
                WcfUserSessionSecurity.Current.MultiStepVerificationCompleted(userName);
                WcfUserSessionUserManager.SetSessionPermissions();
                return new OperationResult(null);
            }
            catch (Exception ex)
            {
                return new OperationResult(ex);
            }
        }

        /// <summary>
        /// Gets a list of all available patients for the user in the current Session
        /// </summary>
        /// <returns>An operation Result indicating success or failure with the list of available users fill in the Strings  variable</returns>
        public OperationResultAsDictionary GetPatientsForUser()
        {
            try
            {
                return new OperationResultAsDictionary(null) { StringDictionary = this.handler.UserManager.GetPatientsForUser(WcfUserSessionSecurity.Current.User.Id).ToDictionary(u => u.Id, u => u.DisplayName) };
            }
            catch (Exception ex)
            {
                return new OperationResultAsDictionary(ex);
            }
        }

        /// <summary>
        /// Logs the user out and ends the session
        /// </summary>
        public void Logout()
        {
            User u = (User)WcfUserSessionSecurity.Current.User;
            try
            {

                WcfUserSessionSecurity.Current.Logout(false);
                Logger.Audit(new Audit(Model.Security.Actions.LOGOUT, AuditEventType.READ, u));
            }
            catch (Exception ex)
            {
                Logger.Audit(new Audit(Model.Security.Actions.LOGOUT, AuditEventType.READ, u, false, ex.Message));
                throw ex;
            }
        }

        /// <summary>
        /// To be called to complete the Registration of a user
        /// </summary>        
        /// <param name="confirmationToken">The confirmation token that was send to the user</param>
        /// <param name="userName">The new user name </param>
        /// <param name="password">The password for the user</param>
        /// <param name="email">The new email</param>
        /// <param name="mobileNumber">The new mobile number</param>
        /// <param name="provider">The provider to set for the user</param>
        /// <param name="securityQuestion">The Security question</param>
        /// <param name="securityAnswer">The security Answer</param>
        /// <returns>The result of completing indicating success or failure</returns>        
        public OperationResult CompleteRegistration(string confirmationToken, string userName, string password, string email, string mobileNumber, string provider, string securityQuestion, string securityAnswer)
        {
            IdentityResult result = this.handler.UserManager.CompleteRegistration(confirmationToken, userName, password, email, mobileNumber, provider, securityQuestion, securityAnswer);
            if (result.Succeeded)
            {
                /*
                var loginresult = WcfUserSessionSecurity.Login(this.handler.UserManager.GetUserName(confirmationToken), password);

                LoginResult result = WcfUserSessionSecurity.Login(userName, password);
                if (loginresult == LoginResult.Success && WcfUserSessionSecurity.Current.MultiStepVerificationEnabled)
                {
                    WcfUserSessionSecurity.Current.MultiStepVerificationCompleted(this.handler.UserManager.GetUserName(confirmationToken));
                }
                */

                return new OperationResult(null);
            }

            string errors = result.Errors.Aggregate((s1, s2) =>
            {
                return s1 + Environment.NewLine + s2;
            });

            return new OperationResult(new PCHIError(ErrorCodes.GENERAL_IDENTITY_RESULT_ERROR, errors));
        }

        /// <summary>
        /// Checks if the user as encrypted in the confirmation token has already completed the registration process or not.
        /// The confirmation token is the token created and send to the user's email when the user is created. This token is reusable and never expires.
        /// </summary>
        /// <param name="confirmationToken">The token as created by the user creation process</param>
        /// <returns>True if the user has completed registration. False if the user doesn't exist or has not completed the registration process.</returns>
        public OperationResultAsBool UserCompletedRegistration(string confirmationToken)
        {
            try
            {
                return new OperationResultAsBool(null, this.handler.UserManager.UserCompletedRegistration(confirmationToken));
            }
            catch (Exception ex)
            {
                return new OperationResultAsBool(ex, false);
            }
        }

        /// <summary>
        /// Sends a two stage authentication code to the current user if, and only if, the user has a stwo stage authentication provider set.
        /// The Data variable in the OperationResultAsBool indicates if the authentication code has been send (true) or not (false).
        /// </summary>        
        /// <returns>An operation result indicating success or failure</returns>
        public OperationResultAsBool SendAuthenticationCode()
        {
            try
            {
                bool isSend = this.handler.UserManager.SendTwoStageAuthenticationCode(WcfUserSessionSecurity.Current.User.Id);
                return new OperationResultAsBool(null, isSend);
            }
            catch (Exception ex)
            {
                return new OperationResultAsBool(ex, false);
            }
        }

        /// <summary>
        /// Changes the password
        /// </summary>
        /// <param name="currentPassword">The current password</param>
        /// <param name="newPassword">The new password</param>
        /// <param name="authenticationToken">The Second Stage Authentication token send to the user. This is only needed if <see cref="M:SendAuthenticationCode(string userId)"/> returns True in the Data variable</param>
        /// <returns>An operation result indicating success and the errors. The Data variable can be ignored</returns>
        public OperationResult ChangePassword(string currentPassword, string newPassword, string authenticationToken)
        {
            if (WcfUserSessionSecurity.Current.User != null)
            {
                IdentityResult result = this.handler.UserManager.ChangePassword(WcfUserSessionSecurity.Current.User.Id, currentPassword, newPassword, authenticationToken);
                if (result.Succeeded) return new OperationResult(null);
                string errors = result.Errors.Aggregate((s1, s2) =>
                {
                    return s1 + Environment.NewLine + s2;
                });
                return new OperationResult(new PCHIError(ErrorCodes.GENERAL_IDENTITY_RESULT_ERROR, errors));
            }

            return new OperationResult(this.handler.MessageManager.GetError(ErrorCodes.USER_SESSION_EXPIRED));
        }

        /// <summary>
        /// Gets a Dictionary of all usernames and displays names of all users inside the database
        /// </summary>
        /// <returns>A dictionary with the key being the userName and the value being the display name of each user</returns>
        public OperationResultAsLists FindUsers()
        {
            try
            {
                List<UserDetails> result = new List<UserDetails>();
                var data = this.handler.UserManager.FindUsers();
                foreach (User u in data)
                {
                    result.Add(new UserDetails(u));
                }

                return new OperationResultAsLists(null) { Users = result };
            }
            catch (Exception ex)
            {
                return new OperationResultAsLists(ex);
            }
        }

        /// <summary>
        /// Gets the details of the current user
        /// </summary>
        /// <returns>The OperationResults continaining user Settings (if successful)</returns>
        public OperationResultAsUserDetails GetDetailsForCurrentUser()
        {
            if (WcfUserSessionSecurity.Current.User == null) return new OperationResultAsUserDetails(this.handler.MessageManager.GetError(ErrorCodes.USER_SESSION_EXPIRED));

            UserDetails settings = new UserDetails(this.handler.UserManager.FindById(WcfUserSessionSecurity.Current.User.Id));

            return new OperationResultAsUserDetails(null) { UserDetails = settings };
        }

        /// <summary>
        /// Updates the user settings
        /// </summary>
        /// <param name="settings">The settings of the user</param>
        /// <param name="password">The password for checking the user is actually the correct user</param>
        /// <returns>An operation result indicating success or failure</returns>
        public OperationResult SaveCurrentUserSettings(UserDetails settings, string password)
        {
            if (WcfUserSessionSecurity.Current.User == null) return new OperationResultAsUserDetails(this.handler.MessageManager.GetError(ErrorCodes.USER_SESSION_EXPIRED));
            settings.Id = WcfUserSessionSecurity.Current.User.Id;
            return this.SaveUserSettings(settings, password);
        }

        /// <summary>
        /// Updates the user settings
        /// </summary>        
        /// <param name="settings">The settings of the user</param>
        /// <param name="password">The password for checking the user is actually the correct user</param>
        /// <returns>An operation result indicating success or failure</returns>     
        private OperationResult SaveUserSettings(UserDetails settings, string password)
        {
            try
            {
                User user = this.handler.UserManager.FindById(settings.Id);
                if (user == null) return new OperationResultAsUserDetails(this.handler.MessageManager.GetError(ErrorCodes.USER_UNKNOWN));

                if (!this.handler.UserManager.CheckPassword(user, password))
                {
                    throw this.handler.MessageManager.GetError(ErrorCodes.PASSWORD_INCORRECT);
                }

                settings.UpdateUser(user);

                if (settings.TwoFactorProvider != null && this.handler.UserManager.TwoFactorProviders.ContainsKey(settings.TwoFactorProvider))
                {
                    user.TwoFactorAuthenticationProvider = settings.TwoFactorProvider;
                    user.TwoFactorEnabled = true;
                }
                else
                {
                    user.TwoFactorAuthenticationProvider = null;
                    user.TwoFactorEnabled = false;
                }

                string errors = null;
                try
                {
                    var result = this.handler.UserManager.Update(user);
                    if (result.Succeeded)
                    {
                        return new OperationResult(null);
                    }

                    errors = result.Errors.Aggregate((s1, s2) =>
                    {
                        return s1 + Environment.NewLine + s2;
                    });
                }
                catch (Exception ex)
                {
                    errors = ex.Message;
                }

                return new OperationResult(new PCHIError(ErrorCodes.GENERAL_IDENTITY_RESULT_ERROR, errors));
            }
            catch (Exception ex)
            {
                return new OperationResult(ex);
            }
        }

        /// <summary>
        /// Sends a two stage authentication code to the user for the given provider for testing purposes
        /// Either the confirmation token send out as part of the registration process or the currently logged in user is used instead
        /// </summary>                
        /// <param name="provider">The provider selected by the user</param>
        /// <param name="confirmationToken">The confirmation token of the user during registration. If not provided, the test is done for the currently logged in user</param>
        /// <returns>An operation result indicating success or failure</returns>        
        public OperationResult SendTwoStageAuthenticationForTest(string provider, string confirmationToken)
        {
            try
            {
                this.handler.UserManager.SendTwoStageAuthenticationForTest(provider, confirmationToken);
                return new OperationResult(null);
            }
            catch (Exception ex)
            {
                return new OperationResult(ex);
            }
        }

        /// <summary>
        /// Verifies if the supplied two stage authentication code is correct for the given user
        /// Either the confirmation token send out as part of the registration process or the currently logged in user is used instead
        /// </summary>
        /// <param name="provider">The provider for the test</param>        
        /// <param name="code">The authentication code send to the user</param>
        /// <param name="confirmationToken">The confirmation token of the user during registration. If not provided, the test is done for the currently logged in user</param>
        /// <returns>An operation result indicating success or failure</returns>        
        public OperationResult VerifyTwoStageAuthenticationForTest(string provider, string code, string confirmationToken)
        {
            try
            {
                if (this.handler.UserManager.VerifyTwoStageAuthenticationForTest(provider, code, confirmationToken))
                {
                    return new OperationResult(null);
                }
                else
                {
                    return new OperationResult(this.handler.MessageManager.GetError(ErrorCodes.CODE_INCORRECT));
                }
            }
            catch (Exception ex)
            {
                return new OperationResult(ex);
            }
        }

        /// <summary>
        /// Sends a forgotten password token by email to the user for resetting the password
        /// </summary>
        /// <param name="username">The name of the user</param>
        /// <returns>An operation result indicating success or failure</returns>        
        public OperationResult ForgottenPassword(string username)
        {
            try
            {
                this.handler.UserManager.ForgottenPassword(username);
                return new OperationResult(null);
            }
            catch (Exception ex)
            {
                return new OperationResult(ex);
            }
        }

        /// <summary>
        /// Resets the password for the user with the given User name
        /// </summary>
        /// <param name="username">The user name of the user to reset the password for</param>
        /// <param name="newPassword">The new password to set</param>
        /// <param name="token">The reset token</param>
        /// <param name="securityAnswer">The answer to the security question</param>
        /// <returns>An Operation Result indicating success or failure</returns>
        public OperationResult ResetPassword(string username, string newPassword, string token, string securityAnswer)
        {
            try
            {
                var result = this.handler.UserManager.ResetPassword(username, newPassword, token, securityAnswer);
                if (result.Succeeded)
                {
                    return new OperationResult(null);
                }
                else
                {
                    return new OperationResult(new PCHIError(ErrorCodes.GENERAL_IDENTITY_RESULT_ERROR, result.Errors.Aggregate((s1, s2) => { return s1 + "\n" + s2; })));
                }
            }
            catch (Exception ex)
            {
                return new OperationResult(ex);
            }

        }

        /// <summary>
        /// Gets the security question for the user with the given Password reset token
        /// </summary>
        /// <param name="passwordResetToken">The password reset token given to the user by calling "ForgottenPassword"</param>
        /// <returns>An OperationResultAsString indicating success or failure with the Security Question filled in the data variable</returns>        
        public OperationResultAsString GetSecurityQuestion(string passwordResetToken)
        {
            try
            {
                return new OperationResultAsString(null, this.handler.UserManager.GetSecurityQuestion(token: passwordResetToken));
            }
            catch (Exception ex)
            {
                return new OperationResultAsString(ex, null);
            }
        }

        /// <summary>
        /// Creates a new User
        /// </summary>
        /// <param name="userName">The name of the user</param>        
        /// <param name="password">The password</param>
        /// <param name="isExternalUser">If true, the user has an external source which needs to be checked for the password</param>
        /// <param name="title">The title of the user</param>
        /// <param name="firstName">The firstname of the user</param>
        /// <param name="lastName">The last name of the user</param>
        /// <param name="email">The email of the user</param>
        /// <param name="phoneNumber">The mobile phone of the user</param>
        /// <param name="externalId">The external id of the user</param>
        /// <param name="role">The role to assign to the user</param>
        /// <returns>An operation result indicating success or failure</returns>
        public OperationResult CreateUser(string userName, string password, bool isExternalUser, string title, string firstName, string lastName, string email, string phoneNumber, string externalId, string role)
        {
            try
            {
                var result = this.handler.UserManager.CreateUser(userName, password, isExternalUser, title, firstName, lastName, email, phoneNumber, externalId, role);
                if (!result.Succeeded)
                {
                    return new OperationResult(new PCHIError(ErrorCodes.GENERAL_IDENTITY_RESULT_ERROR, result.Errors.Aggregate((s1, s2) => { return s1 + "\n" + s2; })));
                }

                return new OperationResult(null);
            }
            catch (Exception ex)
            {
                return new OperationResult(ex);
            }
        }

        /// <summary>
        /// Finds and returns all practitioners.
        /// </summary>
        /// <returns>And operation Result as Dictionary indicating success or failure. StringDictionary contains the Practitioners, with Key being the External Id and Value being the Display Name</returns>        
        public OperationResultAsDictionary GetPractitioners()
        {
            try
            {
                return new OperationResultAsDictionary(null) { StringDictionary = this.handler.UserManager.GetRoleMembers("practitioner").ToDictionary(u => u.ExternalId, u => u.DisplayName) };
            }
            catch (Exception ex)
            {
                return new OperationResultAsDictionary(ex);
            }
        }

        /// <summary>
        /// Gets the audit trail for the Current User
        /// </summary>
        /// <returns>An operation result indicating success or failure</returns>        
        public OperationResultAsLists GetUserAuditTrail()
        {
            try
            {
                return new OperationResultAsLists(null) { AuditTrail = this.handler.UserManager.GetAuditlogForUser(WcfUserSessionSecurity.Current.User.Id) };
            }
            catch (Exception ex)
            {
                return new OperationResultAsLists(ex);
            }
        }

        /// <summary>
        /// Resends registration token for the given username
        /// </summary>
        /// <param name="userName">Username of the account to resend the registration token</param>
        /// <returns>An operation result indicating success or failure</returns>
        public OperationResult ResendRegistrationToken(string userName)
        {
            try
            {
                this.handler.UserManager.ResendRegistrationToken(userName);
                return new OperationResult(null);
            }
            catch (Exception ex)
            {
                return new OperationResult(ex);
            }
        }

        /// <summary>
        /// Resends two factor authentication token for the given username
        /// </summary>
        /// <param name="userName">Username of the account to resend the two factor authentication token</param>
        /// <returns>An operation result indicating success or failure</returns>
        public OperationResult ResendTwoFactorAuthentication(string userName)
        {
            User u = this.handler.UserManager.FindByName(userName);
            string token = this.handler.UserManager.GenerateTwoFactorToken(u.Id, u.TwoFactorAuthenticationProvider);
            var result = this.handler.UserManager.NotifyTwoFactorToken(u.Id, u.TwoFactorAuthenticationProvider, token);
            if (result.Succeeded)
            {
                return new OperationResult(null);
            }
            else
            {
                return new OperationResult(new Exception(result.Errors.ToString()));
            }
        }

        /// <summary>
        /// Checks if the username is available or not
        /// </summary>
        /// <param name="username">The username to test</param>
        /// <param name="confirmationToken">The confirmation token of the user doing the registration for ensuring that when the username is in use it doesn't belongs to the current user. If null or empty, this test is done against the logged in user</param>
        /// <returns>True is the username is available, false otherwise</returns>        
        public bool UserNameAvailable(string username, string confirmationToken)
        {
            return this.handler.UserManager.UserNameAvailable(username, confirmationToken);
        }

        /// <summary>
        /// Validates if the password is valid or not
        /// </summary>
        /// <param name="password">The password to check</param>
        /// <returns>True if the passwords validates, false otherwise</returns>        
        public bool ValidatePassword(string password)
        {
            var pwdResult = this.handler.UserManager.PasswordValidator.ValidateAsync(password);
            pwdResult.Wait();
            return pwdResult.Result.Succeeded;
        }
    }
}
