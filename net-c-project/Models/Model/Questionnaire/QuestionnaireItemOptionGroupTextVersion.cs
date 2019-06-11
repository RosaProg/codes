using PCHI.Model.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire
{
    /// <summary>
    /// Defines a TextVersion for a QuestionnaireItemOptionGroup
    /// </summary>
    public class QuestionnaireItemOptionGroupTextVersion : TextVersion
    {
        /// <summary>
        /// Gets or sets the QuestionnaireElement this belongs to
        /// </summary>
        public QuestionnaireItemOptionGroup QuestionnaireItemOptionGroup { get; set; }
    }
}
