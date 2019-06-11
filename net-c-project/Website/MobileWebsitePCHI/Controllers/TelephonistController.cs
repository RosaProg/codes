using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteSupportLibrary.Models;
using WebsiteSupportLibrary.Models.Attributes;

namespace Website.Controllers
{
    public class TelephonistController : Controller
    {
        // GET: Telephonist
        [AnyRole("Telephonist")]
        public ActionResult Index()
        {
            return View();
        }

        [AllRoles("Telephonist")]
        public ActionResult Telephonist()
        {
            if (TempData["Patients"] != null)
            {
                return View((List<PatientModel>)TempData["Patients"]);
            }
            return View();
        }

        /// <summary>
        /// Inserts the given patient details to the records
        /// </summary>
        /// <param name="model">This variable contains all the details for the new patient</param>
        /// <returns>The view with a proper message indicating if everything was successfully done or if something error happened</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllRoles("Telephonist")]
        public ActionResult CreatePatient(PatientDetails model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    PatientClient c = new PatientClient();
                    var result = c.CreatePatient(model.ExternalId, model.Email, model.Email, model.Title, model.FirstName, model.LastName, model.DateOfBirth.Value, model.PhoneNumber);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Patient", new { id = result.Data });
                    }
                    else
                    {
                        ModelState.AddModelError("", result.ErrorMessages);
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "An error has occurred trying to create your account. Please try again");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AnyRole("Telephonist")]
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
        /// Shows a view to insert a new patient into the records
        /// </summary>
        /// <returns>Returns the proper view</returns>
        [AnyRole("Telephonist")]
        public ActionResult CreatePatient()
        {
            return View();
        }
    }
}