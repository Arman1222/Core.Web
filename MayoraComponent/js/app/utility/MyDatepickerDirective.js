(function () {
    'use strict';
    //http://jsfiddle.net/xB6c2/121/   
    window.app.directive("myDatepicker", function () {
        return {
            restrict: 'AE',
            //replace: true,
            //templateUrl: 'MyDatePicker.tmpl',
            //transclude: true,
            scope: {
                title: '@',
                ngModel: '=?', 
                ngChange: '&',
                ngDisabled: '=?',
                minDate: '=?',
                maxDate: '=?',
                dateFormat: '@?',
                yearRange: '@?'
                //message: '='
            },            
            template:
                //'<div>' +
	            //'<label for="{{title}}" class="control-label col-sm-2">{{title}}</label>' +
	            '<div class="inner-addon right-addon">' +
                '<i class="glyphicon glyphicon-calendar"></i>' +
	            '<input type="text" class="form-control" ng-model="ngModel" ng-disabled="ngDisabled" min-date="minDate" max-date="maxDate" date-format="dateFormat" year-range="yearRange" my-datepicker-input />' +
                '</div>'+
	            //'<span class="input-group-addon">'+
	            //    '<span class="fa fa-calendar"></span>'+
	            //'</span>'+
	            //'</div>' +
	            '</div>'
            ,link: function (scope, elem, attrs) {    
               scope.$watch('ngModel', function () {
                   scope.ngChange();
               });

            },
        }
    });
})();