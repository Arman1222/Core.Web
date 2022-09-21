(function() {
	"use strict";

	window.app.directive('imageclassEdit', imageclassEdit);

	function imageclassEdit() {
		return {
			scope: {
				imageclass: "="
			},
			templateUrl: MyApplication.rootPath + '/imageclass/template/edit.tmpl.cshtml',
			controller: controller,
			controllerAs: 'vm'
		}
	}

	controller.$inject = ['$scope', 'imageclassService'];
	function controller($scope, imageclassService) {
		var vm = this;
		vm.save = save;

		vm.saving = false;
		vm.imageclass = angular.copy($scope.imageclass);
		vm.errorMessage = null;

		function save() {
			vm.saving = true;
		    imageclassService.update($scope.imageclass, vm.imageclass)
				.success(function () {
					//Close the modal
					$scope.$parent.$close();
				})
				.error(function(data) {
				    vm.errorMessage = 'There was a problem saving changes to the imageclass: ' + data;
				})
				.finally(function() {
					vm.saving = false;
				});
		}
	}
})();