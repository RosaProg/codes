using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace PCHI.Model.Episodes
{
    /// <summary>
    /// Defines a Milestone met for an episode
    /// </summary>
    public class EpisodeMilestone
    {
        /// <summary>
        /// Gets or sets the database ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Episode
        /// </summary>
        public virtual Episode Episode { get; set; }

        /// <summary>
        /// Gets or sets the name of the milestone assigned         
        /// </summary>
        public Milestone Milestone { get; set; }

        /// <summary>
        /// Gets or sets the date and time a Milestone was assigned
        /// </summary>
        public DateTime MilestoneDate { get; set; }

        /// <summary>
        /// Gets or sets the Id of the practitioner with whom is the milestone
        /// </summary>
        public string PractitionerId { get; set; }

        /// <summary>
        /// Gets or sets the name of the practitioner with whom is the milestone
        /// This is only for convenience purposes and no longer mapped to the database.
        /// Instead, the practitioner is mapped to the user's external Id via the Practitioner Id
        /// </summary>
        [NotMapped]
        public string PractitionerName { get; set; }
    }
}
