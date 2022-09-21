(function () {
	'use strict';

	angular.module('MyWeb').factory("$exceptionHandler",
		['$log', 'alerts', function ($log, alerts) {
			return function (exception, cause) {
				//alerts.error("There was an error : " + exception.message);
				$log.error(exception, cause);
			};
		}]);

})();
