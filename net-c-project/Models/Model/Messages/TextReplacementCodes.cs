using DSPrima.TextReplaceable.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Messages
{
    /// <summary>
    /// Defines an IReplaceableCode that can be used to define codes to be replaced with values in an object
    /// </summary>
    public class TextReplacementCode : IReplaceableCode<ReplaceableObjectKeys>
    {
        /// <summary>
        /// Gets or sets the database Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the code
        /// </summary>        
        [MaxLength(50), Index(IsUnique = false)]
        public string ReplacementCode { get; set; }

        /// <summary>
        /// Gets or sets the value to use in place of the code
        /// </summary>
        public string ReplacementValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the replacement value is to be used (true) or if the value comes from the code. (false)
        /// If false, only the Codes that are known by the Text parser can be replaced. The other ones are set to string.empty
        /// </summary>
        public bool UseReplacementValue { get; set; }

        /// <summary>
        /// Gets or sets the type of the object to find where the variable specified in the variable path is found
        /// </summary>
        public ReplaceableObjectKeys ObjectKey { get; set; }

        /// <summary>
        /// Gets or sets the variable path to walk in order to find the replacement value
        /// Starting at the object of the type found in ObjectType, the first entry in the path must be in that object (else no value is found)
        /// subsequent variable names must be seperated by =>.         
        /// </summary>
        public string ObjectVariablePath { get; set; }

        /// <summary>
        ///  Gets or sets a parameter for the ToString function
        /// </summary>
        public string ToStringParameter { get; set; }
    }
}
