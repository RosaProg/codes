using PCHI.Model.Questionnaire.Pro;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire.Response
{
    /// <summary>
    /// Holds a result for a ProDomain and patient
    /// </summary>
    public class ProDomainResult
    {
        /// <summary>
        /// Gets or sets the database Id for this Result
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the ProDomainResultSet this belongs to
        /// </summary>
        public virtual ProDomainResultSet ProDomainResultSet { get; set; }

        /// <summary>
        /// Gets or sets The foreign key version of the ProDomain. 
        /// Either set this propery or the Domain property for storing, you don't need to set both
        /// </summary>
        [ForeignKey("Domain")]
        public virtual int DomainId { get; set; }

        /// <summary>
        /// Gets or sets the ProDomain
        /// </summary>
        public virtual ProDomain Domain { get; set; }

        /// <summary>
        /// Gets or sets the calculated score
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// Gets or sets the list of responses
        /// </summary>
        [NotMapped]
        public List<QuestionnaireResponse> Responses { get; set; }

        /// <summary>
        /// Gets the description (meaning) for the score for the domain
        /// </summary>
        /// <returns>The decription of the score</returns>
        public string ScoreDescription()
        {
            return this.Domain.ResultRanges.Where(r => (this.Score >= r.Start) && (this.Score <= r.End)).Single().Meaning;
        }
    }
}
