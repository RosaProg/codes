using PCHI.Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Research
{
    /// <summary>
    /// Allows for searching on patient tags
    /// </summary>
    public class SearchPatient : SearchCondition
    {
        /// <summary>
        /// Gets or sets the name of the tag of the patient to search for
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// Gets the type to search on
        /// </summary>
        public override Type SearchType
        {
            get { return typeof(Patient); }
        }
    }
}
