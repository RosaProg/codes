using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCHI.Model.Episodes
{
    /// <summary>
    /// Defines a questionnaire that has been assigned to a questionnaire
    /// </summary>
    public class AssignedQuestionnaire
    {
        /// <summary>
        /// Gets or sets the Database Id of the User Entity
        /// </summary>        
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Episode this is assigned to
        /// </summary>
        public virtual Episode Episode { get; set; }

        /// <summary>
        /// Gets or sets the name of the questionnaire assigned to the Episode
        /// </summary>
        public string QuestionnaireName { get; set; }

        /// <summary>
        /// Gets or sets the schedule string definition assigned
        /// </summary>
        public string ScheduleString { get; set; }

        /// <summary>
        /// Gets or sets the collection of calculated Patient Schedules 
        /// </summary>
        public virtual ICollection<ScheduledQuestionnaireDate> Schedules { get; set; }
    }
}
