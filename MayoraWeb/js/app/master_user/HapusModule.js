﻿(function() {
	"use strict";

	window.app.directive('hapusModule', hapusModule);

	function hapusModule() {
		return {
			scope: {
				data: "="
			},
			templateUrl: MyApplication.rootPath + '/master_user/template/hapusmodule.tmpl.cshtml',
			controller: controller,
			controllerAs: 'vm'
		}
	}

	controller.$inject = ['$scope', '$timeout', 'master_userService'];
	function controller($scope, $timeout, master_userService) {
		var vm = this;
		vm.save = save;
		vm.saving = false;
		vm.title = "Hapus";
		vm.data = angular.copy($scope.data);
		vm.module = {};
		vm.module.username = vm.data.userName;
		vm.module.module = vm.data.module;
		vm.module.class_id = 0;

		function save() {
		    master_userService.hapusModule(vm.module)
                        .then(function (data) {
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
                        .finally(function () {
                        });
		    
		}
	}
})();