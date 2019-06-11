using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire.Styling.Presentation
{
    /// <summary>
    /// Holds any attributes a Questionnaire Format may have
    /// </summary>
    [Flags]
    public enum QuestionnaireFormatAttributes
    {
        /// <summary>
        /// No attributes are specified
        /// </summary>
        None = 0,

        /// <summary>
        /// The progress bar should be hidden
        /// </summary>
        HideProgressBar = 1
    }
}
