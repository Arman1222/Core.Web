(function() {
	'use strict';

	window.app.controller('master_userController', master_userController);

	master_userController.$inject = ['$scope', '$modal', 'master_userService', 'alerts'];
	function master_userController($scope, $modal, master_userService, alerts) {
	    var vm = this;
	    vm.add = add;
		vm.master_userGetPage = master_userService.getPage;
		vm.message = alerts;
		vm.loading = true;

		vm.tableActions = {
		    onEdit: edit,
		    onHapus: hapus,
		    onCheck: check,
		};

		function add() {
		    var modalInstance = $modal.open({
		        windowClass: 'form-modal-window-1200',
		        backdrop: 'static',
				template: '<add-masteruser />'
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
		        windowClass: 'form-modal-window-1200',
		        backdrop: 'static',
		        keyboard: false,
		        template: '<edit-masteruser data="data" />',
		        scope: angular.extend($scope.$new(true), { data: data })
		    });

		    modalInstance.result.then(function () {
		        alert("Update Success!");
		        vm.refreshData();
		    }, function () {
		        //$log.info('Modal dismissed at: ' + new Date());
		    });
		}

		function hapus(data) {
		    var modalInstance = $modal.open({
		        //windowClass: 'form-modal-window-1200',
		        backdrop: 'static',
		        keyboard: false,
		        template: '<hapus-masteruser data="data" />',
		        scope: angular.extend($scope.$new(true), { data: data })
		    });

		    modalInstance.result.then(function () {
		        alert("Delete Success!");
		        vm.refreshData();
		    }, function () {
		        //$log.info('Modal dismissed at: ' + new Date());
		    });
		}

		function check(data) {
		    $modal.open({
		        windowClass: 'form-modal-window-1200',
		        backdrop: 'static',
		        keyboard: false,
		        template: '<check-masteruser data="data" />',
		        scope: angular.extend($scope.$new(true), { data: data })
		    });
		}
	}
})();