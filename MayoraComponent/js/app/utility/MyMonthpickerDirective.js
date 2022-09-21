(function () {
    'use strict';   
    window.app.directive("myMonthpicker", function () {
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
                '<style type="text/css">' +
                    '.MonthDatePicker .ui-datepicker-year'+
                    '{'+
                        'display:none;'+
                    '}'+
                    '.HideTodayButton .ui-datepicker-buttonpane .ui-datepicker-current'+
                    '{'+
                        'visibility:hidden;'+
                    '}'+
                    '.hide-calendar .ui-datepicker-calendar'+
                    '{'+    
                        'display:none!important;'+
                        'visibility:hidden!important'+
                    '}'+
                '</style>' +
                '<div class="input-group">' +
                    '<span class="input-group-btn">' +
                         '<button class="btn btn-primary" type="button" ng-click="clear()" ng-disabled="ngDisabled"><i class="fa fa-times"></i></button>' +
                    '</span>' +
	                '<div class="inner-addon right-addon">' +
                        '<i class="glyphicon glyphicon-calendar"></i>' +
	                    '<input type="text" class="form-control" ng-model="ngModel" ng-disabled="ngDisabled" min-date="minDate" max-date="maxDate" date-format="dateFormat" year-range="yearRange" my-monthpicker-input />' +
                    '</div>' +
	            '</div>'
	            //'<span class="input-group-addon">'+
	            //    '<span class="fa fa-calendar"></span>'+
	            //'</span>'+
	            //'</div>' +
            ,link: function (scope, elem, attrs) {    
               scope.$watch('ngModel', function () {
                   scope.ngChange();
               });
               scope.clear = function () {
                   scope.ngModel = null;
               };

            },
        }
    });
    //    .run([
    //   '$templateCache',
    //   function ($templateCache) {
    //       $templateCache.put('MyDatePicker.tmpl', myDatePickerTemplate); // This saves the html template we declared before in the $templateCache
    //   }
	//]);

})();