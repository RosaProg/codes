using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Research
{
    /// <summary>
    /// Defines a basic search group with a certain operator
    /// </summary>
    public class SearchGroup : SearchObject
    {
        /// <summary>
        /// Gets or sets a list of children for this Search Object
        /// </summary>
        public List<SearchObject> Children { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the AND operator (true) should be used or the OR operator (false)
        /// </summary>
        public bool IsAndOperator { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchGroup"/> class
        /// </summary>
        public SearchGroup()
        {
            this.Children = new List<SearchObject>();
        }
    }
}
