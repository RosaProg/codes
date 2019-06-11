using DSPrima.WcfUserSession.Behaviours;
using PCHI.BusinessLogic;
using PCHI.Model.Questionnaire;
using PCHI.Model.Questionnaire.Styling.Definition.Sections;
using PCHI.Model.Questionnaire.Styling.Presentation;
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
    /// Implements the logic for the QuestionnaireFormat service
    /// </summary>
    [WcfUserSessionBehaviour]
    public class QuestionnaireFormatService : BaseService, IQuestionnaireFormatService
    {
        /// <summary>
        /// Adds or updates a full Questionnaire Format Container Definition and all children and references
        /// </summary>
        /// <param name="container">The container to add</param>
        /// <returns>An Operation result indicating success orfailure</returns>
        public OperationResult AddOrUpdateFullDefinitionContainer(ContainerFormatDefinition container)
        {
            try
            {
                this.handler.QuestionnaireFormatManager.AddOrUpdateFullDefinitionContainer(container);
                return new OperationResult(null);
            }
            catch (Exception ex)
            {
                return new OperationResult(ex);
            }
        }

        /// <summary>
        /// Adds a full <see cref="Format"/> and all it's children to the database.
        /// Does NOT store the referenced definitions classes
        /// </summary>
        /// <param name="format">The <see cref="Format"/> to store</param>
        /// <returns>An Operation result indicating success orfailure</returns>
        public OperationResult AddOrUpdateFullFormat(Format format)
        {
            try
            {
                this.handler.QuestionnaireFormatManager.AddOrUpdateFullFormat(format);
                return new OperationResult(null);
            }
            catch (Exception ex)
            {
                return new OperationResult(ex);
            }
        }

        /// <summary>
        /// Returns a full format with the given name for a Questionnaire
        /// </summary>        
        /// <param name="formatName">The name of the format</param>
        /// <param name="platform">The platform to get the QuestionnaireFormat for</param>
        /// <returns>A OperationResultAsUserQuestionnaire indicating success or failure with The full format filled in the Format variable</returns>
        public OperationResultAsUserQuestionnaire GetQuestionnaireFormatByName(string formatName, Platform platform)
        {
            try
            {
                return new OperationResultAsUserQuestionnaire(null, null, this.handler.QuestionnaireFormatManager.GetQuestionnaireFormatByName(formatName, platform), null);
            }
            catch (Exception ex)
            {
                return new OperationResultAsUserQuestionnaire(ex, null, null, null);
            }
        }

        /// <summary>
        /// Gets a list of all Formats in the database
        /// </summary>
        /// <returns>A OperationResultAsUserQuestionnaire indicating success or failure with The list of all the formats filled in the Strings Variable</returns>
        public OperationResultAsLists GetAllQuestionnaireFormats()
        {
            try
            {
                return new OperationResultAsLists(null) { Formats = this.handler.QuestionnaireFormatManager.GetAllQuestionnaireFormats() };
            }
            catch (Exception ex)
            {
                return new OperationResultAsLists(ex);
            }
        }
    }
}
