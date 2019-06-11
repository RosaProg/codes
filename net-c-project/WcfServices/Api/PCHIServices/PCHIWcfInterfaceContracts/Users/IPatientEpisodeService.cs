using PCHI.Model.Questionnaire;
using PCHI.Model.Questionnaire.Response;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Base;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Users
{
    /// <summary>
    /// Defines the logic for User Questionnaire interactions
    /// </summary>
    [ServiceContract]
    public interface IPatientEpisodeService : IBaseService
    {
        /// <summary>
        /// Retrieves a list of a slimmed down version of all the Episodes for the given Patient
        /// </summary>
        /// <param name="patientId">The Id of the patient to get the episodes for</param>
        /// <returns>An OperationResult indicating success or failure and containing list of Episodes</returns>
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        OperationResultAsLists GetEpisodesForPatient(string patientId);

        /// <summary>
        /// Retrieves a list of Episodes for the given Patient. Includes all the details
        /// </summary>
        /// <param name="patientId">The Id of the patient to get the episodes for</param>
        /// <returns>An OperationResult indicating success or failure and containing list of Episodes</returns>
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        OperationResultAsLists GetEpisodesWithDetailsForPatient(string patientId);

        /// <summary>
        /// Assigns an Episode to the given Patient
        /// </summary>
        /// <param name="patientId">The Id of the Patient to assign the episode to</param>
        /// <param name="condition">The condition the episode is for</param>
        /// <param name="appointmentDate">The option date an appointment is set</param>
        /// <param name="externalEpisodeId">The external Episode Id</param>
        /// <param name="practitionerId">The Id of the practitioner with whom is the appointment</param>        
        /// <returns>An OperationResult indicating success or failure</returns>
        [OperationContract]
        OperationResult AssignEpisode(string patientId, string condition, DateTime? appointmentDate, string externalEpisodeId, string practitionerId);

        /// <summary>
        /// Assigns an Episode to the given user
        /// </summary>
        /// <param name="episodeId">The Id of the episode to assign the milestone to</param>
        /// <param name="milestoneName">The name of the milestone to add</param>
        /// <param name="date">The date to add the milesone for</param>
        /// <param name="practitionerId">The Id of the practitioner with whom is the appointment</param>        
        /// <returns>An OperationResult indicating success or failure</returns>
        [OperationContract]
        OperationResult AddMileStoneToEpisode(int episodeId, string milestoneName, DateTime date, string practitionerId);

        /// <summary>
        /// Saves the given <see cref="QuestionnaireResponse"/>  to the database
        /// Existing answers are overwritten
        /// </summary>        
        /// <param name="questionnaireId">The Id of the questionnaire to save the responses for</param>
        /// <param name="responseGroupId">The id of the response group to save the questionnaire for</param>        
        /// <param name="questionnaireCompleted">Indicates whehter or user has filled in all responses and the Questionnaire is now completed</param>
        /// <param name="responses">The <see cref="QuestionnaireResponse"/> to save</param>
        /// <returns>An operation result indicating success or failure</returns>
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        OperationResult SaveQuestionnaireResponseForCurrentUser(int questionnaireId, int responseGroupId, bool questionnaireCompleted, List<QuestionnaireResponse> responses);

        /// <summary>
        /// Returns all Questionnaire Responses for a given Patient and Questionnaire for all times this Questionnaire was answered by this Patient
        /// </summary>
        /// <param name="patientId">The Id of the Patient to get the response for</param>
        /// <param name="questionnaireId">The ID of the questionnaire you want to get the response for</param>
        /// <returns>A list of questionnaires</returns>
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        List<QuestionnaireUserResponseGroup> GetAllQuestionnaireResponsesForPatient(string patientId, int questionnaireId);

        /// <summary>
        /// Gets the data for an anoymous Pro request
        /// </summary>
        /// <param name="anonymous">The encrypted string providing anonymous access</param>
        /// <param name="platform">The platform to load for</param>
        /// <returns>The Questionnaire requested</returns>
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        OperationResultAsUserQuestionnaire GetQuestionnaireAnonymous(string anonymous, Platform platform);

        /// <summary>
        /// Gets the data for an anoymous Pro request
        /// </summary>
        /// <param name="patientId">The Id of the patient to get the Questionnaire For</param>
        /// <param name="questionnaireName">The name of the questionnaire to load</param>        
        /// <param name="episodeId">The Id of the episode to load the questionnaire for</param>
        /// <param name="platform">The platform to load the format for</param>
        /// <returns>The Questionnaire requested</returns>
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        OperationResultAsUserQuestionnaire GetQuestionnaireForPatient(string patientId, string questionnaireName, int? episodeId, Platform platform);

        /// <summary>
        /// Saves the questionnaire responses for an anonymous questionnaire
        /// </summary>
        /// <param name="anonymous">The anonymous identifier string</param>
        /// <param name="questionnaireId">The questionnaire id</param>
        /// <param name="groupId">The group Id</param>
        /// <param name="completed">Whether it is completed or not</param>
        /// <param name="questionnaireResponses">The list of Questionnaire Responses</param>
        /// <returns>An OperationResult indicating success or failure</returns>
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        OperationResult SaveAnonymouseQuestionnaireResponse(string anonymous, int questionnaireId, int groupId, bool completed, List<QuestionnaireResponse> questionnaireResponses);

        /// <summary>
        /// Gets a list of outstanding questionnaires for the current user
        /// </summary>
        /// <param name="patientId">The Id of the patient to get the Questionnaires for</param>
        /// <returns>An OperationResult indicating success or failure with the list of Questionnaires outstanding for the given Patient filled in the EpisodeQuestionnaires variable</returns>
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        OperationResultAsDictionary GetOutstandingQuestionnairesForPatient(string patientId);

        /// <summary>
        /// Assigns a Questionnaire to an episode
        /// </summary>
        /// <param name="questionnaire">The name of the questionnaire to assign</param>
        /// <param name="episodeId">The episode to assign the questionnaire to</param>
        /// <param name="schedule">The schedule to execute</param>
        /// <returns>An operation result indicating success or failure</returns>
        [OperationContract]
        OperationResult ScheduleQuestionnaireForEpisode(string questionnaire, int episodeId, string schedule);

        /// <summary>
        /// Gets a list of questionnaires that have been assigend to Episode and when they where answered
        /// </summary>
        /// <param name="episodeId">The Id of the episode</param>
        /// <returns>The list of questionnaires assigned inside the AssignedQuestionnaires variable</returns>        
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        OperationResultAsLists GetAssignedQuestionnairesForEpisode(int episodeId);

        /// <summary>
        /// Gets the last completed first visit questionnaire
        /// </summary>
        /// <param name="patientId" >The ID of the Patient to get the result for</param>
        /// <param name="episodeId">The optional Id of the episode to get the result for, if null or 0 the last filled in is retreived. If not null or greater then 0 the first completed current condition questionnaire is returned AFTER the episode was created</param>
        /// <returns>The completed questionnaire with format and answers</returns>
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        OperationResultAsUserQuestionnaire GetCompletedCurrentConditionQuestionnaire(string patientId, int? episodeId);
    }
}
