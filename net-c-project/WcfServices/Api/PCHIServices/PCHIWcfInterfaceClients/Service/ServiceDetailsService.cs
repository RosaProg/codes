using DSPrima.WcfUserSession.Behaviours;
using PCHI.BusinessLogic;
using PCHI.WcfServices.API.PCHIServices.InterfaceClients.Base;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PCHI.WcfServices.API.PCHIServices.InterfaceClients.Service
{
    /// <summary>
    /// Implements functionality for obtaining details regarding the service and settings
    /// </summary>
    [WcfUserSessionBehaviour]
    public class ServiceDetailsService : BaseService, IServiceDetails
    {
        /// <summary>
        /// Gets or sets the time the service was start up
        /// </summary>
        public static DateTime StartUpTime { get; set; }

        /// <summary>
        /// Gets a list of the Authentication providers available for users
        /// </summary>
        /// <returns>An OperationResultAsLists indicating success or failure with the list of authentication providers available filled in the "strings" variable</returns>
        public OperationResultAsLists GetTwoStageAuthenticationProviders()
        {
            try
            {
                return new OperationResultAsLists(null) { Strings = this.handler.UserManager.TwoFactorProviders.Keys.ToList() };
            }
            catch (Exception ex)
            {
                return new OperationResultAsLists(ex);
            }
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
            try
            {
                return new OperationResultAsDictionary(null) { StringDictionary = this.handler.MessageManager.GetPageText(textIdentifiers, patientId, registrationCode) };
            }
            catch (Exception ex)
            {
                return new OperationResultAsDictionary(ex);
            }
        }

        /// <summary>
        /// Saves a given piece of text for a given Identifier
        /// </summary>
        /// <param name="textIdentifier">The identifier for the text</param>
        /// <param name="text">The text to save</param>
        /// <returns>An operation result indicating success or failure</returns>        
        public OperationResult SavePageText(string textIdentifier, string text)
        {
            try
            {
                this.handler.MessageManager.SavePageText(textIdentifier, text);
                return new OperationResult(null);
            }
            catch (Exception ex)
            {
                return new OperationResult(ex);
            }
        }

        /// <summary>
        /// Returns a list of roles available.
        /// </summary>
        /// <returns>An operation result indicating success or failure and the roles filled in the Strings variable</returns>
        public OperationResultAsLists GetAvailableRoles()
        {
            try
            {
                return new OperationResultAsLists(null) { Strings = this.handler.UserManager.GetAvailableRoles() };
            }
            catch (Exception ex)
            {
                return new OperationResultAsLists(ex);
            }
        }
    }
}
