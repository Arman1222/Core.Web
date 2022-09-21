(function() {
	'use strict';
    //o20170928
	window.app.controller('master_area_Controller', master_area_Controller);

	master_area_Controller.$inject = ['$scope', '$modal', 'master_area_Service', 'alerts'];
	function master_area_Controller($scope, $modal, master_area_Service, alerts) {
	    var vm = this;
	    vm.add = add;
	    vm.master_area_GetPage = master_area_Service.getPage;
		vm.message = alerts;
		vm.loading = true;

		vm.tableActions = {
		    onEdit: edit
		};

		function add() {
		    var modalInstance = $modal.open({
		        //windowClass: 'form-modal-window-1200',
		        backdrop: 'static',
		        template: '<add-masterarea />'
		    });

		    modalInstance.result.then(function () {
		        alert("Insert Success!");
		        vm.refreshData();
		    }, function () {
		        //$log.info('Modal dismissed at: ' + new Date());
		    });
		}
	    
		function edit(data) {
		    var modalInstance = $modal.open({
		        //windowClass: 'form-modal-window-1200',
		        backdrop: 'static',
		        keyboard: false,
		        template: '<edit-masterarea data="data" />',
		        scope: angular.extend($scope.$new(true), { data: data })
		    });

		    modalInstance.result.then(function () {
		        alert("Update Success!");
		        vm.refreshData();
		    }, function () {
		        //$log.info('Modal dismissed at: ' + new Date());
		    });
		}
	}
})();