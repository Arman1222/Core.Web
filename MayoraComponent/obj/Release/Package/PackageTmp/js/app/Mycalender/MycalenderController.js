(function() {
	'use strict';

	window.app.controller('mycalenderController', ApplicationUsersController);

	ApplicationUsersController.$inject = ['$scope', '$modal', 'mycalenderService', 'alerts', '$location', '$compile', 'uiCalendarConfig'];
	function ApplicationUsersController($scope, $modal, mycalenderService, alerts, $location, $compile, uiCalendarConfig) {
	    var vm = this;
	    var date = new Date();
	    var d = date.getDate();
	    var m = date.getMonth();
	    var y = date.getFullYear();
	    vm.modelNgCal = [
                      {
                          title: 'All Day Event',
                          start: new Date(y, m, 1),
                          backgroundColor: "#f56954", //red
                          borderColor: "#f56954" //red
                      },
                      {
                          title: 'Long Event',
                          start: new Date(y, m, d - 5),
                          end: new Date(y, m, d - 2),
                          backgroundColor: "#f39c12", //yellow
                          borderColor: "#f39c12" //yellow
                      },
                      {
                          title: 'Meeting',
                          start: new Date(y, m, d, 10, 30),
                          allDay: false,
                          backgroundColor: "#0073b7", //Blue
                          borderColor: "#0073b7" //Blue
                      },
                      {
                          title: 'Lunch',
                          start: new Date(y, m, d, 12, 0),
                          end: new Date(y, m, d, 14, 0),
                          allDay: false,
                          backgroundColor: "#00c0ef", //Info (aqua)
                          borderColor: "#00c0ef" //Info (aqua)
                      },
                      {
                          title: 'Birthday Party',
                          start: new Date(y, m, d + 1, 19, 0),
                          end: new Date(y, m, d + 1, 22, 30),
                          allDay: false,
                          backgroundColor: "#00a65a", //Success (green)
                          borderColor: "#00a65a" //Success (green)
                      },
                      {
                          title: 'Click for Google',
                          start: new Date(y, m, 28),
                          end: new Date(y, m, 29),
                          url: 'http://google.com/',
                          backgroundColor: "#3c8dbc", //Primary (light-blue)
                          borderColor: "#3c8dbc" //Primary (light-blue)
                      }
	    ];










	    var date = new Date();
	    var d = date.getDate();
	    var m = date.getMonth();
	    var y = date.getFullYear();

	    vm.changeTo = 'Hungarian';
	    /* event source that pulls from google.com */
	    vm.eventSource = null;
	    /* event source that contains custom events on the scope */
	    vm.events = [
          { title: 'All Day Event', start: new Date(y, m, 1) },
          { title: 'Long Event', start: new Date(y, m, d - 5), end: new Date(y, m, d - 2) },
          { id: 999, title: 'Repeating Event', start: new Date(y, m, d - 3, 16, 0), allDay: false },
          { id: 999, title: 'Repeating Event', start: new Date(y, m, d + 4, 16, 0), allDay: false },
          { title: 'Birthday Party', start: new Date(y, m, d + 1, 19, 0), end: new Date(y, m, d + 1, 22, 30), allDay: false },
          { title: 'Click for Google', start: new Date(y, m, 28), end: new Date(y, m, 29), url: 'http://google.com/' }
	    ];
	    /* event source that calls a function on every view switch */
	    vm.eventsF = function (start, end, timezone, callback) {
	        var s = new Date(start).getTime() / 1000;
	        var e = new Date(end).getTime() / 1000;
	        var m = new Date(start).getMonth();
	        var events = [{ title: 'Feed Me ' + m, start: s + (50000), end: s + (100000), allDay: false, className: ['customFeed'] }];
	        callback(events);
	    };

	    vm.calEventsExt = {
	        color: '#f00',
	        textColor: 'yellow',
	        events: [
               { type: 'party', title: 'Lunch', start: new Date(y, m, d, 12, 0), end: new Date(y, m, d, 14, 0), allDay: false },
               { type: 'party', title: 'Lunch 2', start: new Date(y, m, d, 12, 0), end: new Date(y, m, d, 14, 0), allDay: false },
               { type: 'party', title: 'Click for Google', start: new Date(y, m, 28), end: new Date(y, m, 29), url: 'http://google.com/' }
	        ]
	    };
	    /* alert on eventClick */
	    vm.alertOnEventClick = function (date, jsEvent, view) {
	        console.log(date.title + ' was clicked ');
	    };
	    /* alert on Drop */
	    vm.alertOnDrop = function (event, delta, revertFunc, jsEvent, ui, view) {
	        console.log('Event : ' + event + ' Droped to make dayDelta ' + delta);
	    };
	    /* alert on Resize */
	    vm.alertOnResize = function (event, delta, revertFunc, jsEvent, ui, view) {
	        console.log('Event Resized to make dayDelta ' + delta);
	    };
	    /* add and removes an event source of choice */
	    vm.addRemoveEventSource = function (sources, source) {
	        var canAdd = 0;
	        angular.forEach(sources, function (value, key) {
	            if (sources[key] === source) {
	                sources.splice(key, 1);
	                canAdd = 1;
	            }
	        });
	        if (canAdd === 0) {
	            sources.push(source);
	        }
	    };
	    /* add custom event*/
	    vm.addEvent = function () {
	        vm.events.push({
	            title: 'Open Sesame',
	            start: new Date(y, m, 28),
	            end: new Date(y, m, 29),
	            className: ['openSesame']
	        });
	    };
	    /* remove event */
	    vm.remove = function (index) {
	        vm.events.splice(index, 1);
	    };
	    /* Change View */
	    vm.changeView = function (view, calendar) {
	        uiCalendarConfig.calendars[calendar].fullCalendar('changeView', view);
	    };
	    /* Change View */
	    vm.renderCalender = function (calendar) {
	        if (uiCalendarConfig.calendars[calendar]) {
	            uiCalendarConfig.calendars[calendar].fullCalendar('render');
	        }
	    };
	    /* Render Tooltip */
	    vm.eventRender = function (event, element, view) {
	        element.attr({
	            'tooltip': event.title,
	            'tooltip-append-to-body': true
	        });
	        $compile(element)($scope);
	    };
	    /*drop event*/
	    vm.dropEvent = function (date, allDay) { // this function is called when something is dropped

	        // retrieve the dropped element's stored Event Object
	        var originalEventObject = $(this).data('eventObject');

	        // we need to copy it, so that multiple events don't have a reference to the same object
	        var copiedEventObject = $.extend({}, originalEventObject);

	        // assign it the date that was reported
	        copiedEventObject.start = date._d;
	        //copiedEventObject.allDay = allDay;
	        copiedEventObject.backgroundColor = $(this).css("background-color");
	        copiedEventObject.borderColor = $(this).css("border-color");

	        // render the event on the calendar
	        // the last `true` argument determines if the event "sticks" (http://arshaw.com/fullcalendar/docs/event_rendering/renderEvent/)
	        //$element.fullCalendar('renderEvent', copiedEventObject, true);
	        vm.events.push(copiedEventObject);
	        // is the "remove after drop" checkbox checked?
	        if ($('#drop-remove').is(':checked')) {
	            // if so, remove the element from the "Draggable Events" list
	            $(this).remove();
	        }
	    }
	    /* config object */
	    vm.uiConfig = {
	        calendar: {
	            //height: 450,
	            editable: true,
	            droppable: true,
	            header: {
	                left: 'prev,next today',
	                center: 'title',
	                right: 'month,agendaWeek,agendaDay'
	            },
	            buttonText: {
	                today: 'today',
	                month: 'month',
	                week: 'week',
	                day: 'day'
	            },
	            eventClick: vm.alertOnEventClick,
	            eventDrop: vm.alertOnDrop,
	            eventResize: vm.alertOnResize,
	            eventRender: vm.eventRender,
	            drop: vm.dropEvent
	        }
	    };

	    vm.changeLang = function () {
	        if (vm.changeTo === 'Hungarian') {
	            vm.uiConfig.calendar.dayNames = ["Vasárnap", "Hétfő", "Kedd", "Szerda", "Csütörtök", "Péntek", "Szombat"];
	            vm.uiConfig.calendar.dayNamesShort = ["Vas", "Hét", "Kedd", "Sze", "Csüt", "Pén", "Szo"];
	            vm.changeTo = 'English';
	        } else {
	            vm.uiConfig.calendar.dayNames = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
	            vm.uiConfig.calendar.dayNamesShort = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
	            vm.changeTo = 'Hungarian';
	        }
	    };
	    /* event sources array*/
	    vm.eventSources = [vm.events];
	    //vm.eventSources2 = [vm.calEventsExt, vm.eventsF, vm.events];
	}
})();