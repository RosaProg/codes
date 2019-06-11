using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCHI.Model.Episodes
{
    /// <summary>
    /// Gets or sets the history of the statuses of an Episode
    /// </summary>
    public class EpisodeHistory
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
        /// Gets or sets the date and time the status was changed to the new status
        /// </summary>
        public DateTime StatusChanged { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether new status of the Episode is completed or not
        /// </summary>
        public bool NewIsCompletedStatus { get; set; }
    }
}
