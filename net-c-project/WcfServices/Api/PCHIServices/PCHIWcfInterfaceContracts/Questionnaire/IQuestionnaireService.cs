using PCHI.Model.Tag;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Base;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model;
using System.Collections.Generic;
using System.ServiceModel;

namespace PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Questionnaire
{
    /// <summary>
    /// Defines the QuestionnaireService options
    /// </summary>
    [ServiceContract]
    public interface IQuestionnaireService : IBaseService
    {
        /// <summary>
        /// Saves a <see cref="Questionnaire"/> to the database
        /// </summary>
        /// <param name="q">The Questionnaire instance to save</param>
        /// <returns>An OperationResult indicating success or failure</returns>
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        OperationResult SaveFullQuestionnaire(PCHI.Model.Questionnaire.Questionnaire q);

        /// <summary>
        /// Adds the relation between a tag and a questionnaire based on the questionnaire's Id
        /// </summary>
        /// <param name="tagName">The name of the tag</param>
        /// <param name="tagValue">The value of the tag</param>
        /// <param name="questionnaireId">The Id of the questionnaire</param>
        /// <returns>An OperationResult indicating success or failure</returns>
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        OperationResult AddTagToQuestionnaireById(string tagName, string tagValue, int questionnaireId);
        
        /// <summary>
        /// Adds the relation between a tag and a questionnaire based on the questionnaire's Name
        /// </summary>
        /// <param name="tagName">The name of the tag</param>
        /// <param name="tagValue">The value of the tag</param>
        /// <param name="questionnaireName">The name of the questionnaire</param>
        /// <returns>An OperationResult indicating success or failure</returns>
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        OperationResult AddTagToQuestionnaireByName(string tagName, string tagValue, string questionnaireName);

        /// <summary>
        /// Gets a list of all Questionnaire Names in the database
        /// The list of names can be found in the "Questionnaires" variable
        /// </summary>
        /// <returns>A list of all questionnaires</returns>
        /// <returns>An OperationResultAsLists indicating success or failure with the list of names filled in the "strings" variable</returns>
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        OperationResultAsLists GetAllQuestionnairesWithTags();

        /// <summary>
        /// Gets a list of questionnaires and their tags based upon the given list of tags
        /// If no tags are specified, all questionnaires are returned.
        /// </summary>
        /// <param name="tags">The list of tags to search by</param>
        /// <returns>A list of questionnares found</returns>
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        OperationResultAsLists GetQuestionnairesUserResponseGroupsByTag(IEnumerable<Tag> tags);

        /// <summary>
        /// Gets the Questionnaire with the given name
        /// </summary>
        /// <param name="name">The name of the questionnaire</param>
        /// <returns>The questionnaire filled in the Questionnaire variable</returns>
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        OperationResultAsUserQuestionnaire GetQuestionnaireByname(string name);
    }
}
