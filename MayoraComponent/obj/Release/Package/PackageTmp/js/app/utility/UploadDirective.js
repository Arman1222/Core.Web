(function () {
    'use strict';   
    window.app.directive("myUpload", ["$timeout", function () {
        return {
            restrict: 'AE',           
            scope: {
                title: '@',
                columns: '@',
                ngModel: '=?',
                ngChange: '&',
                ngDisabled: '=?',
                uploadData: '=?',
                disableHeaders: '@?',
                range: '@?',
                where: '@?'
            },
            controller: function ($scope, $timeout) {
                
                if ($scope.labelSize === undefined) {
                    $scope.labelSize = 'col-sm-2';
                }

                if ($scope.inputSize === undefined) {
                    $scope.inputSize = 'col-sm-10';
                }

                $scope.resetFile = function () {
                    $scope.ngModel = null;
                };

                $scope.$watch('uploadData', function (value) {
                    $timeout(function () {                       
                        $scope.uploadData = value;
                        $scope.ngChange();
                    });
                });
            },
            //http://stackoverflow.com/questions/11235206/twitter-bootstrap-form-file-element-upload-button
            template:
                //'<div class="form-group">' +
                //'<label for="Upload" class="control-label {{labelSize}}">{{title}}</label>' +
                //            '<div class="{{inputSize}}">' +
                               '<div class="input-group">' +
                                   '<span class="input-group-btn">' +
                                       '<span class="btn btn-primary btn-file" ng-disabled="ngDisabled">' +
                                          '...<input type="file" data-bind-file="" data-ng-model="ngModel" upload-data="uploadData" columns="columns" disable-headers="disableHeaders" range="range" where="where" on-change="vm.uploadFile" />' +
                                       '</span>' +
                                   '</span>' +
                                   '<input type="text" class="form-control" data-ng-model="ngModel.name" readonly="">' +
                                   '<span class="input-group-btn">' +
                                       '<span class="btn btn-primary btn-file" data-ng-click="resetFile()" ng-disabled="ngDisabled">Reset</span>' +
                                   '</span>' +
                               '</div>' //+
                        //    '</div>' +
                        //'</div>'
            , link: function (scope, elem, attrs) {  
            },
        }
    }]);   

})();