using DSPrima.WcfUserSession.Behaviours;
using DSPrima.WcfUserSession.SecurityHandlers;
using PCHI.BusinessLogic;
using PCHI.Model.Questionnaire.Styling.Presentation;
using PCHI.Model.Questionnaire.Response;
using PCHI.Model.Tag;
using PCHI.WcfServices.API.PCHIServices.InterfaceClients.Base;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Questionnaire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.WcfServices.API.PCHIServices.InterfaceClients.Questionnaire
{
    /// <summary>
    /// Holds the implementation of the Questionnaire Server
    /// </summary>
    [WcfUserSessionBehaviour]
    public class QuestionnaireService : BaseService, IQuestionnaireService
    {
        /// <summary>
        /// Saves a <see cref="Questionnaire"/> to the database
        /// </summary>
        /// <param name="q">The Questionnaire instance to save</param>
        /// <returns>An operation result indicating sucess or failure</returns>
        public OperationResult SaveFullQuestionnaire(PCHI.Model.Questionnaire.Questionnaire q)
        {
            Exception exception = null;
            try
            {
                this.handler.QuestionnaireManager.SaveFullQuestionnaire(q);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            return new OperationResult(exception);
        }

        /// <summary>
        /// Adds the relation between a tag and a questionnaire based on the questionnaire's Id
        /// </summary>
        /// <param name="tagName">The name of the tag</param>
        /// <param name="tagValue">The value of the tag</param>
        /// <param name="questionnaireId">The Id of the questionnaire</param>\
        /// <returns>An OperationResult indicating success or failure</returns>
        public OperationResult AddTagToQuestionnaireById(string tagName, string tagValue, int questionnaireId)
        {
            try
            {
                this.handler.QuestionnaireManager.AddTagToQuestionnaireById(tagName, tagValue, questionnaireId);
                return new OperationResult(null);
            }
            catch(Exception ex)
            {
                return new OperationResult(ex);
            }
        }
        
        /// <summary>
        /// Adds the relation between a tag and a questionnaire based on the questionnaire's Name
        /// </summary>
        /// <param name="tagName">The name of the tag</param>
        /// <param name="tagValue">The value of the tag</param>
        /// <param name="questionnaireName">The name of the questionnaire</param>
        /// <returns>An OperationResult indicating success or failure</returns>
        public OperationResult  AddTagToQuestionnaireByName(string tagName, string tagValue, string questionnaireName)
        {
            try
            {
                this.handler.QuestionnaireManager.AddTagToQuestionnaireByName(tagName, tagValue, questionnaireName);
                return new OperationResult(null);
            }
            catch(Exception ex)
            {
                return new OperationResult(ex);
            }
        }

        /// <summary>
        /// Gets a list of all Questionnaire Names in the database
        /// The list of names can be found in the "Questionnaires" variable
        /// </summary>
        /// <returns>An OperationResultAsLists indicating success or failure with the list of names filledin the "strings" variable</returns>
        public OperationResultAsLists GetAllQuestionnairesWithTags()
        {
            try
            {
                return new OperationResultAsLists(null) { Questionnaires = this.handler.QuestionnaireManager.GetAllQuestionnairesWithTags() };
            }
            catch(Exception ex)
            {
                return new OperationResultAsLists(ex);
            }
        }

        /// <summary>
        /// Gets a list of questionnaires and their tags based upon the given list of tags
        /// If no tags are specified, all questionnaires are returned.
        /// </summary>
        /// <param name="tags">The list of tags to search by</param>
        /// <returns>A list of questionnares found</returns>
        public OperationResultAsLists GetQuestionnairesUserResponseGroupsByTag(IEnumerable<Tag> tags)
        {
            try
            {
                // TODO Implement Searches
                return new OperationResultAsLists(null) { /*Questionnaires = this.handler.QuestionnaireManager.GetQuestionnairesUserResponseGroupsByTag(tags)*/ };
            }
            catch(Exception ex)
            {
                return new OperationResultAsLists(ex);
            }
        }

        /// <summary>
        /// Gets the Questionnaire with the given name
        /// </summary>
        /// <param name="name">The name of the questionnaire</param>
        /// <returns>The questionnaire filled in the Questionnaire variable</returns>
        public OperationResultAsUserQuestionnaire GetQuestionnaireByname(string name)
        {
            try
            {
                return new OperationResultAsUserQuestionnaire(null, this.handler.QuestionnaireManager.GetQuestionnaireByName(name), null, null);
            }
            catch(Exception ex)
            {
                return new OperationResultAsUserQuestionnaire(ex, null, null, null);
            }
        }
    }
}
