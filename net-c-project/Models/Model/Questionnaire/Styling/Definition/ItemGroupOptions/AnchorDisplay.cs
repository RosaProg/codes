using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire.Styling.Definition.ItemGroupOptions
{
    /// <summary>
    /// Defines how anchors are to be displayed if the Display Type of a group is Range
    /// </summary>
    public enum AnchorDisplay
    {
        /// <summary>
        /// The anchors are to be displayed before and after the Range data.
        /// There can be only two anchors in this case
        /// </summary>
        BeforeAfter,

        /// <summary>
        /// The anchors are displayed in the proper location inside the Range data
        /// There can be more then two anchors in this case
        /// </summary>
        Anchor
    }
}
