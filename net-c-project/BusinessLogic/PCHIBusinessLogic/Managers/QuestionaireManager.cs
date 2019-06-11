using PCHI.BusinessLogic.Security;
using PCHI.BusinessLogic.Utilities;
using PCHI.BusinessLogic.Utilities.Model;
using PCHI.DataAccessLibrary;
using PCHI.Model.Questionnaire;
using PCHI.Model.Questionnaire.Pro;
using PCHI.Model.Questionnaire.Response;
using PCHI.Model.Security;
using PCHI.Model.Tag;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PCHI.BusinessLogic.Managers
{
    /// <summary>
    /// Handlers the saving and loading of Questionnaires
    /// </summary>
    public class QuestionnaireManager
    {
        /// <summary>
        /// Holds the <see cref="AccessHandlerManager"/> for internal use
        /// </summary>
        private AccessHandlerManager manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionnaireManager"/> class
        /// </summary>
        /// <param name="manager">The <see cref="AccessHandlerManager"/> instance to use</param>
        internal QuestionnaireManager(AccessHandlerManager manager)
        {
            this.manager = manager;
        }

        /// <summary>
        /// Saves a FUll Pro instrument to the database, including all connected objects and classes
        /// </summary>
        /// <param name="pro">The <see cref="Questionnaire"/> to save</param>
        public void SaveFullQuestionnaire(Questionnaire pro)
        {
            try
            {
                SecuritySession.Current.VerifyAccess(Actions.SAVE_QUESTIONNAIRE);
                this.manager.QuestionnaireAccessHandler.AddFullQuestionnaire(pro);
                Logger.Audit(new Audit(Model.Security.Actions.SAVE_QUESTIONNAIRE, AuditEventType.ADD, pro.GetType(), "Name", pro.Name));
            }
            catch (Exception ex)
            {
                Logger.Audit(new Audit(Model.Security.Actions.SAVE_QUESTIONNAIRE, AuditEventType.ADD, pro.GetType(), "Name", pro.Name, false, ex.Message));
                throw ex;
            }
        }

        /// <summary>
        /// Gets the Questionnaire with the given name
        /// </summary>
        /// <param name="name">The name of the questionnaire</param>
        /// <returns>The questionnaire filled in the Questionnaire variable</returns>
        public Questionnaire GetQuestionnaireByName(string name)
        {
            Questionnaire q = this.manager.QuestionnaireAccessHandler.GetFullQuestionnaireByName(name);

            return q;
        }

        #region Pro Specific functions

        /// <summary>
        /// Retrieves the Complete ProInstrument with the given Id from the database
        /// </summary>
        /// <param name="id">The Id of the ProInstrument to load</param>
        /// <returns>The ProInstrument or null if nothing was found</returns>
        public ProInstrument GetFullProInstrumentById(int id)
        {
            Questionnaire q = this.manager.QuestionnaireAccessHandler.GetFullQuestionnaireById(id);
            if (q.GetType() == typeof(ProInstrument)) return (ProInstrument)q;

            return null;
        }

        /// <summary>
        /// Gets the Last created Full Pro Instrument with the given name
        /// </summary>
        /// <param name="name">The name of the ProInstrument to retrieve</param>
        /// <returns>The ProInstrument found or null</returns>
        public ProInstrument GetFullProInstrumentByName(string name)
        {
            Questionnaire q = this.manager.QuestionnaireAccessHandler.GetFullQuestionnaireByName(name);
            if (q != null && q.GetType() == typeof(ProInstrument)) return (ProInstrument)q;

            return null;
        }

        /// <summary>
        /// Retrieves a list of ProInstruments and there concepts and domains
        /// </summary>
        /// <returns>A list of ProInstruments</returns>
        public List<ProInstrument> ListProInstruments()
        {
            return this.manager.QuestionnaireAccessHandler.ListProInstruments();
        }

        #endregion Pro Specific functions

        /// <summary>
        /// Adds the relation between a tag and a questionnaire based on the questionnaire's Id
        /// </summary>
        /// <param name="tagName">The name of the tag</param>
        /// <param name="tagValue">The value of the tag</param>
        /// <param name="questionnaireId">The Id of the questionnaire</param>
        public void AddTagToQuestionnaireById(string tagName, string tagValue, int questionnaireId)
        {
            try
            {
                SecuritySession.Current.VerifyAccess(Actions.ADD_QUESTIONNAIRE_TAG);
                Tag tag = new Tag() { TagName = tagName, Value = tagValue };
                this.manager.QuestionnaireAccessHandler.AddTagToQuestionnaireById(tag, questionnaireId);
                Logger.Audit(new Audit(Model.Security.Actions.ADD_QUESTIONNAIRE_TAG, AuditEventType.ADD, typeof(Questionnaire), "Id", questionnaireId.ToString()));
            }
            catch (Exception ex)
            {
                Logger.Audit(new Audit(Model.Security.Actions.ADD_QUESTIONNAIRE_TAG, AuditEventType.ADD, typeof(Questionnaire), "Id", questionnaireId.ToString(), false, ex.Message));
                throw ex;
            }
        }

        /// <summary>
        /// Adds the relation between a tag and a questionnaire based on the questionnaire's Name
        /// </summary>
        /// <param name="tagName">The name of the tag</param>
        /// <param name="tagValue">The value of the tag</param>
        /// <param name="questionnaireName">The name of the questionnaire</param>
        public void AddTagToQuestionnaireByName(string tagName, string tagValue, string questionnaireName)
        {
            try
            {
                SecuritySession.Current.VerifyAccess(Actions.ADD_QUESTIONNAIRE_TAG);
                Tag tag = new Tag() { TagName = tagName, Value = tagValue };
                this.manager.QuestionnaireAccessHandler.AddTagToQuestionnaireByName(tag, questionnaireName);
                Logger.Audit(new Audit(Model.Security.Actions.ADD_QUESTIONNAIRE_TAG, AuditEventType.ADD, typeof(Questionnaire), "Name", questionnaireName));
            }
            catch (Exception ex)
            {
                Logger.Audit(new Audit(Model.Security.Actions.ADD_QUESTIONNAIRE_TAG, AuditEventType.ADD, typeof(Questionnaire), "Name", questionnaireName, false, ex.Message));
                throw ex;
            }
        }

        /// <summary>
        /// Gets a list of all Questionnaires in the database
        /// </summary>
        /// <returns>A list of all questionnaires</returns>
        public List<Questionnaire> GetAllQuestionnairesWithTags()
        {
            return this.manager.QuestionnaireAccessHandler.GetAllQuestionnairesWithTags<Questionnaire>();
        }

        /// <summary>
        /// Gets a list of all Pro names
        /// </summary>
        /// <returns>An operation result indicating success or failuer and The list of Pro names</returns>
        public List<string> GetProNames()
        {
            return this.manager.QuestionnaireAccessHandler.GetAllQuestionnairesWithTags<ProInstrument>().Select(p => p.Name).ToList();
        }
    }
}