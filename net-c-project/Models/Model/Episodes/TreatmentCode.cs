using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCHI.Model.Episodes
{
    /// <summary>
    /// Holds a treament code assigned to an episode
    /// </summary>
    public class TreatmentCode
    {
        /// <summary>
        /// Gets or sets the database ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Episode this is assigned to
        /// </summary>
        public Episode Episode { get; set; }

        /// <summary>
        /// Gets or sets the code set
        /// </summary>
        public string Code { get; set; }
    }
}
