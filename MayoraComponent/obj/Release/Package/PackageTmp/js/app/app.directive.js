(function () {
    'use strict';
    //http://stackoverflow.com/questions/26383507/listen-for-form-submit-event-in-directive
    window.app.directive('form', function () {
        return {
            restrict: 'E',
            link: function (scope, elem) {
                elem.on('submit', function () {
                    scope.$broadcast('form:submit');
                });
            }
        };
    });

    //http://www.matheuslima.com/angularjs-custom-validation-with-directives/
    //http://stackoverflow.com/questions/12581439/how-to-add-custom-validation-to-an-angularjs-form
    window.app.directive('customValidation', function () {
        return {
            restrict: 'A',
            //require: 'ngModel',
            require: '^form',
            scope: {
                ngModel: '=ngModel'
                , ngForm: '=ngForm'
                , elementName: '@'
                , validateFunction: '=validateFunction'
            },
            link: function ($scope, $element, $attrs, form) {
                //http://stackoverflow.com/questions/17618318/pass-form-to-directive
                //http://stackoverflow.com/questions/12581439/how-to-add-custom-validation-to-an-angularjs-form

                $scope.checkValidation = function () {
                    if (typeof $scope.ngForm !== 'undefined') {
                        //if (typeof $scope.ngForm.$$parentForm[$scope.elementName] !== 'undefined') {
                        if ($scope.validateFunction()) { //if not valid
                            $scope.ngForm.$setValidity($scope.elementName, false);
                        } else { // if valid
                            $scope.ngForm.$setValidity($scope.elementName, true);
                        }
                        $scope.$applyAsync();
                        //}	                    
                    }
                };

                $scope.$on('form:submit', function () {
                    //form.$setPristine();
                    $scope.checkValidation();
                });

                $scope.$watch("ngModel", function () {
                    $scope.checkValidation();
                });
            }
        }
    });

    http://nadeemkhedr.com/angularjs-validation-reusable-component/
    window.app.directive('submitValid', function ($parse) {
            return {
                require: 'form',
                link: function (scope, formElement, attributes, form) {
                    form.attempt = false;
                    formElement.bind('submit', function (event) {
                        form.attempt = true;
                        if (!scope.$$phase) scope.$apply();

                        var fn = $parse(attributes.submitValid);

                        if (form.$valid) {
                            scope.$apply(function () {
                                fn(scope, { $event: event });
                            });
                        }
                    });
                }
            };
    });

    //http://www.matheuslima.com/angularjs-form-validation-after-submit-directive/
    window.app.directive('formSubmit', function ($timeout) {
        return {
            restrict: 'A',
            scope: {
                formSubmit: '&'
            },
            require: '^form',
            link: function (scope, element, attrs, formCtrl) {
                element.on('submit', function () {

                    scope.$broadcast('form:submit');

                    $timeout(function () {
                        if (formCtrl.$valid) {
                            scope.formSubmit();
                        }
                        else {
                            // service to display invalid inputs
                        }
                    }, 1000);

                });
            }
        }
    });

    //http://stackoverflow.com/questions/17470790/how-to-use-a-keypress-event-in-angularjs
    window.app.directive('myEnter', function () {
        return function (scope, element, attrs) {
            element.bind("keydown keypress", function (event) {
                if (event.which === 13) {
                    scope.$apply(function () {
                        scope.$eval(attrs.myEnter);
                    });

                    event.preventDefault();
                }
            });
        };
    });

    //http://stackoverflow.com/questions/17922557/angularjs-how-to-check-for-changes-in-file-input-fields
    window.app.directive('customOnChange', function () {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                var onChangeHandler = scope.$eval(attrs.customOnChange);
                element.bind('change', onChangeHandler);
            }
        };
    });

    //http://stackoverflow.com/questions/17922557/angularjs-how-to-check-for-changes-in-file-input-fields
    window.app.directive('bindFile', ["$timeout", function () {
        return {
            require: "ngModel",
            restrict: 'A',
            scope: {
                title: '@',
                columns: '=?',
                ngModel: '=?',
                ngChange: '&',
                ngDisabled: '=?',
                uploadData: '=?',
                disableHeaders: '=?',
                range: '=?',
                where: '=?'
            },
            controller: function ($scope, $timeout) {
                var vm = this;

                vm.headers = "true";
                if ($scope.disableHeaders !== undefined) {
                    vm.headers = "false";
                }
                vm.range = "";
                if ($scope.range !== undefined && $scope.range !== '') {
                    vm.range = ',range:"' + $scope.range + '"';
                }
                vm.where = "";
                if ($scope.where !== undefined && $scope.where !== '') {
                    vm.where = ' WHERE ' + $scope.where;
                }

                vm.uploadFile = function ($event) {
                    var files = event.target.files;
                    var query = 'SELECT * FROM FILE(?,{headers:' + vm.headers + vm.range + '})' + vm.where;

                    if ($scope.columns !== undefined && $scope.columns !== '')
                        query = 'SELECT ' + $scope.columns + ' FROM FILE(?,{headers:' + vm.headers + vm.range + '})' + vm.where;

                    alasql(query, [event], function (data) {
                        $timeout(function () {
                            $scope.uploadData = data;
                        });
                        //$scope.uploadData = data;     //  eData contains the JSON representation of the Excel sheet rows
                        //$scope.$apply();
                    });
                };
            },
            controllerAs: 'vm',
            link: function ($scope, el, attrs, ngModel) {
                var onChangeHandler = $scope.$eval(attrs.onChange);
                //el.bind('change', onChangeHandler);
                el.bind('change', function (event) {

                    onChangeHandler();

                    ngModel.$setViewValue(event.target.files[0]);

                    $scope.$apply();

                });

                $scope.$watch(function () {
                    return ngModel.$viewValue;
                }, function (value) {
                    if (!value) {
                        el.val("");
                    }
                });
            }
        };
    }]);

})();