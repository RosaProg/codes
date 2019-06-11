using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire.Response
{
    /// <summary>
    /// Gets or sets the result set for a ProDomain
    /// </summary>
    public class ProDomainResultSet
    {
        /// <summary>
        /// Gets or sets the Database Id of this result set
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Id of the QuestionnaireUserResponseGroup this result set is for
        /// </summary>
        [ForeignKey("Group")]
        public int GroupId { get; set; }

        /// <summary>
        /// Gets or sets the QuestionnaireUserResponseGroup that this result set is for
        /// </summary>
        public virtual QuestionnaireUserResponseGroup Group { get; set; }

        /// <summary>
        /// Gets the time the responses for where first filled in
        /// </summary>
        [NotMapped]
        public DateTime GroupStartTime { get { return this.Group.StartTime.HasValue ? this.Group.StartTime.Value : this.Group.DatetimeCreated; } }

        /// <summary>
        /// Gets the time the group was submitted.
        /// </summary>
        [NotMapped]
        public DateTime GroupEndTime { get { return this.Group.DateTimeCompleted.Value; } }

        /// <summary>
        /// Gets or sets the list of results
        /// </summary>
        public virtual ICollection<ProDomainResult> Results { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProDomainResultSet"/> class
        /// </summary>
        public ProDomainResultSet()
        {
            this.Results = new List<ProDomainResult>();
        }
    }
}
