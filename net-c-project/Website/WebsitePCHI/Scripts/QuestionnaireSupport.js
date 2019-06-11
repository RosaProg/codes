/*  
*   Defines all the functions that completes the proper functionality and
*   behaviour of  a questionnaire
*/

function checkCompatibility() {
    /// <summary>
    /// Check compatibility of the current browser with HTML5 elements
    /// </summary>
    if (!Modernizr.inputtypes.range) {
        $("[id^='Support.Slider']");
        var inputRanges = $("input[name^='Slider']");
        $.each(inputRanges, function (index, inputRange) {
            $(document).ready(function () {
                $("[name^='Slider']").remove();
                var javascriptSliders = $("[id^='Support.Slider']");
                javascriptSliders.each(function (index) {
                    var options = $(this).attr("title").split("|");
                    var recoveredOptions = new Array();
                    for (var e = 0; e < options.length; e += 2) {
                        recoveredOptions[options[e]] = options[e + 1];
                    };
                    ((recoveredOptions["value"] == "") ? recoveredOptions["value"] = 0 : recoveredOptions["value"]);
                    //$(this).attr("id") = $(this).attr("id").replace("Support.", "");
                    $(this).slider(
                    {
                        change: function (event, ui) { $("[name='Span." + sliderToUpdate + "']").html($("[id='" + $(this).attr("id") + "']").slider("value")); },
                        max: parseInt(recoveredOptions["max"]),
                        min: parseInt(recoveredOptions["min"]),
                        step: parseInt(recoveredOptions["step"]),
                        value: parseInt(recoveredOptions["value"]),
                        change: function (event, ui) { eval(recoveredOptions["onchange"]); }
                    });
                    if (recoveredOptions["disabled"] === "true") {
                        $(this).slider("option", "disabled", true);
                    }
                });
            });
        });
    }
    if (!Modernizr.inputtypes.date) {
        var inputDates = $("input[name^='DatePicker']");
        $(inputDates).each(function (index) {
            $(this).datepicker({ dateFormat: "yy-mm-dd" });
        });
    }
}

function ConditionalItemBehaviour(itemId, elementName, optionId) {
    /// <summary>
    /// Disables all the options from the given item when the patient decided that the item
    /// does not apply 
    /// </summary>
    /// <param name="itemId">The Id of the item which the options will be disabled or enabled</param>
    /// <param name="elementId">The name of the element that is calling this function</param>
    /// <param name="optionId">The ID of the option that is calling this function</param>
    ///Get the option in the DOM that is calling this function
    var option = $("input[name='" + elementName + "'][value*='" + optionId + "']");
    var makeItemNotApplicable;
    if (option.attr("data-applicable") == "MakeItemNotApplicable") {
        makeItemNotApplicable = true;
    } else if (option.attr("data-applicable") == "MakeItemApplicable") {
        makeItemNotApplicable = false;
    }
    $("input[name*='" + itemId + "']").filter(function () {
        var matching = new RegExp("^[a-z,A-Z]+." + itemId + ".");
        return $(this).attr("name").match(matching);
    }).each(function () {
        if (!$(this).attr("data-applicable"))
            if (makeItemNotApplicable) {
                $(this).attr("disabled", "true");
                $(this).attr("class", "item-disabled");
            } else {
                $(this).removeAttr("disabled");
                $(this).removeAttr("class");
            }
    });

    ///If the option that is calling this function is a checkbox we change it's attibute to make possible
    ///switch between enabling and disabling
    if (option.attr("type") == "checkbox") {
        if (option.attr("data-applicable") == "MakeItemNotApplicable") {
            option.attr("value", optionId + ".MakeItemApplicable");
            option.attr("data-applicable", "MakeItemApplicable");
        } else if (option.attr("data-applicable") == "MakeItemApplicable") {
            option.attr("value", optionId + ".MakeItemNotApplicable");
            option.attr("data-applicable", "MakeItemNotApplicable");
        }
    }
};

//Variable to show the options the patient has selected for each body part
var partsOfTheBodyToolTip = {
    headFront: ['<div><b>head (front part)</b></div>', '', '', '', '', ''],
    neckFront: ['<div><b>neck (front part)</b></div>', '', '', '', '', ''],
    rightShoulderFront: ['<div><b>right shoulder (front part)</b></div>', '', '', '', '', ''],
    leftShoulderFront: ['<div><b>left shoulder (front part)</b></div>', '', '', '', '', ''],
    chestFront: ['<div><b>chest</b></div>', '', '', '', '', ''],
    rightArmFront: ['<div><b>right arm (front part)</b></div>', '', '', '', '', ''],
    leftArmFront: ['<div><b>left arm (front part)</b></div>', '', '', '', '', ''],
    rightElbowFront: ['<div><b>right elbow (front part)</b></div>', '', '', '', '', ''],
    leftElbowFront: ['<div><b>left elbow (front part)</b></div>', '', '', '', '', ''],
    rightForearmFront: ['<div><b>right forearm (front part)</b></div>', '', '', '', '', ''],
    leftForearmFront: ['<div><b>left forearm (front part)</b></div>', '', '', '', '', ''],
    rightWristFront: ['<div><b>right wrist (front part)</b></div>', '', '', '', '', ''],
    leftWristFront: ['<div><b>left wrist (front part)</b></div>', '', '', '', '', ''],
    abdomenFront: ['<div><b>abdomen (front part)</b></div>', '', '', '', '', ''],
    rightHipFront: ['<div><b>right hip (front part)</b></div>', '', '', '', '', ''],
    leftHipFront: ['<div><b>left hip (front part)</b></div>', '', '', '', '', ''],
    groinFront: ['<div><b>groin (front part)</b></div>', '', '', '', '', ''],
    rightThighFront: ['<div><b>right thigh (front part)</b></div>', '', '', '', '', ''],
    leftThighFront: ['<div><b>left thigh (front part)</b></div>', '', '', '', '', ''],
    rightKneeFront: ['<div><b>right knee (front part)</b></div>', '', '', '', '', ''],
    leftKneeFront: ['<div><b>left knee (front part)</b></div>', '', '', '', '', ''],
    rightLegFront: ['<div><b>right leg (front part)</b></div>', '', '', '', '', ''],
    leftLegFront: ['<div><b>left leg (front part)</b></div>', '', '', '', '', ''],
    rightAnkleFront: ['<div><b>right ankle (front part)</b></div>', '', '', '', '', ''],
    leftAnkleFront: ['<div><b>left ankle (front part)</b></div>', '', '', '', '', ''],
    rightFootFront: ['<div><b>right foot (front part)</b></div>', '', '', '', '', ''],
    leftFootFront: ['<div><b>left foot (front part)</b></div>', '', '', '', '', ''],
    rightHandFront: ['<div><b>right hand (front part)</b></div>', '', '', '', '', ''],
    leftHandFront: ['<div><b>left hand (front part)</b></div>', '', '', '', '', ''],
    headBack: ['<div><b>head (back part)</b></div>', '', '', '', '', ''],
    neckBack: ['<div><b>neck (back part)</b></div>', '', '', '', '', ''],
    rightShoulderBack: ['<div><b>right shoulder (back part)</b></div>', '', '', '', '', ''],
    leftShoulderBack: ['<div><b>left shoulder (back part)</b></div>', '', '', '', '', ''],
    thoracicSpineBack: ['<div><b>thoracic spine</b></div>', '', '', '', '', ''],
    rightArmBack: ['<div><b>right arm (back part)</b></div>', '', '', '', '', ''],
    leftArmBack: ['<div><b>left arm (back part)</b></div>', '', '', '', '', ''],
    rightElbowBack: ['<div><b>right elbow (back part)</b></div>', '', '', '', '', ''],
    leftElbowBack: ['<div><b>left elbow (back part)</b></div>', '', '', '', '', ''],
    rightForearmBack: ['<div><b>right forearm (back part)</b></div>', '', '', '', '', ''],
    leftForearmBack: ['<div><b>left forearm (back part)</b></div>', '', '', '', '', ''],
    rightWristBack: ['<div><b>right wrist (back part)</b></div>', '', '', '', '', ''],
    leftWristBack: ['<div><b>left wrist (back part)</b></div>', '', '', '', '', ''],
    lowerBack: ['<div><b>lower back</b></div>', '', '', '', '', ''],
    rightButtockBack: ['<div><b>right buttock</b></div>', '', '', '', '', ''],
    leftButtockBack: ['<div><b>left buttock</b></div>', '', '', '', '', ''],
    rightThighBack: ['<div><b>right thigh (back part)</b></div>', '', '', '', '', ''],
    leftThighBack: ['<div><b>left thigh (back part)</b></div>', '', '', '', '', ''],
    rightKneeBack: ['<div><b>right knee (back part)</b></div>', '', '', '', '', ''],
    leftKneeBack: ['<div><b>left knee (back part)</b></div>', '', '', '', '', ''],
    rightLegBack: ['<div><b>right leg (back part)</b></div>', '', '', '', '', ''],
    leftLegBack: ['<div><b>left leg (back part)</b></div>', '', '', '', '', ''],
    rightAnkleBack: ['<div><b>right ankle (back part)</b></div>', '', '', '', '', ''],
    leftAnkleBack: ['<div><b>left ankle (back part)</b></div>', '', '', '', '', ''],
    rightFootBack: ['<div><b>right foot (back part)</b></div>', '', '', '', '', ''],
    leftFootBack: ['<div><b>left foot (back part)</b></div>', '', '', '', '', ''],
    rightHandBack: ['<div><b>right hand (back part)</b></div>', '', '', '', '', ''],
    leftHandBack: ['<div><b>left hand (back part)</b></div>', '', '', '', '', '']
};

//Indicates which body part has what discomfortness
var partsOfTheBodyChecked = {
    headFront: ['', '', '', '', ''],
    neckFront: ['', '', '', '', ''],
    rightShoulderFront: ['', '', '', '', ''],
    leftShoulderFront: ['', '', '', '', ''],
    chestFront: ['', '', '', '', ''],
    rightArmFront: ['', '', '', '', ''],
    leftArmFront: ['', '', '', '', ''],
    rightElbowFront: ['', '', '', '', ''],
    leftElbowFront: ['', '', '', '', ''],
    rightForearmFront: ['', '', '', '', ''],
    leftForearmFront: ['', '', '', '', ''],
    rightWristFront: ['', '', '', '', ''],
    leftWristFront: ['', '', '', '', ''],
    abdomenFront: ['', '', '', '', ''],
    rightHipFront: ['', '', '', '', ''],
    leftHipFront: ['', '', '', '', ''],
    groinFront: ['', '', '', '', ''],
    rightThighFront: ['', '', '', '', ''],
    leftThighFront: ['', '', '', '', ''],
    rightKneeFront: ['', '', '', '', ''],
    leftKneeFront: ['', '', '', '', ''],
    rightLegFront: ['', '', '', '', ''],
    leftLegFront: ['', '', '', '', ''],
    rightAnkleFront: ['', '', '', '', ''],
    leftAnkleFront: ['', '', '', '', ''],
    rightFootFront: ['', '', '', '', ''],
    leftFootFront: ['', '', '', '', ''],
    rightHandFront: ['', '', '', '', ''],
    leftHandFront: ['', '', '', '', ''],
    headBack: ['', '', '', '', ''],
    neckBack: ['', '', '', '', ''],
    rightShoulderBack: ['', '', '', '', ''],
    leftShoulderBack: ['', '', '', '', ''],
    thoracicSpineBack: ['', '', '', '', ''],
    rightArmBack: ['', '', '', '', ''],
    leftArmBack: ['', '', '', '', ''],
    rightElbowBack: ['', '', '', '', ''],
    leftElbowBack: ['', '', '', '', ''],
    rightForearmBack: ['', '', '', '', ''],
    leftForearmBack: ['', '', '', '', ''],
    rightWristBack: ['', '', '', '', ''],
    leftWristBack: ['', '', '', '', ''],
    lowerBack: ['', '', '', '', ''],
    rightButtockBack: ['', '', '', '', ''],
    leftButtockBack: ['', '', '', '', ''],
    rightThighBack: ['', '', '', '', ''],
    leftThighBack: ['', '', '', '', ''],
    rightKneeBack: ['', '', '', '', ''],
    leftKneeBack: ['', '', '', '', ''],
    rightLegBack: ['', '', '', '', ''],
    leftLegBack: ['', '', '', '', ''],
    rightAnkleBack: ['', '', '', '', ''],
    leftAnkleBack: ['', '', '', '', ''],
    rightFootBack: ['', '', '', '', ''],
    leftFootBack: ['', '', '', '', ''],
    rightHandBack: ['', '', '', '', ''],
    leftHandBack: ['', '', '', '', '']
};

//Variable containing the string with the definition for the options of each body part
var partsOfTheBodyOptions = {
    headFront: '',
    neckFront: '',
    rightShoulderFront: '',
    leftShoulderFront: '',
    chestFront: '',
    rightArmFront: '',
    leftArmFront: '',
    rightElbowFront: '',
    leftElbowFront: '',
    rightForearmFront: '',
    leftForearmFront: '',
    rightWristFront: '',
    leftWristFront: '',
    abdomenFront: '',
    rightHipFront: '',
    leftHipFront: '',
    groinFront: '',
    rightThighFront: '',
    leftThighFront: '',
    rightKneeFront: '',
    leftKneeFront: '',
    rightLegFront: '',
    leftLegFront: '',
    rightAnkleFront: '',
    leftAnkleFront: '',
    rightFootFront: '',
    leftFootFront: '',
    rightHandFront: '',
    leftHandFront: '',
    headBack: '',
    neckBack: '',
    rightShoulderBack: '',
    leftShoulderBack: '',
    thoracicSpineBack: '',
    rightArmBack: '',
    leftArmBack: '',
    rightElbowBack: '',
    leftElbowBack: '',
    rightForearmBack: '',
    leftForearmBack: '',
    rightWristBack: '',
    leftWristBack: '',
    lowerBack: '',
    rightButtockBack: '',
    leftButtockBack: '',
    rightThighBack: '',
    leftThighBack: '',
    rightKneeBack: '',
    leftKneeBack: '',
    rightLegBack: '',
    leftLegBack: '',
    rightAnkleBack: '',
    leftAnkleBack: '',
    rightFootBack: '',
    leftFootBack: '',
    rightHandBack: '',
    leftHandBack: ''
};

//Parts of the body that are defined in the body control
var partsOfTheBody = ['headFront', 'neckFront', 'rightShoulderFront', 'leftShoulderFront', 'chestFront', 'rightArmFront', 'leftArmFront', 'rightElbowFront', 'leftElbowFront', 'rightForearmFront', 'leftForearmFront'
    , 'rightWristFront', 'leftWristFront', 'rightHandFront', 'leftHandFront', 'abdomenFront', 'rightHipFront', 'leftHipFront', 'groinFront', 'rightFootFront', 'leftFootFront', 'rightAnkleFront', 'leftAnkleFront'
, 'rightLegFront', 'leftLegFront', 'rightKneeFront', 'leftKneeFront', 'rightThighFront', 'leftThighFront', 'headBack', 'neckBack', 'rightShoulderBack', 'leftShoulderBack', 'thoracicSpineBack', 'rightArmBack', 'leftArmBack'
, 'rightElbowBack', 'leftElbowBack', 'rightForearmBack', 'leftForearmBack', 'rightWristBack', 'leftWristBack', 'rightHandBack', 'leftHandBack', 'lowerBack', 'rightButtockBack', 'leftButtockBack', 'rightFootBack'
, 'leftFootBack', 'rightAnkleBack', 'leftAnkleBack', 'rightLegBack', 'leftLegBack', 'rightKneeBack', 'leftKneeBack', 'rightThighBack', 'leftThighBack'];

//Variable containing the string to display as the answer of the chat version to the patient when the body part is selected
var partsOfTheBodyDisplayHTML = {
    headFront: '',
    neckFront: '',
    rightShoulderFront: '',
    leftShoulderFront: '',
    chestFront: '',
    rightArmFront: '',
    leftArmFront: '',
    rightElbowFront: '',
    leftElbowFront: '',
    rightForearmFront: '',
    leftForearmFront: '',
    rightWristFront: '',
    leftWristFront: '',
    abdomenFront: '',
    rightHipFront: '',
    leftHipFront: '',
    groinFront: '',
    rightThighFront: '',
    leftThighFront: '',
    rightKneeFront: '',
    leftKneeFront: '',
    rightLegFront: '',
    leftLegFront: '',
    rightAnkleFront: '',
    leftAnkleFront: '',
    rightFootFront: '',
    leftFootFront: '',
    rightHandFront: '',
    leftHandFront: '',
    headBack: '',
    neckBack: '',
    rightShoulderBack: '',
    leftShoulderBack: '',
    thoracicSpineBack: '',
    rightArmBack: '',
    leftArmBack: '',
    rightElbowBack: '',
    leftElbowBack: '',
    rightForearmBack: '',
    leftForearmBack: '',
    rightWristBack: '',
    leftWristBack: '',
    lowerBack: '',
    rightButtockBack: '',
    leftButtockBack: '',
    rightThighBack: '',
    leftThighBack: '',
    rightKneeBack: '',
    leftKneeBack: '',
    rightLegBack: '',
    leftLegBack: '',
    rightAnkleBack: '',
    leftAnkleBack: '',
    rightFootBack: '',
    leftFootBack: '',
    rightHandBack: '',
    leftHandBack: ''
};

function updateDisease(partOfTheBody, disease, whichDisease, bodyId, init) {
    /// <summary>
    /// Updates the visual elements when the patient clicks on the any item of the body 
    /// </summary>
    /// <param name="partOfTheBody">Indicates which part of the body is being updated</param>
    /// <param name="disease">Indicates the type of discomfort that the patient is feeling with this part</param>
    /// <param name="whichDisease">Index of the array with the different types of discomfort for the patitents</param>
    /// <param name="bodyId">Indicates the id of the bodymap to work with</param>
    /// <param name="init">Indicates if the body is been initialized</param>
    if (partsOfTheBodyToolTip[partOfTheBody][whichDisease] == '' || init) {
        //partsOfTheBodyToolTip[partOfTheBody][whichDisease] += "<img src='/Content/images/BodyControl/" + disease.toLowerCase() + ".png' />" + disease + " </br>";
        if (partsOfTheBodyToolTip[partOfTheBody][whichDisease] == '')
            partsOfTheBodyToolTip[partOfTheBody][whichDisease] += "<img src='/Content/images/BodyControl/" + disease.toLowerCase() + ".png' />" + "";
        partsOfTheBodyChecked[partOfTheBody][whichDisease - 1] = "checked";
        checkOptions(partOfTheBody, bodyId, init);
        showDiscomfort(partOfTheBody, whichDisease, bodyId);

    }
    else {
        partsOfTheBodyToolTip[partOfTheBody][whichDisease] = '';
        partsOfTheBodyChecked[partOfTheBody][whichDisease - 1] = '';
        showDiscomfort(partOfTheBody, whichDisease, bodyId);
        checkOptions(partOfTheBody, bodyId);
    }
    if (partsOfTheBodyChecked[partOfTheBody] == ",,,,") {
        $("[id='imgBody." + bodyId + "']").mapster('set_options', {
            areas: [{
                selected: true,
                fillColor: "66cd00",
                staticState: null,
                key: partOfTheBody
            }]
        });
        options = {
            fillColor: "66cd00",
            key: partOfTheBody
        };
        $("[id='imgBody." + bodyId + "']").mapster('set', true, partOfTheBody, options);
        var options2 = $("[id='imgBody." + bodyId + "']").mapster('get_options');

        $("[id='imgBody." + bodyId + "']").mapster('set_options', options2);

    } else {
        $("[id='imgBody." + bodyId + "']").mapster('set_options', {
            areas: [{
                selected: true,
                fillColor: "ff0000",
                staticState: true,
                key: partOfTheBody
            }]
        });
        options = {
            fillColor: "ff0000",
            staticState: true,
            key: partOfTheBody
        };
        $("[id='imgBody." + bodyId + "']").mapster('set', true, partOfTheBody, options);
    }
};

function showDiscomfort(partOfTheBody, whichDisease, bodyId) {
    /// <summary>
    /// Updates the visual elements when the patient clicks on the any item of the body 
    /// </summary>
    /// <param name="partOfTheBody">Indicates which part of the body is being updated</param>
    /// <param name="whichDisease">Index of the array with the different types of discomfort for the patitents</param>
    /// <param name="bodyId">Indicates the id of the bodymap to work with</param>
    partsOfTheBodyDisplayHTML[partOfTheBody] = String(partsOfTheBodyToolTip[partOfTheBody]);
    if (partsOfTheBodyChecked[partOfTheBody] == ",,,,") partsOfTheBodyDisplayHTML[partOfTheBody] = "";
    var htmlSelectedBodyPartsBack = '';
    var htmlSelectedBodyPartsFront = '';
    $.each(partsOfTheBodyDisplayHTML, function (index, bodyPart) {
        if (index.indexOf("Back") > -1) {
            htmlSelectedBodyPartsBack += bodyPart;
            htmlSelectedBodyPartsBack = htmlSelectedBodyPartsBack.replace(/,/, '');
        } else {
            htmlSelectedBodyPartsFront += bodyPart;
            htmlSelectedBodyPartsFront = htmlSelectedBodyPartsFront.replace(/,/, '');
        }
    });
    if (partOfTheBody.indexOf("Back") > -1) {
        $("[id='selectedBodyPartsBack']").html(htmlSelectedBodyPartsBack);
    } else {
        $("[id='selectedBodyPartsFront']").html(htmlSelectedBodyPartsFront);

    }
}

function checkOptions(partOfTheBody, bodyId, init) {
    /// <summary>
    /// Updates the variables when the patient has chosen a type of discomfort
    /// </summary>
    /// <param name="partOfTheBody">Indicates which part of the body is being updated</param>
    /// <param name="bodyId">Indicates the id of the bodymap to work with</param>
    /// <param name="init">Indicates if the body is been initialized</param>
    partsOfTheBodyOptions[partOfTheBody] = "Select the type of pain you have:";
    partsOfTheBodyOptions[partOfTheBody] += "</br> <input class=\"checkbox\" type=\"checkbox\" name=\"" + partOfTheBody + "\" value=\"burning\" onclick=\"updateDisease('" + partOfTheBody + "', 'Burning', 1, '" + bodyId + "', false)\" " + partsOfTheBodyChecked[partOfTheBody][0] + "> <img src='/Content/images/BodyControl/burning.png' /> Burning";
    partsOfTheBodyOptions[partOfTheBody] += "</br> <input class=\"checkbox\" type=\"checkbox\" name=\"" + partOfTheBody + "\" value=\"numbness\" onclick=\"updateDisease('" + partOfTheBody + "', 'Numbness', 2, '" + bodyId + "', false)\" " + partsOfTheBodyChecked[partOfTheBody][1] + "> <img src='/Content/images/BodyControl/numbness.png' /> Numbness";
    partsOfTheBodyOptions[partOfTheBody] += "</br> <input class=\"checkbox\" type=\"checkbox\" name=\"" + partOfTheBody + "\" value=\"pinsNeedles\" onclick=\"updateDisease('" + partOfTheBody + "', 'Pins-Needles', 3, '" + bodyId + "', false)\" " + partsOfTheBodyChecked[partOfTheBody][2] + "> <img src='/Content/images/BodyControl/pins-Needles.png' /> Pins-Needles";
    partsOfTheBodyOptions[partOfTheBody] += "</br> <input class=\"checkbox\" type=\"checkbox\" name=\"" + partOfTheBody + "\" value=\"stabbing\" onclick=\"updateDisease('" + partOfTheBody + "', 'Stabbing', 4, '" + bodyId + "', false)\" " + partsOfTheBodyChecked[partOfTheBody][3] + "> <img src='/Content/images/BodyControl/stabbing.png' /> Stabbing";
    partsOfTheBodyOptions[partOfTheBody] += "</br> <input class=\"checkbox\" type=\"checkbox\" name=\"" + partOfTheBody + "\" value=\"ache\" onclick=\"updateDisease('" + partOfTheBody + "', 'Ache', 5, '" + bodyId + "', false)\" " + partsOfTheBodyChecked[partOfTheBody][4] + "> <img src='/Content/images/BodyControl/ache.png' /> Ache";
    ///Update the correct element to store the answer in the database
    //Not called when init
    if (!init) {
        var bodyParts = $("[name^='HiddenCheckBox." + bodyId + "." + partOfTheBody + "']");
        for (i = 0; i < bodyParts.length; i++) {
            if (partsOfTheBodyChecked[partOfTheBody][i] == "checked")
                bodyParts[i].checked = true;
            else
                bodyParts[i].checked = false;
        }
    }
}

function resizeBody(bodyId, support) {
    /// <summary>
    /// Change the size of the given BodyControl
    /// </summary>
    /// <param name="bodyId">Indicates the id of the bodymap to work with</param>   
    /// <param name="support">Indicates if the element we should take is the HTML5 version of the JS version</param>
    var newWidth;
    if (support === undefined) {
        newWidth = $("[id='bodySize." + bodyId + "']").val();
    } else {
        newWidth = $("[id='Support.Slider." + bodyId + "']").slider("values", 0);
    }
    var newHeight = newWidth;
    var resizeTime = 100;
    $("[id='imgBody." + bodyId + "']").mapster('resize', newWidth, newHeight, resizeTime);
}

function InitializePartsOfTheBody() {
    /// <summary>
    /// Initialize all the elements that are necessay for the given BodyControl
    /// </summary>
    checkCompatibility();
    var partsOfTheBodyOptions = {
        headFront: '',
        neckFront: '',
        rightShoulderFront: '',
        leftShoulderFront: '',
        chestFront: '',
        rightArmFront: '',
        leftArmFront: '',
        rightElbowFront: '',
        leftElbowFront: '',
        rightForearmFront: '',
        leftForearmFront: '',
        rightWristFront: '',
        leftWristFront: '',
        abdomenFront: '',
        rightHipFront: '',
        leftHipFront: '',
        groinFront: '',
        rightThighFront: '',
        leftThighFront: '',
        rightKneeFront: '',
        leftKneeFront: '',
        rightLegFront: '',
        leftLegFront: '',
        rightAnkleFront: '',
        leftAnkleFront: '',
        rightFootFront: '',
        leftFootFront: '',
        rightHandFront: '',
        leftHandFront: '',
        headBack: '',
        neckBack: '',
        rightShoulderBack: '',
        leftShoulderBack: '',
        thoracicSpineBack: '',
        rightArmBack: '',
        leftArmBack: '',
        rightElbowBack: '',
        leftElbowBack: '',
        rightForearmBack: '',
        leftForearmBack: '',
        rightWristBack: '',
        leftWristBack: '',
        lowerBack: '',
        rightButtockBack: '',
        leftButtockBack: '',
        rightThighBack: '',
        leftThighBack: '',
        rightKneeBack: '',
        leftKneeBack: '',
        rightLegBack: '',
        leftLegBack: '',
        rightAnkleBack: '',
        leftAnkleBack: '',
        rightFootBack: '',
        leftFootBack: '',
        rightHandBack: '',
        leftHandBack: ''
    };
    bodyControl = $("[id^='imgBody']");
    bodyId = $(bodyControl[0]).attr('alt');

    var diseases = ["Burning", "Numbness", "Pins-Needles", "Stabbing", "Ache"];

    partsOfTheBody.forEach(function (partOfTheBody) {
        partsOfTheBodyOptions[partOfTheBody] = "Select the type of pain you have:";
        partsOfTheBodyOptions[partOfTheBody] += "</br> <input class=\"checkbox\" type=\"checkbox\" name=\"" + partOfTheBody + "\" value=\"burning\" onclick=\"updateDisease('" + partOfTheBody + "', 'Burning', 1, '" + bodyId + "', false)\" " + partsOfTheBodyChecked[partOfTheBody][0] + "> <img src='/Content/images/BodyControl/burning.png' /> Burning";
        partsOfTheBodyOptions[partOfTheBody] += "</br> <input class=\"checkbox\" type=\"checkbox\" name=\"" + partOfTheBody + "\" value=\"numbness\" onclick=\"updateDisease('" + partOfTheBody + "', 'Numbness', 2, '" + bodyId + "', false)\" " + partsOfTheBodyChecked[partOfTheBody][1] + "> <img src='/Content/images/BodyControl/numbness.png' /> Numbness";
        partsOfTheBodyOptions[partOfTheBody] += "</br> <input class=\"checkbox\" type=\"checkbox\" name=\"" + partOfTheBody + "\" value=\"pinsNeedles\" onclick=\"updateDisease('" + partOfTheBody + "', 'Pins-Needles', 3, '" + bodyId + "', false)\" " + partsOfTheBodyChecked[partOfTheBody][2] + "> <img src='/Content/images/BodyControl/pins-Needles.png' /> Pins-Needles";
        partsOfTheBodyOptions[partOfTheBody] += "</br> <input class=\"checkbox\" type=\"checkbox\" name=\"" + partOfTheBody + "\" value=\"stabbing\" onclick=\"updateDisease('" + partOfTheBody + "', 'Stabbing', 4, '" + bodyId + "', false)\" " + partsOfTheBodyChecked[partOfTheBody][3] + "> <img src='/Content/images/BodyControl/stabbing.png' /> Stabbing";
        partsOfTheBodyOptions[partOfTheBody] += "</br> <input class=\"checkbox\" type=\"checkbox\" name=\"" + partOfTheBody + "\" value=\"ache\" onclick=\"updateDisease('" + partOfTheBody + "', 'Ache', 5, '" + bodyId + "', false)\" " + partsOfTheBodyChecked[partOfTheBody][4] + "> <img src='/Content/images/BodyControl/ache.png' /> Ache";
    });
    var bodyImage = $("[id='imgBody." + bodyId + "']");
    //Use of the plugin for jQuery called Imagemapster
    var result = $("[id='imgBody." + bodyId + "']").mapster({
        fillColor: '66CD00',
        fillOpacity: 0.3,
        stroke: true,
        mapKey: 'title',
        singleSelect: true,
        //listKey: 'name',
        onClick: function (e) {
            if (window.partsOfTheBodyOptions[e.key] == '') {
                $('#selections').html(partsOfTheBodyOptions[e.key]);

            }
            else {
                $('#selections').html(window.partsOfTheBodyOptions[e.key]);
            }
            windowTitle = partsOfTheBodyToolTip[e.key][0];
            windowTitle = windowTitle.replace("<b>", "");
            windowTitle = windowTitle.replace("</b>", "");
            //windowTitle = windowTitle.replace("/b|div|<|>/", "");
            windowTitle = windowTitle.replace("<div>", "");
            windowTitle = windowTitle.replace("</div>", "");
            if (e.key.indexOf("Back") > -1) {
                $("#selections").dialog({ title: windowTitle, position: { my: 'left+500', at: 'top+100', of: $("[title='" + e.key + "']") } });
            } else {
                $("#selections").dialog({ title: windowTitle, position: { my: 'left+100', at: 'top+100', of: $("[title='" + e.key + "']") } });
            }
            $("#selections").dialog("open");

        },
        areas: [
                {
                    key: "headFront",
                    toolTip: partsOfTheBodyToolTip["headFront"][0]
                },
                {
                    key: "neckFront",
                    toolTip: partsOfTheBodyToolTip["neckFront"][0]
                },
                {
                    key: "rightShoulderFront",
                    toolTip: partsOfTheBodyToolTip["rightShoulderFront"][0]
                },
                {
                    key: "leftShoulderFront",
                    toolTip: partsOfTheBodyToolTip["leftShoulderFront"][0]
                },
                {
                    key: "chestFront",
                    toolTip: partsOfTheBodyToolTip["chestFront"][0]
                },
                {
                    key: "rightArmFront",
                    toolTip: partsOfTheBodyToolTip["rightArmFront"][0]
                },
                {
                    key: "leftArmFront",
                    toolTip: partsOfTheBodyToolTip["leftArmFront"][0]
                },
                {
                    key: "rightElbowFront",
                    toolTip: partsOfTheBodyToolTip["rightElbowFront"][0]
                },
                {
                    key: "leftElbowFront",
                    toolTip: partsOfTheBodyToolTip["leftElbowFront"][0]
                },
                {
                    key: "rightForearmFront",
                    toolTip: partsOfTheBodyToolTip["rightForearmFront"][0]
                },
                {
                    key: "leftForearmFront",
                    toolTip: partsOfTheBodyToolTip["leftForearmFront"][0]
                },
                {
                    key: "rightWristFront",
                    toolTip: partsOfTheBodyToolTip["rightWristFront"][0]
                },
                {
                    key: "leftWristFront",
                    toolTip: partsOfTheBodyToolTip["leftWristFront"][0]
                },
                {
                    key: "abdomenFront",
                    toolTip: partsOfTheBodyToolTip["abdomenFront"][0]
                },
                {
                    key: "rightHipFront",
                    toolTip: partsOfTheBodyToolTip["rightHipFront"][0]
                },
                {
                    key: "leftHipFront",
                    toolTip: partsOfTheBodyToolTip["leftHipFront"][0]
                },
                {
                    key: "groinFront",
                    toolTip: partsOfTheBodyToolTip["groinFront"][0]
                },
                {
                    key: "rightThighFront",
                    toolTip: partsOfTheBodyToolTip["rightThighFront"][0]
                },
                {
                    key: "leftThighFront",
                    toolTip: partsOfTheBodyToolTip["leftThighFront"][0]
                },
                {
                    key: "rightKneeFront",
                    toolTip: partsOfTheBodyToolTip["rightKneeFront"][0]
                },
                {
                    key: "leftKneeFront",
                    toolTip: partsOfTheBodyToolTip["leftKneeFront"][0]
                },
                {
                    key: "rightLegFront",
                    toolTip: partsOfTheBodyToolTip["rightLegFront"][0]
                },
                {
                    key: "leftLegFront",
                    toolTip: partsOfTheBodyToolTip["leftLegFront"][0]
                },
                {
                    key: "rightAnkleFront",
                    toolTip: partsOfTheBodyToolTip["rightAnkleFront"][0]
                },
                {
                    key: "leftAnkleFront",
                    toolTip: partsOfTheBodyToolTip["leftAnkleFront"][0]
                },
                {
                    key: "rightFootFront",
                    toolTip: partsOfTheBodyToolTip["rightFootFront"][0]
                },
                {
                    key: "leftFootFront",
                    toolTip: partsOfTheBodyToolTip["leftFootFront"][0]
                },
                {
                    key: "rightHandFront",
                    toolTip: partsOfTheBodyToolTip["rightHandFront"][0]
                },
                {
                    key: "leftHandFront",
                    toolTip: partsOfTheBodyToolTip["leftHandFront"][0]
                },
                {
                    key: "headBack",
                    toolTip: partsOfTheBodyToolTip["headBack"][0]
                },
                {
                    key: "neckBack",
                    toolTip: partsOfTheBodyToolTip["neckBack"][0]
                },
                {
                    key: "rightShoulderBack",
                    toolTip: partsOfTheBodyToolTip["rightShoulderBack"][0]
                },
                {
                    key: "leftShoulderBack",
                    toolTip: partsOfTheBodyToolTip["leftShoulderBack"][0]
                },
                {
                    key: "thoracicSpineBack",
                    toolTip: partsOfTheBodyToolTip["thoracicSpineBack"][0]
                },
                {
                    key: "rightArmBack",
                    toolTip: partsOfTheBodyToolTip["rightArmBack"][0]
                },
                {
                    key: "leftArmBack",
                    toolTip: partsOfTheBodyToolTip["leftArmBack"][0]
                },
                {
                    key: "rightElbowBack",
                    toolTip: partsOfTheBodyToolTip["rightElbowBack"][0]
                },
                {
                    key: "leftElbowBack",
                    toolTip: partsOfTheBodyToolTip["leftElbowBack"][0]
                },
                {
                    key: "rightForearmBack",
                    toolTip: partsOfTheBodyToolTip["rightForearmBack"][0]
                },
                {
                    key: "leftForearmBack",
                    toolTip: partsOfTheBodyToolTip["leftForearmBack"][0]
                },
                {
                    key: "rightWristBack",
                    toolTip: partsOfTheBodyToolTip["rightWristBack"][0]
                },
                {
                    key: "leftWristBack",
                    toolTip: partsOfTheBodyToolTip["leftWristBack"][0]
                },
                {
                    key: "lowerBack",
                    toolTip: partsOfTheBodyToolTip["lowerBack"][0]
                },
                {
                    key: "rightButtockBack",
                    toolTip: partsOfTheBodyToolTip["rightButtockBack"][0]
                },
                {
                    key: "leftButtockBack",
                    toolTip: partsOfTheBodyToolTip["leftButtockBack"][0]
                },
                {
                    key: "rightThighBack",
                    toolTip: partsOfTheBodyToolTip["rightThighBack"][0]
                },
                {
                    key: "leftThighBack",
                    toolTip: partsOfTheBodyToolTip["leftThighBack"][0]
                },
                {
                    key: "rightKneeBack",
                    toolTip: partsOfTheBodyToolTip["rightKneeBack"][0]
                },
                {
                    key: "leftKneeBack",
                    toolTip: partsOfTheBodyToolTip["leftKneeBack"][0]
                },
                {
                    key: "rightLegBack",
                    toolTip: partsOfTheBodyToolTip["rightLegBack"][0]
                },
                {
                    key: "leftLegBack",
                    toolTip: partsOfTheBodyToolTip["leftLegBack"][0]
                },
                {
                    key: "rightAnkleBack",
                    toolTip: partsOfTheBodyToolTip["rightAnkleBack"][0]
                },
                {
                    key: "leftAnkleBack",
                    toolTip: partsOfTheBodyToolTip["leftAnkleBack"][0]
                },
                {
                    key: "rightFootBack",
                    toolTip: partsOfTheBodyToolTip["rightFootBack"][0]
                },
                {
                    key: "leftFootBack",
                    toolTip: partsOfTheBodyToolTip["leftFootBack"][0]
                },
                {
                    key: "rightHandBack",
                    toolTip: partsOfTheBodyToolTip["rightHandBack"][0]
                },
                {
                    key: "leftHandBack",
                    toolTip: partsOfTheBodyToolTip["leftHandBack"][0]
                }
        ],
        showToolTip: true
    });
    var firstTime = true;
    partsOfTheBody.forEach(function (partOfTheBody) {
        $.each(partsOfTheBodyChecked, function (index, partOfTheBodyChecked) {
            if (partsOfTheBodyChecked[partOfTheBody] != ",,,,") {
                firstTime = false;
                return;
            }
        });
    });

    if (firstTime) {
        partsOfTheBody.forEach(function (partOfTheBody) {
            var bodyParts = $("[name^='HiddenCheckBox." + bodyId + "." + partOfTheBody + "']");
            for (var index = 0; index < bodyParts.length; index++) {
                if (bodyParts[index].checked == true) {
                    updateDisease(partOfTheBody, diseases[index], index + 1, bodyId, true);
                }
            }
        });
    } else {
        partsOfTheBody.forEach(function (partOfTheBody) {
            var bodyParts = $("[name^='HiddenCheckBox." + bodyId + "." + partOfTheBody + "']");
            for (var index = 0; index < bodyParts.length; index++) {
                if (partsOfTheBodyChecked[partOfTheBody][index] == "checked") {
                    updateDisease(partOfTheBody, diseases[index], index + 1, bodyId, true);
                }
            }
        });
    }
    
}

function TestTwoStageAuthentication(provider) {
    /// <summary>
    /// Sends a ajax request to the server to issue a two stage authentication code for a user using the selected method.
    /// </summary>
    $.ajax({
        type: 'get',
        url: "../Controls/TestTwoStageAuthentication?provider=" + provider + "&code=" + GetQueryStringParams("code"),
        fail: function (result) {
            alert(result);
        }
    });
}

function VerifyTwoStageAuthentication(token, provider, objectToPass, callOnSuccess, callOnFail) {
    /// <summary>
    /// Sends a ajax request to the server to issue a two stage authentication code for a user using the selected method.
    /// </summary>
    $.ajax({
        type: 'get',
        url: "../Controls/VerifyTwoStageAuthentication?token=" + token + "&code=" + GetQueryStringParams("code") + "&provider=" + provider,
        success: function (result) {
            if (!result.success) callOnFail(objectToPass, result);
            else callOnSuccess(objectToPass, result);
        },
        fail: function (result) {
            callOnFail(result);
        }
    });
}

function updateSliderValue(sliderToUpdate, support) {
    /// <summary>
    /// Updates the span containing the value of the slider
    /// </summary>
    /// <param name="sliderToUpdate">Indicates which slider on the DOM is going to be updated</param>
    /// <param name="support">Indicates if the element we should take is the HTML5 version of the JS version</param>
    var newValue;
    if (support === undefined) {
        newValue = $("[name='" + sliderToUpdate + "']").val()
    } else {
        newValue = $("[id='Support." + sliderToUpdate + "']").slider("values", 0);
    }
    $("[name='Span." + sliderToUpdate + "']").html(newValue);
}