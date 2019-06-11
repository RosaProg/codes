using DSPrima.WcfUserSession.Proxy;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Service;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Service
{
    /// <summary>
    /// Implements functionality for obtaining details regarding the service and settings
    /// </summary>
    public class ServiceDetailsClient : BaseClient<IServiceDetails>, IServiceDetails
    {
        /// <summary>
        /// Gets a list of the Authentication providers available for users
        /// </summary>
        /// <returns>An OperationResultAsLists indicating success or failure with the list of authentication providers available filled in the "strings" variable</returns>
        public OperationResultAsLists GetTwoStageAuthenticationProviders()
        {
            return this.Channel.GetTwoStageAuthenticationProviders();
        }
        
        /// <summary>
        /// Gets a list of Text based upon the Text Identifiers send
        /// </summary>
        /// <param name="textIdentifiers">All the text identifiers you want the text for</param>
        /// <param name="patientId">The Id of the patient if available</param>
        /// <param name="registrationCode">The registration code</param>
        /// <returns>AN operation result indicating success or failure with the text inside the StringDictionary. Key is the Idenfitier, Value is the text</returns>        
        public OperationResultAsDictionary GetPageText(List<string> textIdentifiers, string patientId = null, string registrationCode = null)
        {
            return this.Channel.GetPageText(textIdentifiers, patientId, registrationCode);
        }

        /// <summary>
        /// Saves a given piece of text for a given Identifier
        /// </summary>
        /// <param name="textIdentifier">The identifier for the text</param>
        /// <param name="text">The text to save</param>
        /// <returns>An operation result indicating success or failure</returns>        
        public OperationResult SavePageText(string textIdentifier, string text)
        {
            return this.Channel.SavePageText(textIdentifier, text);
        }

        /// <summary>
        /// Returns a list of roles available.
        /// </summary>
        /// <returns>An operation result indicating success or failure and the roles filled in the Strings variable</returns>
        public OperationResultAsLists GetAvailableRoles()
        {
            return this.Channel.GetAvailableRoles();
        }
    }
}
