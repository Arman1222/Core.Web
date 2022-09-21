(function() {
	'use strict';
    //o20170927
	window.app.controller('master_office_Controller', master_office_Controller);

	master_office_Controller.$inject = ['$scope', '$modal', 'master_office_Service', 'alerts'];
	function master_office_Controller($scope, $modal, master_office_Service, alerts) {
	    var vm = this;
	    vm.add = add;
	    vm.master_office_GetPage = master_office_Service.getPage;
		vm.message = alerts;
		vm.loading = true;

		vm.tableActions = {
		    onEdit: edit
		};

        //o20170927
		function add() {
		    var modalInstance = $modal.open({
		        windowClass: 'form-modal-window-1200',
		        backdrop: 'static',
				template: '<add-masteroffice />'
		    });

		    modalInstance.result.then(function () {
		        alert("Insert Success!");
		        vm.refreshData();
		    }, function () {
		        //$log.info('Modal dismissed at: ' + new Date());
		    });
		}
	    //o20170928
		function edit(data) {
		    var modalInstance = $modal.open({
		        windowClass: 'form-modal-window-1200',
		        backdrop: 'static',
		        keyboard: false,
		        template: '<edit-masteroffice data="data" />',
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