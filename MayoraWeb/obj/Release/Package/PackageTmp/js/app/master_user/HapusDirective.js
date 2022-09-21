(function() {
	"use strict";

	window.app.directive('hapusMasteruser', hapusMasteruser);

	function hapusMasteruser() {
		return {
			scope: {
				data: "="
			},
			templateUrl: MyApplication.rootPath + '/master_user/template/hapus.tmpl.cshtml',
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
		vm.masteruser = angular.copy($scope.data);

		function save() {
		    master_userService.hapus(vm.masteruser)
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