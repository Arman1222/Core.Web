(function () {
    'use strict';

    window.app.directive('myDatepickerInput', ['$filter',function ($filter) {
    return {
        require: 'ngModel',
        scope: {           
            ngModel: '=', 
            minDate: '=?',
            maxDate: '=?',
            refreshDate: '&',
            dateFormat: '=?',
            yearRange: '=?'
        },
        link: function (scope, element, attrs, ngModelCtrl) {            
            //http://stackoverflow.com/questions/15343068/using-angularjs-filter-in-input-element
            //http://stackoverflow.com/questions/34063785/how-to-filter-the-ng-model-value-using-angularjs-directive

            // view --> model (change date to string)
            //ngModelCtrl.$parsers.push(function (viewValue) {
            //    return viewValue.toISOString();
            //});
            // model --> view (change string to date)
            ngModelCtrl.$formatters.push(function (modelValue) {
                //return $filter('date')(modelValue, "dd-MM-yy");
                return $filter('dateFormat')(modelValue);
            });            
            
            if (!scope.dateFormat) {
                scope.dateFormat = 'dd/mm/yy';
            }      
            
            var startDate = new Date();
            startDate.setYear(1980);           

            if (scope.minDate !== undefined && scope.minDate instanceof Date) {
                startDate = scope.minDate;                
            }

            var endDate = new Date();
            endDate.setYear(startDate.getFullYear() + 100);

            if (scope.maxDate !== undefined && scope.maxDate instanceof Date) {
                endDate = scope.maxDate;                
            }

            if (!scope.yearRange) {
                scope.yearRange = startDate.getFullYear() + ':' + endDate.getFullYear();
            }

            var dateFunction = function () {
                $(function () {
                    element.datepicker({
                        numberOfMonths: 2,
                        //showOn: "both", jika mau ada tombol juga
                        changeYear: true,
                        changeMonth: true,
                        dateFormat: scope.dateFormat,
                        minDate: startDate,
                        maxDate: endDate,
                        yearRange: scope.yearRange,
                        beforeShow: function (e, t) {
                            //var id = $(this).attr('id');
                            //t.dpDiv.addClass("hide-calendar");
                            //t.dpDiv.addClass("MonthDatePicker");
                            //t.dpDiv.addClass("HideTodayButton");
                            $("#ui-datepicker-div").removeClass("hide-calendar");
                            //$("#ui-datepicker-div").addClass('MonthDatePicker');
                            $("#ui-datepicker-div").removeClass('HideTodayButton');
                        },
                        //showButtonPanel: true,
                        onSelect: function (dateText, inst) {
                            var parsedDate = $.datepicker.parseDate(scope.dateFormat, dateText);
                            ngModelCtrl.$setViewValue(parsedDate);
                            //if (scope.$root.$$phase != '$apply' && scope.$root.$$phase != '$digest') {
                            //    scope.$apply();
                            //}
                            scope.$applyAsync();
                        }
                    });
                });
            };
            scope.refreshDate = function () {
                dateFunction();
            };

            scope.refreshDate();            
        }
    }
    }]);

})();