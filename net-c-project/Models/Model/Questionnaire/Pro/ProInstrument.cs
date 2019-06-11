using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire.Pro
{
    /// <summary>
    /// Defines a Pro Instrument type of Questionnaire
    /// </summary>
    public class ProInstrument : Questionnaire
    {
        /// <summary>
        /// Gets or sets the <see cref="ProDomain"/> that belong to this Instrument
        /// </summary>        
        public virtual ICollection<ProDomain> Domains { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProInstrument"/> class
        /// </summary>
        public ProInstrument() : base()
        {
            this.Domains = new List<ProDomain>();
        }
    }
}
