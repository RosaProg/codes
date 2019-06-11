(function() {
    angular.module('htmlHelpers', ['ngSanitize'])
    .filter('unsafe', ['$sce', function($sce) {
        return $sce.trustAsHtml;
    }]);

    //END
})();