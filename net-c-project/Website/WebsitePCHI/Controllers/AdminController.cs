using PCHI.Model.Episodes;
using PCHI.Model.Questionnaire.Response;
using PCHI.Model.Users;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Questionnaire;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Service;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Users;
using PCHI.WcfServices.InterfaceProxies.Researcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteSupportLibrary.Controls;
using Website.Models;
using WebsiteSupportLibrary.Models.Attributes;

namespace Website.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin    
        [AnyRole("Administrator")]
        public ActionResult Index()
        {
            return View();
        }

        [AllRoles("Administrator"), HttpGet]
        public ActionResult Users()
        {
            UserClient uc = new UserClient();
            var users = uc.FindUsers();

            ServiceDetailsClient sdc = new ServiceDetailsClient();
            var roles = sdc.GetAvailableRoles();

            if (users.Succeeded && roles.Succeeded)
            {
                ViewBag.Roles = roles.Strings.Where(r=>r != "PatientProxy").ToList();
                return View(users.Users);
            }

            ViewBag.ErrorMessage = users.ErrorMessages;

            return View();
        }

        [AllRoles("Administrator"), HttpPost]
        public ActionResult Users(string UserName, bool IsExternalUser, string UserTitle, string FirstName, string LastName, string Email, string PhoneNumber, string ExternalId, string Password, string Role)
        {
            UserClient uc = new UserClient();
            var result = uc.CreateUser(UserName, Password, IsExternalUser, UserTitle, FirstName, LastName, Email, PhoneNumber, ExternalId, Role);
           
            if (result.Succeeded)
            {
                TempData["NotificationMessage"] = "The user has been created";
                return RedirectToAction("Users");
            }
            else
            {
                ViewBag.ErrorMessage = result.ErrorMessages;
                ViewBag.UserName = UserName;
                ViewBag.UserTitle = UserTitle;
                ViewBag.FirstName = FirstName;
                ViewBag.LastName = LastName;
                ViewBag.Email = Email;
                ViewBag.PhoneNumber = PhoneNumber;
                ViewBag.ExternalId = ExternalId;
                ViewBag.Role = Role;
                ViewBag.NewIsOpen = true;
                return this.Users();
            }
        }
    }
}