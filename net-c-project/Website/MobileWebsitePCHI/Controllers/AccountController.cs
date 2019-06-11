using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Service;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteSupportLibrary.Models;

namespace Website.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        //
        // GET: /Account/Manage

        public ActionResult Manage(string patientId, ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            //ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage", new { patientId = patientId });
            return View();
        }

        //
        // GET: /Account/Manager
        public ActionResult ChangeSettings(string patientId, bool resendTwoFactorAuthentication = false)
        {
            ServiceDetailsClient client = new ServiceDetailsClient();
            List<string> providers = client.GetTwoStageAuthenticationProviders().Strings;
            client.Close();
            UserClient userClient = new UserClient();
            var result = userClient.GetDetailsForCurrentUser();
            ViewBag.PatientId = patientId;
            if (result.Succeeded)
            {
                ViewBag.Providers = new SelectList(providers);
                if (resendTwoFactorAuthentication)
                {
                    ViewBag.ShowPopUp = "PopUpAuthenticationCodeField";
                    ViewBag.ActionName = "ChangeSettings";
                    ViewBag.ControllerName = "Account";
                    return View();
                }
                else
                {
                    return View(result.UserDetails);
                }
            }
            else
            {
                ViewBag.ErrorMessage = result.ErrorMessages;
                ViewBag.ErrorRetrieving = "Error";
            }
            
            return View();
        }

        //
        // Post: /Account/ChangeSettings
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeSettings(UserDetails settings, string patientId, string authenticationCode, string password, string resendCode)
        {
            UserClient userClient = new UserClient();
            ServiceDetailsClient client = new ServiceDetailsClient();
            List<string> providers = client.GetTwoStageAuthenticationProviders().Strings;
            client.Close();
            ViewBag.Providers = new SelectList(providers);
            var currentSettings = userClient.GetDetailsForCurrentUser();

            if (currentSettings.UserDetails.TwoFactorProvider == null)
            {
                if (password != null)
                {
                    var result = userClient.SaveCurrentUserSettings(settings, password);
                    userClient.Close();
                    //Verify user's password and save modifications
                    if (!result.Succeeded)
                    {
                        ViewBag.ErrorMessage = result.ErrorMessages;
                    }
                    else
                    {
                        ViewBag.NotificationMessage = "Your settings have been saved";
                    }
                }
                else
                {
                    ViewBag.ShowPopUp = "PopUpAuthenticationPassword";
                    ViewBag.ActionName = "ChangeSettings";
                    ViewBag.ControllerName = "Account";
                }
            }
            else
            {
                //Verify second step
                if (authenticationCode == null || authenticationCode=="")
                {
                    if (password != null)
                    {
                        var result = userClient.SaveCurrentUserSettings(settings, password);
                        userClient.Close();
                        //Verify user's password and save modifications
                        if (!result.Succeeded)
                        {
                            ViewBag.ErrorMessage = result.ErrorMessages;
                        }
                        else
                        {
                            ViewBag.NotificationMessage = "Your settings have been saved";
                        }
                    }
                    else
                    {
                        if (userClient.SendTwoStageAuthenticationForTest(currentSettings.UserDetails.TwoFactorProvider, null).Succeeded)
                        {
                            ViewBag.ShowPopUp = "PopUpAuthenticationCodeField";
                            ViewBag.ActionName = "ChangeSettings";
                            ViewBag.ControllerName = "Account";
                            TempData["NotificationMessage"] = "";
                            if (resendCode != "" && resendCode != null)
                            {
                                TempData["NotificationMessage"] = "Code has been send";
                            }
                        }
                    }

                }
                else
                {
                    if (userClient.VerifyTwoStageAuthenticationForTest(authenticationCode, currentSettings.UserDetails.TwoFactorProvider, null).Succeeded)
                    {
                        ViewBag.ShowPopUp = "PopUpAuthenticationPassword";
                        ViewBag.ActionName = "ChangeSettings";
                        ViewBag.ControllerName = "Account";
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "The security code wasn't correct";
                    }
                }
            }

            return View(settings);
        }

        //
        // Get: /Account/ChangePassword                
        public ActionResult ChangePassword(string patientId)
        {
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(LocalPasswordModel model, string patientId, string authenticationCode)
        {
            if (ModelState.IsValid)
            {
                UserClient c = new UserClient();
                if (authenticationCode == null && c.SendAuthenticationCode().Data)
                {
                    ViewBag.ShowPopUp = "PopUpAuthenticationCodeField";
                    ViewBag.ActionName = "ChangeSettings";
                    ViewBag.ControllerName = "Account";
                    return View(model);
                }
                else
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {

                        changePasswordSucceeded = c.ChangePassword(model.OldPassword, model.NewPassword, authenticationCode).Succeeded;
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        TempData["NotificationMessage"] = "Your password have been successfully changed";
                        return RedirectToAction("ChangePassword", new { patientId = patientId, Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult MyAuditTrail(string patientId)
        {
            UserClient uc = new UserClient();
            var result = uc.GetUserAuditTrail();
            if (result.Succeeded)
            {
                return View(result.AuditTrail);
            }

            ViewBag.ErrorMessage = "An error has occurred while retrieving your audit trail data.";
            return View();
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
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