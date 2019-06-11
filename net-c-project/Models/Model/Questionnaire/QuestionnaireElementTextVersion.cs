using PCHI.Model.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire
{
    /// <summary>
    /// Indicates a text that can be used for an element
    /// </summary>
    public class QuestionnaireElementTextVersion : TextVersion
    {
        /// <summary>
        /// Gets or sets the QuestionnaireElement this belongs to
        /// </summary>
        public QuestionnaireElement QuestionnaireElement { get; set; }
    }
}
