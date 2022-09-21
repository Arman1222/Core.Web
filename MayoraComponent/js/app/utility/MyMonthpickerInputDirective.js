(function () {
    'use strict';

    window.app.directive('myMonthpickerInput', ['$filter',function ($filter) {
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
            //http://jsfiddle.net/robattfield/M5PHP/
            //http://stackoverflow.com/questions/20795597/how-to-convert-jquery-date-picker-to-select-only-month-and-year

            if (!scope.dateFormat) {
                scope.dateFormat = 'mm/yy';
            }

            // view --> model (change date to string)
            //ngModelCtrl.$parsers.push(function (viewValue) {
            //    return viewValue.toISOString();
            //});
            // model --> view (change string to date)
            ngModelCtrl.$formatters.push(function (modelValue) {
                return $.datepicker.formatDate(scope.dateFormat, modelValue);
                //return $filter('date')(modelValue, "dd-MM-yy");                
                //return $filter('dateFormat')(modelValue);
            });

            var startDate = new Date();
            startDate.setYear(1980);
            if (scope.minDate !== undefined && scope.minDate instanceof Date)
            {
                startDate = scope.minDate;
            }
            var endDate = new Date();
            endDate.setYear(2030);
            if (scope.maxDate !== undefined && scope.maxDate instanceof Date) 
            {
                endDate = scope.maxDate;
            }       

            if (!scope.yearRange) {
                scope.yearRange = '1980:2030';
            }
            
            var dateFunction = function () {
                $(function () {
                    element.datepicker({
                        //numberOfMonths: 2,
                        //showOn: "both", jika mau ada tombol juga
                        changeYear: true,
                        changeMonth: true,
                        dateFormat: scope.dateFormat,
                        minDate: startDate,
                        maxDate: endDate,
                        yearRange: scope.yearRange,
                        showButtonPanel: true,
                        beforeShow: function (e, t) {                           
                            //var id = $(this).attr('id');
                            //t.dpDiv.addClass("hide-calendar");
                            //t.dpDiv.addClass("MonthDatePicker");
                            //t.dpDiv.addClass("HideTodayButton");

                            $("#ui-datepicker-div").addClass("hide-calendar");
                            //$("#ui-datepicker-div").addClass('MonthDatePicker');
                            $("#ui-datepicker-div").addClass('HideTodayButton');
                        },                        
                        onClose: function (dateText, inst) {
                            //var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                            var month = inst.selectedMonth;

                            //var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                            var year = inst.selectedYear;

                            //$.datepicker('setDate', new Date(year, month, 1));
                            $('#' + inst.id).datepicker('setDate', new Date(year, month, 1));
                            //http://stackoverflow.com/questions/5250244/jquery-date-formatting
                            var selectedDateText = $.datepicker.formatDate('dd/mm/yy', new Date(year, month, 1));
                            var parsedDate = $.datepicker.parseDate('dd/mm/yy', selectedDateText);
                            ngModelCtrl.$setViewValue(parsedDate);                            
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