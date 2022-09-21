(function() {
	"use strict";

	window.app.directive('editMasteruser', editMasteruser);

	function editMasteruser() {
		return {
			scope: {
				data: "="
			},
			templateUrl: MyApplication.rootPath + '/master_user/template/edit.tmpl.cshtml',
			controller: controller,
			controllerAs: 'vm'
		}
	}

	controller.$inject = ['$scope', '$timeout', 'master_userService'];
	function controller($scope, $timeout, master_userService) {
		var vm = this;
		vm.save = save;
		vm.saving = false;
		vm.title = "Edit";
		vm.masteruser = angular.copy($scope.data);

		vm.check = true;

		vm.officeGetPage = master_userService.getOffice;

		vm.selected_office = { officeId: vm.masteruser.officeId, officeDesc: vm.masteruser.office };
		vm.onSelect_office = function () {
		    vm.masteruser.officeId = vm.selected_office == null ? null : vm.selected_office.officeId;
		}

	    //validation Office
		vm.is_office_valid = function () {
		    return vm.masteruser.officeId == null;
		}

		function save() {
		    if (vm.masteruser.maxPassWrong == 0) {
		        vm.message.error("Max Pass Wrong Harap Diisi !");
		        return false;
		    }
		    vm.saving = true;
		    if (vm.check) {
		        master_userService.update(vm.masteruser)
                        .then(function (data) {
                            //Close the modal
                            $scope.$parent.$close();
                        })
                        .catch(function (response) {
                            vm.saving = false;
                            if (response.data.errorMessage) {
                                vm.message.error("There was a problem saving the issue: <br/>" + response.data.errorMessage + "<br/>Please try again.");
                            } else {
                                vm.message.error("There was a problem saving the issue. Please try again.");
                            }
                        })
                        .finally(function () {
                        });
		    }
		    else if (!vm.check) {
		        if (vm.masteruser.password == null || vm.masteruser.password == '') {
		            vm.message.error("Password Harap Diisi !");
		            vm.saving = false;
		            return false;
		        }
		        master_userService.updatewithpass(vm.masteruser)
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
	}
})();