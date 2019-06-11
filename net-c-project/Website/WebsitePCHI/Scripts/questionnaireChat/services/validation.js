(function(){
    /**
     * JCV 03-Dec-2014
     * Module to creat validation functions, some functions were extracted from http://jqueryvalidation.org/
     */
    angular.module('validationService', [])
    .factory('qValidate', function qValidateFactory(){
        var that = {};
        
        that.messages = {
            required: "This field is required.",
            email: "Please enter a valid email address.",
            url: "Please enter a valid URL.",
            date: "Please enter a valid date.",
            dateISO: "Please enter a valid date ( ISO ).",
            number: "Please enter a valid number.",
            digits: "Please enter only digits.",
            equalTo: "Please enter the same value again.",
            minLength: "Oops! It must contain at least {{minLength}} characters!",
            oneDigit: "Oops! password must contain at least one number (0-9)!",
            oneUpperCaseletter: "Oops! password must contain at least one uppercase letter (A-Z)!",
            equalPassword: 'Oops! Those passwords didn’t quite match. Please try re-typing your password.',
            codeCheckRadio: 'The code has been send again'
        };


        //JCV 03-Dec-2014 Checks if an element is checkable
        function checkable( type ) {
            if( typeof type !=='string'){
                type = type.type;//type is an DOM element
            }
            return ( /radio|checkbox|hiddencheckbox/i ).test( type );
        }

        //JCV 03-Dec-2014 Checks if at least one element is selected or checked
        function getLength ( value, element ) {
            switch ( element.nodeName.toLowerCase() ) {
                case "select":
                    return $( "option:selected", element ).length;
                case "input":
                    if ( checkable( element ) ) {
                        return that.findByName( element.name ).filter( ":checked" ).length;
                    }
            }

            return value.length;
        }

        that.init = function( container ) {
            that.container = container;
        };

        that.findByName = function ( name ) {
            return that.container.find( "[name='" + name + "']" );
        };

        that.required = function ( element ) {
            var value = $(element).val();
            if ( element.nodeName.toLowerCase() === "select" ) {
                // could be an array for select-multiple or a string, both are fine this way
                return value && value.length > 0;
            }
            if ( checkable( element ) ) {
                return getLength( value, element ) > 0;
            }
            return $.trim( value ).length > 0;
        };

        // http://jqueryvalidation.org/date-method/
        that.date = function( value, element ) {
            return !/Invalid|NaN/.test( new Date( value ).toString() );
        };

        // http://jqueryvalidation.org/dateISO-method/
        that.dateISO = function( value, element ) {
            return ( /^\d{4}[\/\-](0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])$/ ).test( value );
        };

        // http://jqueryvalidation.org/minlength-method/
        that.minLength = function( value, element, param ) {
            var length = $.isArray( value ) ? value.length : this.getLength( value, element );
            return length >= param;
        };

        // http://jqueryvalidation.org/maxlength-method/
        that.maxLength = function( value, element, param ) {
            var length = $.isArray( value ) ? value.length : this.getLength( value, element );
            return length <= param;
        };

        that.oneDigit = function( value ) {
            return (/[0-9]/).test(value);
        };

        that.oneUpperCaseLetter = function( value ) {
            return (/[A-Z]/).test(value);
        };

        that.equal = function( value, value2) {
            return value === value2;
        };

        return that;

    });

})();