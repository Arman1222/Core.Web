(function() {
	'use strict';

	window.app.controller('CompanyController', CompanyController);

	CompanyController.$inject = ['$scope', '$modal', 'companyService', 'alerts'];
	function CompanyController($scope, $modal, companyService, alerts) {
		var vm = this;
		vm.add = add;
		vm.detail = detail;
		vm.edit = edit;
		vm.companyLoadAll = companyService.loadAll;
		vm.message = alerts;

		vm.loading = true;
	    companyService
            .loadAll()
            .then(function (data) {
                vm.companies = companyService.list;
            })
            .catch(function (response) {
                alerts.error(response.errorMessage);
            })
            .finally(function () {
                vm.loading = false;
            });
		

		vm.tableActions = {
		    onEdit: edit,
		    onDetail: detail,
		    onDelete: function (value) {
		        alert('Delete : ' + value);
		    }
		};

		function add() {		
			var modalInstance = $modal.open({			  
			    backdrop: 'static',
			    template: '<add-company />'
			});

			modalInstance.result.then(function () {
			    vm.refreshData();
			}, function () {
			    $log.info('Modal dismissed at: ' + new Date());
			});
		}

		function edit(data) {
		    $modal.open({
		        template: '<edit-company data="data" />',
		        scope: angular.extend($scope.$new(true), { data: data })
		    });
		}

		function detail(data) {
		    $modal.open({
		        template: '<details-company data="data" />',
		        scope: angular.extend($scope.$new(true), { data: data })
		    });
		}
	}
})();