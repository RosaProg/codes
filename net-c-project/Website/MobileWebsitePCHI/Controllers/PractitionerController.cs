using PCHI.Model.Episodes;
using PCHI.Model.Questionnaire.Response;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Questionnaire;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteSupportLibrary.Controls;
using WebsiteSupportLibrary.Models;
using WebsiteSupportLibrary.Models.Attributes;

namespace Website.Controllers
{
    public class PractitionerController : Controller
    {
        // GET: Practitioner
        [AnyRole("Practitioner")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Shows the current condition of the selected patient
        /// </summary>
        /// <param name="patientId">Id of the patient showing the current condition</param>
        /// <param name="episodeId">Id of the episode which is displayed</param>
        /// <param name="collection">Variables returned from the view</param>
        /// <returns>The proper view all the necessary data for the view to render the correct results of the given patient (and episode)</returns>
        [AllRoles("Practitioner")]
        public ActionResult CurrentCondition(string patientId, int? episodeId, FormCollection collection)
        {
            PatientEpisodeClient uec = new PatientEpisodeClient();
            PatientClient client = new PatientClient();
            var clientResult = client.GetDetailsForPatient(patientId);

            if (!clientResult.Succeeded)
            {
                ViewBag.ErrorMessage += clientResult.ErrorMessages;
            }
            else
            {
                //uec.GetConditionQuestionnaire(episodeId);
                ProClient pro = new ProClient();

                QuestionnaireClient qc = new QuestionnaireClient();
                ViewBag.Questionnaires = qc.GetAllQuestionnairesWithTags().Questionnaires;

                var uecResult = uec.GetEpisodesWithDetailsForPatient(patientId);

                if (!uecResult.Succeeded)
                {
                    ViewBag.ErrorMessage = uecResult.ErrorMessages;
                    return View();
                }

                uecResult.Episodes.Sort((e1, e2) =>
                {
                    if (e1.MileStones.Count == 0 && e2.MileStones.Count == 0) return 0;
                    if (e1.MileStones.Count > 0 && e2.MileStones.Count == 0) return -1;
                    if (e1.MileStones.Count == 0 && e2.MileStones.Count > 0) return 1;

                    DateTime e1D = e1.MileStones.OrderByDescending(e => e.MilestoneDate).First().MilestoneDate;
                    DateTime e2D = e2.MileStones.OrderByDescending(e => e.MilestoneDate).First().MilestoneDate;

                    return e1D.CompareTo(e2D);
                });

                ViewBag.Episodes = uecResult.Episodes;
                ViewBag.Patient = clientResult.PatientDetails;
                ViewBag.PatientId = patientId;

                if (!episodeId.HasValue)
                {
                    episodeId = 0;
                    ViewBag.Episode = new Episode();
                }
                else
                {
                    ViewBag.Episode = uecResult.Episodes.Where(e => e.Id == episodeId.Value).FirstOrDefault();
                }

                //if(episodeId.HasValue)
                {
                    ViewBag.EpisodeId = episodeId.Value;
                    var assignedQuestionnaires = uec.GetAssignedQuestionnairesForEpisode(episodeId.Value).AssignedQuestionnaires;
                    ViewBag.AssignedQuestionnaires = assignedQuestionnaires;
                    Dictionary<Episode, Dictionary<string, Dictionary<string, object>>> data;
                    Dictionary<Episode, Dictionary<string, List<ProDomainResultSet>>> sets = ProResults.GetProDomainResultSetForPatient(patientId, episodeId.Value, out data);
                    ViewBag.DomainResultsSetData = data;
                    ViewBag.DomainResultsSet = sets;
                    ViewBag.howManyColumns = 5;
                }
            }

            return View();
        }

        /// <summary>
        /// Shows a list of existing patients
        /// </summary>
        /// <returns>The view with the data from all the existing patients</returns>
        [AllRoles("Practitioner")]
        public ActionResult Physician()
        {
            if (TempData["Patients"] != null)
            {
                return View((List<PatientModel>)TempData["Patients"]);
            }

            return View();
        }

        [AnyRole("Practitioner")]
        public ActionResult Patient(string patientId, PatientDetails details, string externalEpisodeId, string Condition, string PractitionerId, DateTime? Date)
        {
            if (string.IsNullOrWhiteSpace(patientId)) return RedirectToAction("SeachPatients");

            if (details != null && !string.IsNullOrWhiteSpace(details.FirstName) && !string.IsNullOrWhiteSpace(details.LastName))
            {
                details.Id = patientId;
                PatientClient pc = new PatientClient();
                var result = pc.SavePatientDetails(patientId, details);
                if (result.Succeeded) return RedirectToAction("Patient", new { Id = patientId });
                ViewBag.ErrorMessage = result.ErrorMessages;
            }
            else if (!string.IsNullOrWhiteSpace(Condition))
            {
                PatientEpisodeClient uec = new PatientEpisodeClient();
                OperationResult result = uec.AssignEpisode(patientId, Condition, Date, externalEpisodeId, PractitionerId);

                if (result.Succeeded) return RedirectToAction("Patient", new { Id = patientId });
                ViewBag.ErrorMessage = result.ErrorMessages;

            }

            PatientClient client = new PatientClient();
            var clientResult = client.GetDetailsForPatient(patientId);
            if (!clientResult.Succeeded)
            {
                ViewBag.ErrorMessage += clientResult.ErrorMessages;
                return View();
            }
            else
            {
                PatientEpisodeClient uec = new PatientEpisodeClient();
                var uecResult = uec.GetEpisodesForPatient(patientId);

                if (!uecResult.Succeeded)
                {
                    ViewBag.ErrorMessage = uecResult.ErrorMessages;
                    return View();
                }

                uecResult.Episodes.Sort((e1, e2) =>
                {
                    if (e1.MileStones.Count == 0 && e2.MileStones.Count == 0) return 0;
                    if (e1.MileStones.Count > 0 && e2.MileStones.Count == 0) return -1;
                    if (e1.MileStones.Count == 0 && e2.MileStones.Count > 0) return 1;

                    DateTime e1D = e1.MileStones.OrderByDescending(e => e.MilestoneDate).First().MilestoneDate;
                    DateTime e2D = e2.MileStones.OrderByDescending(e => e.MilestoneDate).First().MilestoneDate;

                    return e1D.CompareTo(e2D);
                });

                ViewBag.Episodes = uecResult.Episodes;
                ViewBag.PatientId = patientId;

                var ucClient = new UserClient();
                var ucResult = ucClient.GetPractitioners();
                ViewBag.Practitioners = ucResult.StringDictionary;
                return View(clientResult.PatientDetails);
            }
        }

        /// <summary>
        /// Assigns a Questionnaire to an episode
        /// </summary>
        /// <param name="patientId">Id of the patient which the questionnaire will be assigned</param>
        /// <param name="episodeId">Id of the episode for the patient to assign the questionnaire</param>
        /// <param name="collection">Variables returned from the view</param>
        /// <returns></returns>
        [AllRoles("Practitioner")]
        public ActionResult ScheduleQuestionnaire(string patientId, int episodeId, FormCollection collection)
        {
            PatientEpisodeClient uec = new PatientEpisodeClient();
            bool success = true;
            List<string> messages = new List<string>();
            foreach (string key in collection.Keys)
            {
                if (key.StartsWith("questionnaire."))
                {
                    OperationResult result = uec.ScheduleQuestionnaireForEpisode(collection[key], episodeId, string.Empty);
                    if (!result.Succeeded)
                    {
                        success = false;
                        messages.Add(result.ErrorMessages);
                    }

                }
            }

            //OperationResult result = uec.ScheduleQuestionnaireForEpisode(pro, episodeId, schedule);

            if (success) TempData["NotificationMessage"] = "The questionnaires have been assigned";
            else TempData["ErrorMessage"] = "There has been an error assigned the questionnaires. " + messages.Aggregate((s1, s2) => { return s1 + "<br />" + s2; });
            return RedirectToAction("CurrentCondition", new { patientId = patientId, episodeId = episodeId });
        }
    }
}