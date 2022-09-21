(function() {
	'use strict';

	window.app.controller('ApplicationController', ApplicationController);

	ApplicationController.$inject = ['$scope', '$modal', 'applicationService','alerts'];
	function ApplicationController($scope, $modal, applicationService, alerts) {
		var vm = this;
		vm.add = add;
		vm.detail = detail;
		vm.edit = edit;        
		vm.applications = applicationService.list;
		vm.applicationLoadAll = applicationService.loadAll;
		vm.applicationGetPage = applicationService.getPage;
		vm.message = alerts;

		vm.title = "Application Master"

		vm.tableActions = {
		    onEdit: edit,
		    onDetail: detail,
		    onDelete: function (value) {
		        alert('Delete : ' + value);
		    }
		};

		function add() {
		    var modalInstance = $modal.open({
		        //windowClass: 'form-modal-window-1000',
		        backdrop: 'static',
		        template: '<add-application />'
		    });

		    modalInstance.result.then(function () {
		        vm.refreshData();
		    }, function () {
		        //$log.info('Modal dismissed at: ' + new Date());
		    });		  
		}

		function edit(data) {
		    $modal.open({
		        backdrop: 'static',
                keyboard: false,
		        template: '<edit-application data="data" />',
		        scope: angular.extend($scope.$new(true), { data: data })
		    });
		}

		function detail(data) {
		    $modal.open({
		        template: '<details-application data="data" />',
		        scope: angular.extend($scope.$new(true), { data: data })
		    });
		}
	}
})();