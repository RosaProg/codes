using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire.Styling.Definition.ItemGroupOptions
{
    /// <summary>
    /// Containers supported methods for how to display Options for responding
    /// </summary>
    public enum ItemOptionDisplayType
    {
        /// <summary>
        /// Nothing is to be displayed for options
        /// </summary>
        None = 0,

        /// <summary>
        /// Display responses as a radiobutton
        /// </summary>
        RadioButton = 2,

        /// <summary>
        /// Display responses as a checkbox
        /// </summary>
        CheckBox = 3,

        /// <summary>
        /// Display responses in a dropdown
        /// </summary>
        DropDown = 4,

        /// <summary>
        /// Display responses using a slider
        /// </summary>
        Slider = 5,

        /// <summary>
        /// Display a textbox for each option
        /// </summary>
        TextBox = 6,

        /// <summary>
        /// Display a Text area (multiline text box) for each option
        /// </summary>
        TextArea = 7,

        /// <summary>
        /// Display responses as a checkbox with an image
        /// </summary>
        HiddenCheckBox = 8,

        /// <summary>
        /// Display responses as a using a Datepicker
        /// </summary>
        DatePicker = 9,

        /// <summary>
        /// Display textbox for password
        /// </summary>
        Password = 10
    }
}
