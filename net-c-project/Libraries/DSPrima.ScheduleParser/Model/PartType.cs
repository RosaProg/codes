using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPrima.ScheduleParser.Model
{
    /// <summary>
    /// Defines the types of parts there can be
    /// </summary>
    public enum PartType
    {
        /// <summary>
        /// Part is a day
        /// </summary>
        Day = 1,

        /// <summary>
        /// Part is a week
        /// </summary>
        Week = 2,

        /// <summary>
        /// Part is a Month
        /// </summary>
        Month = 3,

        /// <summary>
        /// Part is a Year
        /// </summary>
        Year = 4,

        /// <summary>
        /// Part is a Full date
        /// </summary>
        FullDate = 5,

        /// <summary>
        /// Part is a Injection
        /// </summary>
        Injection = 6
    }
}
