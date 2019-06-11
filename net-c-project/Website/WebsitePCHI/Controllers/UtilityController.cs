using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Users;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PCHI.Model.Episodes;
using PCHI.Model.Questionnaire.Response;

namespace Website.Controllers
{
    public class UtilityController : Controller
    {
        private PatientEpisodeClient userQuestionnaireClient = new PatientEpisodeClient();

	}
}