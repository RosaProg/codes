using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Episodes
{
    /// <summary>
    /// Defines a milestone and it's meaning
    /// </summary>
    public class Milestone
    {
        /// <summary>
        /// Gets or sets the name of the Milestone
        /// </summary>
        [Key, MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a description describing what a milestone is
        /// </summary>
        public string Description { get; set; }
    }
}
