using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire
{
    /// <summary>
    /// Indicates the platform something is suitable for. This can be used for Text, Format, etc.
    /// Supports Flags
    /// </summary>
    [Flags]
    public enum Platform
    {
        /// <summary>
        /// Defines the classic platform
        /// </summary>
        Classic = 1,

        /// <summary>
        /// Inicates the Chat platform
        /// </summary>
        Chat = 2,

        /// <summary>
        /// Indicates Mobile Platform
        /// </summary>
        Mobile = 4
    }
}
