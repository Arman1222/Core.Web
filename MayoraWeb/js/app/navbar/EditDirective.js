(function() {
	"use strict";

	window.app.directive('editNavbar', editNavbar);

	function editNavbar() {
		return {
			scope: {
				data: "="
			},
			templateUrl: MyApplication.rootPath + '/navbar/template/edit.tmpl.cshtml',
			controller: controller,
			controllerAs: 'vm'
		}
	}

	controller.$inject = ['$scope', '$timeout', 'navbarmanuService'];
	function controller($scope, $timeout, navbarmanuService) {
		var vm = this;
		vm.save = save;
		vm.saving = false;
		
		vm.mode = angular.copy($scope.data.mode);
		vm.title = vm.mode + " Navbar";
		vm.application = angular.copy($scope.data.application);
		vm.parent = angular.copy($scope.data.parent);
		vm.navbar =
            vm.navbar = $scope.data.navbar == null ? 
            {
                menuId: null
              , parentId: ( vm.parent == null ? null : vm.parent.id )
              , name: (vm.parent == null ? '' : vm.parent.name + '-')
              , text: ''
              , controller: ''
              , action: ''
              , area: null
              , faicon: 'fa fa-circle-o text-red'
              , description: null
              , imageClassId: 1
              , activeli: null
              , status: 0
              , isParent: 0
              , applicationId: $scope.data.application.applicationId
            } : angular.copy($scope.data.navbar);
		function save() {
		    vm.saving = true;
		    if (vm.mode.indexOf("Add") !== -1) {
		        navbarmanuService.add(vm.navbar)
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
				.finally(function () {
				    vm.saving = false;
				});
		    } else {
		        navbarmanuService.update(vm.navbar)
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
				.finally(function () {
				    vm.saving = false;
				});
		    }
		    
		}
	}
})();