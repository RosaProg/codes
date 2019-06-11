using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Research
{
    /// <summary>
    /// Allows searching on the name of the questionnaire
    /// </summary>
    public class SearchQuestionnaire : SearchCondition
    {
        /// <summary>
        /// Gets the type to search on
        /// </summary>
        public override Type SearchType
        {
            get
            {
                return typeof(Questionnaire.Pro.ProInstrument);
            }
        }
    }
}
