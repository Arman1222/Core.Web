(function() {
	"use strict";

	window.app.directive('editApplication', editApplication);

	function editApplication() {
		return {
			scope: {
				data: "="
			},
			templateUrl: MyApplication.rootPath + '/application/template/edit.tmpl.cshtml',
			controller: controller,
			controllerAs: 'vm'
		}
	}

	controller.$inject = ['$scope', '$timeout', 'applicationService'];
	function controller($scope, $timeout, applicationService) {
		var vm = this;
		vm.save = save;
		vm.title = "Edit Application";
		vm.saving = false;
		vm.application = angular.copy($scope.data);		
		
		function save() {		    
			vm.saving = true;
		    applicationService.update($scope.data, vm.application)
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