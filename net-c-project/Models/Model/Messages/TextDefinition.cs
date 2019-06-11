using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Messages
{
    /// <summary>
    /// Holds a text definition with an Text and HTML equivalent
    /// </summary>
    public class TextDefinition
    {
        /// <summary>
        /// Gets or sets the Definition Name 
        /// </summary>
        [Key, MaxLength(50)]        
        public string DefinitionCode { get; set; }

        /// <summary>
        /// Gets or sets the text version of this definition
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the Html version of this definition
        /// </summary>
        public string Html { get; set; }
    }
}
