(function() {
	"use strict";

	window.app.directive('editCustomer', editCustomer);

	function editCustomer() {
		return {
			scope: {
				data: "="
			},
			templateUrl: MyApplication.rootPath + '/customer/template/edit.tmpl.cshtml',
			controller: controller,
			controllerAs: 'vm'
		}
	}

	controller.$inject = ['$scope', '$timeout', 'customerService', 'imageclassService'];
	function controller($scope, $timeout, customerService, imageclassService) {
		var vm = this;
		vm.save = save;
		vm.title = "Edit Customer";
		vm.saving = false;
		vm.customer = angular.copy($scope.data);		
		vm.imageclassPicklist = imageclassService.picklist;
	    imageclassService
            .loadAll()
            .then(function (obj) {
                vm.selectedImageclass = imageclassService.getData(vm.customer.imageClassId);
            });
		//$timeout(function () {
		//    vm.selectedImageclass = imageclassService.getData(vm.customer.imageClassId);
		//}, 200);
		vm.onSelectImageClass = function () {
		    console.log('selectedImageClass = ' + JSON.stringify(vm.selectedImageclass, null, 4));
		    vm.customer.imageClassId = vm.selectedImageclass.imageClassId;
		}

		function save() {		    
			vm.saving = true;
		    customerService.update($scope.data, vm.customer)
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