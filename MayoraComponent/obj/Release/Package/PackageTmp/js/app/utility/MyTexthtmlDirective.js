(function () {
    'use strict';
    //http://blog.yodersolutions.com/add-comma-separated-thousands-to-number-input-in-angularjs/
    window.app.directive("myTexthtml", function () {
        return {
            restrict: 'AE',
            scope: {
                title: '@',
                ngModel: '=?',
                type: '@',
                hint: '@?',
                ngChange: '&',
                ngBlur: '&',
                ngDisabled: '@'
            }
            , template:
                '<div class="form-group">' +
                    '<label class="control-label col-sm-12">{{title}}</label>' +
                    '<div class="col-sm-12">' +
                      '<textarea ui-tinymce="tinymceOptions" ng-model="ngModel"></textarea>' +
                      '<small ng-show="hint" class="number-input-hint">{{hint}}</small>' +
                    '</div>' +
                '</div>'
            , 
            controllerAs: 'vm',
            controller: function ($scope) {
                var plugin1 = 'advlist autolink autosave link image lists charmap print preview hr anchor pagebreak searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking table contextmenu directionality emoticons template textcolor paste fullpage textcolor colorpicker textpattern';
                var toolbar1 = 'newdocument fullpage | bold italic underline strikethrough | alignleft aligncenter alignright alignjustify | styleselect formatselect fontselect fontsizeselect';
                var toolbar2 = 'table | cut copy paste | searchreplace | bullist numlist | outdent indent blockquote | undo redo | link unlink anchor image media code | insertdatetime preview | forecolor backcolor';
                var toolbar3 = 'hr removeformat | subscript superscript | charmap emoticons | print fullscreen | ltr rtl | visualchars visualblocks nonbreaking template pagebreak restoredraft';
                if ($scope.ngDisabled == "true") {
                    $scope.tinymceOptions = {
                        menubar: false,
                        toolbar: false,
                        readonly: $scope.ngDisabled == "true" ? 1 : 0,
                        setup: function (editor) {
                            editor.on('change', function (e) {
                                $scope.ngChange();
                            });
                            editor.on('blur', function (e) {
                                $scope.ngBlur();
                            });
                        }
                    };
                }else{
                    if ($scope.type == 'simple') {
                        $scope.tinymceOptions = {
                                plugins: plugin1,
                                toolbar1: toolbar1,
                                readonly: $scope.ngDisabled == "true" ? 1 : 0,
                                setup: function (editor) {
                                    editor.on('change', function (e) {
                                        $scope.ngChange();
                                    });
                                    editor.on('blur', function (e) {
                                        $scope.ngBlur();
                                    });
                                }
                            };
                    } else if ($scope.type == 'fullfitur') {
                        $scope.tinymceOptions = {
                            plugins: plugin1,
                            toolbar1: toolbar1,
                            toolbar2: toolbar2,
                            toolbar3: toolbar3,
                            readonly: $scope.ngDisabled == "true" ? 1 : 0,
                            setup: function (editor) {
                                editor.on('change', function (e) {
                                    $scope.ngChange();
                                });
                                editor.on('blur', function (e) {
                                    $scope.ngBlur();
                                });
                            }
                        };
                    } else {
                        $scope.tinymceOptions = {
                            plugins: plugin1,
                            toolbar1: toolbar1,
                            toolbar2: toolbar2,
                            readonly: $scope.ngDisabled == "true" ? 1 : 0,
                            setup: function (editor) {
                                editor.on('change', function (e) {
                                    $scope.ngChange();
                                });
                                editor.on('blur', function (e) {
                                    $scope.ngBlur();
                                });
                            }
                        };
                    }
                }
                
            }
        }
    });


    

})();