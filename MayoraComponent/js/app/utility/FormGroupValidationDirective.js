(function () {
	'use strict';

	window.app.directive('formGroupValidation', formGroupValidation);

	function formGroupValidation() {
		return {
			require: '^form',
			scope: { 
				field: '@formGroupValidation'
			},
			controller: controller,
			controllerAs: 'vm',
			link: function (scope, element, attrs, formCtrl) {
				scope.form = formCtrl;
			},
			template:
				'<div class="has-feedback" ng-class="vm.getValidationClass()">' +
					'<ng-transclude></ng-transclude>' +
					'<input-validation-icons field="vm.field"></input-validation-icons>' +
				'</div>',
			transclude: true,
			replace: true
		}
	}

	controller.$inject = ['$scope'];
	function controller($scope) {
		var vm = this;

		vm.field = $scope.field;
		vm.getValidationClass = getValidationClass;

		function getValidationClass() {
			if (!canBeValidated()) return '';

			if (isValid()) return 'has-success';

			return 'has-error';
		}

		function canBeValidated() {
		    if (typeof $scope.form[vm.field] !== 'undefined') {
		        return ($scope.form[vm.field].$touched || $scope.form.$submitted);
		    }
		    return false;
		}

		function isValid() {
			return $scope.form[vm.field].$valid;
		}
	}
})();