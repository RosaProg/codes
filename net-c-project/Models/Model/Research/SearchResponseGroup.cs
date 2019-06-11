using PCHI.Model.Questionnaire.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Research
{
    /// <summary>
    /// Defines the fields that can be searched on for the QuestionnaireUserResponseGroup
    /// </summary>
    public enum SearchResponseGroupFields 
    { 
        /// <summary>
        /// Look for when the ResponseGroup was started
        /// </summary>
        DateTimeStarted, 

        /// <summary>
        /// Look for when the reponse group was completed
        /// </summary>
        DateTimeCompleted 
    }

    /// <summary>
    /// Defines the Search filter for a QuestionnaireUserResponseGroup
    /// </summary>
    public class SearchResponseGroup : SearchCondition
    {
        /// <summary>
        /// Gets or sets the field to search on
        /// </summary>
        public SearchResponseGroupFields SearchField { get; set; }

        /// <summary>
        /// Gets the type to search on
        /// </summary>
        public override Type SearchType
        {
            get { return typeof(QuestionnaireUserResponseGroup); }
        }
    }
}
