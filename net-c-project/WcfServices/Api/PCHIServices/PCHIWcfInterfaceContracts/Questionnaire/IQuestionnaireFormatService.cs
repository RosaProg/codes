using PCHI.Model.Questionnaire;
using PCHI.Model.Questionnaire.Styling.Definition.Sections;
using PCHI.Model.Questionnaire.Styling.Presentation;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Base;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Questionnaire
{
    /// <summary>
    /// Defines the contracts for the Questionnaire Format service
    /// </summary>
    [ServiceContract]
    public interface IQuestionnaireFormatService : IBaseService
    {
        /// <summary>
        /// Adds or updates a full Questionnaire Format Container Definition and all children and references
        /// </summary>
        /// <param name="container">The container to add</param>
        /// <returns>An Operation result indicating success orfailure</returns>
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        OperationResult AddOrUpdateFullDefinitionContainer(ContainerFormatDefinition container);

        /// <summary>
        /// Adds a full <see cref="Format"/> and all it's children to the database.
        /// Does NOT store the referenced definitions classes
        /// </summary>
        /// <param name="format">The <see cref="Format"/> to store</param>
        /// <returns>An Operation result indicating success orfailure</returns>
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        OperationResult AddOrUpdateFullFormat(Format format);

        /// <summary>
        /// Returns a full format with the given name for a Questionnaire
        /// </summary>
        /// <param name="formatName">The name of the format</param>
        /// <param name="platform">The platform to load the format for</param>
        /// <returns>A OperationResultAsUserQuestionnaire indicating success or failure with The full format filled in the Format variable</returns>
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        OperationResultAsUserQuestionnaire GetQuestionnaireFormatByName(string formatName, Platform platform);

        /// <summary>
        /// Gets a list of all Formats in the database
        /// </summary>
        /// <returns>A OperationResultAsUserQuestionnaire indicating success or failure with The list of all the formats filled in the Strings Variable</returns>
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        OperationResultAsLists GetAllQuestionnaireFormats();
    }
}
