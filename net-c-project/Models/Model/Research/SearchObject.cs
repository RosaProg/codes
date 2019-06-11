using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Research
{
    /// <summary>
    /// Defines a Search Object base class
    /// </summary>
    [KnownType(typeof(SearchGroup))]
    [KnownType(typeof(SearchCondition))]
    public abstract class SearchObject
    {
    }
}
