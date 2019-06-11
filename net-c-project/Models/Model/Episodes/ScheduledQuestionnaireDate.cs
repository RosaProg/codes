using PCHI.Model.Questionnaire.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PCHI.Model.Episodes
{
    /// <summary>
    /// Defines a resolved patient schedule. 
    /// This resolved date/schedule is not final unless it has already been executed
    /// </summary>
    public class ScheduledQuestionnaireDate
    {
        /// <summary>
        /// Gets or sets the database ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the AssignedQuestionnaire this schedule belongs to
        /// </summary>
        [Required(ErrorMessage = "A questionnaire must be assigned to a scheduled date")]
        public virtual AssignedQuestionnaire AssignedQuestionnaire { get; set; }

        /// <summary>
        /// Gets or sets the schedule string for this resolved schedule
        /// </summary>
        public string ScheduleString { get; set; }

        /// <summary>
        /// Gets or sets the date that was calculated already. 
        /// Can be NULL if the schedule can't be calculated yet
        /// </summary>
        public DateTime? CalculatedDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the scheduler has already parsed and send out the Pro belonging to this schedule
        /// </summary>
        public bool ScheduleHasBeenExecuted { get; set; }

        /// <summary>
        /// Gets or sets the ResponseGroup assigned to this Scheduled Date
        /// </summary>
        public virtual QuestionnaireUserResponseGroup ResponseGroup { get; set; }
    }
}
