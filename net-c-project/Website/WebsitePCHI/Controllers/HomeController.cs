using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using PCHI.Model.Questionnaire;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Questionnaire;
using PCHI.Model.Questionnaire.Pro;
using Website.Models;
using PCHI.Model.Questionnaire.Styling.Presentation;
using PCHI.Model.Questionnaire.Response;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Users;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model;
using WebsiteSupportLibrary.Controls;
using System.Collections.Specialized;
using WebsiteSupportLibrary.Models;

namespace Website.Controllers
{

    public class HomeController : Controller
    {

        private ProClient proClient = new ProClient();
        private QuestionnaireClient questionnaireClient = new QuestionnaireClient();
        private QuestionnaireFormatClient questionnaireFormatClient = new QuestionnaireFormatClient();
        private PatientEpisodeClient userQuestionnaireClient = new PatientEpisodeClient();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult QuestionnaireSaved(int submitted)
        {
            if (submitted != 0)
            {
                ViewBag.Submitted = true;
            }
            else
            {
                ViewBag.Submitted = false;
            }

            return View();
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

        /// <summary>
        /// This is the ActionResult that is called when the submit button is clicked with some of the responses
        /// </summary>
        /// <param name="formCollection">Contains all the information of the partially submitted PRO</param>
        /// <returns>>Call the proper view</returns>
        [HttpPost]
        [WebsiteSupportLibrary.ControllerHelpers.ControllersHelper.MultipleButton(Name = "action", Argument = "Save")]
        public ActionResult Save(FormCollection formCollection)
        {
            this.SaveQuestionnaireResponses(formCollection, false);
            //return Redirect("ProResult?userId=" + ids[0] + "&QuestionnaireId=" + ids[1]);
            return Redirect("~/Home/QuestionnaireSaved?submitted=0");
        }

        /// <summary>
        /// This is the ActionResult that is called when the submit button is clicked with all the responses 
        /// </summary>
        /// <param name="formCollection">Contains all the information of the submitted PRO</param>
        /// <returns>Call the proper view</returns>
        [HttpPost]
        [WebsiteSupportLibrary.ControllerHelpers.ControllersHelper.MultipleButton(Name = "action", Argument = "SaveSubmit")]
        public ActionResult SaveSubmit(FormCollection formCollection)
        {
            this.SaveQuestionnaireResponses(formCollection, true);
            //return Redirect("ProResult?userId=" + ids[0] + "&QuestionnaireId=" + ids[1]);
            return Redirect("~/Home/QuestionnaireSaved?submitted=1");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult SaveChatMode()
        {
            if (TempData.ContainsKey("QuestionnaireModel"))
            {
                QuestionnaireModel model = TempData["QuestionnaireModel"] as QuestionnaireModel;
                NameValueCollection querystring = TempData["QueryString"] as NameValueCollection;
                //Dictionary<string, string> data = (from x in model.Items where x.AnsweredStatus == ItemAnsweredStatus.Answered.ToString() select new { key = x.AnswerNames, value = x.AnswerValues }).ToDictionary(d => d.key, d => d.value);  --> before array
                Dictionary<string, string> data = new Dictionary<string, string>();
                foreach (WebsiteSupportLibrary.Models.QuestionnaireItem item in model.Items)
                {
                    for (int e = 0; e < item.AnswerNames.Count; e++)
                    {
                        data.Add(item.AnswerNames.ElementAt(e), item.AnswerValues.ElementAt(e));
                    }
                }
                data.Add("questionnaireId", model.QuestionnaireId);
                data.Add("groupId", model.GroupId);
                data.Add("anonymous", model.Anonymous);

                bool completed = ((bool?)TempData["Submit"]).Value;
                var result = Support.SaveQuestionnaireResponses(data, completed);

                if (model.IsPro && completed)
                {
                    // TODO Redirect to result pagestring userId, int questionnaireId
                    //return this.RedirectToAction("QuestionnaireResults", "Patient", new RouteValueDictionary() { { "episodeId", querystring["episodeId"] }, { "questionnaireId", model.QuestionnaireId } });
                    return Redirect("~/Home/QuestionnaireSaved?submitted=0");
                }
                else
                {
                    //return this.RedirectToAction("QuestionnaireSaved", new RouteValueDictionary() { { "submitted", (completed ? "1" : "0") } });
                    return Redirect("~/Home/QuestionnaireSaved?submitted=1");
                }
            }

            throw new NotImplementedException();
            // TODO Return to error page
        }

        /// <summary>
        /// Loads a Pro to display
        /// </summary>
        /// <param name="PRO">The name of the Pro</param>
        /// <param name="Display">The name of the format</param>
        /// <param name="Anonymous">The anonymous access string (either this or the previous 2 must be here)</param>
        /// <returns>The view to display</returns>
        public ActionResult AnonQuestionnaireClassic(string PRO, string Display, string Anonymous)
        {
            Questionnaire q = null;
            Format f;
            QuestionnaireUserResponseGroup group;

            try
            {
                if (Anonymous != null)
                {
                    var result = this.userQuestionnaireClient.GetQuestionnaireAnonymous(Anonymous, Platform.Classic);
                    q = result.Questionnaire;
                    f = result.Format;
                    group = result.QuestionnaireUserResponseGroup;
                    ViewBag.html = "<input type=\"hidden\" name=\"anonymous\" value=" + Anonymous + ">";

                    ViewBag.html += new QuestionnaireFormatRenderer(Platform.Classic).GenerateUi(q, f, group.Responses);
                    ViewBag.html += "<input type=\"hidden\" name=\"questionnaireId\" value=" + q.Id + "><input type=\"hidden\" name=\"groupId\" value=\"" + group.Id + "\">";
            
                }

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
        /// Loads a Pro to display
        /// </summary>
        /// <param name="PRO">The name of the Pro</param>
        /// <param name="Display">The name of the format</param>
        /// <param name="Anonymous">The anonymous access string (either this or the previous 2 must be here)</param>
        /// <returns>The view to display</returns>
        public ActionResult AnonQuestionnaireChat(string PRO, string Display, string Anonymous)
        {
            Questionnaire q = null;
            Format f;
            QuestionnaireUserResponseGroup group;

            try
            {
                if (Anonymous != null)
                {
                    var result = this.userQuestionnaireClient.GetQuestionnaireAnonymous(Anonymous, Platform.Chat);
                    q = result.Questionnaire;
                    f = result.Format;
                    group = result.QuestionnaireUserResponseGroup;
                    ViewBag.html = "<input type=\"hidden\" name=\"anonymous\" value=" + Anonymous + ">";


                    QuestionnaireFormatRenderer renderer = new QuestionnaireFormatRenderer(Platform.Chat);
                    string html = renderer.GenerateUi(q, f, group.Responses);

                    //ViewBag.html += 
                    ViewBag.html += "<input type=\"hidden\" name=\"questionnaireId\" value=" + q.Id + "><input type=\"hidden\" name=\"groupId\" value=\"" + group.Id + "\">";
                    renderer.Model.QuestionnaireId = q.Id.ToString();
                    renderer.Model.GroupId = group.Id.ToString();
                    renderer.Model.Anonymous = Anonymous;
                    return View(renderer.Model);
                }

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

        // GET: Error
        /// <summary>
        /// Action result to manage all the errors for the user
        /// </summary>
        /// <returns>The error page</returns>
        public ActionResult Error()
        {
            System.Web.Mvc.HandleErrorInfo exc = new HandleErrorInfo(new Exception("Error"), "Home", "Error");
            return View("Error", exc);
        }
    }
}