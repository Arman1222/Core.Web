(function() {
	"use strict";

	window.app.directive('editMytask', editMytask);

	function editMytask() {
		return {
			scope: {
				data: "="
			},
			templateUrl: MyApplication.rootPath + '/mytask/template/edit.tmpl.cshtml',
			controller: controller,
			controllerAs: 'vm'
		}
	}

	controller.$inject = ['$scope', '$timeout', 'noteandtaskService', 'imageclassService'];
	function controller($scope, $timeout, noteandtaskService, imageclassService) {
		var vm = this;
		vm.save = save;
		vm.title = "Edit Task";
		vm.saving = false;
		vm.mytask = angular.copy($scope.data);
		function save() {		    
			vm.saving = true;
		    noteandtaskService.updateTask(vm.mytask)
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