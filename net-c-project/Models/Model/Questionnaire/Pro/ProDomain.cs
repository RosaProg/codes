using PCHI.Model.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire.Pro
{
    /// <summary>
    /// Defines a Domain within a Pro Instrument. 
    /// A domain defines what a possible result of a Pro means based upon (some of) the Items inside the Pro
    /// </summary>
    public class ProDomain
    {
        /// <summary>
        /// Gets or sets the database Id of the ProDomain
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Name of the Pro Domain
        /// </summary>
        [Required(ErrorMessage = "A Pro Domain needs a name")]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Description of the Pro Domain
        /// </summary>
        [Required(ErrorMessage = "A ProDomain needs a description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets teh Instrument this domain is a part of
        /// </summary>
        [Required(ErrorMessage = "A Pro Domain has to be part of an Instrument")]
        public virtual Questionnaire Instrument { get; set; }

        /// <summary>
        /// Gets or sets the score formula for this Domain
        /// </summary>
        public string ScoreFormula { get; set; }

        /// <summary>
        /// Gets or sets the Result ranges that belong to this Pro Domain
        /// </summary>
        public virtual ICollection<ProDomainResultRange> ResultRanges { get; set; }

        /// <summary>
        /// Gets or sets the type of users this is domain visible to.
        /// </summary>
        public UserTypes Audience { get; set; }

        /// <summary>
        /// Gets or sets the clarification for scoring clarification such as "Higher is Better"
        /// </summary>
        public string ScoringNote { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Higher is a better value (true) or not (false)
        /// </summary>
        public bool HigherIsBetter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not this domain is a total (true) or a detailed (false) domain
        /// </summary>
        public bool IsTotalDomain { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProDomain"/> class
        /// </summary>
        public ProDomain()
        {
            this.Audience = UserTypes.Patient | UserTypes.Physician;
            this.ResultRanges = new List<ProDomainResultRange>();
        }

        /// <summary>
        /// Gets the list of Action Ids that belong to the internal formula
        /// </summary>
        [NotMapped]
        public List<string> ItemActionIds
        {
            get
            {
                if (this.ScoreFormula == null) return new List<string>();
                Regex exp = new Regex(@"\{.*?\}");
                return exp.Matches(this.ScoreFormula).Cast<Match>().Select(a => a.Value.Replace("{", string.Empty).Replace("}", string.Empty)).ToList();                        
            }
        }
    }
}
