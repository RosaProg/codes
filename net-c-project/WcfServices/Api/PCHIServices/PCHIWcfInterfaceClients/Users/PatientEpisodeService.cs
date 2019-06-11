using DSPrima.WcfUserSession.Behaviours;
using DSPrima.WcfUserSession.SecurityHandlers;
using PCHI.BusinessLogic;
using PCHI.Model.Messages;
using PCHI.Model.Questionnaire;
using PCHI.Model.Questionnaire.Styling.Presentation;
using PCHI.Model.Questionnaire.Response;
using PCHI.WcfServices.API.PCHIServices.InterfaceClients.Base;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCHI.Model.Episodes;
using PCHI.BusinessLogic.Security;

namespace PCHI.WcfServices.API.PCHIServices.InterfaceClients.Users
{
    /// <summary>
    /// The server site implementation of the IPatientEpisodeService
    /// </summary>
    [WcfUserSessionBehaviour]
    public class PatientEpisodeService : BaseService, IPatientEpisodeService
    {
        /// <summary>
        /// Retrieves a list of a slimmed down version of all the Episodes for the given Patient
        /// </summary>
        /// <param name="patientId">The Id of the patient to get the episodes for</param>
        /// <returns>An OperationResult indicating success or failure and containing list of Episodes</returns>
        public OperationResultAsLists GetEpisodesForPatient(string patientId)
        {
            try
            {
                return new OperationResultAsLists(null) { Episodes = this.handler.UserEpisodeManager.GetEpisodesForPatient(patientId) };
            }
            catch(Exception ex)
            {
                return new OperationResultAsLists(ex);
            }
        }

        /// <summary>
        /// Retrieves a list of Episodes for the given Patient. Includes all the details
        /// </summary>
        /// <param name="patientId">The Id of the patient to get the episodes for</param>
        /// <returns>An OperationResult indicating success or failure and containing list of Episodes</returns>
        public OperationResultAsLists GetEpisodesWithDetailsForPatient(string patientId)
        {
            try
            {
                return new OperationResultAsLists(null) { Episodes = this.handler.UserEpisodeManager.GetEpisodesWithDetailsForPatient(patientId) };
            }
            catch (Exception ex)
            {
                return new OperationResultAsLists(ex);
            }
        }

        /// <summary>
        /// Assigns an Episode to the given Patient
        /// </summary>
        /// <param name="patientId">The Id of the Patient to assign the episode to</param>
        /// <param name="condition">The condition the episode is for</param>
        /// <param name="appointmentDate">The option date an appointment is set</param>
        /// <param name="externalEpisodeId">The external Episode Id</param>
        /// <param name="practitionerId">The Id of the practitioner with whom is the appointment</param>        
        /// <returns>An OperationResult indicating success or failure</returns>
        public OperationResult AssignEpisode(string patientId, string condition, DateTime? appointmentDate, string externalEpisodeId, string practitionerId)
        {
            Exception exception = null;
            try
            {
                Episode e = this.handler.UserEpisodeManager.AssignEpisode(patientId, condition, externalEpisodeId);
                if(appointmentDate != null)
                {
                    this.handler.UserEpisodeManager.AddMileStoneToEpisode(e.Id, "Appointment", appointmentDate.Value, practitionerId);
                }
            }
            catch(Exception ex)
            {
                exception = ex;
            }

            return new OperationResult(exception);
        }

        /// <summary>
        /// Assigns an Episode to the given user
        /// </summary>
        /// <param name="episodeId">The Id of the episode to assign the milestone to</param>
        /// <param name="milestoneName">The name of the milestone to add</param>
        /// <param name="date">The date to add the milesone for</param>
        /// <param name="practitionerId">The Id of the practitioner with whom is the appointment</param>        
        /// <returns>An OperationResult indicating success or failure</returns>
        public OperationResult AddMileStoneToEpisode(int episodeId, string milestoneName, DateTime date, string practitionerId)
        {
            Exception exception = null;
            try
            {
                this.handler.UserEpisodeManager.AddMileStoneToEpisode(episodeId, milestoneName, date, practitionerId);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            return new OperationResult(exception);
        }

        /// <summary>
        /// Saves the given <see cref="QuestionnaireResponse"/>  to the database
        /// Existing answers are overwritten
        /// </summary>
        /// <param name="userId">The Id of the user to get the response for</param>
        /// <param name="questionnaireId">The ID of the questionnaire you want to get the response for</param>
        /// <param name="questionnaireCompleted">Indicates whehter or user has filled in all responses and the Questionnaire is now completed</param>
        /// <param name="responses">The <see cref="QuestionnaireResponse"/> to save</param>
        public OperationResult SaveQuestionnaireResponseForCurrentUser(int questionnaireId, int responseGroupId, bool questionnaireCompleted, List<QuestionnaireResponse> responses)
        {
            if (WcfUserSessionSecurity.Current.User == null) return new OperationResult(this.handler.MessageManager.GetError(ErrorCodes.USER_SESSION_EXPIRED));

            this.handler.UserEpisodeManager.SaveQuestionnaireResponse(questionnaireId, responseGroupId, questionnaireCompleted, responses);
            return new OperationResult(null);
        }

        /// <summary>
        /// Returns all Questionnaire Responses for a given Patient and Questionnaire for all times this Questionnaire was answered by this Patient
        /// </summary>
        /// <param name="patientId">The Id of the Patient to get the response for</param>
        /// <param name="questionnaireId">The ID of the questionnaire you want to get the response for</param>
        /// <returns>A list of questionnaires</returns>
        public List<QuestionnaireUserResponseGroup> GetAllQuestionnaireResponsesForPatient(string patientId, int questionnaireId)
        {
            if (string.IsNullOrWhiteSpace(patientId)) return this.handler.UserEpisodeManager.GetAllQuestionnaireResponsesForPatient(patientId, 0, questionnaireId);
            return this.handler.UserEpisodeManager.GetAllQuestionnaireResponsesForPatient(patientId, 0, questionnaireId);
        }

        /// <summary>
        /// Gets the data for an anoymous Pro request
        /// </summary>
        /// <param name="anonymous">The encrypted string providing anonymous access</param>
        /// <param name="platform">The platform to load the format for</param>
        /// <returns>The Questionnaire requested</returns>
        public InterfaceContracts.Model.OperationResultAsUserQuestionnaire GetQuestionnaireAnonymous(string anonymous, Platform platform)
        {
            PCHI.Model.Questionnaire.Questionnaire q = null;
            Format f = null;
            QuestionnaireUserResponseGroup g = null;

            try
            {
                this.handler.UserEpisodeManager.GetQuestionnaireAnonymous(anonymous, platform, ref q, ref f, ref g);
                OperationResultAsUserQuestionnaire result;
                if (q != null && f != null && g != null) result = new OperationResultAsUserQuestionnaire(null, q, f, g);
                else result = new OperationResultAsUserQuestionnaire(this.handler.MessageManager.GetError(ErrorCodes.DATA_LOAD_ERROR), q, f, g);
                return result;
            }
            catch (Exception ex)
            {
                return new OperationResultAsUserQuestionnaire(ex, q, f, g);
            }
        }

        /// <summary>
        /// Gets the data for an anoymous Pro request
        /// </summary>
        /// <param name="patientId">The Id of the patient to get the Questionnaire For</param>
        /// <param name="questionnaireName">The name of the questionnaire to load</param>
        /// <param name="episodeId">The Id of the episode to load the questionnaire for</param>
        /// <param name="platform">The platform to load the format for</param>
        /// <returns>The Questionnaire requested</returns>
        public OperationResultAsUserQuestionnaire GetQuestionnaireForPatient(string patientId, string questionnaireName, int? episodeId, Platform platform)
        {
            if (WcfUserSessionSecurity.Current.User == null) return new OperationResultAsUserQuestionnaire(this.handler.MessageManager.GetError(ErrorCodes.USER_SESSION_EXPIRED), null, null, null);

            PCHI.Model.Questionnaire.Questionnaire q = null;
            Format f = null;
            QuestionnaireUserResponseGroup g = null;

            try
            {
                this.handler.UserEpisodeManager.GetQuestionnaireForPatient(patientId, questionnaireName, episodeId, platform, ref q, ref f, ref g);
                OperationResultAsUserQuestionnaire result;
                if (q != null && f != null && g != null) result = new OperationResultAsUserQuestionnaire(null, q, f, g);
                else result = new OperationResultAsUserQuestionnaire(this.handler.MessageManager.GetError(ErrorCodes.DATA_LOAD_ERROR), q, f, g);
                return result;
            }
            catch (Exception ex)
            {
                return new OperationResultAsUserQuestionnaire(ex, q, f, g);
            }

        }

        /// <summary>
        /// Saves the questionnaire responses for an anonymous questionnaire
        /// </summary>
        /// <param name="anonymous">The anonymous identifier string</param>
        /// <param name="groupId">The group Id</param>
        /// <param name="completed">Whether it is completed or not</param>
        /// <param name="questionnaireResponses">The list of Questionnaire Responses</param>
        public OperationResult SaveAnonymouseQuestionnaireResponse(string anonymous, int questionnaireId, int groupId, bool completed, List<QuestionnaireResponse> questionnaireResponses)
        {
            try
            {
                this.handler.UserEpisodeManager.SaveAnonymouseQuestionnaireResponse(anonymous, questionnaireId, groupId, completed, questionnaireResponses);
                return new OperationResult(null);
            }
            catch(Exception ex)
            {
                return new OperationResult(ex);
            }
        }

        /// <summary>
        /// Gets a list of outstanding questionnaires for the current user
        /// </summary>
        /// <param name="patientId">The Id of the patient to get the Questionnaires for</param>
        /// <returns>An OperationResult indicating success or failure with the list of Questionnaires outstanding for the given Patient filled in the EpisodeQuestionnaires variable</returns>
        public OperationResultAsDictionary GetOutstandingQuestionnairesForPatient(string patientId)
        {
            if (WcfUserSessionSecurity.Current.User == null) return new OperationResultAsDictionary(this.handler.MessageManager.GetError(ErrorCodes.USER_SESSION_EXPIRED));

            try
            {
                return new OperationResultAsDictionary(null) { EpisodeQuestionnaires = this.handler.UserEpisodeManager.GetQuestionnairesForPatientByEpisode(patientId, false) };
            }
            catch(Exception ex)
            {
                return new OperationResultAsDictionary(ex);
            }
        }

        /// <summary>
        /// Assigns a Questionnaire to an episode
        /// </summary>
        /// <param name="questionnaire">The name of the questionnaire to assign</param>
        /// <param name="episodeId">The episode to assign the questionnaire to</param>
        /// <param name="schedule">The schedule to execute</param>
        /// <returns>An operation result indicating success or failure</returns>
        public OperationResult ScheduleQuestionnaireForEpisode(string questionnaire, int episodeId, string schedule)
        {
            try
            {
                this.handler.UserEpisodeManager.ScheduleQuestionnaireForEpisode(questionnaire, episodeId, schedule);
                return new OperationResult(null);
            }
            catch(Exception ex)
            {
                return new OperationResult(ex);
            }
        }

        /// <summary>
        /// Gets a list of questionnaires that have been assigend to Episode and when they where answered
        /// </summary>
        /// <param name="episodeId">The Id of the episode</param>
        /// <returns>The list of questionnaires assigned inside the AssignedQuestionnaires variable</returns>        
        public OperationResultAsLists GetAssignedQuestionnairesForEpisode(int episodeId)
        {
            try
            {
                return new OperationResultAsLists(null) { AssignedQuestionnaires = this.handler.UserEpisodeManager.GetAssignedQuestionnairesForEpisode(episodeId) };
            }
            catch(Exception ex)
            {
                return new OperationResultAsLists(ex);
            }
        }

        /// <summary>
        /// Gets the last completed first visit questionnaire
        /// </summary>
        /// <param name="patientId" >The ID of the Patient to get the result for</param>
        /// <param name="episodeId">The optional Id of the episode to get the result for, if null or 0 the last filled in is retreived. If not null or greater then 0 the first completed current condition questionnaire is returned AFTER the episode was created</param>
        /// <returns>The completed questionnaire with format and answers</returns>
        public OperationResultAsUserQuestionnaire GetCompletedCurrentConditionQuestionnaire(string patientId, int? episodeId)
        {
            PCHI.Model.Questionnaire.Questionnaire q = null;            
            QuestionnaireUserResponseGroup g = null;

            try
            {
                this.handler.UserEpisodeManager.GetCompletedCurrentConditionQuestionnaire(patientId, episodeId, ref q, ref g);
                OperationResultAsUserQuestionnaire result;
                if (q != null && g != null) result = new OperationResultAsUserQuestionnaire(null, q, null, g);
                else result = new OperationResultAsUserQuestionnaire(this.handler.MessageManager.GetError(ErrorCodes.DATA_LOAD_ERROR), q, null, g);
                return result;
            }
            catch (Exception ex)
            {
                return new OperationResultAsUserQuestionnaire(ex, q, null, g);
            }
        }
    }
}
