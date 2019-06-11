using DSPrima.WcfUserSession.ClientSession;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Questionnaire;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Users;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.Models;
using WebsiteSupportLibrary.Models;
using WebsiteSupportLibrary.ControllerHelpers;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Service;
using System.Text;

namespace Website.Controllers
{
    public class AccessController : Controller
    {
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl, string code)
        {
            if (!string.IsNullOrWhiteSpace(code))
            {
                UserClient uc = new UserClient();
                if (!uc.UserCompletedRegistration(code).Data) return RedirectToAction("CompleteRegistration", new { returnUrl = returnUrl, code = code });
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl, string code)
        {
            UserClient p = new UserClient();

            var result = p.Login(model.UserName, model.Password);
            if (result.Succeeded)
            {
                TempData["UserName"] = model.UserName;
                if (result.Data)
                {
                    return RedirectToAction("VerifyCode", new { ReturnUrl = returnUrl });
                }
                else
                {
                    return RedirectToAction("SelectRole", new { ReturnUrl = returnUrl });
                }
            }
            else
            {
                if (result.ErrorCode == PCHI.Model.Messages.ErrorCodes.REGISTRATION_NOT_COMPLETED)
                {
                    ViewBag.RegistrationNotCompleted = true;
                }
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", result.ErrorMessages);
            return View(model);
        }


        [AllowAnonymous]
        public ActionResult VerifyCode(string returnUrl, bool resendTwoFactorAuthentication = false)
        {
            if (!TempData.ContainsKey("UserName")) return RedirectToAction("Login", new { ReturnUrl = returnUrl });
            TwoFactorModel m = new TwoFactorModel();
            m.UserName = TempData["UserName"].ToString();
            TempData["UserName"] = m.UserName; //The value of TempData persists until it is read or until the current user’s session times out. So we set it again incase the user refreshes the page.
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.ActionName = "VerifyCode";
            ViewBag.ControllerName = "Access";
            if (resendTwoFactorAuthentication)
            {
                UserClient uc = new UserClient();
                var result = uc.ResendTwoFactorAuthentication(m.UserName);
                if (result.Succeeded)
                {
                    TempData["NotificationMessage"] = "Code has been send";
                }
                else
                {
                    ModelState.AddModelError("", result.ErrorMessages);
                }
            }
            return View(m);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult VerifyCode(TwoFactorModel model, string returnUrl)
        {
            UserClient uc = new UserClient();
            if (uc.TwoFactorAuthenticate(model.UserName, model.TwoFactorCode))
            {
                TempData["UserName"] = model.UserName;
                return RedirectToAction("SelectRole", new { ReturnUrl = returnUrl });
            }
            TempData["NotificationMessage"] = "";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        /// <summary>
        /// Makes appear a popUp window on the login view, so the user can provide an username to resend the registration token
        /// </summary>
        /// <param name="username">Username of the account to resend the registration token</param>
        /// <param name="closing">Indicates if the function was called while the process of closing the popup window</param>
        /// <returns>The login view with the popUp window displayed</returns>
        public ActionResult ResendRegistrationToken(string username, string returnUrl, bool closing = false)
        {
            if (username == null)
            {
                TempData["login"] = true;
                ViewBag.ActionName = "Login";
                ViewBag.ControllerName = "Access";
                if (!closing)
                {
                    ViewBag.ShowPopUp = "ResendRegistrationToken";
                }
                else
                {
                    ViewBag.RegistrationNotCompleted = true;
                }
            }
            else
            {
                UserClient uc = new UserClient();
                var result = uc.ResendRegistrationToken(username);
                if (result.Succeeded)
                {
                    TempData["NotificationMessage"] = "Code has been send";
                }
                else
                {
                    ModelState.AddModelError("", result.ErrorMessages);
                }
            }
            ViewBag.ReturnUrl = returnUrl;
            return View("Login");
        }

        public ActionResult SelectRole(string username, string roleName, string returnUrl, bool login = true)
        {
            if (!TempData.ContainsKey("UserName") && string.IsNullOrWhiteSpace(username) && !Request.IsAuthenticated) return RedirectToAction("Login", new { ReturnUrl = returnUrl });

            if (roleName == null)
            {
                UserClient uc = new UserClient();
                var result = uc.UserHasMultipleRoles(TempData.ContainsKey("UserName") ? TempData["UserName"].ToString() : null);
                ViewBag.ReturnUrl = returnUrl;
                if (!result.Succeeded)
                {
                    ViewBag.ErrorMessage = result.ErrorMessages;
                    return View();
                }
                else
                {
                    string patientId = string.Empty;
                    if (result.Strings.Count == 1)
                    {

                        if (result.Strings.Contains("PatientProxy"))
                        {
                            var resultSelectingRole = uc.SelectRole(TempData["UserName"].ToString(), "PatientProxy");
                            if (!resultSelectingRole.Succeeded)
                            {
                                ViewBag.ReturnUrl = returnUrl;
                                ViewBag.ErrorMessage = resultSelectingRole.ErrorMessages;
                                var result2 = uc.UserHasMultipleRoles(TempData["UserName"].ToString());
                                return View(result2.Strings);
                            }
                            if (returnUrl == null)
                            {
                                var patientsForCurrentUser = uc.GetPatientsForUser();
                                if (patientsForCurrentUser.StringDictionary.Count > 1)
                                {
                                    return RedirectToAction("SelectPatient", "Patient");
                                }
                                else
                                {
                                    if (patientsForCurrentUser.StringDictionary.Count == 1)
                                        patientId = patientsForCurrentUser.StringDictionary.ElementAt(0).Key;
                                }
                            }
                            else
                            {
                                return RedirectToLocal(returnUrl, "PatientProxy", patientId);
                            }

                        }

                        return RedirectToLocal(returnUrl, result.Strings.First(), patientId);
                    }
                    else
                    {
                        ViewBag.ShowPopUp = "SelectRole";
                        ViewBag.ReturnUrl = returnUrl;
                        if (login)
                        {
                            TempData["login"] = true;
                            ViewData["Roles"] = result.Strings.ToDictionary(x => x, x => x);
                            return View("Login");
                        }
                        else
                        {
                            if (result.Strings.Contains("IsPatient"))
                            {
                                ViewBag.ActionName = "Index";
                                ViewBag.ControllerName = "Patient";
                            }
                            else
                            {
                                ViewBag.ActionName = "Index";
                                ViewBag.ControllerName = "Admin";
                            }
                            TempData["login"] = false;
                            return View(result.Strings.ToDictionary(x => x, x => x));
                        }
                    }
                }
            }
            else
            {
                UserClient uc = new UserClient();
                var result = uc.SelectRole(username, roleName);
                ViewBag.ReturnUrl = returnUrl;
                if (!result.Succeeded)
                {
                    ViewBag.ErrorMessage = result.ErrorMessages;
                    var result2 = uc.UserHasMultipleRoles(TempData["UserName"].ToString());
                    return View(result2.Strings);
                }
                else
                {
                    return RedirectToLocal(returnUrl, roleName);
                }
            }
        }

        /// <summary>
        /// Handles the view to reset the password
        /// </summary>       
        /// <param name="token">Token to reset the password</param>
        /// <returns>The proper view to reset the user's password</returns>
        public ActionResult ResetPassword(string token)
        {
            ResetPassword model = new ResetPassword();

            if (token != null)
            {
                UserClient p = new UserClient();
                OperationResultAsString securityQuestion = p.GetSecurityQuestion(token);

                if (securityQuestion.Succeeded)
                {
                    model.SecurityQuestion = securityQuestion.Data.ToString();
                    TempData["SecurityQuestion"] = securityQuestion.Data.ToString();
                }
            }
            return View(model);

        }

        /// <summary>
        /// Handles the response of the user when he clicks submit in the form to reset the password
        /// </summary>
        /// <param name="model">Contains all the necessary elements to reset the password, checking for possible errors, like a password not secure enough or an unidentified user</param>
        /// <returns>If everything goes well, returs the login page so the user can login with the new password, if not we redisplay the same form with the obtained errors</returns>
        [HttpPost]
        public ActionResult ResetPassword(ResetPassword model)
        {
            UserClient p = new UserClient();
            var result = p.ResetPassword(model.UserName, model.NewPassword, model.Token, model.SecurityAnswer);
            if (result.Succeeded)
            {
                return View("Login");
            }
            else
            {
                // If we got this far, something failed, redisplay form
                ModelState.AddModelError("", result.ErrorMessages);
                return View(model);
            }

        }

        /// <summary>
        /// Makes appear a popUp windown on the login view, so the user can provide an email to reset its password
        /// </summary>
        /// <param name="emailForgottenPassword">Email where all the necessary information to reset the new password will be sent</param>
        /// <param name="closing">Indicates if the function was called while the process of closing the popup window</param>
        /// <returns>The login view with the popUp window displayed or closed</returns>
        public ActionResult ForgotPassword(string emailForgottenPassword, bool closing = false)
        {
            if (emailForgottenPassword != null)
            {
                UserClient p = new UserClient();
                p.ForgottenPassword(emailForgottenPassword);
            }
            else
            {
                ViewBag.ActionName = "Login";
                ViewBag.ControllerName = "Access";
                if (!closing)
                {
                    ViewBag.ShowPopUp = "ForgotPassword";
                }
            }
            return View("Login");
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            UserClient p = new UserClient();
            p.Logout();
            //WebSecurity.Logout();

            return RedirectToAction("Login", "Access");
        }

        public ActionResult CompleteRegistration(string code, string userName, string password, string repeatPassword, string securityQuestion, string securityAnswer, string SecurityCodeProvider, string email, string mobile, string authenticationCode, bool? resendCode)
        {
            if (!string.IsNullOrWhiteSpace(userName))
            {
                ServiceDetailsClient sdc = new ServiceDetailsClient();
                UserClient uc = new UserClient();
                var providers = sdc.GetTwoStageAuthenticationProviders().Strings;

                StringBuilder errors = new StringBuilder();
                if (string.IsNullOrWhiteSpace(userName)) errors.Append("The user name cannot be empty<br/>");
                else if (userName.Length > 450) errors.Append("The username cannot exceed 450 characters<br/>");
                else if (!uc.UserNameAvailable(userName, code)) errors.Append("The user name is not available<br/>");
                if (password != repeatPassword) errors.Append("The passwords do not match<br/>");
                else if (!uc.ValidatePassword(password)) errors.Append("The passwords must contain at least 1 capital, 1 lowercase, one number and one non-alphanumeric character<br/>");
                if (string.IsNullOrWhiteSpace(securityQuestion)) errors.Append("The security question cannot be empty<br/>");
                if (string.IsNullOrWhiteSpace(securityAnswer)) errors.Append("The security answer cannot be empty<br/>");
                if (providers.Contains(SecurityCodeProvider) && SecurityCodeProvider.Contains("mail") && string.IsNullOrWhiteSpace(email)) errors.Append("The email must be specified when the secure code is send by email<br/>");
                if (providers.Contains(SecurityCodeProvider) && SecurityCodeProvider.Contains("sms") && string.IsNullOrWhiteSpace(mobile)) errors.Append("The mobile number must be specified when the secure code is send by SMS<br/>");

                if (errors.Length == 0)
                {
                    if ((providers.Contains(SecurityCodeProvider) && authenticationCode == null) || (resendCode.HasValue && resendCode.Value))
                    {
                        uc.SendTwoStageAuthenticationForTest(SecurityCodeProvider, code);
                        ViewBag.ShowPopup = true;
                    }
                    else
                    {
                        bool success = false;
                        if (providers.Contains(SecurityCodeProvider) && !string.IsNullOrWhiteSpace(authenticationCode))
                        {
                            var result = uc.VerifyTwoStageAuthenticationForTest(SecurityCodeProvider, authenticationCode, code);
                            if (!result.Succeeded)
                            {
                                ViewBag.ErrorMessage = result.ErrorMessages;
                                ViewBag.ShowPopup = true;
                            }
                            else
                            {
                                success = true;
                            }
                        }
                        else
                        {
                            success = true;
                        }

                        if (success)
                        {

                            var res = uc.CompleteRegistration(code, userName, password, email, mobile, SecurityCodeProvider, securityQuestion, securityAnswer);
                            if (res.Succeeded)
                            {
                                return RedirectToAction("Login", "Access");
                            }

                            TempData["ErrorMessage"] = res.ErrorMessages;

                            TempData.Remove("QuestionnaireModel");
                            return this.RedirectToAction("RegistrationCompleted", new { code = code });
                        }
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = errors.ToString();
                }
            }

            {
                UserClient uc = new UserClient();
                var r = uc.UserCompletedRegistration(code);
                if (r.Succeeded && r.Data) return RedirectToAction("Login");
            }

            ViewBag.UserName = userName;
            ViewBag.Password = password;
            ViewBag.PasswordRepeat = repeatPassword;
            ViewBag.SecurityQuestion = securityQuestion;
            ViewBag.SecurityAnswer = securityAnswer;
            ViewBag.SecurityCodeProvider = SecurityCodeProvider;
            ViewBag.Email = email;
            ViewBag.Mobile = mobile;

            return View();
        }

        public ActionResult RegistrationCompleted()
        {
            return View();
        }

        [HttpGet]
        public JsonResult NameTest(string code, string userName)
        {
            UserClient uc = new UserClient();
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(userName)) errors.Append("The user name cannot be empty");
            else if (userName.Length > 450) errors.Append("The username cannot exceed 450 characters");
            else if (!uc.UserNameAvailable(userName, code)) errors.Append("The user name is not available");

            if (errors.Length == 0)
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = false, message = errors.ToString() }, JsonRequestBehavior.AllowGet);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl, string role, string patientId = null)
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                var sessionData = WcfUserClientSession.Current.Config.SessionData<PCHI.Model.Security.ClientSessionDetails>();
                if ((sessionData.SelectedRole == "PatientProxy") && string.IsNullOrWhiteSpace(returnUrl))
                {
                    returnUrl = "~/Patient/Index?patientId=" + patientId;
                }
                else
                {
                    string controllerRole = "";
                    switch (role)
                    {
                        case "Administrator":
                            controllerRole = "Admin";
                            break;
                        case "PatientProxy":
                            controllerRole = "Patient";
                            break;
                        case "Telephonist":
                            controllerRole = "Telephonist";
                            break;
                        case "Practitioner":
                            controllerRole = "Practitioner";
                            break;
                        case "Researcher":
                            controllerRole = "Researcher";
                            break;
                    }
                    returnUrl = "~/" + controllerRole + "/Index";
                }
            }

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        #endregion
    }
}