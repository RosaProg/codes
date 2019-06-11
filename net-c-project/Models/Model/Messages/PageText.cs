using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Messages
{
    /// <summary>
    /// Defines a piece of text for on the page
    /// </summary>
    public class PageText
    {
        /// <summary>
        /// Gets or sets the Text Identifier
        /// </summary>
        [Key, MaxLength(255)]
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the text
        /// </summary>
        public string Text { get; set; }
    }
}
