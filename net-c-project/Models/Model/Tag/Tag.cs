using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Tag
{
    /// <summary>
    /// Defines data for a tag
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// Gets or sets the TagIdentifier of the <see cref="Tag"/>
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "A Tag must have a name")]
        [MaxLength(50)]
        [Key, Column(Order = 0)]
        public string TagName { get; set; }

        /// <summary>
        /// Gets or sets the Value of the <see cref="Tag"/>
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "A Tag must have a value")]
        [MaxLength(50)]
        [Key, Column(Order = 1)]
        public string Value { get; set; }
    }
}
