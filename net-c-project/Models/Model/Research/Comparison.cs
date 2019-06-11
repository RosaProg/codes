using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Research
{
    /// <summary>
    /// Defines the available comparison options
    /// </summary>
    public enum Comparison
    {
        /// <summary>
        /// The two value must match
        /// </summary>
        Equals = 1,

        /// <summary>
        /// The two values must not match
        /// </summary>
        NotEquals = 2,

        /// <summary>
        /// The value in the database must be smaller than the given value
        /// </summary>
        SmallerThan = 3,

        /// <summary>
        /// The value in the database must be smaller than or equals the given value
        /// </summary>
        SmallerOrEquals = 4,

        /// <summary>
        /// The value in the database must be greater than the given value
        /// </summary>
        GreaterThan = 5,

        /// <summary>
        /// The value in the database must be greater than or equals the given value
        /// </summary>
        GreaterOrEquals = 6
    }
}
