(function() {
	"use strict";

	window.app.directive('editMynote', editMynote);

	function editMynote() {
		return {
			scope: {
				data: "="
			},
			templateUrl: MyApplication.rootPath + '/mynote/template/edit.tmpl.cshtml',
			controller: controller,
			controllerAs: 'vm'
		}
	}

	controller.$inject = ['$scope', '$timeout', 'noteandtaskService', 'imageclassService'];
	function controller($scope, $timeout, noteandtaskService, imageclassService) {
		var vm = this;
		vm.save = save;
		vm.title = "Edit Note";
		vm.saving = false;
		vm.mynote = angular.copy($scope.data);
		function save() {
			vm.saving = true;
		    noteandtaskService.updateNote(vm.mynote)
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