using DSPrima.Security;
using Microsoft.AspNet.Identity;
using PCHI.BusinessLogic.IIdentityMessageServices;
using PCHI.BusinessLogic.Properties;
using PCHI.BusinessLogic.Security;
using PCHI.BusinessLogic.TokenProviders;
using PCHI.BusinessLogic.Utilities;
using PCHI.BusinessLogic.Utilities.Model;
using PCHI.DataAccessLibrary;
using PCHI.Model.Messages;
using PCHI.Model.Notifications;
using PCHI.Model.Questionnaire.Response;
using PCHI.Model.Security;
using PCHI.Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCHI.BusinessLogic.Managers
{
    /// <summary>
    /// The UserManager class is responsible for managing Users
    /// </summary>
    public class UserManager : UserManager<User>
    {
        /// <summary>
        /// Holds the <see cref="AccessHandlerManager"/> for internal use
        /// </summary>
        private AccessHandlerManager manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserManager"/> class
        /// </summary>
        /// <param name="manager">The <see cref="AccessHandlerManager"/> instance to use</param>
        internal UserManager(AccessHandlerManager manager)
            : base(manager.UserAccessHandler)
        {
            this.manager = manager;

            this.UserTokenProvider = new EmailTokenProvider();

            // Example for two factor authentication can be found here: http://www.hanselman.com/blog/AddingTwoFactorAuthenticationToAnASPNETApplication.aspx
            this.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider()
            {
                Subject = "TwoStageCodeSubject",
                BodyFormat = "TwoStageCodeBody",
                Manager = this.manager
            });

            this.EmailService = new EmailService();

            UserValidator<User> userValidator = new UserValidator<User>(this);
            userValidator.AllowOnlyAlphanumericUserNames = false;

            PasswordValidator passwordValidator = new PasswordValidator();
            passwordValidator.RequireDigit = true;
            passwordValidator.RequiredLength = 6;
            passwordValidator.RequireLowercase = true;
            passwordValidator.RequireNonLetterOrDigit = true;
            passwordValidator.RequireUppercase = true;
            this.PasswordValidator = passwordValidator;

            this.UserValidator = userValidator;

            this.MaxFailedAccessAttemptsBeforeLockout = Settings.Default.MaxFailedAccessAttemptsBeforeLockout;
            this.UserLockoutEnabledByDefault = true;
            this.DefaultAccountLockoutTimeSpan = Settings.Default.LockoutTimeSpan;
        }

        /// <summary>
        /// Gets all users the current User can see
        /// </summary>
        /// <returns>The List of all users the current user is allowed to see</returns>
        public List<User> FindUsers()
        {
            // TODO Add security and filtering based upon permission
            return this.manager.UserAccessHandler.FindUsers().ToList();
        }

        /// <summary>
        /// Finds and returns the User based upon the email
        /// </summary>
        /// <param name="userName">The email of the user</param>
        /// <returns>The found User or null if not found</returns>
        public User GetUserByUsername(string userName)
        {
            try
            {
                SecuritySession.Current.VerifyAccess(Actions.GET_USER, userName: userName);
                var result = this.manager.UserAccessHandler.GetUserByUsername(userName);

                if (result != null)
                {
                    Logger.Audit(new Audit(Actions.GET_USER, AuditEventType.READ, result));
                }
                else
                {
                    Logger.Audit(new Audit(Actions.GET_USER, AuditEventType.READ, typeof(User), "UserName", userName, false, "User not found"));
                }

                return result;
            }
            catch (Exception ex)
            {
                Logger.Audit(new Audit(Actions.GET_USER, AuditEventType.READ, typeof(User), "UserName", userName, false, ex.Message));
                throw ex;
            }
        }

        /// <summary>
        /// Creates or updates a patient.
        /// If a user with the given Username doesn't exist it will be created, if it does exist, the patient will be added to that user
        /// </summary>
        /// <param name="externalId">The external ID of the patient</param>
        /// <param name="userName">The username of the user</param>
        /// <param name="email">The email</param>
        /// <param name="title">the title of the patient</param>
        /// <param name="firstName">The first name</param>
        /// <param name="lastName">The last name</param>
        /// <param name="dateOfBirth">The date of birth</param>
        /// <param name="mobilePhone">The patients mobile phone</param>
        /// <returns>The created or updated Patient</returns>
        public Patient CreateOrUpdatePatient(string externalId, string userName, string email, string title, string firstName, string lastName, DateTime dateOfBirth, string mobilePhone)
        {
            try
            {
                SecuritySession.Current.VerifyAccess(Actions.CREATE_OR_UPDATE_PATIENT);
                if (userName.Length > 450) throw this.manager.MessageHandler.GetError(ErrorCodes.USERNAME_LENGTH_EXCEEDED);
                if (userName.Contains("\\") || userName.Contains("/")) throw this.manager.MessageHandler.GetError(ErrorCodes.USERNAME_CONTAINS_ILLEGAL_CHARACTERS);
            }
            catch (Exception ex)
            {
                Logger.Audit(new Audit(Actions.CREATE_OR_UPDATE_PATIENT, AuditEventType.ADD, typeof(Patient), "Email", email, false, ex.Message));
                throw ex;
            }

            IdentityResult result = new IdentityResult(null);
            User existing = this.Users.Where(u => u.UserName == userName).SingleOrDefault();
            User user = null;
            if (existing == null)
            {
                try
                {
                    result = UserManagerExtensions.Create(this, new User() { UserName = userName, Email = email, PhoneNumber = mobilePhone, Title = title, FirstName = firstName, LastName = lastName });
                    if (result.Succeeded) user = this.Users.Where(u => u.UserName == userName).SingleOrDefault();
                    else throw new PCHIError(ErrorCodes.GENERAL_IDENTITY_RESULT_ERROR, result.Errors.Aggregate((s1, s2) => { return s1 + "\n" + s2; }));
                    Logger.Audit(new Audit(Actions.CREATE_OR_UPDATE_PATIENT, AuditEventType.ADD, user));
                }
                catch (Exception ex)
                {
                    Logger.Audit(new Audit(Actions.CREATE_OR_UPDATE_PATIENT, AuditEventType.ADD, typeof(User), "UserName", userName, false, ex.Message));
                    throw ex;
                }
            }
            else
            {
                user = existing;
            }

            Patient patient = null;
            Patient newPatient = null;
            if (user != null)
            {
                patient = !string.IsNullOrWhiteSpace(externalId) ? this.manager.UserAccessHandler.GetPatientByExternalId(externalId) : null;
                try
                {
                    if (patient != null)
                    {
                        Patient p = patient;
                        p.Title = title;
                        p.FirstName = firstName;
                        p.LastName = lastName;
                        p.ProxyUserPatientMap.Add(new ProxyUserPatientMap(user, p));
                        p.DateOfBirth = dateOfBirth;
                        p.Email = email;
                        p.PhoneNumber = mobilePhone;
                        p.ExternalId = externalId;
                        this.manager.UserAccessHandler.Update(p);
                        Logger.Audit(new Audit(Actions.CREATE_OR_UPDATE_PATIENT, AuditEventType.MODIFIED, p));
                    }
                    else
                    {
                        Patient p = new Patient();
                        p.Title = title;
                        p.FirstName = firstName;
                        p.LastName = lastName;
                        p.ProxyUserPatientMap.Add(new ProxyUserPatientMap(user, p));
                        p.DateOfBirth = dateOfBirth;
                        p.Email = email;
                        p.PhoneNumber = mobilePhone;
                        p.ExternalId = externalId;
                        this.manager.UserAccessHandler.Add(p);
                        newPatient = p;
                        Logger.Audit(new Audit(Actions.CREATE_OR_UPDATE_PATIENT, AuditEventType.ADD, p));
                    }

                    this.AddToRole(user.Id, "PatientProxy");
                }
                catch (Exception ex)
                {
                    Logger.Audit(new Audit(Actions.CREATE_OR_UPDATE_PATIENT, AuditEventType.ADD, typeof(Patient), "Email", email, false, ex.Message));
                    throw ex;
                }
            }

            // Only send the registration mail if the user is created (i.e. existing is null)
            if (existing == null && user != null)
            {
                UserSecurityCode confirmationToken = null;
                do
                {
                    confirmationToken = UserSecurityCode.CreateSecurityCode(user, "Registration");
                } while (this.FindUserForRegistrationToken(confirmationToken.Code) != null);

                user.RegistrationConfirmationToken = confirmationToken.EncryptedCode;
                UserManagerExtensions.Update(this, user);

                // string confirmationToken = HttpUtility.UrlEncode(MachineKeyEncryption.Encrypt(user.UserName));
                TextParser parser = new TextParser(this.manager);
                TextDefinition td = parser.ParseMessage("RegistrationEmail", new Dictionary<Model.Messages.ReplaceableObjectKeys, object>()
                {
                    { ReplaceableObjectKeys.Patient, newPatient },
                    { ReplaceableObjectKeys.Code, confirmationToken.Code }
                });

                SmtpMailClient.SendMail(user.Email, "OPSMC RePLAY Registration", td.Text, td.Html);
            }

            if (newPatient != null)
            {
                try
                {
                    QuestionnaireUserResponseGroup group = this.manager.QuestionnaireAccessHandler.CreateQuestionnaireUserResponseGroup(newPatient.Id, BusinessLogic.Properties.Settings.Default.NewRegistrationQuestionnaire, null, null);
                    Logger.Audit(new Audit(Actions.CREATE_OR_UPDATE_PATIENT, AuditEventType.ADD, group));
                }
                catch (Exception ex)
                {
                    Logger.Audit(new Audit(Actions.CREATE_OR_UPDATE_PATIENT, AuditEventType.ADD, typeof(QuestionnaireUserResponseGroup), "Id", null, false, ex.Message));
                    throw ex;
                }
            }

            return newPatient == null ? patient : newPatient;
        }

        /// <summary>
        /// Checks if the user as encrypted in the confirmation token has already completed the registration process or not.
        /// The confirmation token is the token created and send to the user's email when the user is created. This token is reusable and never expires.
        /// </summary>
        /// <param name="confirmationToken">The token as created by the user creation process</param>
        /// <returns>True if the user has completed registration. False if the user doesn't exist or has not completed the registration process.</returns>
        public bool UserCompletedRegistration(string confirmationToken)
        {
            User user = this.FindUserForRegistrationToken(confirmationToken);
            if (user == null || (user != null && !user.EmailConfirmed)) return false;

            return true;
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
        public IdentityResult CompleteRegistration(string confirmationToken, string userName, string password, string email, string mobileNumber, string provider, string securityQuestion, string securityAnswer)
        {
            User user = null;

            try
            {
                if (!this.UserNameAvailable(userName, confirmationToken)) throw this.manager.MessageHandler.GetError(ErrorCodes.USERNAME_ALREADY_INUSE);
                if (userName.Contains("\\") || userName.Contains("/")) throw this.manager.MessageHandler.GetError(ErrorCodes.USERNAME_CONTAINS_ILLEGAL_CHARACTERS);
                user = this.FindUserForRegistrationToken(confirmationToken);

                if (user == null) return new IdentityResult("The user doesn't exist");
                if (user.EmailConfirmed) throw this.manager.MessageHandler.GetError(ErrorCodes.USER_ALREADY_COMPLETED_REGISTRATION);
                if (string.IsNullOrWhiteSpace(securityQuestion) || string.IsNullOrWhiteSpace(securityAnswer)) throw this.manager.MessageHandler.GetError(ErrorCodes.SECURITY_QUESTION_ANSWER_ARE_MANDATORY);

                string userId = user.Id;
                string token = this.GeneratePasswordResetToken(userId);
                IdentityResult result = UserManagerExtensions.ResetPassword(this, userId, token, password);

                if (result.Succeeded)
                {
                    if (provider != null && this.TwoFactorProviders.ContainsKey(provider))
                    {
                        user.TwoFactorAuthenticationProvider = provider;
                        user.TwoFactorEnabled = true;
                    }
                    else
                    {
                        user.TwoFactorEnabled = false;
                        user.TwoFactorAuthenticationProvider = null;
                    }
                    user.UserName = string.IsNullOrWhiteSpace(userName) ? user.UserName : userName;
                    user.Email = string.IsNullOrWhiteSpace(email) ? user.Email : email;
                    user.PhoneNumber = string.IsNullOrWhiteSpace(mobileNumber) ? user.PhoneNumber : mobileNumber;
                    user.EmailConfirmed = true;
                    user.SecurityQuestion = securityQuestion;
                    user.SecurityAnswer = securityAnswer;
                    // TODO remove comment tags
                    user.RegistrationConfirmationToken = null;

                    UserManagerExtensions.Update(this, user);

                    this.manager.NotificationHandler.CreateNotification(NotificationType.RegistrationComplete, userId);
                }

                Logger.Audit(new Audit(Actions.COMPLETE_REGISTRATION, AuditEventType.MODIFIED, user));

                return result;
            }
            catch (Exception ex)
            {
                if (user != null)
                {
                    Logger.Audit(new Audit(Actions.COMPLETE_REGISTRATION, AuditEventType.MODIFIED, user, false, ex.Message));
                }
                else
                {
                    Logger.Audit(new Audit(Actions.COMPLETE_REGISTRATION, AuditEventType.MODIFIED, typeof(User), "RegistrationConfirmationToken", confirmationToken, false, ex.Message));
                }

                throw ex;
            }
        }

        /// <summary>
        /// Changes a users password
        /// </summary>
        /// <param name="userId">The Id of the user</param>
        /// <param name="currentPassword">The current password</param>
        /// <param name="newPassword">The new password</param>
        /// <param name="authenticationToken">The authentication code send to the user. If the user has No two factor authentication provider set, this value can be ignored</param>
        /// <returns>The result indicating success or failure</returns>
        public IdentityResult ChangePassword(string userId, string currentPassword, string newPassword, string authenticationToken = null)
        {
            try
            {
                SecuritySession.Current.VerifyAccess(Actions.CHANGE_PASSWORD, userId: userId);
                User u = this.FindById(userId);
                if (u == null) throw this.manager.MessageHandler.GetError(ErrorCodes.USER_UNKNOWN);
                if (u.TwoFactorAuthenticationProvider != null && this.TwoFactorProviders.ContainsKey(u.TwoFactorAuthenticationProvider) && string.IsNullOrWhiteSpace(authenticationToken)) throw this.manager.MessageHandler.GetError(ErrorCodes.CODE_INCORRECT);

                if (u.TwoFactorAuthenticationProvider != null && this.TwoFactorProviders.ContainsKey(u.TwoFactorAuthenticationProvider))
                {
                    if (!this.VerifyTwoFactorToken(userId, u.TwoFactorAuthenticationProvider, authenticationToken))
                    {
                        Logger.Audit(new Audit(Actions.COMPLETE_REGISTRATION, AuditEventType.MODIFIED, u, false, "The provided code is not correct"));
                        return new IdentityResult("The provided code is not correct");
                    }
                }

                this.manager.NotificationHandler.CreateNotification(NotificationType.PasswordChanged, userId);

                IdentityResult result = UserManagerExtensions.ChangePassword(this, userId, currentPassword, newPassword);
                Logger.Audit(new Audit(Actions.CHANGE_PASSWORD, AuditEventType.MODIFIED, u));
                return result;
            }
            catch (Exception ex)
            {
                Logger.Audit(new Audit(Actions.CHANGE_PASSWORD, AuditEventType.MODIFIED, typeof(User), "Id", userId, false, ex.Message));
                throw ex;
            }
        }

        /// <summary>
        /// Get the password reset token for the user
        /// </summary>
        /// <param name="userId">The Id of the user</param>
        /// <returns>The reset token</returns>
        private string GeneratePasswordResetToken(string userId)
        {
            string token = UserManagerExtensions.GeneratePasswordResetToken(this, userId);
            return token;
        }

        /// <summary>
        /// Updates user data
        /// </summary>
        /// <param name="user">The user to change the data for</param>
        /// <returns>An identity result indicating success or failure and any errors that occurred</returns>
        public IdentityResult Update(User user)
        {
            try
            {
                SecuritySession.Current.VerifyAccess(Actions.UPDATE_USER_DATA, userId: user.Id);
                if (user.UserName.Length > 450) throw this.manager.MessageHandler.GetError(ErrorCodes.USERNAME_LENGTH_EXCEEDED);
                User tmpUser = this.FindById(user.Id);
                if (!tmpUser.IsExternalUser && (user.UserName.Contains("\\") || user.UserName.Contains("/"))) throw this.manager.MessageHandler.GetError(ErrorCodes.USERNAME_CONTAINS_ILLEGAL_CHARACTERS);
                IdentityResult result;
                if (SecuritySession.Current.User.Id == user.Id && !SecuritySession.Current.User.UserName.Equals(user.UserName, StringComparison.CurrentCultureIgnoreCase))
                {
                    if (this.Users.Where(u => u.UserName != user.UserName).FirstOrDefault() != null) throw this.manager.MessageHandler.GetError(ErrorCodes.USERNAME_ALREADY_INUSE);
                }

                result = UserManagerExtensions.Update(this, user);
                if (result.Succeeded)
                {
                    this.manager.NotificationHandler.CreateNotification(NotificationType.UserDetailsChanged, user.Id);
                }

                Logger.Audit(new Audit(Actions.UPDATE_USER_DATA, AuditEventType.MODIFIED, user));
                return result;
            }
            catch (Exception ex)
            {
                Logger.Audit(new Audit(Actions.UPDATE_USER_DATA, AuditEventType.MODIFIED, user, false, ex.Message));
                throw ex;
            }
        }

        /// <summary>
        /// Updates the given Patient and the user it Belongs to
        /// </summary>
        /// <param name="patient">The patient to update</param>
        /// <returns>An identity result indicating success or failure and any errors that occurred</returns>
        public bool Update(Patient patient)
        {
            try
            {
                SecuritySession.Current.VerifyAccess(Actions.UPDATE_PATIENT_DATA, patientId: patient.Id);
                if (patient.ProxyUserPatientMap.Count == 0) throw this.manager.MessageHandler.GetError(ErrorCodes.USER_IS_REQUIRED);
                this.manager.UserAccessHandler.Update(patient);

                Logger.Audit(new Audit(Actions.UPDATE_PATIENT_DATA, AuditEventType.MODIFIED, patient));
                return true;
            }
            catch (Exception ex)
            {
                Logger.Audit(new Audit(Actions.UPDATE_PATIENT_DATA, AuditEventType.MODIFIED, patient, false, ex.Message));
                throw ex;
            }
        }

        /// <summary>
        /// Finds a list of users based upon any of the data provided.
        /// All parameters are optional
        /// </summary>
        /// <param name="id">The Id of the patient</param>
        /// <param name="firstName">The first name of the user</param>
        /// <param name="lastName">The last name of the user</param>
        /// <param name="dob">The date of birth</param>
        /// <param name="email">The email</param>
        /// <param name="phoneNumber">The phone number</param>
        /// <param name="externalId">The external ID of the user</param>
        /// <returns>The list of users found</returns>
        public List<Patient> FindPatient(string id = null, string firstName = null, string lastName = null, DateTime? dob = null, string email = null, string phoneNumber = null, string externalId = null)
        {
            // TODO, Limit list based upon access rights
            return SecuritySession.Current.Filter(this.manager.UserAccessHandler.FindPatient(id, firstName, lastName, dob, email, phoneNumber, externalId));
        }

        /// <summary>
        /// Finds the details for the Patient with the given ID
        /// </summary>
        /// <param name="patientId">The ID of the Patient to find</param>
        /// <returns>The Patient or null</returns>
        public Patient FindPatient(string patientId)
        {
            try
            {
                SecuritySession.Current.VerifyAccess(Actions.FIND_PATIENT, patientId: patientId);
                var result = this.manager.UserAccessHandler.FindPatient(patientId);
                if (result != null)
                {
                    Logger.Audit(new Audit(Actions.FIND_PATIENT, AuditEventType.READ, result));
                }
                else
                {
                    Logger.Audit(new Audit(Actions.FIND_PATIENT, AuditEventType.READ, typeof(Patient), "Id", patientId, false, "Patient not found"));
                }

                return result;
            }
            catch (Exception ex)
            {
                Logger.Audit(new Audit(Actions.FIND_PATIENT, AuditEventType.READ, typeof(Patient), "Id", patientId, false, ex.Message));
                throw ex;
            }
        }

        /// <summary>
        /// Find and returns the list of functionality assigned to a user via it's roles
        /// </summary>
        /// <param name="userId">The Id of the user to get the list for</param>
        /// <returns>The list found</returns>
        internal List<Permission> GetPermissions(string userId)
        {
            return this.manager.UserAccessHandler.GetPermissionsForUser(userId);
        }

        /// <summary>
        /// Gets a list of functionalities that belong to a given Role
        /// </summary>
        /// <param name="roleName">The name of the role to get the functionalitites for</param>
        /// <returns>A list of functionalitites for a given Role</returns>
        internal List<Permission> GetPermissionsForRole(string roleName)
        {
            return this.manager.UserAccessHandler.GetPermissionsForRole(roleName);
        }

        /// <summary>
        /// Sends a two stage authentication code to the user for the given provider for testing purposes
        /// Either the confirmation token send out as part of the registration process or the username must be provided
        /// </summary>
        /// <param name="provider">The provider selected by the user</param>        
        /// <param name="confirmationToken">The confirmation token of the user during registration. If not provided, the test is done for the currently logged in user</param>
        public void SendTwoStageAuthenticationForTest(string provider, string confirmationToken)
        {
            User u = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(confirmationToken))
                {
                    u = this.FindUserForRegistrationToken(confirmationToken);
                }
                else if (SecuritySession.Current.User != null)
                {
                    u = this.FindByName(SecuritySession.Current.User.UserName);
                }

                if (u == null) throw this.manager.MessageHandler.GetError(ErrorCodes.USER_UNKNOWN);

                if (this.TwoFactorProviders.ContainsKey(provider))
                {
                    string token = this.GenerateTwoFactorToken(u.Id, provider);

                    /*

                    SmtpMailClient.SendMail(u.Email, subject.Text, bodyFormat.Text, bodyFormat.Html);
                     */
                    this.NotifyTwoFactorToken(u.Id, provider, token);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Verifies if the supplied two stage authentication code is correct for the given user
        /// Either the confirmation token send out as part of the registration process or the username must be provided
        /// </summary>
        /// <param name="provider">The provider for the test</param>        
        /// <param name="token">The authentication code send to the user</param>        
        /// <param name="confirmationToken">The confirmation token of the user</param>
        /// <returns>An operation result indicating success or failure</returns>        
        public bool VerifyTwoStageAuthenticationForTest(string provider, string token, string confirmationToken)
        {
            User u = null;
            if (!string.IsNullOrWhiteSpace(confirmationToken))
            {
                u = this.FindUserForRegistrationToken(confirmationToken);
            }
            else if (SecuritySession.Current.User != null)
            {
                u = this.FindByName(SecuritySession.Current.User.UserName);
            }

            if (u == null) throw this.manager.MessageHandler.GetError(ErrorCodes.USER_UNKNOWN);

            return this.VerifyTwoFactorToken(u.Id, provider, token);
        }

        /// <summary>
        /// Gets if the user of the current Session has multiple entities
        /// </summary>
        /// <param name="userId">The Id of the user to check this for</param>
        /// <returns>True if the user has multiple entities, false otherwise</returns>
        public bool UserHasMultipleEntities(string userId)
        {
            return this.manager.UserAccessHandler.GetPatients(userId).Count > 1;
        }

        /// <summary>
        /// Returns the List of patients assigned to the given user
        /// </summary>
        /// <param name="userId">The Id of the user to get the list of Patients for</param>
        /// <param name="username">The username of the user to get the list of Patients for</param>
        /// <returns>The list of PCHIUserEntities</returns>
        public List<Patient> GetPatientsForUser(string userId = null, string username = null)
        {
            try
            {
                SecuritySession.Current.VerifyAccess(Actions.GET_PATIENTS_ASSIGNED_TO_USER, userId: userId, userName: username);
                var result = this.manager.UserAccessHandler.GetPatients(userId, username);
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    Logger.Audit(new Audit(Actions.GET_PATIENTS_ASSIGNED_TO_USER, AuditEventType.READ, typeof(User), "Id", userId));
                }
                else
                {
                    Logger.Audit(new Audit(Actions.GET_PATIENTS_ASSIGNED_TO_USER, AuditEventType.READ, typeof(User), "UserName", username));
                }

                return result;
            }
            catch (Exception ex)
            {
                if (userId != null)
                {
                    Logger.Audit(new Audit(Actions.GET_PATIENTS_ASSIGNED_TO_USER, AuditEventType.READ, typeof(User), "Id", userId, false, ex.Message));
                }
                else
                {
                    Logger.Audit(new Audit(Actions.GET_PATIENTS_ASSIGNED_TO_USER, AuditEventType.READ, typeof(User), "UserName", username, false, ex.Message));
                }

                throw ex;
            }
        }

        /// <summary>
        /// Gets a list of patients for the given user by Id. Bypasses security checks
        /// </summary>
        /// <param name="userId">The Id of the user to get hte patients for</param>
        /// <returns>The List of patients found</returns>
        internal List<Patient> GetPatientsForUserInSession(string userId)
        {
            return this.manager.UserAccessHandler.GetPatients(userId);
        }

        /// <summary>
        /// Sends a two stage authentication code to the user if, and only if, the user has a two stage authentication provider set.
        /// The result indicates if the authentication code has been send (true) or not (false) due to the user not having a provider set.
        /// </summary>
        /// <param name="userId">The Id of the user to send the authentication code to</param>
        /// <returns>An boolean indicating if the authentication code had send success or failure</returns>
        public bool SendTwoStageAuthenticationCode(string userId)
        {
            User u = this.FindById(userId);
            if (u == null) throw this.manager.MessageHandler.GetError(ErrorCodes.USER_UNKNOWN);
            if (!this.TwoFactorProviders.ContainsKey(u.TwoFactorAuthenticationProvider)) return false;
            string token = this.GenerateTwoFactorToken(u.Id, u.TwoFactorAuthenticationProvider);
            this.NotifyTwoFactorToken(u.Id, u.TwoFactorAuthenticationProvider, token);
            return true;
        }

        /// <summary>
        /// Sends a forgotten password token by email to the user for resetting the password
        /// </summary>
        /// <param name="username">The name of the user</param>            
        public void ForgottenPassword(string username)
        {
            User u = null;
            try
            {
                u = this.FindByName(username);
                if (u == null) throw this.manager.MessageHandler.GetError(ErrorCodes.USER_UNKNOWN);
                string token = this.GeneratePasswordResetToken(u.Id); // HttpUtility.UrlEncode(MachineKeyEncryption.Encrypt(u.Id + "|" + this.GeneratePasswordResetToken(u.Id)));
                TextParser parser = new TextParser(this.manager);
                TextDefinition start = parser.ParseMessage("NotificationStart", new Dictionary<ReplaceableObjectKeys, object>() { { ReplaceableObjectKeys.User, u }, { ReplaceableObjectKeys.Code, token } });
                TextDefinition end = parser.ParseMessage("NotificationEnd", null);
                TextDefinition td = parser.ParseMessage("PasswordReset", new Dictionary<ReplaceableObjectKeys, object>() { { ReplaceableObjectKeys.User, u }, { ReplaceableObjectKeys.Code, token } });
                string text = start.Text + td.Text + end.Text;
                string html = start.Html + td.Html + end.Html;
                SmtpMailClient.SendMail(u.Email, "Password reset token", text, html);

                Logger.Audit(new Audit(Actions.FORGOT_PASSWORD, AuditEventType.READ, u));
            }
            catch (Exception ex)
            {
                Logger.Audit(new Audit(Actions.FORGOT_PASSWORD, AuditEventType.READ, typeof(User), "UserName", username, false, ex.Message));
                throw ex;
            }
        }

        /// <summary>
        /// Gets the security question for the given User Id or Token.
        /// Only one has to be provided
        /// </summary>
        /// <param name="userId">The Id of the user</param>
        /// <param name="token">The password reset token send to the user using <see cref="M:ForgottenPassword"/></param>
        /// <returns>The Security Question for the user</returns>
        public string GetSecurityQuestion(string userId = null, string token = null)
        {
            User u = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    u = this.FindById(userId);
                }
                else if (!string.IsNullOrWhiteSpace(token))
                {
                    string id = MachineKeyEncryption.Decrypt(token);
                    id = id.Substring(0, id.IndexOf("|"));
                    u = this.FindById(id);
                }

                if (u == null) throw this.manager.MessageHandler.GetError(ErrorCodes.USER_UNKNOWN);
                Logger.Audit(new Audit(Actions.GET_SECURITY_QUESTION, AuditEventType.READ, u));

                return u != null ? u.SecurityQuestion : null;
            }
            catch (Exception ex)
            {
                Logger.Audit(new Audit(Actions.GET_SECURITY_QUESTION, AuditEventType.READ, typeof(User), "Id", userId, false, ex.Message));
                throw ex;
            }
        }

        /// <summary>
        /// Resets the password for the user with the given User name
        /// </summary>
        /// <param name="username">The user name of the user to reset the password for</param>
        /// <param name="newPassword">The new password to set</param>
        /// <param name="securityToken">The reset token</param>
        /// <param name="securityAnswer">The answer to the security question</param>
        /// <returns>An identify Result indicating success or failure</returns>
        public IdentityResult ResetPassword(string username, string newPassword, string securityToken, string securityAnswer)
        {
            try
            {
                User u = this.FindByName(username);
                if (u == null) throw this.manager.MessageHandler.GetError(ErrorCodes.USER_UNKNOWN);
                IdentityResult result;
                if (u.SecurityAnswer.Equals(securityAnswer, StringComparison.CurrentCultureIgnoreCase))
                {
                    result = UserManagerExtensions.ResetPassword(this, u.Id, securityToken, newPassword);
                }
                else
                {
                    result = new IdentityResult("The supplied security answer is incorrect");
                }

                Logger.Audit(new Audit(Actions.RESET_PASSWORD, AuditEventType.READ, u, result.Succeeded, result.Succeeded ? null : result.Errors.Aggregate((s1, s2) => { return s1 + "\n" + s2; })));
                return result;
            }
            catch (Exception ex)
            {
                Logger.Audit(new Audit(Actions.RESET_PASSWORD, AuditEventType.READ, typeof(User), "UserName", username, false, ex.Message));
                throw ex;
            }
        }

        /// <summary>
        /// Finds a user for a given Registratin token
        /// </summary>
        /// <param name="registrationToken">The registration token to find the user for</param>
        /// <returns>The User found or null if not found</returns>
        internal User FindUserForRegistrationToken(string registrationToken)
        {
            List<User> users = this.Users.Where(u => u.RegistrationConfirmationToken != null).ToList();

            foreach (User u in users)
            {
                if (registrationToken == new UserSecurityCode(u, "Confirmation", u.RegistrationConfirmationToken).Code)
                {
                    return u;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns a list of available roles
        /// </summary>
        /// <returns>The list of available roles</returns>
        public List<string> GetAvailableRoles()
        {
            return this.manager.UserAccessHandler.GetAvailableRoles();
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
        public IdentityResult CreateUser(string userName, string password, bool isExternalUser, string title, string firstName, string lastName, string email, string phoneNumber, string externalId, string role)
        {
            try
            {
                if (!isExternalUser)
                {
                    var pwdResult = this.PasswordValidator.ValidateAsync(password);
                    pwdResult.Wait();
                    if (!pwdResult.Result.Succeeded) return pwdResult.Result;
                }

                if (this.Users.Where(u => u.UserName.Equals(userName, StringComparison.CurrentCultureIgnoreCase)) != null) throw this.manager.MessageHandler.GetError(ErrorCodes.USERNAME_ALREADY_INUSE);
                if (userName.Length > 450) throw this.manager.MessageHandler.GetError(ErrorCodes.USERNAME_LENGTH_EXCEEDED);
                if (!isExternalUser && (userName.Contains("\\") || userName.Contains("/"))) throw this.manager.MessageHandler.GetError(ErrorCodes.USERNAME_CONTAINS_ILLEGAL_CHARACTERS);

                User user = new User();
                user.UserName = userName;
                user.IsExternalUser = isExternalUser;
                user.Title = title;
                user.FirstName = firstName;
                user.LastName = lastName;
                user.Email = email;
                user.EmailConfirmed = true;
                user.PhoneNumber = phoneNumber;
                user.ExternalId = externalId;

                var result = this.Create(user);
                if (result.Succeeded)
                {
                    if (!isExternalUser) result = this.AddPassword(user.Id, password);

                    if (result.Succeeded)
                    {
                        this.AddToRole(user.Id, role);
                    }
                }

                Logger.Audit(new Audit(Actions.CREATE_USER, AuditEventType.ADD, user));
                return result;
            }
            catch (Exception ex)
            {
                Logger.Audit(new Audit(Actions.CREATE_USER, AuditEventType.ADD, typeof(User), "UserName", userName, false, ex.Message));
                throw ex;
            }
        }

        /// <summary>
        /// Gets the auditlog for the given user
        /// </summary>
        /// <param name="userId">The Id of the user to get the audit log for</param>
        /// <returns>The audit trail</returns>
        public List<AuditTrailEntry> GetAuditlogForUser(string userId)
        {
            try
            {
                SecuritySession.Current.VerifyAccess(Actions.GET_AUDIT_TRAIL, userId: userId);
                var result = this.manager.AuditHandler.GetAudit(userId);
                Logger.Audit(new Audit(Actions.GET_AUDIT_TRAIL, AuditEventType.READ, typeof(User), "Id", userId));
                return result;
            }
            catch (Exception ex)
            {
                Logger.Audit(new Audit(Actions.GET_AUDIT_TRAIL, AuditEventType.READ, typeof(User), "Id", userId, false, ex.Message));
                throw ex;
            }
        }

        /// <summary>
        /// Finds and returns all users that belong to a certain role
        /// </summary>
        /// <param name="role">The role to look for</param>
        /// <returns>List of matching users</returns>        
        public List<User> GetRoleMembers(string role)
        {
            return this.manager.UserAccessHandler.GetRoleMembers(role);
        }

        /// <summary>
        /// Sends registration token for the given username
        /// </summary>
        /// <param name="user">User to send the registration token</param>
        public void SendRegistrationToken(User user)
        {
            UserSecurityCode confirmationToken = null;
            do
            {
                confirmationToken = UserSecurityCode.CreateSecurityCode(user, "Registration");
            } while (this.FindUserForRegistrationToken(confirmationToken.Code) != null);

            user.RegistrationConfirmationToken = confirmationToken.EncryptedCode;
            UserManagerExtensions.Update(this, user);

            TextParser parser = new TextParser(this.manager);
            TextDefinition td = parser.ParseMessage("RegistrationEmail", new Dictionary<Model.Messages.ReplaceableObjectKeys, object>()
                {
                    { ReplaceableObjectKeys.User, user },
                    { ReplaceableObjectKeys.Code, confirmationToken.Code }
                });
            IdentityResult result = new IdentityResult();
            try
            {
                SmtpMailClient.SendMail(user.Email, "OPSMC RePLAY Registration", td.Text, td.Html);
            }
            catch (Exception ex)
            {
                // TODO add logger audit
                throw ex;
            }
        }

        /// <summary>
        /// Resends registration token for the given username
        /// </summary>
        /// <param name="username">Username of the account to resend the registration token</param>
        public void ResendRegistrationToken(string username)
        {
            var user = this.manager.UserAccessHandler.GetUserByUsername(username);
            if (user != null)
            {
                try
                {
                    this.SendRegistrationToken(user);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new Exception("Username not found");
            }
        }

        /// <summary>
        /// Saves teh details for the given patient
        /// </summary>
        /// <param name="patient">The patient to save</param>
        public void SavePatientDetails(Patient patient)
        {
            SecuritySession.Current.VerifyAccess(Actions.UPDATE_PATIENT_DATA, patient.Id);
            this.manager.UserAccessHandler.Update(patient);
        }

        /// <summary>
        /// Returns a list of tags assigned to Patients
        /// </summary>
        /// <returns>A list of name of patient tags</returns>
        public List<string> GetPatientTags()
        {
            return this.manager.UserAccessHandler.GetPatientTagNames();
        }

        /// <summary>
        /// Checks if the username is available or not
        /// </summary>
        /// <param name="username">The username to test</param>
        /// <returns>True is the username is available, false otherwise</returns>
        public bool UserNameAvailable(string username, string confirmationToken)
        {
            User u = this.FindByName(username);
            if (u == null) return true;
            return new UserSecurityCode(u, "Confirmation", u.RegistrationConfirmationToken).Code == confirmationToken;
        }
    }
}