(function() {
	'use strict';

	window.app.controller('HomeMenuRoleController', HomeMenuRoleController);

	HomeMenuRoleController.$inject = ['$scope','$modal', 'homemenuroleService','alerts'];
	function HomeMenuRoleController($scope, $modal, homemenuroleService, alerts) {
		var vm = this;
		vm.add = add;
		vm.detail = detail;
		vm.edit = edit;
		vm.HomeMenuRoleGetPage = homemenuroleService.getPage;
		vm.message = alerts;
		vm.loading = true;

		vm.tableActions = {
		    onEdit: edit,
		    onDetail: detail,
		    onDelete: deleteData,
		    onClick: function (value) {
		        //alert('Click : ' + value);
		    },
		    onDblClick: function (value) {
		        //alert('DblClick : ' + value);
		    }
		};

		function add() {
		    var modalInstance = $modal.open({
		        windowClass: 'form-modal-window-1000',
				template: '<add-homemenurole />'
		    });

		    modalInstance.result.then(function () {
		        vm.refreshData();
		    }, function () {
		        //$log.info('Modal dismissed at: ' + new Date());
		    });
		}

		function edit(data) {
		    var modalInstance = $modal.open({
		        template: '<edit-homemenurole data="data" />',
		        scope: angular.extend($scope.$new(true), { data: data })
		    });

		    modalInstance.result.then(function () {
		        vm.refreshData();
		    }, function () {
		        //$log.info('Modal dismissed at: ' + new Date());
		    });
		}

		function detail(data) {
		    $modal.open({
		        template: '<details-homemenurole data="data" />',
		        scope: angular.extend($scope.$new(true), { data: data })
		    });
		}

		function deleteData(data) {
		    var modalInstance = $modal.open({
		        template: '<delete-homemenurole data="data" />',
		        scope: angular.extend($scope.$new(true), { data: data })
		    });

		    modalInstance.result.then(function () {
		        vm.refreshData();
		    }, function () {
		        //$log.info('Modal dismissed at: ' + new Date());
		    });
		}
	}
})();