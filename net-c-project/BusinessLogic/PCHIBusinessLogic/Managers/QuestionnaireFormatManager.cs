using PCHI.BusinessLogic.Security;
using PCHI.BusinessLogic.Utilities;
using PCHI.BusinessLogic.Utilities.Model;
using PCHI.DataAccessLibrary;
using PCHI.Model.Questionnaire;
using PCHI.Model.Questionnaire.Styling.Definition.Sections;
using PCHI.Model.Questionnaire.Styling.Presentation;
using PCHI.Model.Security;
using System;
using System.Collections.Generic;

namespace PCHI.BusinessLogic.Managers
{
    /// <summary>
    /// Manages the businesslogic for the QuestionniareFormatManager
    /// </summary>
    public class QuestionnaireFormatManager
    {
        /// <summary>
        /// Holds the <see cref="AccessHandlerManager"/> for internal use
        /// </summary>
        private AccessHandlerManager manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionnaireFormatManager"/> class
        /// </summary>
        /// <param name="manager">The <see cref="AccessHandlerManager"/> instance to use</param>
        internal QuestionnaireFormatManager(AccessHandlerManager manager)
        {
            this.manager = manager;
        }

        /// <summary>
        /// Adds or updates a full Questionnaire Format Container Definition and all children and references
        /// </summary>
        /// <param name="container">The container to add</param>
        public void AddOrUpdateFullDefinitionContainer(ContainerFormatDefinition container)
        {
            try
            {
                SecuritySession.Current.VerifyAccess(Actions.SAVE_FORMAT_DEFINITION);
                this.manager.QuestionnaireFormatAccessHandler.AddOrUpdateFullDefinitionContainer(container);
                Logger.Audit(new Audit(Model.Security.Actions.SAVE_FORMAT_DEFINITION, AuditEventType.ADD, container));
            }
            catch (Exception ex)
            {
                Logger.Audit(new Audit(Model.Security.Actions.SAVE_FORMAT_DEFINITION, AuditEventType.ADD, container, false, ex.Message));
                throw ex;
            }
        }

        /// <summary>
        /// Adds a full <see cref="Format"/> and all it's children to the database.
        /// Does NOT store the referenced definitions classes
        /// </summary>
        /// <param name="format">The <see cref="Format"/> to store</param>
        public void AddOrUpdateFullFormat(Format format)
        {
            try
            {
                SecuritySession.Current.VerifyAccess(Actions.SAVE_FORMAT);
                this.manager.QuestionnaireFormatAccessHandler.AddOrUpdateFullFormat(format);
                Logger.Audit(new Audit(Model.Security.Actions.SAVE_FORMAT, AuditEventType.ADD, format));
            }
            catch (Exception ex)
            {
                Logger.Audit(new Audit(Model.Security.Actions.SAVE_FORMAT, AuditEventType.ADD, format, false, ex.Message));
                throw ex;
            }
        }

        /// <summary>
        /// Returns a full format with the given name for a Questionnaire
        /// </summary>
        /// <param name="formatName">The name of the format to load</param>
        /// <param name="platform">The platformt to get the questionnaire format for</param>
        /// <returns>The full format</returns>
        public Format GetQuestionnaireFormatByName(string formatName, Platform platform)
        {
            return this.manager.QuestionnaireFormatAccessHandler.GetFullFormatByName(formatName, platform);
        }

        /// <summary>
        /// Gets a list of all Formats in the database
        /// </summary>
        /// <returns>A list of all the formats</returns>
        public List<Format> GetAllQuestionnaireFormats()
        {
            return this.manager.QuestionnaireFormatAccessHandler.GetAllQuestionnaireFormats();
        }
    }
}