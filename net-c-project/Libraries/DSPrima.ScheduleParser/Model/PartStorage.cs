using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPrima.ScheduleParser.Model
{
    /// <summary>
    /// Holds storage for parts found in the schedule
    /// </summary>
    public class PartStorage
    {
        /// <summary>
        /// Gets or sets the type of the part stored
        /// </summary>
        public PartType Type { get; set; }

        /// <summary>
        /// Gets or sets the string value of the part stored
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the Injection
        /// </summary>
        public string Injection { get; set; }

        /// <summary>
        /// Gets or sets any indexes for the injection
        /// </summary>
        public List<int> Indexes { get; set; }

        /// <summary>
        /// Gets or sets the calculated date of the part.
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether something like "+3m*2" is used (true) or not (false)
        /// </summary>
        public bool OwnSection { get; set; }
    }
}
