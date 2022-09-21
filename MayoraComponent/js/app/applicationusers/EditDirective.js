(function() {
	"use strict";

	window.app.directive('editUsersapp', editUsersapp);

	function editUsersapp() {
		return {
			scope: {
				data: "="
			},
			templateUrl: MyApplication.rootPath + '/applicationusers/template/edit.tmpl.cshtml',
			controller: controller,
			controllerAs: 'vm'
		}
	}

	controller.$inject = ['$scope', '$timeout', 'applicationusersService', 'branchService', 'rolemanageService'];
	function controller($scope, $timeout, applicationusersService, branchService, rolemanageService) {
		var vm = this;
		vm.save = save;
		vm.title = "Edit Users Akses";
		vm.saving = false;
		vm.applicationusers = angular.copy($scope.data);
		vm.refreshRole = function () {
		    rolemanageService.getAll().then(function (result) {
		        vm.listRole = result.data;
		    });
		}
		vm.refreshRole();
		vm.companyGet = branchService.loadAll;
		function save() {		    
			vm.saving = true;
		    applicationusersService.update($scope.data, vm.applicationusers)
				.success(function () {
					//Close the modal
					$scope.$parent.$close();
				})
				.catch(function (response) {
				    if (response.data.errorMessage) {
				        vm.message.error("There was a problem saving data: <br/>" + response.data.errorMessage + "<br/>Please try again.");
				    } else {
				        vm.message.error("There was a problem saving data. Please try again.");
				    }
				})
				.finally(function() {
					vm.saving = false;
				});
		}
	}
})();