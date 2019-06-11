using Microsoft.AspNet.Identity;
using PCHI.BusinessLogic.Utilities;
using PCHI.DataAccessLibrary;
using PCHI.Model.Messages;
using System.Threading.Tasks;

namespace PCHI.BusinessLogic.IIdentityMessageServices
{
    /// <summary>
    /// IIdentifyMessageService that can be used for sending emails
    /// </summary>
    public class EmailService : IIdentityMessageService
    {
        /// <summary>
        /// Sends an email message asynchronously
        /// Gets the subject and body from the MessageHandler as Text Definitions. The definitions requested are based upon the message.Subject and message.Body respectively
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <returns>The async message</returns>
        public Task SendAsync(IdentityMessage message)
        {            
            AccessHandlerManager manager = new AccessHandlerManager();
            TextDefinition subject = manager.MessageHandler.GetTextDefinitionByCode(message.Subject);
            TextDefinition bodyFormat = manager.MessageHandler.GetTextDefinitionByCode(message.Body);

            SmtpMailClient.SendMail(message.Destination, subject == null ? message.Subject : subject.Text, bodyFormat == null ? message.Body : bodyFormat.Text, bodyFormat == null ? message.Body : bodyFormat.Html);

            return Task.FromResult(0);
        }
    }
}