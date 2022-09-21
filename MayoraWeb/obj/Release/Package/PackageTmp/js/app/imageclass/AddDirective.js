(function() {
	"use strict";

	window.app.directive('addImageclass', addImageclass);

	function addImageclass() {
		return {
		    templateUrl: MyApplication.rootPath + '/imageclass/template/add.tmpl.cshtml',
			controller: controller,
			controllerAs: 'vm'
		}
	}

	controller.$inject = ['$scope', 'imageclassService'];
	function controller($scope, imageclassService) {
		var vm = this;
		vm.add = add;

		vm.saving = false;
		vm.imageclass = {};
		vm.errorMessage = null;

		function add() {
			vm.saving = true;
		    imageclassService.add(vm.imageclass)
				.success(function () {
					//Close the modal
					$scope.$close();
				})
				.error(function(data) {
					vm.errorMessage = 'There was a problem adding the image class: ' + data;
				})
				.finally(function() {
					vm.saving = false;
				});
		}
	}
})();