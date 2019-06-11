using PCHI.BusinessLogic.Utilities;
using PCHI.DataAccessLibrary;
using PCHI.Model.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.BusinessLogic.TokenProviders
{
    /// <summary>
    /// A token provider for sending via email
    /// </summary>
    public class EmailTokenProvider : UserTokenProvider
    {
        /// <summary>
        /// Gets or sets the Text definition for the subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the text definition for the body format
        /// </summary>
        public string BodyFormat { get; set; }

        /// <summary>
        /// Gets or sets the manager used to get the data from
        /// </summary>
        public AccessHandlerManager Manager { get; set; }

        /// <summary>
        /// Notifies the given user of the token
        /// </summary>
        /// <param name="token">The token to send</param>
        /// <param name="manager">The manger that requested it</param>
        /// <param name="user">The user to send the token to</param>
        /// <returns>A task that is sending the token</returns>
        public override Task NotifyAsync(string token, Microsoft.AspNet.Identity.UserManager<Model.Users.User, string> manager, Model.Users.User user)
        {
            TextParser parser = new TextParser(this.Manager);
            TextDefinition subject = parser.ParseMessage(this.Subject, new Dictionary<ReplaceableObjectKeys, object>() { { ReplaceableObjectKeys.Code, token }, { ReplaceableObjectKeys.User, user } });
            TextDefinition body = parser.ParseMessage(this.BodyFormat, new Dictionary<ReplaceableObjectKeys, object>() { { ReplaceableObjectKeys.Code, token }, { ReplaceableObjectKeys.User, user } });
            new TaskFactory().StartNew(() => { SmtpMailClient.SendMail(user.Email, subject.Text, body.Text, body.Html); });

            return Task.FromResult<int>(0);
        }
    }
}
