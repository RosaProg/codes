using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Base;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Service
{
    /// <summary>
    /// Describes functionality for obtaining details regarding the service and settings
    /// </summary>
    [ServiceContract]
    public interface IServiceDetails : IBaseService
    {
        /// <summary>
        /// Gets a list of the Authentication providers available for users
        /// </summary>
        /// <returns>An OperationResultAsLists indicating success or failure with the list of authentication providers available filled in the "strings" variable</returns>
        [OperationContract]
        OperationResultAsLists GetTwoStageAuthenticationProviders();

        /// <summary>
        /// Gets a list of Text based upon the Text Identifiers send
        /// </summary>
        /// <param name="textIdentifiers">All the text identifiers you want the text for</param>
        /// <param name="patientId">The Id of the patient if available</param>
        /// <param name="registrationCode">The registration code</param>
        /// <returns>AN operation result indicating success or failure with the text inside the StringDictionary. Key is the Idenfitier, Value is the text</returns>
        [OperationContract]
        OperationResultAsDictionary GetPageText(List<string> textIdentifiers, string patientId = null, string registrationCode = null);

        /// <summary>
        /// Saves a given piece of text for a given Identifier
        /// </summary>
        /// <param name="textIdentifier">The identifier for the text</param>
        /// <param name="text">The text to save</param>
        /// <returns>An operation result indicating success or failure</returns>
        [OperationContract]
        OperationResult SavePageText(string textIdentifier, string text);

        /// <summary>
        /// Returns a list of roles available.
        /// </summary>
        /// <returns>An operation result indicating success or failure and the roles filled in the Strings variable</returns>
        [OperationContract]
        OperationResultAsLists GetAvailableRoles();
    }
}
