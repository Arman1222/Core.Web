(function() {
	'use strict';

	window.app.controller('portalController', portalController);

	portalController.$inject = ['$scope', '$modal', 'portalService', 'alerts', 'applicationusersService', '$location'];
	function portalController($scope, $modal, portalService, alerts, applicationusersService, $location) {
	    var vm = this;
	    portalService.myApplicationList().then(function (data) {
	        vm.listaplikasi = data.data;
	    });
	    applicationusersService.getMypeopleDashBoardData().then(function (result) {
	        if (result.data.length > 0) {
	            vm.dataMyPeople = result.data[0];
	        }
	    });
	    applicationusersService.getEmployee().then(function (result) {
	        vm.dataEmployee = result;
	    });
	}
})();