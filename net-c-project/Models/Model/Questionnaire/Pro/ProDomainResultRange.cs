using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire.Pro
{
    /// <summary>
    /// Defines a range of results for a Pro Domain. All point wihtin a range are inclusive
    /// </summary>
    public class ProDomainResultRange
    {
        /// <summary>
        /// Gets or sets the database Id of this Result Range
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the first point in a Result range
        /// </summary>
        public double Start { get; set; }

        /// <summary>
        /// Gets or sets the last point within the Result range. 
        /// </summary>
        public double End { get; set; }

        /// <summary>
        /// Gets or sets the meaning of this range
        /// </summary>
        [Required(ErrorMessage = "A Result Range must have a meaning")]
        public string Meaning { get; set; }

        /// <summary>
        /// Gets or sets a short version of the meaning. Predominately shown to the patient
        /// </summary>
        public string Qualifier { get; set; }

        /// <summary>
        /// Gets or sets the Domain that this Result range is a part of
        /// </summary>
        [Required(ErrorMessage = "A Pro Domain is required")]
        public virtual ProDomain Domain { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProDomainResultRange"/> class
        /// </summary>
        public ProDomainResultRange()
        {
        }
    }
}
