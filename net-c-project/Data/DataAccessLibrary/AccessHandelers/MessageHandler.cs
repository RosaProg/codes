using LinqKit;
using PCHI.DataAccessLibrary.Context;
using PCHI.Model.Messages;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace PCHI.DataAccessLibrary.AccessHandelers
{
    /// <summary>
    /// Handles restoring and retrieving of Messages and Errors
    /// </summary>
    public class MessageHandler
    {
        /// <summary>
        /// The context manager to use to get data from the database
        /// </summary>
        private MainDatabaseContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageHandler"/> class
        /// </summary>
        /// <param name="context">The <see cref="DatabaseContext"/> to use</param>
        internal MessageHandler(MainDatabaseContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets the Error related to the errorCode.
        /// Throws an exception if the error code doesn't exist
        /// </summary>
        /// <param name="errorCode">The code for the error to retrieve</param>
        /// <returns>The error instance</returns>
        public PCHIError GetError(ErrorCodes errorCode)
        {
            PCHIError err = this.context.ErrorsMessages.Where(e => e.ErrorCode == errorCode).SingleOrDefault();
            if (err == null)
            {
                err = this.context.ErrorsMessages.Where(e => e.ErrorCode == ErrorCodes.ERROR_DOES_NOT_EXIST).Single();
            }

            return err;
        }

        /// <summary>
        /// Retrieves all replacement codes from the database
        /// </summary>
        /// <returns>The list of replacement codes</returns>
        public List<TextReplacementCode> GetReplacementCodes()
        {
            return this.context.TextReplacementCodes.ToList();
        }

        /// <summary>
        /// Gets the Text definition with the given name
        /// </summary>
        /// <param name="textDefinitionName">The name of the text definition to find</param>
        /// <returns>The text definition found or null if not found</returns>
        public TextDefinition GetTextDefinitionByCode(string textDefinitionName)
        {
            return this.context.TextDefinitions.Where(td => td.DefinitionCode == textDefinitionName).SingleOrDefault();
        }

        /// <summary>
        /// Gets all the PageText instances in a Dictionary for all the given Identifiers.        
        /// </summary>
        /// <param name="textIdentifiers">The text identifiers to ge the text for</param>
        /// <returns>A Dictionary with the Key being the identifier and the Value being the text</returns>
        public Dictionary<string, string> GetPageText(List<string> textIdentifiers)
        {
            var pr = PredicateBuilder.False<PageText>();
            foreach (string ti in textIdentifiers)
            {
                pr = pr.Or(t => t.Identifier == ti);
            }

            var q = this.context.PageTexts.AsExpandable().Where(pr);
            return q.ToDictionary(t => t.Identifier, t => t.Text);
        }

        /// <summary>
        /// Saves a given piece of Page Text to the database
        /// </summary>
        /// <param name="textIdentifier">The text identifier</param>
        /// <param name="text">the text</param>
        public void SavePageText(string textIdentifier, string text)
        {
            this.context.PageTexts.AddOrUpdate(new PageText() { Identifier = textIdentifier, Text = text });
            this.context.SaveChanges();
        }
    }
}
