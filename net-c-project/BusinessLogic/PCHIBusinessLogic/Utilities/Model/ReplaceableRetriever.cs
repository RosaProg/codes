using DSPrima.TextReplaceable.Interfaces;
using PCHI.BusinessLogic.Properties;
using PCHI.DataAccessLibrary;
using PCHI.Model.Messages;
using System.Collections.Generic;
using System.Linq;

namespace PCHI.BusinessLogic.Utilities.Model
{
    /// <summary>
    /// Defines the IReplaceable instance used for the Text Replaceable library
    /// </summary>
    public class ReplaceableRetriever : IReplaceable<ReplaceableObjectKeys>
    {
        /// <summary>
        /// The AccessHandlerManager instance to use
        /// </summary>
        private AccessHandlerManager ahm;

        /// <summary>
        /// Defines the dictionary of objects that are used for parsing
        /// </summary>
        private Dictionary<ReplaceableObjectKeys, object> objectsForParsing;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplaceableRetriever"/> class
        /// </summary>
        /// <param name="ahm">The accessHandlerManager used to access the data layer</param>
        /// <param name="objectsForParsing">The dictionary of objects available for parsing</param>
        public ReplaceableRetriever(AccessHandlerManager ahm, Dictionary<ReplaceableObjectKeys, object> objectsForParsing)
        {
            this.ahm = ahm;
            this.objectsForParsing = objectsForParsing == null ? new Dictionary<ReplaceableObjectKeys, object>() : objectsForParsing;
        }

        /// <summary>
        /// Gets the list of Replaceable codes
        /// </summary>
        /// <returns>The list of codes to replace</returns>
        public List<IReplaceableCode<ReplaceableObjectKeys>> GetReplaceableCodes()
        {
            List<TextReplacementCode> codes = new List<TextReplacementCode>();
            codes.Add(new TextReplacementCode() { ReplacementCode = "<%hostname%/>", ReplacementValue = Settings.Default.WebsiteUrl.ToString(), UseReplacementValue = true });

            // TODO add more hardcoded values
            codes.AddRange(this.ahm.MessageHandler.GetReplacementCodes());

            return codes.Cast<IReplaceableCode<ReplaceableObjectKeys>>().ToList();
        }

        /// <summary>
        /// Gets the list of replaceables object and there key
        /// </summary>
        /// <returns>A dictionary containig the objects available and the type of object it is</returns>
        public Dictionary<ReplaceableObjectKeys, object> GetReplaceables()
        {
            return this.objectsForParsing;
        }
    }
}