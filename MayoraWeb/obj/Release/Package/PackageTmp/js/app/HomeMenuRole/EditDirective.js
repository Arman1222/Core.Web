(function() {
	"use strict";

	window.app.directive('editHomemenurole', editHomemenurole);

	function editHomemenurole() {
		return {
			scope: {
				data: "="
			},
			templateUrl: MyApplication.rootPath + '/homemenurole/template/edit.tmpl.cshtml',
			controller: controller,
			controllerAs: 'vm'
		}
	}

	controller.$inject = ['$scope', 'homemenuroleService', 'imageclassService', 'applicationusersService', 'alerts', 'Upload', '$timeout', 'MessageBox', 'blockUI', 'menuroleService'];
	function controller($scope, homemenuroleService, imageclassService, applicationusersService, alerts, Upload, $timeout, MessageBox, blockUI, menuroleService) {
		var vm = this;
		vm.save = save;
		vm.title = "Edit HomeMenuRole";
		vm.saving = false;
		vm.homemenurole = angular.copy($scope.data);	
	    
		vm.selectedApplication = { applicationId: vm.homemenurole.apilcationId, applicationName: vm.homemenurole.application };
		vm.selectedRole = { id: vm.homemenurole.roleId, name: vm.homemenurole.roleName };
		vm.getPageRole = menuroleService.getPageRole;
		vm.getPageApplication = menuroleService.getPageApplication;

		vm.onSelectRole = function () {
		    vm.homemenurole.roleId = vm.selectedRole.id;
		};
		vm.onSelectApplication = function () {
		    vm.homemenurole.apilcationId = vm.selectedApplication.applicationId;
		};

		function save() {		    
			vm.saving = true;
		    homemenuroleService.update($scope.data, vm.homemenurole)
				.then(function () {
					//Close the modal
					$scope.$parent.$close();
				})
				.catch(function (response) {
				    if (response.data.errorMessage) {
				        vm.message.error("There was a problem saving the issue: <br/>" + response.data.errorMessage + "<br/>Please try again.");
				    } else {
				        vm.message.error("There was a problem saving the issue. Please try again.");
				    }
				})
				.finally(function() {
					vm.saving = false;
				});
		}
	}
})();