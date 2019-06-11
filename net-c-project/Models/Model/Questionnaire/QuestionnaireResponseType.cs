using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCHI.Model.Questionnaire
{
    /// <summary>
    /// Enum that indicates the Type of response expected
    /// </summary>
    public enum QuestionnaireResponseType
    {
        /// <summary>
        /// Indicates the Response should be generated between a certain Range. 
        /// </summary>
        Range,

        /// <summary>
        /// Indicates the Response should be generated with a conditional question. 
        /// </summary>
        ConditionalItem,

        /// <summary>
        /// Indicates the Response should be a selection from a list of these responses
        /// </summary>
        List,

        /// <summary>
        /// Indicates the Response is Text typed in a text field or area
        /// </summary>
        Text,

        /// <summary>
        /// Indicates there can be more then one answer per option group
        /// </summary>
        MultiSelect
    }
}
