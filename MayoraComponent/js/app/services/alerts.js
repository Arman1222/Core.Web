(function () {
	'use strict';

	var serviceId = 'alerts';

	angular.module('MyWeb').factory(serviceId, function () {	    
		    return window.alerts;
	});

})();
