using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPrima.TextReplaceable.Interfaces
{
    /// <summary>
    /// Defines an IReplaceable object
    /// </summary>
    /// <typeparam name="TKey">The identifier of objects that can be used to replace values in text</typeparam>
    public interface IReplaceable<TKey> where TKey : IComparable, IFormattable, IConvertible
    {
        /// <summary>
        /// Gets the list of Replaceable codes
        /// </summary>
        /// <returns>The list of codes to replace</returns>
        List<IReplaceableCode<TKey>> GetReplaceableCodes();

        /// <summary>
        /// Gets the list of replaceables object and there key
        /// </summary>
        /// <returns>A dictionary containig the objects available and the type of object it is</returns>
        Dictionary<TKey, object> GetReplaceables();
    }
}
