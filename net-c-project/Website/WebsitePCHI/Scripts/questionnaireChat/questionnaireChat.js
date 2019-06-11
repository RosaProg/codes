/**
 * JCV 02-Dec-2014 Wrapped into a self-invoking function to avoid glolbal variables
 * @return {[type]} [description]
 */
(function() {

    var modules =   [
                            'dataService',
                            'htmlHelpers',
                            'replyService',
                            'chatPanelDirectives'
                        ];
    //JCV 20-Dic-2014, if is ie7, we have to load ie7-support module
    if( $('html').hasClass('ie7') ) {
        modules.unshift( 'ie7-support' );
    }
    
    var CHAT = angular.module('questionnaireChat', modules );
    /**
     * JCV 2-Dec-2014
     * Controller's constructor function, we are using an array as a parameter which contains the names of the dependencies.
     * In this way we avoid problems when the dependency injector needs to identify services.
     * @param  {String}     scope [description]
     * @return {Function}        [description]
     */
    CHAT.controller('questionnaireCtrl', ['$scope', 'qModel', 'qReply', function ($scope, qModel, qReply ) {
        var progressBarUnit;

        $scope.continueNewQuestion = true; //continue with the next question, If the chat needs to go to the next question
        $scope.showQuestion = false;//show ompc question
        

        $scope.questionnaire = qModel.getData();//Model Object obtained from backend
        $scope.questionnaire.CurrentItem = $scope.questionnaire.CurrentItem || 0; //Set the current Index from the data obtained
        $scope.total = $scope.questionnaire.Items.length;//total questions        

        progressBarUnit = (100/$scope.total); //Unit to update the progressbar
        $scope.arrHistoricalQuestions = [];
        $scope.progressBarValue = 0;//Init Value for the progressbar
        $scope.historicalQuestions = {};

        /**
         * Obtain all the historical questions, and the others types of questions
         * @return {[type]} [description]
         */
        angular.forEach( $scope.questionnaire.Items, function( question, idx ) {
            question.idx = idx;//JCV 1-Dec-2014 Add index to each question
            if( question.Status === 'Historical' ||  question.Status === 'Skipped' ) {
                $scope.historicalQuestions[idx] = question;
                $scope.arrHistoricalQuestions.push( idx );
            }
         });

        $scope.currentQuestion = $scope.questionnaire.Items[$scope.questionnaire.CurrentItem];//Set the current question
        $scope.currentQuestionEditing = undefined;//Indicates if the chat is editing an historical question
        //JCV 01-Dec-2014 Update progressbar value
        $scope.updateProgressBar = function(){
            $scope.progressBarValue= progressBarUnit * ( $scope.questionnaire.CurrentItem + 1);
        };
        //JCV 01-Dec-2014 Set the data of the current question
        $scope.setCurrentQuestion = function( idxItem ) {
            $scope.currentQuestion = $scope.questionnaire.Items[ idxItem ];
        };

        $scope.updateCurrentItem = function() {
            $scope.questionnaire.CurrentItem +=  1;
        };

        $scope.setCurrentQuestionEditing = function(idxItem) {
            $scope.currentQuestionEditing =  $scope.questionnaire.Items[ idxItem ];
        };

        
    //END questionnaireCtrl
    }]);

})();