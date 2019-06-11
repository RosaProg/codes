(function(){
    var htmlIds = {
        questionnaireModel: 'questionnaireModel',
        historyPanel: 'chatHistoryPanel',
        progressBarText: 'progress-bar-text',
        modelSave:  'modelSave',
        modelSubmit: 'modelSubmit'
    };

    var htmlClasses = {
        currentQuestion: 'currentQuestion',
        replying: 'replying',
        editing: 'editing',
        responsePanel: 'responsePanel'
    };

    var config =  {
        timeResponse: 2000,
        timeScroll: 1
    };

    var QQ = angular.module('chatPanelDirectives', []);

    QQ.directive('chatTitle', function() {
        return {
            restrict: 'A',
            //@TODO JCV It should be a html template file
            //template: ,
            controller: function( $scope, $element, $attrs) {
    
            }
        };
    });


    QQ.directive('chatPanel', ['qReply', function( qReply ) {
        // Runs during compile
        return {
            // name: '',
            // priority: 1,
            // terminal: true,
            // scope: {}, // {} = isolate, true = child, false/undefined = no change
            controller: function($scope, $element, $attrs, $transclude) {
                /**
                 * Function to update the informacion replied and when someone skip the current question
                 * @param  {Object}             question [description]
                 * @param  {Objecy|undefined}   response [description]
                 * @return {undefined}
                 */
                function updateOnSend( question, response ) {
                    var idx = question.idx;

                    if( response ) {
                        question.Answer.Value = response.tpl;
                        question.AnswerNames = response.answerNames;
                        question.AnswerValues = response.answerValues;
                        question.Status = 'Historical';
                        question.AnsweredStatus = 'Answered';
                    } else {
                        question.Status = 'Skipped';
                    }
                    
                    $scope.historicalQuestions[idx] = question;
                    //JCV 17-Dic-2014, We augment the array to show hiistoricla question which we are using it in a ng-repeat directive, we don't do that when someone is editing
                    if( $scope.showQuestion ) {
                        $scope.arrHistoricalQuestions.push(idx);
                    }

                    $scope.questionnaire.Items[idx] = question;//JCV 1-Dec-2014 Update the questionnare Object
                }
                /**
                 * Update the status of the historical question starting ffrom "fromIdx" parameter.
                 * @param  {String}     status  New Status
                 * @param  {Integer}    fromIdx Starting index, in this case is the key of an object
                 * @return {undefined}  
                 */
                function updateHistoricalStatus( status, fromIdx  ) {
                    var fromIdx = fromIdx + 1;
                    while( $scope.historicalQuestions[fromIdx] !== undefined ) {
                        $scope.historicalQuestions[fromIdx].Status = status;
                        fromIdx++;
                    }
                }

                function scrollToEnd () {
                    var historyPanel = $element.closest('#' + htmlIds.historyPanel );
                    historyPanel.animate({ scrollTop: historyPanel[0].scrollHeight }, config.timeScroll);
                }
                /**
                 * Inner Function to show the next question, the next question is already loaded in DOM, here we just show it and updatethe progressbar
                 * @param  {[Number]} nextIdx Index of the next question
                 * @return {undefined}        By Default
                 */
                function showNewQuestion( ) {
                    setTimeout(function() {
                        //JCV 07-Dec 2014, We need to use $apply function to update the values of currentItem and progressbar, because setTimeout is out of the scope of angular
                        //See http://www.sitepoint.com/understanding-angulars-apply-digest/
                        $scope.$apply(function() {
                            $scope.showQuestion = true;//Show Bubbles that contains the replyingPanel
                        });
                    }, config.timeResponse );
                }

                this.edit = function ( idx ) {
                    $scope.currentQuestionEditing =  $scope.questionnaire.Items[ idx ];
                    $scope.showQuestion = false; //To hide the currentQuestion
                    updateHistoricalStatus( 'Future', idx ); //Set Future to the questions after this one (idx)
                };

                this.sendEditing = function ( idx ) {
                    var response;

                    $scope.currentQuestionEditing = $scope.questionnaire.Items[idx];
                    
                    response = qReply.updateAnswer( $element.find('.' + htmlClasses.editing ), $scope.currentQuestionEditing );
                    
                    if( !response.valid ) {
                        return false;
                    }

                    updateOnSend( $scope.currentQuestionEditing, response );

                    this.closeEditing(idx);
                };

                this.continueQuestionnaire = function(){
                    scrollToEnd();
                };

                this.closeEditing = function(idx) {
                    $scope.showQuestion = true;
                    $scope.continueNewQuestion = false;
                    updateHistoricalStatus( 'Historical', idx );
                    $scope.currentQuestionEditing = undefined;
                };

                /**
                 * Save the state of current question in the Object Model, then we can send the object Model updated to the backend
                 * @param  {[type]} idx [description]
                 * @return {[type]}         [description]
                 */
                this.send = function( idx, skip ) {
                    //console.log( $scope );
                    var nextIdx = idx + 1;
                    var response;
                    if( skip !== true ) {
                        response = qReply.updateAnswer( $element.find('.' + htmlClasses.replying ), $scope.currentQuestion );
                    
                        if( !response.valid ) {
                            return false;
                        }
                    }

                    updateOnSend( $scope.currentQuestion, response );

                    $scope.showQuestion = false;//Hide Bubbles that contains the replyingPanel
                    
                    if( $scope.questionnaire.CurrentItem < $scope.total ){
                        $scope.setCurrentQuestion( nextIdx );
                        $scope.continueNewQuestion = true; //To update $scope.questionnaire.CurrentItem, the progressBarValue and do scroll at the end of the questionnnaire
                        showNewQuestion( );
                    }
                };

                this.savePartial = function( ) {
                    var input = angular.element( '#' + htmlIds.modelSave );
                    // console.log( JSON.stringify( $scope.questionnaire ));
                    //@TODO JCV 17-Dic-2014,  check if we have to delete the idx property from all the Items
                    input.val( JSON.stringify( $scope.questionnaire ) );
                    input.closest('form').submit();
                };

                this.finish = function( idx ) {
                    this.send( idx );
                    var input = angular.element( '#' + htmlIds.modelSubmit );
                    //console.log( JSON.stringify( $scope.questionnaire ));
                    //@TODO JCV 17-Dic-2014,  check if we have to delete the idx property from all the Items
                    input.val( JSON.stringify( $scope.questionnaire ) );
                    input.closest('form').submit();
                };

        
                showNewQuestion();

                $scope.$watch( 'showQuestion', function( val ) {
                    if( val === true && $scope.continueNewQuestion ) {
                        $scope.updateCurrentItem();//JCV update the $scope.questionnaire.CurrentItem
                        $scope.updateProgressBar();
                        scrollToEnd();
                    }
                });


            },
            controllerAs: 'chatCtrl',
            // require: 'ngModel', // Array = multiple requires, ? = optional, ^ = check parent elements
            restrict: 'A' // E = Element, A = Attribute, C = Class, M = Comment
            //template: ,
            //templateUrl: '',
            //replace: true,
            //transclude: true, //JCV Set the ability to contain another Angular DOM components
            //compile: function(tElement, tAttrs, function transclude(function(scope, cloneLinkingFn){ return function linking(scope, elm, attrs){}})),
            // link: function($scope, iElm, iAttrs, controller) {
               
            // }
        };
    }]);
    //JCV 11-Dic-2014, Directive to update the form fields of the response with previous data
    QQ.directive('chatEditing', ['qReply', function ( qReply ) {
        return {
            restrict: 'A',
            link: function( scope, iElement, iAttrs, controller ) {
                scope.$watch( 'showQuestion', function( value ) { //when click on editi button, showQuestion is set to false
                    if( value === false ) {
                        qReply.loadAnswer(  iElement.find('.' + htmlClasses.responsePanel ), scope.currentQuestionEditing );
                    }
                });
            }
        };

    }]);

    //END
})();