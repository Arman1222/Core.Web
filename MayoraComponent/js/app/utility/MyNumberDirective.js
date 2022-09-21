(function () {
    'use strict';
    //http://blog.yodersolutions.com/add-comma-separated-thousands-to-number-input-in-angularjs/
    window.app.directive("myNumber", function () {
        return {
            restrict: 'AE',         
            scope: {
                title: '@',
                min: '@?',
                 max: '@?',
                maxDecimals: '@?',
                maxDigits: '@?',
                append: '@?',
                prepend: '@?',
                ngModel: '=?',
                hint: '@?',
                ngChange: '&',
                ngBlur: '&',
                ngDisabled: '@'
                //message: '='
            }
            ,template:
            //    '<div class="form-group">'+
            //        '<label class="control-label col-sm-2">{{title}}</label>' +
            //        '<div class="col-sm-10">' +
                      '<input type="text" min="min" max="max" max-decimals="maxDecimals" max-digits="maxDigits" prepend="prepend" append="append" ng-model="ngModel" ng-disabled="ngDisabled" ng-change="ngChange" ng-blur="ngBlur" fcsa-number="options" class="form-control" />' +
                      '<small ng-show="hint" class="number-input-hint">{{hint}}</small>'
            //        '</div>'+
            //    '</div>'
            , link: function (scope, elem, attrs) {
                scope.$watch('ngModel', function () {
                    scope.ngChange();
                });               

            },
        }
    });   

})();