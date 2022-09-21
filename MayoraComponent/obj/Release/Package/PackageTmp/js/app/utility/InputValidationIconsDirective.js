(function () {
	'use strict';

	window.app.directive('inputValidationIcons', inputValidationIcons);

	function inputValidationIcons() {
		return {
			require: '^form',
			scope: { 
				field: '='
			},
			controller: controller,
			controllerAs: 'vm',
			link: function (scope, element, attrs, formCtrl) {
			    scope.form = formCtrl;

			    //scope.$watch(scope.form.$name + '.' + scope.field + '.$valid'
                //    , function () {
                //        scope.fieldIsValid = scope.form[scope.field].$valid;
                //    }
                //);
                //http://stackoverflow.com/questions/15899389/angularjs-directive-watch-validity
			    scope.$watch(function () { return scope.form[scope.field].$valid; }
                    , function ()
                    {
                        scope.fieldIsValid = scope.form[scope.field].$valid;
                    }
                );
			},
			template:
				'<span ng-show="vm.canBeValidated() && (vm.isValid() || fieldIsValid)" ' +
					'class="fa fa-2x fa-check-square form-control-feedback"></span>' +
				'<span ng-show="vm.canBeValidated() && (!vm.isValid() || !fieldIsValid)" ' +
					'class="fa fa-2x fa-exclamation-triangle form-control-feedback"></span>'
		}
	}

	controller.$inject = ['$scope'];
	function controller($scope) {
		var vm = this;

		vm.field = $scope.field;
		vm.canBeValidated = canBeValidated;
		vm.isValid = isValid;		

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