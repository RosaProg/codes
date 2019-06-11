(function() {
    /**
     * JCV 03-Dec-2014
     * Module to create model functions like get and save data
     */
    angular.module('dataService',[])
    .factory('qModel', function qModelFactory(){
        var data = angular.fromJson( $('#questionnaireModel').val() ),
            inputModelSubmit = $('#modelSubmit'),
            inputModelSave =  $('#modelSave'),
            formSubmit = $("#myformForKnockoutSubmit"),
            formSave = $("#myformForKnockout");

        return {
            getData: function() {
                return data;
            },
            saveData: function( data, idx ) {
                //inputModelSave.val( angular.toJson( data ) );
                //formSave.submit();
            },
            submitData: function( data ) {
                //inputModelSave.val( angular.toJson( data ) );
                //formSubmit.submit();
            }
        };
    });
    
})();