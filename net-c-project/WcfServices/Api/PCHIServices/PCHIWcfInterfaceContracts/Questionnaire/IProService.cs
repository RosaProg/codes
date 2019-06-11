using PCHI.Model.Questionnaire;
using PCHI.Model.Questionnaire.Pro;
using PCHI.Model.Questionnaire.Response;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Base;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model;
using System.Collections.Generic;
using System.ServiceModel;

namespace PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Questionnaire
{
    /// <summary>
    /// Defines the Contract for an ProService
    /// </summary>
    [ServiceContract]
    public interface IProService : IBaseService
    {
        /// <summary>
        /// Retrievs a complete ProInstrument for the given Id from the database
        /// The data can be found in the Questionnaire Variable
        /// </summary>
        /// <param name="id">The Id of the ProInstrument to load</param>
        /// <returns>The ProInstrument found or null</returns>
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        OperationResultAsUserQuestionnaire GetFullProInstrumentById(int id);

        /// <summary>
        /// Gets the Last created Full Pro Instrument with the given name
        /// The data can be found in the Questionnaire Variable
        /// </summary>
        /// <param name="name">The name of the ProInstrument to retrieve</param>
        /// <returns>The ProInstrument found or null</returns>
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        OperationResultAsUserQuestionnaire GetFullProInstrumentByName(string name);

        /// <summary>
        /// Retrieves and Calculates the results for all domains of the given Questionnaire for all of the questionnaires filled in by the User
        /// </summary>
        /// <param name="patientId">The Id of the patient to get the data for</param>
        /// <param name="episodeId">The Id of the episode to use</param>
        /// <param name="questionnaireId">The Id of the Questionnaire to get the results for</param>
        /// <returns>A list of Results grouped by Episode and internally by Dictionary</returns>
        [OperationContract(Name = "GetProDomainResultsForCurrentPatientByEpisodeAndQuestionnaire")]
        [ReferencePreservingDataContractFormat]
        OperationResultAsDictionary GetProDomainResultsForCurrentPatient(string patientId, int episodeId, int questionnaireId);

        /// <summary>
        /// Retrieves and Calculates the results for all domains of the given episode and questionnaireName for all of the ProInstrumentsfilled 
        /// </summary>
        /// <param name="patientId">The Id of the patient the result is supposed to be for</param>
        /// <param name="episodeId">The Id of the episode</param>        
        /// <returns>A list of Results grouped by Episode and internally by Dictionary</returns>
        [OperationContract(Name = "GetProDomainResultsForEpisodeAndQuestionnaire")]
        [ReferencePreservingDataContractFormat]
        OperationResultAsDictionary GetProDomainResults(string patientId, int episodeId);
        
        /// <summary>
        /// Gets a list of all Pro names
        /// </summary>
        /// <returns>An operation result indicating success or failuer and The list of Pro names</returns>
        [OperationContract]
        OperationResultAsLists GetProNames();
    }
}
