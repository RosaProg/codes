using PCHI.Model.Episodes;
using PCHI.Model.Questionnaire;
using PCHI.Model.Questionnaire.Styling.Presentation;
using PCHI.Model.Questionnaire.Response;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Questionnaire;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Users;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using WebsiteSupportLibrary.Controls;
using Website.Models;
using DSPrima.WcfUserSession.ClientSession;
using PCHI.Model.Security;
using WebsiteSupportLibrary.Models.Attributes;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Service;
using WebsiteSupportLibrary.Models;


namespace Website.Controllers
{

    public class PatientController : Controller
    {

        private ProClient proClient = new ProClient();
        private QuestionnaireClient questionnaireClient = new QuestionnaireClient();
        private QuestionnaireFormatClient questionnaireFormatClient = new QuestionnaireFormatClient();
        private PatientEpisodeClient userQuestionnaireClient = new PatientEpisodeClient();
        private ServiceDetailsClient serviceDetailsClient = new ServiceDetailsClient();
        private PatientClient patientClient = new PatientClient();
        //
        // GET: /Patient/
        [AllRoles("PatientProxy")]
        public ActionResult Index(string patientId, bool closing = false)
        {
            Dictionary<Episode, List<QuestionnaireUserResponseGroup>> result = this.userQuestionnaireClient.GetOutstandingQuestionnairesForPatient(patientId).EpisodeQuestionnaires;

            Dictionary<Episode, List<QuestionnaireUserResponseGroup>> tmp = result == null ? new Dictionary<Episode, List<QuestionnaireUserResponseGroup>>() : result.ToDictionary(r => r.Key, r => r.Value.Where(g => !g.Completed).ToList());

            tmp = tmp.OrderByDescending(e => e.Key.DateCreated).ToDictionary(d=>d.Key, d=>d.Value);

            foreach (KeyValuePair<Episode, List<QuestionnaireUserResponseGroup>> e in tmp)
            {
                List<QuestionnaireUserResponseGroup> value = e.Value.OrderByDescending(g => g.DueDate).OrderBy(g => g.Completed).ToList();
                e.Value.Clear();
                e.Value.AddRange(value);
            }

            ViewBag.Outstanding = tmp;
            Dictionary<Episode, List<QuestionnaireUserResponseGroup>> data = result == null ? new Dictionary<Episode, List<QuestionnaireUserResponseGroup>>() : result.ToDictionary(r => r.Key, r => r.Value.Where(g => g.Completed).ToList());
            
            /*List<QuestionnaireUserResponseGroup> noEpisodes = data.Where(r => r.Key.Condition == null).SelectMany(r => r.Value).ToList();
            noEpisodes.OrderBy(g => g.Completed).ThenByDescending(g => g.DateTimeCompleted);
            ViewBag.NoEpisodes = noEpisodes;
            */
            data = data.OrderByDescending(e => e.Key.DateCreated).ToDictionary(d=>d.Key, d=>d.Value);
            //Dictionary<Episode, List<QuestionnaireUserResponseGroup>> tmp2 = data.Where(r => r.Key.Condition != null).ToDictionary(r => r.Key, r => r.Value);
            foreach (KeyValuePair<Episode, List<QuestionnaireUserResponseGroup>> e in data)
            {
                List<QuestionnaireUserResponseGroup> value = e.Value.OrderByDescending(g => g.DateTimeCompleted).OrderBy(g => g.Completed).ToList();
                e.Value.Clear();
                e.Value.AddRange(value);
            }

            ViewBag.Completed = data;

            if (!closing)
            {
                var resultWithPatientDetails = patientClient.GetDetailsForPatient(patientId);
                if (resultWithPatientDetails.Succeeded)
                {
                    if (resultWithPatientDetails.PatientDetails.ShareDataForQualityAssurance == null || resultWithPatientDetails.PatientDetails.ShareDataWithResearcher == null)
                    {
                        ViewBag.ShowPopUp = "SelectConfidentialityScope";
                    }
                }
            }
            
            return View();
        }

        /// <summary>
        /// Gets all the details for the selected patient and allows the edition of them
        /// </summary>
        /// <param name="patientId">Id of the patient which details are going to be changed</param>
        /// <returns>The proper view with the data about the patient's details</returns>
        [AllRoles("PatientProxy")]
        public ActionResult ChangePatientDetails(string patientId)
        {
            ViewBag.PatientId = patientId;
            var result = patientClient.GetDetailsForPatient(patientId);
            if (result.Succeeded) 
            {
                return View(result.PatientDetails);
            }
            else
            {
                ViewBag.ErrorMessage = result.ErrorMessages;
                ViewBag.ErrorRetrieving = "Error";
            }
            return View();
        }

        /// <summary>
        /// Function that receives the post data when user submits the form, manage this information and updates the patient's details
        /// </summary>
        /// <param name="patientDetails">Varibal containing the new patient details</param>
        /// <param name="patientId">Id of the patient which details are going to be changed</param>
        /// <returns>Return a message to the view with the result of this operation</returns>
        [HttpPost]
        public ActionResult ChangePatientDetails(PatientDetails patientDetails, string patientId)
        {
            var resultWithPatientDetails = patientClient.GetDetailsForPatient(patientId);
            if (!resultWithPatientDetails.Succeeded)
            {
                TempData["ErrorMessage"] = resultWithPatientDetails.ErrorMessages;
            }
            else
            {
                resultWithPatientDetails.PatientDetails.Email = (String.IsNullOrWhiteSpace(patientDetails.Email)) ? resultWithPatientDetails.PatientDetails.Email : patientDetails.Email;
                resultWithPatientDetails.PatientDetails.ShareDataForQualityAssurance = patientDetails.ShareDataForQualityAssurance;
                resultWithPatientDetails.PatientDetails.ShareDataWithResearcher = patientDetails.ShareDataWithResearcher;
                var result = patientClient.SavePatientDetails(patientId, resultWithPatientDetails.PatientDetails);
                if (!result.Succeeded)
                {
                    ViewBag.ErrorMessage = result.ErrorMessages;
                }
                else
                {
                    ViewBag.NotificationMessage = "Your patient details have been saved";
                }
            }
           
            return View(patientDetails);
        }


        /// <summary>
        /// When the patient hasn't selected the type of confidentiality 
        /// manage to show a popup for the user to select what kind of scope they want for their information
        /// </summary>
        /// <param name="patientId">Id of the patient which their confidentiality variables are going to be set</param>
        /// <param name="qualityAssurance">Value for the quality assurance of this patient</param>
        /// <param name="withResearcher">Value for the with researcher of this patient</param>
        /// <returns>The index view of the patient for them to continue as normally</returns>
        [AllRoles("PatientProxy")]
        public ActionResult SelectConfidentialityScope(string patientId, string qualityAssurance, string withResearcher)
        {
            var resultWithPatientDetails = patientClient.GetDetailsForPatient(patientId);
            if (!resultWithPatientDetails.Succeeded)
            {
                TempData["ErrorMessage"] = resultWithPatientDetails.ErrorMessages;
            }
            else
            {
                if (qualityAssurance != null && withResearcher != null)
                {
                    resultWithPatientDetails.PatientDetails.ShareDataForQualityAssurance = (qualityAssurance == "Yes") ? true : false;
                    resultWithPatientDetails.PatientDetails.ShareDataWithResearcher = (withResearcher == "Yes") ? true : false; ;
                    var result = patientClient.SavePatientDetails(patientId, resultWithPatientDetails.PatientDetails);
                    if (result.Succeeded)
                    {
                        TempData["NotificationMessage"] = "Your confideality scope has been updated";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = result.ErrorMessages;
                    }
                }
            }
            return RedirectToAction("Index", new { patientId = patientId, closing = true });
        }

        /// <summary>
        /// This is the ActionResult that is called when the submit button is clicked with some of the responses
        /// </summary>
        /// <param name="formCollection">Contains all the information of the partially submitted PRO</param>
        /// <returns>>Call the proper view</returns>
        [HttpPost]
        [WebsiteSupportLibrary.ControllerHelpers.ControllersHelper.MultipleButton(Name = "action", Argument = "Save")]
        [AllRoles("PatientProxy")]
        public ActionResult Save(string patientId, FormCollection formCollection)
        {
            this.SaveQuestionnaireResponses(formCollection, false);
            //return Redirect("ProResult?userId=" + ids[0] + "&QuestionnaireId=" + ids[1]);
            return Redirect("~/Patient/QuestionnaireSaved?patientId=" + patientId + "submitted=0");
        }

        /// <summary>
        /// This is the ActionResult that is called when the submit button is clicked with all the responses 
        /// </summary>
        /// <param name="formCollection">Contains all the information of the submitted PRO</param>
        /// <returns>Call the proper view</returns>
        [HttpPost]
        [WebsiteSupportLibrary.ControllerHelpers.ControllersHelper.MultipleButton(Name = "action", Argument = "SaveSubmit")]
        [AllRoles("PatientProxy")]
        public ActionResult SaveSubmit(string patientId, FormCollection formCollection)
        {
            this.SaveQuestionnaireResponses(formCollection, true);
            //return Redirect("ProResult?userId=" + ids[0] + "&QuestionnaireId=" + ids[1]);
            return Redirect("~/Patient/QuestionnaireSaved?patientId=" + patientId + "submitted=1");
        }

        /// <summary>
        /// Saves the patient's response when the chat mode of the questionnaire was used
        /// </summary>
        /// <returns>If the user completed the questionnaire returns a view with their results
        /// if not, returns a view indicating that the questionnaire was saved</returns>
        [AllRoles("PatientProxy")]
        public ActionResult SaveChatMode()
        {
            if (TempData.ContainsKey("QuestionnaireModel"))
            {

                QuestionnaireModel model = TempData["QuestionnaireModel"] as QuestionnaireModel;
                NameValueCollection querystring = TempData["QueryString"] as NameValueCollection;
                Dictionary<string, string> data = new Dictionary<string, string>();
                foreach (WebsiteSupportLibrary.Models.QuestionnaireItem item in model.Items)
                {
                    if (item.AnswerNames != null) // Indicat the item is not answered
                    {
                        for (int e = 0; e < item.AnswerNames.Count; e++)
                        {
                            data.Add(item.AnswerNames.ElementAt(e), item.AnswerValues.ElementAt(e));
                        }
                    }
                }
                data.Add("questionnaireId", model.QuestionnaireId);
                data.Add("groupId", model.GroupId);
                data.Add("anonymous", model.Anonymous);

                bool completed = ((bool?)TempData["Submit"]).Value;
                var result = Support.SaveQuestionnaireResponses(data, completed);

                if (model.IsPro && completed)
                {
                    return this.RedirectToAction("QuestionnaireResults", "Patient", new RouteValueDictionary() { { "patientId", querystring["patientId"] }, { "episodeId", querystring["episodeId"] }, { "questionnaireId", model.QuestionnaireId } });
                }
                else
                {
                    return this.RedirectToAction("QuestionnaireSaved", new RouteValueDictionary() { { "patientId", querystring["patientId"] }, { "submitted", (completed ? "1" : "0") } });
                }
            }

            throw new NotImplementedException();
            // TODO Return to error page
        }

        private OperationResult SaveQuestionnaireResponses(FormCollection formCollection, bool completed)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (string key in formCollection)
            {
                dic.Add(key, formCollection[key]);
            }

            return Support.SaveQuestionnaireResponses(dic, completed);
        }

        /*
        /// <summary>
        /// Function that saves the answer of the submitted PRO
        /// </summary>
        /// <param name="formCollection">Contains all the information of the PRO</param>
        /// <param name="completed">Indicates if the PRO is fully answered or not</param>
        private OperationResult SaveQuestionnaireResponses(Dictionary<string, string> formCollection, bool completed)
        {
            ViewBag.Message = "Your PRO responses have been submitted.";
            int questionnaireId = formCollection.ContainsKey("questionnaireId") ? Convert.ToInt32(formCollection["questionnaireId"]) : 0;
            string anonymous = formCollection.ContainsKey("anonymous") ? formCollection["anonymous"] : null;
            int groupId = formCollection.ContainsKey("groupId") ? Convert.ToInt32(formCollection["groupId"]) : 0;

            List<QuestionnaireResponse> questionnaireResponses = ResponseParser.ParseResponses(formCollection);
            if (!string.IsNullOrWhiteSpace(anonymous))
            {
                return this.userQuestionnaireClient.SaveAnonymouseQuestionnaireResponse(anonymous, questionnaireId, groupId, completed, questionnaireResponses);
            }
            else
            {
                return this.userQuestionnaireClient.SaveQuestionnaireResponseForCurrentUser(questionnaireId, groupId, completed, questionnaireResponses);
            }
        }*/

        /// <summary>
        /// ActionResult called when the patient has more than one patient assigned, in order to indicate which one wants to select for this session
        /// </summary>
        /// <returns>The view to display</returns>
        [AllRoles("PatientProxy")]
        public ActionResult SelectPatient()
        {
            UserClient uc = new UserClient();
            var patientsForCurrentUser = uc.GetPatientsForUser();

            List<PatientModel> data = new List<PatientModel>();
            PatientClient client = new PatientClient();

            foreach (var u in patientsForCurrentUser.StringDictionary)
            {
                OperationResultAsUserDetails result = client.GetDetailsForPatient(u.Key);
                if (!result.Succeeded)
                {
                    TempData["ErrorMessage"] = result.ErrorMessages;
                }
                else
                {
                    data.Add(new PatientModel() { Id = result.PatientDetails.Id, Firstname = result.PatientDetails.FirstName, LastName = result.PatientDetails.LastName, Email = result.PatientDetails.Email, DateOfBirth = result.PatientDetails.DateOfBirth, mobileNumber = result.PatientDetails.PhoneNumber, ExternalId = result.PatientDetails.ExternalId });
                }
            }
            TempData["Patients"] = data;

            return View();
        }

        /// <summary>
        /// Loads a Pro to display
        /// </summary>
        /// <param name="PRO">The name of the Pro</param>
        /// <param name="Display">The name of the format</param>
        /// <param name="Anonymous">The anonymous access string (either this or the previous 2 must be here)</param>
        /// <returns>The view to display</returns>
        [AllRoles("PatientProxy")]
        public ActionResult PatientQuestionnaireClassic(string patientId, string PRO, int? episodeId)
        {
            Questionnaire q = null;
            Format f;
            QuestionnaireUserResponseGroup group;

            try
            {
                var result = this.userQuestionnaireClient.GetQuestionnaireForPatient(patientId, PRO, episodeId, Platform.Classic);
                q = result.Questionnaire;
                f = result.Format;
                group = result.QuestionnaireUserResponseGroup;
                ViewBag.html += new QuestionnaireFormatRenderer(Platform.Classic).GenerateUi(q, f, group.Responses);
                ViewBag.html += "<input type=\"hidden\" name=\"questionnaireId\" value=" + q.Id + "><input type=\"hidden\" name=\"groupId\" value=\"" + group.Id + "\">";
            }
            catch (Exception)
            {
                if (q == null)
                {
                    ViewBag.ErrorMessage = "There is currently no questionnaire available for you with this name";
                }
                else
                {
                    ViewBag.ErrorMessage = "An error has occurred trying to retrieve the Pro. Please go back and try again.";
                }
            }

            return View();
        }

        /// <summary>
        /// Loads a Pro to display with the chat mode
        /// </summary>
        /// <param name="PRO">The name of the Pro</param>        
        /// <param name="episodeId">The optional episode Id to use</param>
        /// <returns>The view to display</returns>
        [AllRoles("PatientProxy")]
        public ActionResult PatientQuestionnaireChat(string patientId, string PRO, int? episodeId)
        {
            Questionnaire q = null;
            Format f;
            QuestionnaireUserResponseGroup group;

            try
            {
                var result = this.userQuestionnaireClient.GetQuestionnaireForPatient(patientId, PRO, episodeId, Platform.Chat);
                q = result.Questionnaire;
                f = result.Format;
                group = result.QuestionnaireUserResponseGroup;

                QuestionnaireFormatRenderer renderer = new QuestionnaireFormatRenderer(Platform.Chat);
                string html = renderer.GenerateUi(q, f, group.Responses);

                ViewBag.html += "<input type=\"hidden\" name=\"questionnaireId\" value=" + q.Id + "><input type=\"hidden\" name=\"groupId\" value=\"" + group.Id + "\">";
                renderer.Model.QuestionnaireId = q.Id.ToString();
                renderer.Model.GroupId = group.Id.ToString();
                renderer.Model.Anonymous = string.Empty;

                // Format properties
                renderer.Model.ShowProgressBar = !f.HasAttribute(QuestionnaireFormatAttributes.HideProgressBar);
                renderer.Model.CanSavePartial = true;

                foreach (WebsiteSupportLibrary.Models.QuestionnaireItem item in renderer.Model.Items)
                {
                    item.ResponsePanel.CanSkip = true;
                    item.ResponsePanel.PreventEdit = true;
                }
                return View(renderer.Model);
            }
            catch (Exception)
            {
                if (q == null)
                {
                    ViewBag.ErrorMessage = "There is currently no questionnaire available for you with this name";
                }
                else
                {
                    ViewBag.ErrorMessage = "An error has occurred trying to retrieve the Pro. Please go back and try again.";
                }
            }

            return View();
        }

        /// <summary>
        /// Called when the user saved a questionnaire
        /// </summary>
        /// <param name="patientId">Id of patient which is saving the questionnaire</param>
        /// <param name="submitted">Indicates if the questionnaire was completed or not</param>
        /// <returns>The view letting know the user that the questionnaire has been saved</returns>
        [AllRoles("PatientProxy")]
        public ActionResult QuestionnaireSaved(string patientId, string submitted)
        {
            if (submitted == "1") ViewBag.Submitted = true;
            else ViewBag.Submitted = false;
            return View();
        }

        /// <summary>
        /// Loads all the results for the selected patient with given criteria
        /// </summary>
        /// <param name="patientId">Id of the patient which results are going to be retrieved</param>
        /// <param name="episodeId">Id of the episode from where the results are going to be taken</param>
        /// <param name="questionnaireId">Id of the questionnaire from where the results are going to be taken</param>
        /// <returns>The view with all data retrieved from the database</returns>
        [AllRoles("PatientProxy")]
        public ActionResult QuestionnaireResults(string patientId, int? episodeId, int? questionnaireId)
        {
            Dictionary<Episode, Dictionary<string, Dictionary<string, object>>> data;
            Dictionary<Episode, Dictionary<string, List<ProDomainResultSet>>> sets = ProResults.GetProDomainResultSetForQuestionnaireAndPatient(patientId, episodeId.HasValue ? episodeId.Value : 0, questionnaireId.HasValue ? questionnaireId.Value : 0, out data);
            ViewBag.Data = data;
            ViewBag.howManyColumns = 5;
            return View(sets);
        }

        //
        // GET: /MyQuestionnaires/
        [AllRoles("PatientProxy")]
        public ActionResult MyQuestionnaires(string patientId)
        {
            var result = this.userQuestionnaireClient.GetOutstandingQuestionnairesForPatient(patientId);
            Dictionary<Episode, List<QuestionnaireUserResponseGroup>> data = result.EpisodeQuestionnaires;
            List<QuestionnaireUserResponseGroup> noEpisodes = data.Where(r => r.Key.Condition == null).SelectMany(r => r.Value).ToList();
            noEpisodes.OrderBy(g => g.Completed).ThenByDescending(g => g.DateTimeCompleted);
            ViewBag.General = noEpisodes;

            Dictionary<Episode, List<QuestionnaireUserResponseGroup>> tmp = data.Where(r => r.Key.Condition != null).ToDictionary(r => r.Key, r => r.Value);
            foreach (KeyValuePair<Episode, List<QuestionnaireUserResponseGroup>> e in tmp)
            {
                List<QuestionnaireUserResponseGroup> value = e.Value.OrderByDescending(g => g.DateTimeCompleted).OrderBy(g => g.Completed).ToList();
                e.Value.Clear();
                e.Value.AddRange(value);
            }

            ViewBag.Outstanding = tmp;
            return View();
        }
    }
}