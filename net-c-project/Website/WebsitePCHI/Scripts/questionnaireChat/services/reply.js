(function() {
    
    var htmlClasses = {
        error: 'error'
    };

    angular.module('replyService', ['validationService'])
    .factory('qReply', ['qValidate', function qReplyFactory(qValidate){
        var that = {};

        function answerString( tpl, name, value) {
            for (var i = 0; i < that.question.PossibleAnswers.length; i++) {
                var obj = that.question.PossibleAnswers[i];
                if(obj.Name === name && obj.Value === value ) {
                    return formatAnswer(tpl, obj.AnswerText );
                }
            }

            return formatAnswer( tpl, '');
        }

        function formatAnswer( tpl, value ) {
            return tpl.replace('%Value%', value );
        }

        function setMessage( elem, msgName, objReplace) {
            var msg = "";
            elem.next( '.' + htmlClasses.error ).remove();//Delete the previous message to always have only one error message per field
            if( !objReplace ) {
                msg = qValidate.messages[msgName];
            }
            else {
                msg = qValidate.messages[msgName];
                angular.forEach( objReplace,function ( replace, idx ) {
                     msg += msg.replace( replace.name,  replace.value );
                });
            }

            elem.after( '<span class="response error">' +  msg + '</span>');
            
        }

        that.loadAnswer = function ( responsePanel, question ) {
            angular.forEach( question.AnswerNames, function(name, idx ) {
                //We declare here the variables because of javascript hoisting
                var type = name.split('.');
                var value = question.AnswerValues[idx];
                switch( type[0] ) {
                    case "Radio":
                    case "CheckBox":
                    case "HiddenCheckBox":
                        responsePanel.find("input[name='" + name + "'][value='" + value + "']").click();
                        if (type[0] === "HiddenCheckBox") {//JCV It is question about the body
                            if (value !== undefined) {
                                selectedBodyParts[type[2]] = 1;
                                selectedBodyParts["length"]++;
                            }
                            //tpl = FormatAnswerBody(tpl, selectedBodyParts);
                        }
                        break;
                    case "DropDown":
                    case "TextBox":
                    case "TextArea":
                    case "DatePicker":
                    case "Slider":
                        //@TODO JCV Check if is the correct way to set a value of an slider
                        responsePanel.find('input[name="' + name + '"]').val( value );
                        break;
                }//End Switch

                //@TODO JCV Check the values of the body
                // if (hiddenCheckBox) {
                //   answersText = FormatAnswerBody(answersText, selectedBodyParts);
                //   hiddenCheckBox = false;
                // }
                // this.Answer().Value(answersText);
            });
        };


        that.updateAnswer = function ( responsePanel, question ) {
            that.question           = question;
            var tpl                 = question.Answer.HtmlTemplate;
            var selectedBodyParts   = [];
            var valid               = true;
            var answerNames         = [];
            var answerValues        = [];

            qValidate.init( responsePanel );
            //Each field
            angular.forEach(question.ItemNames, function(name, idx) {
                //We declare here the variables because of javascript hoisting
                var type = name.split('.');
                var elem = qValidate.findByName( name );
                var value = '';
                var checked;
                var passwordSuccess = true;
                elem.next( '.' + htmlClasses.error ).remove();

                if( elem.prop('disabled') ) {
                    return false; //go to Next Item
                }

                if( !qValidate.required( elem[0] ) && type[0] !== 'CheckBox') {//Checkboxes could be skip it
                    setMessage( elem.eq(0), 'required' );//If is ra
                    valid = false;
                    return false; //got to next Item
                }

                switch( type[0] ) {
                    case "Radio":
                    case "CheckBox":
                    case "HiddenCheckBox":
                        checked = elem.filter(':checked');
                        value = checked.val();
                        if (responsePanel.find("input[name='" + name + "'][value='" + value + "']").attr("data-applicable") === "MakeItemNotApplicable") {
                            tpl = tpl.substring(0, tpl.indexOf("%Value%"));
                            tpl += "%Value%";
                        }
                        tpl = answerString( tpl, name, value );

                        if (type[0] === "HiddenCheckBox") {
                            if (value !== undefined) {
                                selectedBodyParts[type[2]] = 1;
                                selectedBodyParts["length"]++;
                            }
                            //tpl = FormatAnswerBody(tpl, selectedBodyParts);
                        }
                        
                        break;
                    case "DropDown":
                        value = elem.filter(':selected').val();
                        tpl = answerString( tpl, name, value);
                        
                        break;
                    case "TextBox":
                    case "TextArea":
                    case "DatePicker":
                        value = elem.val();
                        tpl = formatAnswer(tpl, value);
                        break;
                    case "Slider":
                        if (!Modernizr.inputtypes.range) {
                            value = responsePanel.find('#Support.' + name ).slider("value");
                        } else {
                            value = elem.val();
                        }
                        tpl = answerString(tpl, name, value);
                        break;
                    case "Password":
                        value = elem.val();
                        var confirm = responsePanel.find('input[name="passwordrepeat"]').val();

                        if(!qValidate.minLength(value, elem, 6 )) {
                            setMessage( elem.eq(0), 'minLength', { '{{minLength}}': 6 } );
                            valid = false;
                            break;
                        }

                        if(!qValidate.oneDigit( value )) {
                            setMessage( elem.eq(0), 'oneDigit' );
                            valid = false;
                            break;
                        }

                        if(!qValidate.oneUpperCaseLetter( value )) {
                            setMessage( elem.eq(0), 'oneUpperCaseLetter' );
                            valid = false;
                            break;
                        }

                        if ( !qValidate.equal( value, confirm ) ) {
                            setMessage( elem.eq(0), 'equalPassword');
                            valid = false;
                            break;
                        }
                        tpl = answerString(tpl, name, value );
                        break;
                    case "Provider": //@TODO JCV check if it works in a real questionnaire
                        value = util.chatFooter.find('[name="' + name + '"]:checked').val();
                        if (value !== "None") {
                            TestTwoStageAuthentication(value);
                        }
                        tpl = answerString(tpl, name, value );
                        break;
                    case "codeCheckRadio": //@TODO JCV check if it works in a real questionnaire
                        elem = responsePanel.find('[name="codeCheckRadio"]:checked');
                        value = elem.val();
                        if (value == 'No') {
                            setMessage( elem.eq(0), 'codeCheckRadio');
                            TestTwoStageAuthentication( question.AnswerValues[0] );
                            valid = false;
                            break;
                        }
                        break;
                    case "TwoStageCode":
                         value = elem.val();
                          // TODO add on success function and a wait status
                          question.Status = "Wait";
                          question.Answer.Value = "Please wait while we verify your response";
                          VerifyTwoStageAuthentication(value, question.AnswerValues[0], this,
                              function (item, result) {
                                  question.Status = "Historical";
                                  question.Answer.Value = "The two stage authentication method is working";
                                  //util.self.GoToNextItem(); JCV Check how to make this works
                              },
                              function (item, result) {
                                  question.Status = "Current";
                                  setMessage( elem.eq(0), 'codeCheckRadio');//"Oops! That code is not correct."
                              });
                            valid = false;//JCV Check why it return false in the original script
                        break;
                }//End Switch

                if( !valid ) {
                    return false; //got to the next item
                }
                
                answerNames.push(name);
                answerValues.push(value);//To save the answer and show it when editin+ g it

            });//End forEach

            //@TODO JCV Check the values of the body
                // if (hiddenCheckBox) {
                //   answersText = FormatAnswerBody(answersText, selectedBodyParts);
                //   hiddenCheckBox = false;
                // }
                // this.Answer().Value(answersText);
            
            return {
                tpl: tpl,
                valid: valid,
                answerNames: answerNames,
                answerValues: answerValues
            };
        };//End updateAnswer

        return that;
    }]);

})();