using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire
{
    /// <summary>
    /// Indicates a response that can be given to a ProItem
    /// </summary>
    public class QuestionnaireItemOption
    {
        /// <summary>
        /// Gets or sets the database Id of this <see cref="QuestionnaireItemOption"/>
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Id of an option that may be displayed
        /// </summary>
        public string DisplayId { get; set; }

        /// <summary>
        /// Gets or sets the Text of this option
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the value of this option
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets a possible action as a result of selecting this option
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the order of the Option in the OptionGroup
        /// This is set upon storing and should not be set manually.
        /// </summary>
        public int OrderInGroup { get; set; }

        /// <summary>
        /// Gets or sets the default value for the Option. 
        /// This overwrites the Default Value in the OptionGroup
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets the ID for mapping this Option to the same Option accross versions of the Questionnaire        
        /// </summary>        
        public string ActionId { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="QuestionnaireItemOptionGroup"/> this Option is a part of
        /// </summary>
        [Required(ErrorMessage = "An option must belong to an option group")]
        public virtual QuestionnaireItemOptionGroup Group { get; set; }
    }
}
