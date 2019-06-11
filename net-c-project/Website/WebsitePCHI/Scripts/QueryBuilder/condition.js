var PatientFields;
var QuestionnaireFields;
var ResponseFields = ["Date Completed", "Date Started"];
window.QueryBuilder = (function (exports, ko) {

    function Condition() {
        var self = this;

        self.templateName = 'condition-template';
        self.__type = "condition";
        self.classType = ko.observableArray(["Questionnaire", "Patient", "Response"]);
        self.selectedClass = ko.observable('Questionnaire');
        self.ShowPrimaryFields = ko.observable(false);
        self.ShowSecondaryFields = ko.observable(true);
        
        
        self.fields = ko.observableArray(QuestionnaireFields);
        self.selectedField = ko.observable(QuestionnaireFields[0]);
        
        self.comparisons = ko.observableArray(['=', '<>', '<', '<=', '>', '>=']);

        self.selectedComparison = ko.observable('=');

        self.value = ko.observable('');

        self.ClassChanged = function (event) {
            if(self.selectedClass() == 'Questionnaire')
            {
                self.fields(QuestionnaireFields);
                self.selectedField(QuestionnaireFields[0]);
                self.ShowPrimaryFields(false);
                self.ShowSecondaryFields(true);
            }
            else if(self.selectedClass() == "Patient")
            {
                self.fields(PatientFields);
                self.selectedField(PatientFields[0]);
                self.ShowPrimaryFields(true);
                self.ShowSecondaryFields(false);
            }
            else if(self.selectedClass() == "Response")
            {
                self.fields(ResponseFields);
                self.selectedField(ResponseFields[0]);
                self.ShowPrimaryFields(true);
                self.ShowSecondaryFields(false);
            }
        }                        

        // the text() function is just an example to show output
        self.text = ko.computed(function () {
            if (self.selectedClass() == 'Questionnaire') {
                return self.selectedClass() + ' ' + self.selectedComparison() + " " + self.selectedField();
            }
            else {
                return self.selectedClass() +
                    ' ' +
                self.selectedField() +
                  ' ' +
                  self.selectedComparison() +
                  ' ' +
                  self.value();
            }
        });
    }

    exports.Condition = Condition;
    return exports;

})(window.QueryBuilder || {}, window.ko);