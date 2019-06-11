using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Users
{
    /// <summary>
    /// Defines a tag for the patient
    /// </summary>
    public class PatientTag
    {
        /// <summary>
        /// Gets or sets the TagIdentifier of the <see cref="PatientTag"/>
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "A Patient Tag must have a name")]
        [MaxLength(50)]
        [Key, Column(Order = 0)]
        public string TagName { get; set; }

        /// <summary>
        /// Gets or sets the string value
        /// </summary>
        [MaxLength(500)]
        public string TextValue { get; set; }

        /// <summary>
        /// Gets or sets the patient Id
        /// </summary>
        [Key, Column(Order = 1), ForeignKey("Patient")]
        public string PatientId { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Patient"/> it has to map to
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "A Patient is required when specifying a PatientTag")]
        public Patient Patient { get; set; }
    }
}
