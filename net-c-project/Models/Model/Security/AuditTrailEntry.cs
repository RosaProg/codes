using PCHI.Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Security
{
    /// <summary>
    /// Holds the result of an Audit search.
    /// This differs from the Audit Log as that is for storage and this is for easy use by clients for Display Purposes
    /// </summary>
    public class AuditTrailEntry
    {
        /// <summary>
        /// Gets or sets the User
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets the AuditLog entry
        /// </summary>
        public AuditLog AuditLog { get; set; }

        /// <summary>
        /// Gets or sets the Patient
        /// </summary>
        public Patient Patient { get; set; }

        /// <summary>
        /// Gets or sets the questionnaire
        /// </summary>
        public Questionnaire.Questionnaire Questionnaire { get; set; }

        /// <summary>
        /// Gets or sets the Episode
        /// </summary>
        public Episodes.Episode Episode { get; set; }

        /// <summary>
        /// Gets or sets the target user of an action
        /// </summary>
        public Users.User TargetUser { get; set; }
    }
}
