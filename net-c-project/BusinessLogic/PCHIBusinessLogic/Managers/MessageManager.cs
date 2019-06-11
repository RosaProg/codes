using PCHI.BusinessLogic.Security;
using PCHI.BusinessLogic.Utilities;
using PCHI.BusinessLogic.Utilities.Model;
using PCHI.DataAccessLibrary;
using PCHI.Model.Messages;
using PCHI.Model.Security;
using PCHI.Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PCHI.BusinessLogic.Managers
{
    /// <summary>
    /// Defines a manager for dealing with any form of messaging
    /// </summary>
    public class MessageManager
    {
        /// <summary>
        /// Holds the <see cref="AccessHandlerManager"/> for internal use
        /// </summary>
        private AccessHandlerManager manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageManager"/> class
        /// </summary>
        /// <param name="manager">The <see cref="AccessHandlerManager"/> instance to use</param>
        internal MessageManager(AccessHandlerManager manager)
        {
            this.manager = manager;
        }

        /// <summary>
        /// Gets the Error related to the errorCode.
        /// Throws an exception if the error code doesn't exist
        /// </summary>
        /// <param name="errorCode">The code for the error to retrieve</param>
        /// <returns>The error instance</returns>
        public PCHIError GetError(ErrorCodes errorCode)
        {
            return this.manager.MessageHandler.GetError(errorCode);
        }

        /// <summary>
        /// Gets all the PageText instances in a Dictionary for all the given Identifiers.        
        /// </summary>
        /// <param name="textIdentifiers">The text identifiers to ge the text for</param>
        /// <param name="patientId">The Id of the patient if available</param>
        /// <returns>A Dictionary with the Key being the identifier and the Value being the text</returns>
        public Dictionary<string, string> GetPageText(List<string> textIdentifiers, string patientId = null, string registrationCode = null)
        {
            Dictionary<string, string> text = this.manager.MessageHandler.GetPageText(textIdentifiers);
            Patient patient = null;
            if (!string.IsNullOrWhiteSpace(patientId))
            {
                patient = this.manager.UserAccessHandler.FindPatient(patientId);
            }

            User user = null;
            if (!string.IsNullOrWhiteSpace(registrationCode))
            {
                user = new UserManager(this.manager).FindUserForRegistrationToken(registrationCode);
            }
            else if (SecuritySession.Current.User != null)
            {
                user = SecuritySession.Current.User;
            }

            TextParser parser = new TextParser(this.manager);
            var objects = new Dictionary<ReplaceableObjectKeys, object>();
            if (user != null) objects.Add(ReplaceableObjectKeys.User, user);
            if (patient != null) objects.Add(ReplaceableObjectKeys.Patient, patient);

            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (string key in text.Keys)
            {
                result.Add(key, parser.ParseText(text[key], objects));
            }

            return result;
        }

        /// <summary>
        /// Saves a given piece of Page Text to the database
        /// </summary>
        /// <param name="textIdentifier">The text identifier</param>
        /// <param name="text">the text</param>
        public void SavePageText(string textIdentifier, string text)
        {
            try
            {
                SecuritySession.Current.VerifyAccess(Actions.SAVE_PAGE_TEXT);
                this.manager.MessageHandler.SavePageText(textIdentifier, text);
                Logger.Audit(new Audit(Model.Security.Actions.SAVE_PAGE_TEXT, AuditEventType.ADD, typeof(PageText), "Identifier", textIdentifier));
            }
            catch (Exception ex)
            {
                Logger.Audit(new Audit(Model.Security.Actions.SAVE_PAGE_TEXT, AuditEventType.ADD, typeof(PageText), "Identifier", textIdentifier, false, ex.Message));
                throw ex;
            }
        }
    }
}