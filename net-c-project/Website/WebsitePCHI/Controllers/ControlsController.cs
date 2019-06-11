using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteSupportLibrary.Controls;
using WebsiteSupportLibrary.Models;

namespace Website.Controllers
{
    public class ControlsController : Controller
    {
        public ActionResult SearchPatients(string action, string controller, string mandatoryFields)
        {
            ViewBag.SearchParentAction = action;
            ViewBag.SearchParentController = controller;
            ViewBag.MandatoryFields = mandatoryFields;
            return View();
        }

        [HttpPost]
        public ActionResult SearchPatients(string action, string controller, string mandatoryFields, string FirstName, string LastName, DateTime? DateOfBirth, string Email, string PhoneNumber, string ExternalId)
        {
            PatientModel searchModel = new PatientModel() { Firstname = FirstName, LastName = LastName, DateOfBirth = DateOfBirth, Email = Email, mobileNumber = PhoneNumber, ExternalId = ExternalId };
            TempData["SearchModel"] = searchModel;

            bool error = false;
            if (!string.IsNullOrWhiteSpace(mandatoryFields))
            {
                string errorMessage = string.Empty;
                List<string> mandatory = mandatoryFields.ToLower().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (string field in mandatory)
                {
                    switch (field)
                    {
                        case "email":
                            if (string.IsNullOrWhiteSpace(Email))
                            {
                                errorMessage += "Email is mandatory\n";
                                error = true;
                            }

                            break;
                        case "firstname":
                            if (string.IsNullOrWhiteSpace(FirstName))
                            {
                                errorMessage += "First name is mandatory\n";
                                error = true;
                            }

                            break;
                        case "lastname":
                            if (string.IsNullOrWhiteSpace(LastName))
                            {
                                errorMessage += "Last name is mandatory\n";
                                error = true;
                            }

                            break;
                        case "dateofbirth":
                            if (!DateOfBirth.HasValue)
                            {
                                errorMessage += "Date of birth is mandatory\n";
                                error = true;
                            }

                            break;
                        case "mobile":
                            if (string.IsNullOrWhiteSpace(PhoneNumber))
                            {
                                errorMessage += "PhoneNumber is mandatory\n";
                                error = true;
                            }

                            break;
                    }
                }

                if (error) TempData["ErrorMessage"] = errorMessage;
            }

            if (!error)
            {
                PatientClient client = new PatientClient();
                OperationResultAsLists result = client.FindPatient(FirstName, LastName, DateOfBirth, Email, PhoneNumber, ExternalId);

                if (!result.Succeeded)
                {
                    TempData["ErrorMessage"] = result.ErrorMessages;
                }
                else
                {
                    List<PatientModel> data = new List<PatientModel>();
                    foreach (var u in result.Patients)
                    {
                        data.Add(new PatientModel() { Id = u.Id, Firstname = u.FirstName, LastName = u.LastName, Email = u.Email, DateOfBirth = u.DateOfBirth, mobileNumber = u.PhoneNumber, ExternalId = u.ExternalId });
                    }

                    TempData["Patients"] = data;
                }
            }

            return RedirectToAction(action, controller);
        }

        // TODO Test and remove. All submission should be done via Submit and not via Ajax
        /*
        [HttpPost]
        public JsonResult SaveJsonAjax(QuestionnaireModel model)
        {
            return this.SaveJson(model, false);
        }

        [HttpPost]
        public JsonResult SaveJsonSubmitAjax(QuestionnaireModel model)
        {
            return this.SaveJson(model, true);
        }*/

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveJsonSubmit(string modelSubmit, string redirectActionSubmit, string redirectControllerSubmit)
        {
            QuestionnaireModel m = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<QuestionnaireModel>(modelSubmit);

            TempData["QuestionnaireModel"] = m;
            TempData["Submit"] = true;
            TempData["QueryString"] = Request.QueryString;
            return RedirectToAction(redirectActionSubmit, redirectControllerSubmit);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveJson(string modelSave, string redirectActionSave, string redirectControllerSave)
        {            
            QuestionnaireModel m = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<QuestionnaireModel>(modelSave);

            TempData["QuestionnaireModel"] = m;
            TempData["Submit"] = false;
            TempData["QueryString"] = Request.QueryString;
            return RedirectToAction(redirectActionSave, redirectControllerSave);
        }

        // TODO Test and remove. All submission should be done via Submit and not via Ajax
        /*
        private JsonResult SaveJson(QuestionnaireModel model, bool completed)
        {
            //Dictionary<string, string> data = (from x in model.Items where x.AnsweredStatus == ItemAnsweredStatus.Answered.ToString() select new { key = x.AnswerNames, value = x.AnswerValues }).ToDictionary(d => d.key, d => d.value); --> before array
            Dictionary<string, string> data = new Dictionary<string, string>();
            foreach (QuestionnaireItem item in model.Items)
            {
                for (int e = 0; e < item.AnswerNames.Count; e++)
                {
                    data.Add(item.AnswerNames.ElementAt(e), item.AnswerValues.ElementAt(e));
                }
            }
            data.Add("questionnaireId", model.QuestionnaireId);
            data.Add("groupId", model.GroupId);
            data.Add("anonymous", model.Anonymous);
            Support.SaveQuestionnaireResponses(data, completed);

            return Json(new { success = true });

        }*/

        [HttpGet]
        public JsonResult TestTwoStageAuthentication(string provider, string code)
        {
            UserClient uc = new UserClient();
            var result = uc.SendTwoStageAuthenticationForTest(provider, code);
            return Json(new { success = result.Succeeded }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult VerifyTwoStageAuthentication(string code, string token, string provider)
        {
            UserClient uc = new UserClient();
            bool result = uc.VerifyTwoStageAuthenticationForTest(provider, code, token).Succeeded;
            return Json(new { success = result }, JsonRequestBehavior.AllowGet);
        }
    }
}