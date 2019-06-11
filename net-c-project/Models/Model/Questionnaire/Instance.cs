using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire
{
    /// <summary>
    /// Defines the instance a certain element can be used for    
    /// Supports flags
    /// </summary>    
    [Flags]
    public enum Instance
    {
        /// <summary>
        /// Indicates the baseline (First time)
        /// </summary>
        Baseline = 1,

        /// <summary>
        /// Indicates a follow up (second time or more)
        /// </summary>
        Followup = 2
    }
}
