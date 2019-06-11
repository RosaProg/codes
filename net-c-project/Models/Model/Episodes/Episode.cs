using PCHI.Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Episodes
{
    /// <summary>
    /// Defines an Episode for a Patient
    /// </summary>
    public class Episode
    {
        /// <summary>
        /// Gets or sets the database ID of this Episode
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Patient for this Episode
        /// </summary>
        public virtual Patient Patient { get; set; }

        /// <summary>
        /// Gets or sets the condition assigned to this Episode
        /// </summary>
        public string Condition { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not this Episode has been completed
        /// </summary>
        public bool IsCompletedStatus { get; set; }

        /// <summary>
        /// Gets or sets the Episode History
        /// </summary>
        public virtual ICollection<EpisodeHistory> EpisodeHistory { get; set; }

        /// <summary>
        /// Gets or sets the Diagnosis codes assigned to this Episode
        /// </summary>
        public virtual ICollection<DiagnosisCode> DiagnosisCodes { get; set; }
        
        /// <summary>
        /// Gets or sets the Treatment codes assigned to this questionnaire
        /// </summary>
        public virtual ICollection<TreatmentCode> TreatmentCodes { get; set; }

        /// <summary>
        /// Gets or sets the collection of milestones that have been met
        /// </summary>
        public virtual ICollection<EpisodeMilestone> MileStones { get; set; }

        /// <summary>
        /// Gets or sets the list of assigned questionnaires
        /// </summary>
        public virtual ICollection<AssignedQuestionnaire> AssignedQuestionnaires { get; set; }

        /// <summary>
        /// Gets or sets the external Id of an Episode in what ever patients management system is in use
        /// </summary>
        public string ExternalId { get; set; }

        /// <summary>
        /// Gets or sets the data and time the episode was created
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Episode"/> class
        /// </summary>
        public Episode()
        {
            this.EpisodeHistory = new List<EpisodeHistory>();
            this.DiagnosisCodes = new List<DiagnosisCode>();
            this.TreatmentCodes = new List<TreatmentCode>();
            this.MileStones = new List<EpisodeMilestone>();
            this.AssignedQuestionnaires = new List<AssignedQuestionnaire>();
            this.DateCreated = DateTime.Now;
        }        
    }
}
