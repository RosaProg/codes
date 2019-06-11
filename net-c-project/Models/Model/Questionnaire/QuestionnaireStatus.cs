using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire
{
    /// <summary>
    /// Indicates the possible statuses of a ProInstrument
    /// </summary>
    public enum QuestionnaireStatus
    {
        /// <summary>
        /// Indicates the ProInstrument is still under development and not ready to be assigned to Patients
        /// </summary>
        Indevelopment,

        /// <summary>
        /// Indicates the ProInstrument is ready, development of it has completed and may be assigned to patients. It has not been validated yet however.
        /// </summary>
        Ready,

        /// <summary>
        /// Indicates the ProInstrument is read and validated and can be assigned to patients. It can not be modified anymore after this status has been reached
        /// </summary>
        Validated,
    }
}
