using DSPrima.WcfUserSession.Behaviours;
using DSPrima.WcfUserSession.SecurityHandlers;
using PCHI.BusinessLogic;
using PCHI.BusinessLogic.Security;
using PCHI.Model.Questionnaire;
using PCHI.Model.Questionnaire.Pro;
using PCHI.Model.Questionnaire.Response;
using PCHI.Model.Users;
using PCHI.WcfServices.API.PCHIServices.InterfaceClients.Base;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Questionnaire;
using System;
using System.Collections.Generic;

namespace PCHI.WcfServices.API.PCHIServices.InterfaceClients.Questionnaire
{
    /// <summary>
    /// Holds the client implementation for the ProService
    /// </summary>
    [WcfUserSessionBehaviour]
    public class ProService : BaseService, IProService
    {
        /// <summary>
        /// Retrievs a complete ProInstrument for the given Id from the database
        /// The data can be found in the Questionnaire Variable
        /// </summary>
        /// <param name="id">The Id of the ProInstrument to load</param>
        /// <returns>The ProInstrument found or null</returns>
        public OperationResultAsUserQuestionnaire GetFullProInstrumentById(int id)
        {
            try
            {
                ProInstrument instrument = this.handler.QuestionnaireManager.GetFullProInstrumentById(id);
                return new OperationResultAsUserQuestionnaire(null, instrument, null, null);
            }
            catch (Exception ex)
            {
                return new OperationResultAsUserQuestionnaire(ex, null, null, null);
            }
        }

        /// <summary>
        /// Gets the Last created Full Pro Instrument with the given name
        /// The data can be found in the Questionnaire Variable
        /// </summary>
        /// <param name="name">The name of the ProInstrument to retrieve</param>
        /// <returns>The ProInstrument found or null</returns>
        public OperationResultAsUserQuestionnaire GetFullProInstrumentByName(string name)
        {
            try
            {
                ProInstrument instrument = this.handler.QuestionnaireManager.GetFullProInstrumentByName(name);
                return new OperationResultAsUserQuestionnaire(null, instrument, null, null);
            }
            catch (Exception ex)
            {
                return new OperationResultAsUserQuestionnaire(ex, null, null, null);
            }
        }

        /// <summary>
        /// Retrieves and Calculates the results for all domains of the given Questionnaire for all of the questionnaires filled in by the User
        /// </summary>
        /// <param name="patientId">The Id of the patient to get the data for</param>
        /// <param name="episodeId">The Id of the User to use</param>
        /// <param name="questionnaireId">The Id of the Questionnaire to get the results for</param>
        /// <returns>A list of Results grouped by Episode and internally by Dictionary</returns>
        public OperationResultAsDictionary GetProDomainResultsForCurrentPatient(string patientId, int episodeId, int questionnaireId)
        {
            try
            {
                if (WcfUserSessionSecurity.Current.User == null) return new OperationResultAsDictionary(this.handler.MessageManager.GetError(Model.Messages.ErrorCodes.USER_SESSION_EXPIRED));
                return new OperationResultAsDictionary(null) { ProDomainResultSets = this.handler.UserEpisodeManager.GetProDomainResults(patientId, episodeId, questionnaireId, UserTypes.Patient) };
            }
            catch (Exception ex)
            {
                return new OperationResultAsDictionary(ex);
            }
        }

        /// <summary>
        /// Retrieves and Calculates the results for all domains of the given episode and questionnaireName for all of the ProInstrumentsfilled 
        /// </summary>
        /// <param name="patientId">The Id of the patient the result is supposed to be for</param>
        /// <param name="episodeId">The Id of the episode</param>        
        /// <returns>A list of Results grouped by Episode and internally by Dictionary</returns>
        public OperationResultAsDictionary GetProDomainResults(string patientId, int episodeId)
        {
            try
            {
                return new OperationResultAsDictionary(null) { ProDomainResultSets = this.handler.UserEpisodeManager.GetProDomainResults(patientId: patientId, episodeId: episodeId) };
            }
            catch (Exception ex)
            {
                return new OperationResultAsDictionary(ex);
            }
        }

        /// <summary>
        /// Gets a list of all Pro names
        /// </summary>
        /// <returns>An operation result indicating success or failuer and The list of Pro names</returns>
        public OperationResultAsLists GetProNames()
        {
            try
            {
                return new OperationResultAsLists(null) { Strings = this.handler.QuestionnaireManager.GetProNames() };
            }
            catch (Exception ex)
            {
                return new OperationResultAsLists(ex);
            }
        }
    }
}
