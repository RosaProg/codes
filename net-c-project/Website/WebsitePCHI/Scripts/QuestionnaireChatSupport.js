var validatePassword = false;
var passwordToValidate = "";
var heightOff = 0;
var DontShow = ko.observable(true);
var DontShowSkipped = ko.observable(true);

window.onresize = function () {
    ResizeChat();
};
$(document).ready(function () {
    setTimeout("ResizeChat();", 100);
    $('input[name=SliderZoomBodyControl]').val('396');
    $('input[name=SliderZoomBodyControl]').change(function () {
        setTimeout("ResizeChat();", 100);
        //alert($('input[name=SliderZoomBodyControl]').val());
    });
  //  alert($('input[name=SliderZoomBodyControl]').val());
    
  /*  $('#mapster_wrap_0').css('height', '400px');
    $('#mapster_wrap_0').css('width', '800px');
    $('#mapster_wrap_0').css('overflow-y', 'auto');
    
    $('#mapster_wrap_0').css('overflow-x', 'hidden');*/
});

//Variable containing the string to display as the answer of the chat version to the patient when the body part is selected
var partsOfTheBodyDisplayValuesChat = {
    headFront: 'on my head (front part)',
    neckFront: 'on my neck (front part)',
    rightShoulderFront: 'on my right shoulder (front part)',
    leftShoulderFront: 'on my left shoulder (front part)',
    chestFront: 'on my chest (front part)',
    rightArmFront: 'on my right arm (front part)',
    leftArmFront: 'on my left arm (front part)',
    rightElbowFront: 'on my right elbow (front part)',
    leftElbowFront: 'on my left elbow (front part)',
    rightForearmFront: 'on my right forearm (front part)',
    leftForearmFront: 'on my left forearm (front part)',
    rightWristFront: 'on my right wrist (front part)',
    leftWristFront: 'on my left wrist (front part)',
    abdomenFront: 'on my abdomen (front part)',
    rightHipFront: 'on my right hip (front part)',
    leftHipFront: 'on my left hip (front part)',
    groinFront: 'on my groin (front part)',
    rightThighFront: 'on my right thigh (front part)',
    leftThighFront: 'on my left thigh (front part)',
    rightKneeFront: 'on my right knee (front part)',
    leftKneeFront: 'on my left knee (front part)',
    rightLegFront: 'on my right leg (front part)',
    leftLegFront: 'on my left leg (front part)',
    rightAnkleFront: 'on my right ankle (front part)',
    leftAnkleFront: 'on my left ankle (front part)',
    rightFootFront: 'on my right foot (front part)',
    leftFootFront: 'on my left foot (front part)',
    rightHandFront: 'on my right hand (front part)',
    leftHandFront: 'on my left hand (front part)',
    headBack: 'on my head (back part)',
    neckBack: 'on my neck (back part)',
    rightShoulderBack: 'on my right shoulder (back part)',
    leftShoulderBack: 'on my left shoulder (back part)',
    thoracicSpineBack: 'on my thoracic spine',
    rightArmBack: 'on my right arm back (back part)',
    leftArmBack: 'on my left arm back (back part)',
    rightElbowBack: 'on my right elbow (back part)',
    leftElbowBack: 'on my left elbow (back part)',
    rightForearmBack: 'on my right forearm (back part)',
    leftForearmBack: 'on my left forearm (back part)',
    rightWristBack: 'on my right wrist (back part)',
    leftWristBack: 'on my left wrist (back part)',
    lowerBack: 'on my lower back',
    rightButtockBack: 'on my right buttock',
    leftButtockBack: 'on my left buttock',
    rightThighBack: 'on my right thigh (back part)',
    leftThighBack: 'on my left thigh (back part)',
    rightKneeBack: 'on my right knee (back part) ',
    leftKneeBack: 'on my left knee (back part)',
    rightLegBack: 'on my right leg (back part)',
    leftLegBack: 'on my right leg (back part)',
    rightAnkleBack: 'on my right ankle (back part)',
    leftAnkleBack: 'on my left ankle (back part)',
    rightFootBack: 'on my right foot (back part)',
    leftFootBack: 'on my left foot (back part)',
    rightHandBack: 'on my right hand (back part)',
    leftHandBack: 'on my left hand (back part)'
};

function ScrollHistory() {
    heightOff = heightOff + $('#chatHistoryPanel‏').height() * 2;
    $('#chatHistoryPanel‏').scrollTop(heightOff);
}

function PaintResponse() {
    $('#CurrentResponsePanel').hide();
    setTimeout("ResizeChat();", 10);
    $('#CurrentResponsePanelTexting').css('display', 'block');
    setTimeout("$('#CurrentResponsePanelTexting').css('display', 'none').hide(); $('#CurrentResponsePanel').show(); EnableChat();", 2500);
    $('.currentBubble').css('display', 'none');
    setTimeout("$('.currentBubble').css('display', 'block'); ResizeChat();", 2500);
    ScrollHistory();
    $('#Msg-Box').css('display', 'none');
    setTimeout("$('#Msg-Box').show(); ResizeChat(); ", 2500);
    ScrollHistory();
}

function DisableChat() {
    $('.Msg-Send').prop('disabled', true);
    setTimeout('EnableChat();', 2500);
}

function EnableChat() {
    $('.Msg-Send').prop('disabled', false);
}

function calculateDistance(elem, mouseX, mouseY) {
    return Math.floor(Math.sqrt(Math.pow(mouseX - (elem.offset().left + (elem.width() / 2)), 2) + Math.pow(mouseY - (elem.offset().top + (elem.height() / 2)), 2)));
}

function ResizeChat() {
    var h = window.innerHeight;
    var header = 10 + 90 + 10 + $('#chatTitle-Background').height() + 5;
    var footer = $('#bottomBar-Background').height() + 9;
    var alturaresultado = (h - (header + footer)) + 1 - 115;
    $('#chatHistoryPanel‏').css('height', '' + alturaresultado + 'px');
    ScrollHistory();
}

function QuestionnaireModel(questionnaireModel) {
    self = this;
    this.Title = questionnaireModel.Title;
    this.QuestionnaireId = questionnaireModel.QuestionnaireId;
    this.GroupId = questionnaireModel.GroupId;
    this.Items = ko.observableArray(LoadItems(questionnaireModel.Items));
    this.CurrentResponsePanel = ko.observable(SetCurrentResponsePanel(LoadItems(questionnaireModel.Items)));
    this.CurrentQuestionPanel = ko.observable(SetCurrentQuestionPanel(LoadItems(questionnaireModel.Items)));
    this.CurrentItemIndex = ko.observable(GetCurrentItemIndex(LoadItems(questionnaireModel.Items)));
    this.CurrentItemCanSkip = ko.observable(SetCurrentItemCanSkip(LoadItems(questionnaireModel.Items)));
    this.CurrentItemCanEdit = ko.observable(SetCurrentItemCanEdit(LoadItems(questionnaireModel.Items)));
    this.IsPro = questionnaireModel.IsPro;
    this.Anonymous = questionnaireModel.Anonymous;
    // Progress Bar
    this.TotalAmountOfQuestions = LoadItems(questionnaireModel.Items).length;
    this.QuestionsAnswered = ko.observable(SetAnsweredCount(LoadItems(questionnaireModel.Items)));
    this.ShowProgressBar = questionnaireModel.ShowProgressBar;
    this.CanSavePartial = questionnaireModel.CanSavePartial;
    this.ErrorMessage = ko.observable();
    this.CanShow = ko.observable(true);

    this.RefreshItems = function (items) {
        self.Items();
        self.Items(items);

    }

    this.SendAnswer = function (responsePanel) {
        ResizeChat();
        self.ErrorMessage('');
        if (responsePanel == undefined) return;
        if (!self.Items()[self.CurrentItemIndex()].UpdateAnswer()) return;
        this.GoToNextItem();
    }

    this.SendAnswerEdited = function (responsePanel) {
        ResizeChat();
        self.ErrorMessage('');
        if (responsePanel == undefined) return;
        if (!self.Items()[self.CurrentItemIndex()].UpdateAnswer()) return;

        $('#ContinuetheQuestionnaire').css('display', 'block');
        $('.text-buttons').css('display', 'none');

        ResizeChat();
        alert("Response Changed");
        self.NextItemEdited(self.Items()[self.CurrentItemIndex()]);
        DontShow(true);
        DontShowSkipped(true);
        $('#Msg-Box').css('display', 'none');
    }

    this.SendAnswerSkiped = function (responsePanel) {

        self.ErrorMessage('');
        if (!self.Items()[self.CurrentItemIndex()].UpdateAnswer()) return;
        ResizeChat();
        self.NextItemSkipped(self.Items()[self.CurrentItemIndex()]);
        self.SetCurrentItem();
        // Refresh / Order
        var items = self.Items();
        self.RefreshItems(items);
        PaintResponse();
        DontShowSkipped(true);

    }
    this.SendAnswerSkipedEdited = function (responsePanel) {
        ResizeChat();
        self.ErrorMessage('');
        if (responsePanel == undefined) return;
        if (!self.Items()[self.CurrentItemIndex()].UpdateAnswer()) return;

        $('#ContinuetheQuestionnaireSkipped').css('display', 'block');
        $('.text-buttons').css('display', 'none');

        ResizeChat();
        alert("Response Changed");
        self.NextItemEditedSkip(self.Items()[self.CurrentItemIndex()]);
        DontShow(true);
        DontShowSkipped(true);
        $('#Msg-Box').css('display', 'none');
    }
    this.GoToNextItem = function () {
        // Set current Item
        self.NextItem(self.Items()[self.CurrentItemIndex()]);
        // Set how many questions answered
        self.QuestionsAnswered(SetAnsweredCount(self.Items()));
        self.SetCurrentItem();
        // Refresh / Order
        var items = self.Items();
        self.RefreshItems(items);
        PaintResponse();
        DontShowSkipped(true);
        //*************
        $('#skipbtn').css('display', 'block');
        $('#skipEdit').css('display', 'none');
        //*************

    }
    this.GoToNextItemAfterSkip = function () {
        // Set current Item
        self.NextItemSkipped(self.Items()[self.CurrentItemIndex()]);
        // Set how many questions answered
        self.QuestionsAnswered(SetAnsweredCount(self.Items()));
        self.SetCurrentItem();
        // Refresh / Order
        var items = self.Items();
        self.RefreshItems(items);/* */
        PaintResponse();
        DontShowSkipped(true);
        //*************
        $('#skipbtn').css('display', 'block');
        $('#skipEdit').css('display', 'none');
        //**************

    }


    this.SetCurrentItem = function () {
        self.CurrentResponsePanel(SetCurrentResponsePanel(self.Items()));
        self.CurrentQuestionPanel(SetCurrentQuestionPanel(self.Items()));
        self.CurrentItemCanSkip(SetCurrentItemCanSkip(self.Items()));
        self.CurrentItemCanEdit(SetCurrentItemCanEdit(self.Items()));
        self.CurrentItemIndex(GetCurrentItemIndex(self.Items()));
    }

    this.NextItem = function (item) {
        item.Status("Historical");
        item.AnsweredStatus("Answered");

        var found = false;
        for (var j = item.Index; j < this.Items().length; j++) {
            var itm = this.Items()[j];
            if ((itm.Status() == "Future" || itm.Status() == "AutoSkipped" || itm.Status() == "Wait") && !found) {
                var actions = new Array();
                if (item.AnswerAction != null) { actions = item.AnswerAction.split(";"); }
                var itemToFind = itm.ActionId;
                for (var i = 0; i < actions.length; i++) {
                    var action = actions[i].split(" ");
                    switch (action[0]) {
                        case "GOTO":
                            itemToFind = action[1];
                            break;
                            //TODO implement different actions for chat options
                        default:
                            break;
                    }
                }
                if (itm.ActionId == itemToFind) { itm.Status("Current"); found = true; }
                else { itm.Status("AutoSkipped"); }
            }
        }
    }

    this.NextItemEdited = function (item) {
        item.Status("Historical");
        item.AnsweredStatus("Answered");
    }
    this.NextItemEditedSkip = function (item) {
        item.Status("Skipped");
        item.AnsweredStatus("Skipped");

    }
    this.NextItemSkipped = function (item) {
        item.Status("Skipped");
        item.AnsweredStatus("Skipped");

        var found = false;
        for (var j = item.Index; j < this.Items().length; j++) {
            var itm = this.Items()[j];
            if ((itm.Status() == "Future" || itm.Status() == "AutoSkipped" || itm.Status() == "Wait") && !found) {
                var actions = new Array();
                if (item.AnswerAction != null) { actions = item.AnswerAction.split(";"); }
                var itemToFind = itm.ActionId;
                for (var i = 0; i < actions.length; i++) {
                    var action = actions[i].split(" ");
                    switch (action[0]) {
                        case "GOTO":
                            itemToFind = action[1];
                            break;
                            //TODO implement different actions for chat options
                        default:
                            break;
                    }
                }

                if (itm.ActionId == itemToFind) { itm.Status("Current"); found = true; }
                else { itm.Status("AutoSkipped"); }
            }
        }
    }

    this.Save = function (submit) {

        /// <summary>
        /// Parses all the HTML Content to ensure the Html tags are escaped for safe submission to the webserver
        /// </summary>
        /// <param name="submit">Not in use</param> 
        $.each(this.Items(), function (index, item) {
            item.ResponsePanel().HtmlContent = escape(item.ResponsePanel().HtmlContent);
            item.Question.HtmlContent = escape(item.Question.HtmlContent);
            item.Answer().HtmlTemplate = escape(item.Answer().HtmlTemplate);
        });

    }
}


function Item(index, actionId, status, question, responsePanel, answer, answeredStatus, itemNames, possibleAnwers, AnswerAction, answerNames, answerValues, canSkip, PreventEdit) {
    this.Index = index;
    this.Status = ko.observable(status);
    this.ActionId = actionId;
    this.Question = question;
    this.ResponsePanel = ko.observable();
    this.ResponsePanel(responsePanel);
    this.Answer = ko.observable();
    this.Answer(answer);
    this.Answer().Value = ko.observable();
    this.Answer().Value(FormatAnswer(this.Answer().HtmlTemplate, this.Answer().Value()))
    this.ItemNames = itemNames;
    this.PossibleAnswers = possibleAnwers;
    this.AnswerAction = AnswerAction;
    this.AnsweredStatus = ko.observable(answeredStatus);
    this.AnswerNames = ((answerNames != null) ? answerNames : new Array());
    this.AnswerValues = ((answerValues != null) ? answerValues : new Array());

    this.EditAnswer = function (item) {
        var Confirmation = confirm("Do you want to edit your answer to the question ?");
        if (Confirmation == true) { // All current items will become historical or future

            $.each(self.Items(), function (index, itm) {
                if (itm.Status() == 'Current') {
                    if (itm.AnsweredStatus() == 'NotAnswered') {
                        itm.Status('Future');
                    } else {
                        itm.Status('Historical');
                    }
                }
            });

            item.Status('Current');        // Become the current item
            self.SetCurrentItem();        // Set the current item panels 
            $('.Msg-Send').css('display', 'none');
            $('#Edited').css('display', 'block');

            //*************
            $('#skipbtn').css('display', 'none');
            $('#skipEdit').css('display', 'block');
            //*************

            DontShow(false);
            change();
        }
    }

    this.EditAnswerSkipped = function (item) {
        var Confirmation = confirm("Do you want to edit your answer to the question ?");
        if (Confirmation == true) { // All current items will become historical or future

            $.each(self.Items(), function (index, itm) {
                if (itm.Status() == 'Current') {
                    if (itm.AnsweredStatus() == 'NotAnswered') {
                        itm.Status('Future');
                    } else {
                        itm.Status('Historical');
                    }
                }
            });
            DontShowSkipped(false);

            item.Status('Current');        // Become the current item
            self.SetCurrentItem();// Set the current item panels 
            $('.Msg-Send').css('display', 'none');
            $('#Edited').css('display', 'block');
            //*************
            $('#skipbtn').css('display', 'none');
            $('#skipEdit').css('display', 'block');
            //*************
            DontShow(false);
            change();
        }
    }

    this.LoadAnwers = function () {
        var e = 0;
        var hiddenCheckBox = false;
        var selectedBodyParts = new Array();
        var answersText = this.Answer().HtmlTemplate;
        for (var i = 0; i < this.ItemNames.length; i++) {
            var name = this.ItemNames[i];
            var split = name.split(".");
            var value = '';
            if (split[0] == "Radio" || split[0] == "CheckBox" || split[0] == "DropDown") {
                answerName = this.AnswerNames[e];
                value = this.AnswerValues[e];
                answersText = FindAnswerText(this, answersText, name, value);
                if (name == answerName) { e++; }
            }
            else if (split[0] == "Slider" || split[0] == "TextBox" || split[0] == "TextArea" || split[0] == "HiddenCheckBox" || split[0] == "DatePicker")//Other items
            {
                answerName = this.AnswerNames[e];
                value = this.AnswerValues[e];
                if (name == answerName) {
                    answersText = FormatAnswer(answersText, value);

                    if (split[0] == "HiddenCheckBox") {
                        hiddenCheckBox = true;
                        if (value != undefined) {
                            selectedBodyParts[split[2]] = 1;
                            selectedBodyParts["length"]++;
                        }
                    }
                    e++;
                } else { answersText = FormatAnswer(answersText, '') }
            }
        }
        if (hiddenCheckBox) {
            answersText = FormatAnswerBody(answersText, selectedBodyParts);
            hiddenCheckBox = false;
        }
        this.Answer().Value(answersText);
        return true;
    }

    this.UpdateAnswer = function () {

        removeErrorText();
        var answersText = this.Answer().HtmlTemplate;
        var hiddenCheckBox = false;
        var selectedBodyParts = new Array();
        for (var i = 0; i < this.ItemNames.length; i++) {
            var name = this.ItemNames[i];
            var split = name.split(".");
            var value = '';

            if (split[0] == "Radio" || split[0] == "CheckBox" || split[0] == "HiddenCheckBox") {
                value = $('[name="' + name + '"]:checked').val();
                var option = $("input[name='" + name + "'][value='" + value + "']");
                if (option.attr("data-applicable") == "MakeItemNotApplicable") {
                    answersText = answersText.substring(0, answersText.indexOf("%Value%"));
                    answersText += "%Value%";
                }
                answersText = FindAnswerText(this, answersText, name, value);

                this.AnswerAction = FindAction(this, name, value);
                if (split[0] == "HiddenCheckBox") {
                    hiddenCheckBox = true;
                    if (value != undefined) {
                        selectedBodyParts[split[2]] = 1;
                        selectedBodyParts["length"]++;
                    }
                }
            }
            else if (split[0] == "DropDown")//Other items
            {
                value = $('[name="' + name + '"]:selected');
                this.AnswerAction = FindAction(this, name, value);
                value = $('[name="' + name + '"]:selected').val();
                answersText = FindAnswerText(this, answersText, name, value);
            } else if (split[0] == "TextBox" || split[0] == "TextArea" || split[0] == "DatePicker" /*HTML5 datepicker*/) {
                value = $('[name="' + name + '"]').val();
                answersText = FormatAnswer(answersText, value);
                this.AnswerAction = FindAction(this, name, value);
            } else if (split[0] == "Slider")//HTML5 slider
            {
                if (!Modernizr.inputtypes.range) {
                    value = $('[id="Support.' + name + '"]').slider("value");
                } else {
                    value = $('[name="' + name + '"]').val();
                }
                answersText = FormatAnswer(answersText, value);
                this.AnswerAction = FindAction(this, name, value);
            }

            else if (split[0] == "Password") {
                value = $('[name="' + name + '"]').val();
                var value2 = $('[name="passwordrepeat"]').val();
                var errorText = '';
                var success = true;
                if (value.length < 6) {
                    errorText = "Oops! Password must contain at least 6 characters!"
                    success = false;
                }
                re = /[0-9]/;
                if (!re.test(value)) {
                    errorText = ("Oops! password must contain at least one number (0-9)!");
                    success = false;
                }
                re = /[A-Z]/;
                if (!re.test(value)) {
                    errorText = ("Oops! password must contain at least one uppercase letter (A-Z)!");
                    success = false;
                }
                if (validatePassword == false) {
                    validatePassword = true;
                    passwordToValidate = value;
                    //answersText = "My password will be *****";
                    if (value2 != undefined) {
                        if (value != value2) {
                            errorText = ('Oops! Those passwords didn’t quite match. Please try re-typing your password.');
                            success = false;
                        } else {
                            passwordToValidate = "";
                            validatePassword = true;
                        }
                    }
                } else {
                    if (value != value2) {
                        errorText = ('Oops! Those passwords didn’t quite match. Please try re-typing your password.');
                        success = false;
                    }

                    //answersText = "Password matched";
                    validatePassword = false;
                    passwordToValidate = "";
                }

                if (!success) {
                    setErrorText(errorText);
                    return false;
                }
                answertext = FindAnswerText(this, answersText, name, value);
                this.AnswerAction = FindAction(this, name, undefined);
            }
            else if (split[0] == "Provider") {
                value = $('[name="' + name + '"]:checked').val();
                if (value != "None") TestTwoStageAuthentication(value)

                answersText = FindAnswerText(this, answersText, name, value);
                this.AnswerAction = FindAction(this, name, value);
            }
            else if (split[0] == 'codeCheckRadio') {
                value = $('[name="codeCheckRadio"]:checked').val();
                if (value == 'No') {
                    setErrorText("The code has been send again");
                    TestTwoStageAuthentication(self.Items()[this.Index - 1].AnswerValues[0]);
                    return false;
                }
            }
            else if (split[0] == "TwoStageCode") {
                value = $('[name="' + name + '"]').val();
                // TODO add on success function and a wait status
                this.Status("Wait");
                this.Answer().Value("Please wait while we verify your response");
                VerifyTwoStageAuthentication(value, self.Items()[this.Index - 1].AnswerValues[0], this,
                    function (item, result) {
                        item.Status("Historical");
                        item.Answer().Value("The two stage authentication method is working");
                        //self.NextItem(item);
                        self.GoToNextItem();
                    },
                    function (item, result) {
                        item.Status("Current");
                        setErrorText("Oops! That code is not correct.");
                    });

                return false;
            }
            if (value != undefined && value != "") {
                var newElement = true;
                for (var lookingNew = 0; lookingNew < this.AnswerNames.length; lookingNew++) {
                    if (this.AnswerNames[lookingNew] == name) {
                        newElement = false;
                        this.AnswerValues[lookingNew] = value;
                        lookingNew = this.AnswerNames.length;
                    }
                }
                if (newElement) {
                    this.AnswerNames.push(name);
                    this.AnswerValues.push(value);
                }
            }
        }
        if (hiddenCheckBox) {
            answersText = FormatAnswerBody(answersText, selectedBodyParts);
            hiddenCheckBox = false;
        }
        this.Answer().Value(answersText);
        return true;
    }

}


function change() {
    if (DontShow() == false) {
        $('.userBubble').css('box-shadow', ' 8px 8px 4px 4px #0f0');
        $('.userBubble').css('-webkit-box-shadow', '-1px 17px 53px 41px rgba(0,0,0,0.75)');
        $('.userBubble').css('-moz-box-shadow', '-1px 17px 53px 41px rgba(0,0,0,0.75)');
        setTimeout("changeBack();", 500);
    } else {
        $('.userBubble').css('box-shadow', ' 0px 0px 0px #fff');
    }
}

function changeBack() {
    $('.userBubble').css('box-shadow', ' 0px 0px 0px #fff');
    setTimeout("change();", 500);
}

function setErrorText(errorText) {
    var currentQuestionPanel = self.CurrentQuestionPanel();
    currentQuestionPanel.HtmlContent = "<div id=\"chatErrorMessage\"><strong>" + errorText + "</strong><br /></div>" + currentQuestionPanel.HtmlContent;
    self.CurrentQuestionPanel(currentQuestionPanel);
}

function removeErrorText() {

    var currentQuestionPanel = self.CurrentQuestionPanel();
    html = $.parseHTML(currentQuestionPanel.HtmlContent);
    //$(html).remove('#chatErrorMessage');// = "<div id=\"chatErrorMessage\"><strong>" + errorText + "</strong><br /></div>" + currentQuestionPanel.HtmlContent;
    var content = '';
    for (var i = 0; i < html.length; i++) {
        if (html[i].nodeName == '#text') content += html[i].data;
        else if (html[i].id != 'chatErrorMessage') content += html[i].innerHTML;
    }
    currentQuestionPanel.HtmlContent = content;
    self.CurrentQuestionPanel(currentQuestionPanel);
}

function FormatAnswerBody(answersText, selectedBodyParts) {
    if (selectedBodyParts.length > 0) {
        answersText = answersText.replace("me: ", "me: I feel <br>")
        $.each(partsOfTheBodyDisplayValuesChat, function (index, bodyPart) {
            if (selectedBodyParts[index] != undefined) {
                answersText = answersText.replace(index, bodyPart);
            } else {
                answersText = answersText.replace(index + " </br>", "");
            }
        });
    } else {
        answersText = "me: I feel no discomfort on my body";
    }

    return answersText

}

function FindAction(item, name, value) {
    if (value == undefined) {
        for (var i = 0; i < item.PossibleAnswers.length; i++) {
            if (item.PossibleAnswers[i].Name == name) {
                return item.PossibleAnswers[i].Action;
            }
        }
    } else {
        for (var i = 0; i < item.PossibleAnswers.length; i++) {
            if (item.PossibleAnswers[i].Name == name && item.PossibleAnswers[i].Value == value) {
                return item.PossibleAnswers[i].Action;
            }
        }
    }
    return '';
}

function FindAnswerText(item, answersText, name, value) {
    for (var i = 0; i < item.PossibleAnswers.length; i++) {
        if (item.PossibleAnswers[i].Name == name && item.PossibleAnswers[i].Value == value) {
            return FormatAnswer(answersText, item.PossibleAnswers[i].AnswerText);
        }
    }
    return FormatAnswer(answersText, '');
}

function FormatAnswer(template, answer) {
    if (answer == undefined) return '';
    if (template == undefined) return answer;
    return template.replace('%Value%', answer);
}

function LoadItems(items) {

    var itemList = new Array();
    $.each(items, function (index, item) {
        var itemRow = new Item(index, item.ActionId, item.Status, item.Question, item.ResponsePanel, item.Answer, item.AnsweredStatus, item.ItemNames, item.PossibleAnswers, item.AnswerAction, item.AnswerNames, item.AnswerValues, item.CanSkip);

        itemRow.LoadAnwers();
        itemList.push(itemRow);
    });
    return itemList;
}

function SetCurrentResponsePanel(items) {
    var currentResponsePanel;
    $.each(items, function (index, item) {
        if (item.Status() == 'Current') {
            currentResponsePanel = item.ResponsePanel;
            return currentResponsePanel;
        }
    });
    return currentResponsePanel;
}

function SetCurrentItemCanSkip(items) {
    var canSkip;
    $.each(items, function (index, item) {
        if (item.Status() == 'Current') {
            canSkip = item.ResponsePanel.CanSkip;
            return canSkip;
        }
    });
    return canSkip;
}

function SetCurrentItemCanEdit(items) {
    var canEdit;
    $.each(items, function (index, item) {
        if (item.Status() == 'Historical') {
            canEdit = item.ResponsePanel.canEdit;
            return canEdit;
        }
    });
    return canEdit;
}

function SetCurrentQuestionPanel(items) {
    var currentQuestionPanel;
    $.each(items, function (index, item) {
        if (item.Status() == 'Current') {
            currentQuestionPanel = item.Question;
            return currentQuestionPanel;
        }
    });
    return currentQuestionPanel;
}

function GetCurrentItemIndex(items) {
    var currentIndex;
    $.each(items, function (index, item) {
        if (item.Status() == 'Current') {
            currentIndex = index;
            var indexCurrent = items.length - 1;
            if (index == indexCurrent) {
                EndQuestionnaire();
            } else { ShowSend(); } return currentIndex;
        }
    });
    return currentIndex;
}

function SetAnsweredCount(items) {

    var count = 1;
    $.each(items, function (index, itm) {

        if (itm.AnsweredStatus() == 'Answered') {
            count++;

        }
    });
    return count;
}

function EndQuestionnaire() {
    $('.Msg-Submit').css('display', 'block');
    $('.Msg-Submit').css('right', '25px');
    $('.Msg-Submit').css('width', '140px');
    $('.Msg-Send').css('display', 'none');
    // show Submit & hide send
}

function ShowSend() {
    $('.Msg-Submit').css('display', 'none');
    $('.Msg-Send').css('display', 'block');
    // show Send & hide Submit
}
