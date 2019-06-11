using PCHI.Model.Questionnaire.Response;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebsiteSupportLibrary.Models;

namespace WebsiteSupportLibrary.Controls
{
    public class Support
    {
        /// <summary>
        /// Function that saves the answer of the submitted PRO
        /// </summary>
        /// <param name="formCollection">Contains all the information of the PRO</param>
        /// <param name="completed">Indicates if the PRO is fully answered or not</param>
        public static OperationResult SaveQuestionnaireResponses(Dictionary<string, string> formCollection, bool completed)
        {
            PatientEpisodeClient uec = new PatientEpisodeClient();
            //ViewBag.Message = "Your PRO responses have been submitted.";
            int questionnaireId = formCollection.ContainsKey("questionnaireId") ? Convert.ToInt32(formCollection["questionnaireId"]) : 0;
            string anonymous = formCollection.ContainsKey("anonymous") ? formCollection["anonymous"] : null;
            int groupId = formCollection.ContainsKey("groupId") ? Convert.ToInt32(formCollection["groupId"]) : 0;

            List<QuestionnaireResponse> questionnaireResponses = ResponseParser.ParseResponses(formCollection);
            if (!string.IsNullOrWhiteSpace(anonymous))
            {
                return uec.SaveAnonymouseQuestionnaireResponse(anonymous, questionnaireId, groupId, completed, questionnaireResponses);
            }
            else
            {
                return uec.SaveQuestionnaireResponseForCurrentUser(questionnaireId, groupId, completed, questionnaireResponses);
            }
        }
    }
}