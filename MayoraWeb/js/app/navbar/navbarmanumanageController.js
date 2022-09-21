(function() {
	'use strict';


	window.app.controller('navbarmanumanageController', navbarmanumanageController);

	navbarmanumanageController.$inject = ['$scope', '$modal', 'navbarmanuService', 'alerts', 'applicationService'];
	function navbarmanumanageController($scope, $modal, navbarmanuService, alerts, applicationService) {
	    var vm = this;
	    applicationService.loadAll().then(function (data) {
	        vm.listapplication = data.data;
	    });
	    vm.pilihapp = function (app) {
	        vm.selectedapp = app;
	        vm.refreshNavbar();
	    };
        
	    vm.refreshNavbar = function () {
	        navbarmanuService.gettreemenubyapp({ application: vm.selectedapp.applicationName }).then(function (result) {
	            vm.listmenuselectedapp = result.data;
	        });
	    }

	    vm.editNavbar = function (mode, parent, navbar) {
	        var modalInstance = $modal.open({
	            //windowClass: 'form-modal-window-1200',
	            template: '<edit-navbar data="data" />',
	            scope: angular.extend($scope.$new(true), { data: { mode: mode, application: vm.selectedapp, parent: parent, navbar: navbar } })
	        });
	        modalInstance.result.then(function () {
	            vm.refreshNavbar();
	        }, function () {
	            //$log.info('Modal dismissed at: ' + new Date());
	        });
	    }

	    vm.deleteNavbar = function (data) {
	        var result = confirm("Are you sure, Want to delete this data?");
	        if (result) {
	            navbarmanuService.datadelete(data)
				.then(function () {
				    //Close the modal
				    vm.refreshNavbar();
				})
	        }
	    }

	    vm.moveupNavbar = function (data) {
	        navbarmanuService.moveup(data).then(function (result) {
	            vm.refreshNavbar();
	        }).catch(function (response) {
	            alert(response.data.errorMessage);
	        })
	    }

	    vm.movedownNavbar = function (data) {
	        navbarmanuService.movedown(data).then(function (result) {
	            vm.refreshNavbar();
	        }).catch(function (response) {
	            alert(response.data.errorMessage);
	        })
	    }

	    function detailNavbar(data) {
	        $modal.open({
	            windowClass: 'form-modal-window-1200',
	            template: '<details-mynote data="data" />',
	            scope: angular.extend($scope.$new(true), { data: data })
	        });
	    }
	}
})();