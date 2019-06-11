using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire
{
    /// <summary>
    /// Defines the attributes that can be set on an Item using AND and OR operators.    
    /// </summary>
    [Flags]
    public enum QuestionnaireItemAttributes
    {
        /// <summary>
        /// There are no additional attributes
        /// </summary>
        None = 0,

        /// <summary>
        /// The item can be skipped
        /// </summary>
        CanSkip = 1,

        /// <summary>
        /// When set, the Item should not be editable
        /// </summary>
        PreventEdit = 2
    }
}
