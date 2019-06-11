using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire.Response
{
    /// <summary>
    /// Defines the different statuses the QuestionnaireUserResponseGroup can have
    /// </summary>
    public enum QuestionnaireUserResponseGroupStatus
    {
        /// <summary>
        /// Indicates the status is new 
        /// </summary>
        New,

        /// <summary>
        /// Indicates the status is in progress. At least one answer has already been given
        /// </summary>
        InProgress,

        /// <summary>
        /// Indicates the questionnaire has been completed
        /// </summary>
        Completed,

        /// <summary>
        /// Indicates the questionnaire has not been completed but the target data has been missed
        /// </summary>
        Missed
    }
}
