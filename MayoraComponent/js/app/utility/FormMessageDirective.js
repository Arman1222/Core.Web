(function () {
	'use strict';

	window.app.directive('formMessage', ["alerts", function (alerts) {
	    return {
	        restrict: 'AE',
	        scope: {
	            model: '=' 
	        },
	        controller: controller,
	        controllerAs: 'vm',
	        link: function (scope, element, attrs) {	        

	            scope.model = {
	                error: function (message) {
	                    alerts.error(message, element);
	                }
	            };
	        },
	        template:
				'<div></div>',
	        transclude: true,
	        replace: true
	    }
	}]);

	controller.$inject = ['$scope'];
	function controller($scope) {
		var vm = this;		
	}
})();