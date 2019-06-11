using DSPrima.WcfUserSession.Proxy;
using PCHI.Model.Questionnaire;
using PCHI.Model.Questionnaire.Styling.Definition.Sections;
using PCHI.Model.Questionnaire.Styling.Presentation;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Questionnaire;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Questionnaire
{
    /// <summary>
    /// Defines the Proxy to access the QuestionnaireFormat WCF Service
    /// </summary>
    public class QuestionnaireFormatClient : BaseClient<IQuestionnaireFormatService>, IQuestionnaireFormatService
    {
        /// <summary>
        /// Adds or updates a full Questionnaire Format Container Definition and all children and references
        /// </summary>
        /// <param name="container">The container to add</param>
        /// <returns>An Operation result indicating success orfailure</returns>
        public OperationResult AddOrUpdateFullDefinitionContainer(ContainerFormatDefinition container)
        {
            return this.Channel.AddOrUpdateFullDefinitionContainer(container);
        }

        /// <summary>
        /// Adds a full <see cref="Format"/> and all it's children to the database.
        /// Does NOT store the referenced definitions classes
        /// </summary>
        /// <param name="format">The <see cref="Format"/> to store</param>
        /// <returns>An Operation result indicating success orfailure</returns>
        public OperationResult AddOrUpdateFullFormat(Format format)
        {
            return this.Channel.AddOrUpdateFullFormat(format);
        }
        
        /// <summary>
        /// Returns a full format with the given name for a Questionnaire
        /// </summary>
        /// <param name="formatName">The name of the format</param>
        /// <param name="platform">The platform to get the format for</param>
        /// <returns>A OperationResultAsUserQuestionnaire indicating success or failure with The full format filled in the Format variable</returns>
        public OperationResultAsUserQuestionnaire GetQuestionnaireFormatByName(string formatName, Platform platform)
        {
            return this.Channel.GetQuestionnaireFormatByName(formatName, platform);
        }

        /// <summary>
        /// Gets a list of all Formats in the database
        /// </summary>
        /// <returns>A OperationResultAsUserQuestionnaire indicating success or failure with The list of all the formats filled in the Strings Variable</returns>
        public OperationResultAsLists GetAllQuestionnaireFormats()
        {
            return this.Channel.GetAllQuestionnaireFormats();
        }
    }
}
