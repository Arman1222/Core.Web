(function () {
    'use strict';

    //http://stackoverflow.com/questions/25971845/custom-time-picker-using-angular-js

    //Template for the time picker, no CSS, pure HTML. The time-picker tag will be replaced by this
	var timePickerTemplate = [
        '<div class="timePicker">',
        '<label ng-click="toggleTimePicker()">',
        '<input type="text" ng-model="timeLabel" ng-bind="timeValue" disabled>',
        '</label>',
        '<div class="cal-wraper shadow"  ng-show="selecting">',
        '<table>',
        '<tr class="navigation">',
        '<tr class="time">',
        '<td class="mer"><div ng-click="selectMeridian(meridian)" ng-repeat="meridian in meridians" ng-bind="meridian"></div></td>',
        '<td class="hours"><div ng-click="selectHour(hour)" ng-repeat="hour in hours" ng-bind="hour.label"></div></td>',
        '<td class="minutes"><div ng-click="selectMinute(minute)" ng-repeat="minute in minutes" ng-bind="minute"></div></td>',
        '</tr>',
        '</table>',
        '</div>',
        '</div>'
	].join('\n');

	window.app.directive("myTimepicker", function () {
	    return {
	        restrict: 'E',
	        templateUrl: "timePicker.tmpl",
	        transclude: true,
	        controller: function ($scope) {

	            var timeObj = { AM: [], PM: [] };
	            for (var i = 0; i <= 11; i++) {
	                timeObj.AM.push({ label: (i < 10 ? '0' + i : i), value: i });
	            }
	            timeObj.PM.push({ label: 12, value: 12 });
	            for (var i = 1; i <= 11; i++) {
	                timeObj.PM.push({ label: (i < 10 ? '0' + i : i), value: i + 12 });
	            }

	            $scope.meridians = ["AM", "PM"];
	            $scope.hours = timeObj.AM;
	            $scope.minutes = ["00", "15", "30", "45"];

	            if ($scope.timeValue == undefined) {
	                $scope.timeValue = 9 * 60 * 60 * 1000;
	            }

	            $scope.toggleTimePicker = function () {
	                $scope.selecting = !$scope.selecting;
	            };

	            $scope.$watch('timeValue', function (val) {
	                $scope.updateLabel(val);
	            });

	            $scope.selectMeridian = function (meridian) {
	                $scope.hours = timeObj[meridian];
	                $scope.timeValue = (meridian == "AM" ? (9 * 60 * 60 * 1000) : (15 * 60 * 60 * 1000));
	            };

	            $scope.selectHour = function (hour) {
	                $scope.timeValue = (hour.value * 60 * 60 * 1000);
	             };

	            $scope.selectMinute = function (minute) {
	                var time = $scope.timeValue;
	                var mts = time % (60 * 60 * 1000);
	                $scope.timeValue = (time - mts + minute * 60 * 1000);
	            };

	            $scope.updateLabel = function (timeValue) {
	                var mts = timeValue % (60 * 60 * 1000);
	                var hrs = (timeValue - mts) / (60 * 60 * 1000);
	                var mts2 = mts / (60 * 1000);
	                var mer = (hrs < 11) ? "AM" : "PM";
	                var hrs2 = (hrs > 12) ? hrs - 12 : hrs;

	                $scope.timeLabel = (hrs2 < 10 ? '0' + hrs2 : hrs2) + ":" + (mts2 == 0 ? '00' : mts2) + " " + mer;
	            };
	        }
	    }
	}).run([
       '$templateCache',
       function ($templateCache) {
           $templateCache.put('timePicker.tmpl', timePickerTemplate); // This saves the html template we declared before in the $templateCache
       }
	]);

})();