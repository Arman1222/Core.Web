(function() {
	'use strict';

	window.app.directive('imageclassDetails', imageclassDetails);
	function imageclassDetails() {
		return {
			scope: {
				imageclass: '='
			},
			templateUrl: MyApplication.rootPath + '/imageclass/template/details.tmpl.cshtml',
			controller: controller,
			controllerAs: 'vm'
		}
	}

	controller.$inject = ['$scope', '$modal'];
	function controller($scope, $modal) {
		var vm = this;

		vm.imageclass = $scope.imageclass;
		vm.selectedView = 'details';
		vm.setView = setView;
		vm.edit = edit;	

		function setView(view) {
			vm.selectedView = view;
		}

		function edit() {
			$modal.open({
			    template: '<edit-imageclass imageclass="imageclass" />',
			    scope: angular.extend($scope.$new(true), { imageclass: vm.imageclass })
			});
		}		
	}
})();