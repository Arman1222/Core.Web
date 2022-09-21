(function() {
	'use strict';


	window.app.controller('applicationRoleController', applicationRoleController);

	applicationRoleController.$inject = ['$scope', '$modal', 'navbarmanuService', 'alerts', 'applicationService', 'rolemanageService'];
	function applicationRoleController($scope, $modal, navbarmanuService, alerts, applicationService, rolemanageService) {
	    var vm = this;

	    //applicationService.loadAll().then(function (data) {
	    //    vm.listapplication = data.data;
	    //});
	    vm.refreshRole = function () {
	        rolemanageService.getAll().then(function (result) {
	            vm.listRole = result.data;
	        });
	    }
	    vm.refreshApp = function () {
	        rolemanageService.getAllApplication({ roleId: vm.selectedrole.id }).then(function (result) {
	            vm.listapplication = result.data;
	        });
	    };
	    vm.tambahRole = function (data) {
	        rolemanageService.add({ id: null, name: data }).then(function (data) {
	            vm.refreshRole();
	        });
	    };
	    vm.hapusRole = function (data) {
	        rolemanageService.dataDelete(data).then(function (data) {
	            vm.refreshRole();
	        });
	    };
	    vm.refreshNavbar = function () {
	        rolemanageService.getAllNavbar({ roleId: vm.selectedrole.id, applicationId: vm.selectedapp.applicationId }).then(function (result) {
	            vm.listmenuselectedapp = result.data;
	        });
	    };
	    vm.changeLink = function (data) {
	        if (data.used) {
	            vm.link({ roleId: vm.selectedrole.id, navbarId: data.menuId }).then(function (result) {
	                vm.refreshNavbar();
	            });
	        } else {
	            vm.unlink({ roleId: vm.selectedrole.id, navbarId: data.menuId }).then(function (result) {
	                vm.refreshNavbar();
	            });
	        }
	    }
	    

	    vm.refreshRole();
	    vm.link = rolemanageService.linkRoleNavbar;
	    vm.unlink = rolemanageService.unlinkRoleNavbar;
	    vm.selectedapp = null;
	    vm.selectedrole = null;
	    vm.pilihRole = function (role) {
	        if (vm.selectedrole === null) {
	            vm.listmenuselectedapp = {};
	            vm.selectedrole = role;
	            vm.selectedapp = null;
	            vm.refreshApp();
	        } else if (role.id != vm.selectedrole.id) {
	            vm.listmenuselectedapp = {};
	            vm.selectedapp = null;
	            vm.selectedrole = role;
	            vm.refreshApp();
	        }
	    };

	    vm.pilihApp = function (app) {
	        if (vm.selectedapp === null) {
	            vm.selectedapp = app;
	            vm.refreshNavbar();
	        } else if (app.applicationId != vm.selectedapp.applicationId) {
	            vm.selectedapp = app;
	            vm.refreshNavbar();
	        }
	    };
	}
})();