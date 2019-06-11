using PCHI.Model.Tag;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Questionnaire;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Base;
using System.Collections.Generic;

namespace PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Questionnaire
{
    /// <summary>
    /// Defines the Proxy to access the Questionnaire WCF Service
    /// </summary>
    public class QuestionnaireClient : BaseClient<IQuestionnaireService>, IQuestionnaireService
    {
        /// <summary>
        /// Saves a <see cref="Questionnaire"/> to the database
        /// </summary>
        /// <param name="q">The Questionnaire instance to save</param>
        /// <returns>An Operation Result indicating success or failuer</returns>
        public OperationResult SaveFullQuestionnaire(PCHI.Model.Questionnaire.Questionnaire q)
        {
            return this.Channel.SaveFullQuestionnaire(q);
        }

        /// <summary>
        /// Adds the relation between a tag and a questionnaire based on the questionnaire's Id
        /// </summary>
        /// <param name="tagName">The name of the tag</param>
        /// <param name="tagValue">The value of the tag</param>
        /// <param name="questionnaireId">The Id of the questionnaire</param>
        /// <returns>An OperationResult indicating success or failure</returns>
        public OperationResult AddTagToQuestionnaireById(string tagName, string tagValue, int questionnaireId)
        {
            return this.Channel.AddTagToQuestionnaireById(tagName, tagValue, questionnaireId);
        }

        /// <summary>
        /// Adds the relation between a tag and a questionnaire based on the questionnaire's Name
        /// </summary>
        /// <param name="tagName">The name of the tag</param>
        /// <param name="tagValue">The value of the tag</param>
        /// <param name="questionnaireName">The name of the questionnaire</param>
        /// <returns>An OperationResult indicating success or failure</returns>
        public OperationResult AddTagToQuestionnaireByName(string tagName, string tagValue, string questionnaireName)
        {
            return this.Channel.AddTagToQuestionnaireByName(tagName, tagValue, questionnaireName);
        }

        /// <summary>
        /// Gets a list of all Questionnaire Names in the database
        /// The list of names can be found in the "Questionnaires" variable
        /// </summary>
        /// <returns>An OperationResultAsLists indicating success or failure with the list of names filledin the "strings" variable</returns>
        public OperationResultAsLists GetAllQuestionnairesWithTags()
        {
            return this.Channel.GetAllQuestionnairesWithTags();
        }

        /// <summary>
        /// Gets a list of questionnaires and their tags based upon the given list of tags
        /// If no tags are specified, all questionnaires are returned.
        /// </summary>
        /// <param name="tags">The list of tags to search by</param>
        /// <returns>A list of questionnares found</returns>        
        public OperationResultAsLists GetQuestionnairesUserResponseGroupsByTag(IEnumerable<Tag> tags)
        {
            return this.Channel.GetQuestionnairesUserResponseGroupsByTag(tags);
        }

        /// <summary>
        /// Gets the Questionnaire with the given name
        /// </summary>
        /// <param name="name">The name of the questionnaire</param>
        /// <returns>The questionnaire filled in the Questionnaire variable</returns>
        public OperationResultAsUserQuestionnaire GetQuestionnaireByname(string name)
        {
            return this.Channel.GetQuestionnaireByname(name);
        }
    }
}
