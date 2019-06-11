using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPrima.TextReplaceable.Interfaces
{
    /// <summary>
    /// Defines a basic structure for replaceable codes.
    /// </summary>
    /// <typeparam name="TKey">The enum that holds the object Keys</typeparam>
    public interface IReplaceableCode<TKey> where TKey : IComparable, IFormattable, IConvertible
    {
        /// <summary>
        /// Gets or sets the code
        /// </summary>                
        string ReplacementCode { get; set; }

        /// <summary>
        /// Gets or sets the value to use in place of the code
        /// </summary>
        string ReplacementValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the replacement value is to be used (true) or if the value comes from the code. (false)
        /// If false, only the Codes that are known by the Text parser can be replaced. The other ones are set to string.empty
        /// </summary>
        bool UseReplacementValue { get; set; }

        /// <summary>
        /// Gets or sets the key of the object to find where the variable specified in the variable path is found
        /// </summary>
        TKey ObjectKey { get; set; }

        /// <summary>
        /// Gets or sets the variable path to walk in order to find the replacement value
        /// Starting at the object found in ObjectKey, the first entry in the path must be in that object (else no value is found)
        /// subsequent variable names must be seperated by a full stop ".".         
        /// </summary>
        string ObjectVariablePath { get; set; }

        /// <summary>
        /// Gets or sets any ToString parameter
        /// If the to string function needs any parameter, add it here
        /// </summary>
        string ToStringParameter { get; set; }
    }
}
