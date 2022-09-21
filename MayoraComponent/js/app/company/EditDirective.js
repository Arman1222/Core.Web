(function() {
	"use strict";

	window.app.directive('editCompany', editCompany);

	function editCompany() {
		return {
			scope: {
				data: "="
			},
			templateUrl: MyApplication.rootPath + '/company/template/edit.tmpl.cshtml',
			controller: controller,
			controllerAs: 'vm'
		}
	}

	controller.$inject = ['$scope', '$timeout', 'companyService'];
	function controller($scope, $timeout, companyService ) {
		var vm = this;
		vm.save = save;
		vm.title = "Edit Company";
		vm.saving = false;
		vm.company = angular.copy($scope.data);		
	 		
		function save() {		    
			vm.saving = true;
		    companyService.update($scope.data, vm.company)
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