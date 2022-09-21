(function() {
	'use strict';

	window.app.controller('MenuRoleController', MenuRoleController);

	MenuRoleController.$inject = ['$modal', 'menuroleService','$timeout'];
	function MenuRoleController($modal, menuroleService,$timeout) {
		var vm = this;
		vm.add = add;
		vm.menurole = {};
		vm.menuPicklist = menuroleService.menuPicklist;
		vm.rolePicklist = menuroleService.rolePicklist;
		vm.getPageMenu = menuroleService.getPageMenu;
		vm.getPageRole = menuroleService.getPageRole;
		vm.getPageApplication = menuroleService.getPageApplication;
		vm.menutree = '';		
		vm.selectedRole = { roleId: 1 };

		vm.onSelectRole = function () {		   
		    vm.menurole.roleId = vm.selectedRole.id;
		    if (vm.selectedApplication !== undefined) {
		        vm.menurole.applicationName = vm.selectedApplication.applicationName;
		        menuroleService.getMenusByRole(vm.menurole)
                    .then(function (data) {
                        vm.menutree = data;
                    });
		    }
		};

		vm.onSelectApplication = function () {		    
		    vm.menurole.applicationName = vm.selectedApplication.applicationName;
		    if (vm.selectedRole !== undefined) {
		        vm.menurole.roleId = vm.selectedRole.id;
		        menuroleService.getMenusByRole(vm.menurole)
                    .then(function (data) {
                        vm.menutree = data;
                    });
		    }
		};

		vm.onSelectMenu = function () {
		    vm.menurole.menuId = vm.selectedMenu.id;
		};

		function add() {
		    vm.saving = true;
		    menuroleService.add(vm.menurole)
				.then(function (data) {
				    $timeout(function () {
				        vm.menutree = data;
				    }, 200);				    
				    
				    //Close the modal
				    //$scope.$close();
				})
				.catch(function (response) {
				    if (response.data.errorMessage) {
				        //vm.errorMessage = "There was a problem saving the issue: <br/>" + response.data.errorMessage + "<br/>Please try again.";
				        alerts.error("There was a problem saving the issue: <br/>" + response.data.errorMessage + "<br/>Please try again.");
				    } else {
				        //vm.errorMessage = "There was a problem saving the issue. Please try again.";
				        alerts.error("There was a problem saving the issue. Please try again.");
				    }
				})
				.finally(function () {
				    vm.saving = false;
				});
		}

		//vm.deleteMenuFromRole = function (menuIdInput, roleIdInput) {
		//    vm.saving = true;

		//    vm.menurole = {
		//        menuId : menuIdInput
        //        ,roleId : roleIdInput
		//    }

		//    menuroleService.add(controllerPath + 'DeleteMenuFromRole', vm.menurole)
		//		.then(function (data) {
		//		    vm.menutree = data;				   
		//		})
		//		.catch(function (response) {
		//		    if (response.data.errorMessage) {
		//		        //vm.errorMessage = "There was a problem saving the issue: <br/>" + response.data.errorMessage + "<br/>Please try again.";
		//		        alerts.error("There was a problem saving the issue: <br/>" + response.data.errorMessage + "<br/>Please try again.");
		//		    } else {
		//		        //vm.errorMessage = "There was a problem saving the issue. Please try again.";
		//		        alerts.error("There was a problem saving the issue. Please try again.");
		//		    }
		//		})
		//		.finally(function () {
		//		    vm.saving = false;
		//		});
		//}
	}
})();