using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Research
{
    /// <summary>
    /// Defines a search condition
    /// </summary>
    [KnownType(typeof(SearchQuestionnaire))]
    [KnownType(typeof(SearchPatient))]
    [KnownType(typeof(SearchResponseGroup))]
    public abstract class SearchCondition : SearchObject
    {
        /// <summary>
        /// Gets the type to search on
        /// </summary>
        public abstract Type SearchType { get; }

        /// <summary>
        /// Gets or sets the 
        /// </summary>
        public Comparison Comparison { get; set; }

        /// <summary>
        /// Gets or sets the value to search on
        /// </summary>
        public string Value { get; set; }
    }
}
