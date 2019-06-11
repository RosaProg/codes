using PCHI.PMS.MessageDataLibrary.Context;
using PCHI.PMS.MessageDataLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.PMS.MessageDataLibrary
{
    /// <summary>
    /// Defines a message store
    /// </summary>
    public class MessageStore
    {
        /// <summary>
        /// Stores a message that was received
        /// </summary>
        /// <param name="messageReference">The message reference</param>
        /// <param name="message">The message</param>
        /// <param name="messageDateTime">The date and time the message was received.</param>
        public static void MessageReceived(string messageReference, string message, DateTime messageDateTime)
        {
            MessageStore.StoreMessage(new Message() { MessageReference = messageReference, MessageText = message, dateTimeOfMessage = messageDateTime, WasSent = false, Success = true });
        }

        /// <summary>
        /// Stores a message that was send
        /// </summary>
        /// <param name="messageReference">The message reference</param>
        /// <param name="message">The message</param>
        /// <param name="messageDateTime">The date and time the message was send.</param>
        /// <param name="success">Indicates if the message was successfully send</param>
        public static void MessageSend(string messageReference, string message, DateTime messageDateTime, bool success = true)
        {
            MessageStore.StoreMessage(new Message() { MessageReference = messageReference, MessageText = message, dateTimeOfMessage = messageDateTime, WasSent = true, Success = success });
        }

        /// <summary>
        /// Stores a message to the database
        /// </summary>
        /// <param name="m">The message to store</param>
        private static void StoreMessage(Message m)
        {
            MessageContext context = new MessageContext();
            context.Messages.Add(m);
            context.SaveChanges();
        }

        /// <summary>
        /// Checks if the given Message reference was already received
        /// </summary>
        /// <param name="messageReference">The message reference to check for</param>
        /// <returns>True if the message reference already exists, false otherwise</returns>
        public static bool WasMessageReceived(string messageReference)
        {
            return new MessageContext().Messages.Any(m => m.MessageReference == messageReference);
        }
    }
}
