(function () {
    'use strict';   
    window.app.directive("myUploadFile", ["$timeout", "Upload", function () {
        return {
            restrict: 'AE',           
            scope: {
                title: '@',
                path: '@',
                accept: '@',
                file: '=?',
                response: '=?',
                error: '=?',
                maxSize: '@',
                ngModel: '=?',
                ngChange: '&',
                ngDisabled: '=?',
                labelSize: '@?',
                inputSize: '@?'
            },
            controller: function ($scope, $timeout, Upload) {
                if ($scope.labelSize === undefined) {
                    $scope.labelSize = 'col-sm-2';
                }

                if ($scope.inputSize === undefined) {
                    $scope.inputSize = 'col-sm-10';
                }

                if ($scope.maxSize === undefined) {
                    $scope.maxSize = '10MB';
                }

                if ($scope.accept === undefined) {
                    $scope.accept = 'application/*,video/*,image/*';
                }

                $scope.uploadFiles = function (files, errFiles) {
                   
                    //$scope.files = files;                    
                    for(var file in files) {
                        $scope.singleFile = files[file];
                        break;
                    }
                    $scope.errFiles = errFiles;
                    angular.forEach(files, function (file) {
                        $scope.file = file;
                        file.upload = Upload.upload({
                            url: MyApplication.rootPath + $scope.path,
                            data: { file: file }
                        });

                        file.upload.then(function (response) {
                            $timeout(function () {
                                file.result = response.data;
                                $scope.response = response;
                                $scope.ngChange();
                            });
                        }, function (response) {
                            if (response.status > 0)
                                $scope.error = response.status + ': ' + response.data;
                        }, function (evt) {
                            file.progress = Math.min(100, parseInt(100.0 *
                                                     evt.loaded / evt.total));
                        });
                    });
                }
            },            
            template:
                '<div class="form-group">' +
                            '<label for="Upload" class="control-label {{labelSize}}">{{title}}</label>' +
                            '<div class="{{inputSize}}">' +
                                '<div class="input-group">' +
                                    '<span class="input-group-btn">' +
                                        '<span class="btn btn-primary btn-file" ngf-select="uploadFiles($files, $invalidFiles)" ng-disabled="ngDisabled"' +
                                            //'multiple' +
                                            'accept="{{accept}}"' +
                                            'ngf-max-height="1000"' +
                                             'ngf-max-size="{{maxSize}}">' +
                                            '...' +
                                        '</span>' +
                                    '</span>' +                                    
                                    //'<span ng-repeat="f in files" style="font:smaller">' +
                                        '<input type="text" class="form-control" ng-value="singleFile.progress> 0 ? singleFile.progress + ' + "'% '" + ' + singleFile.name : ' + "''" + '" readonly="" ng-style="{ width: singleFile.progress > 0 ? singleFile.progress + ' + "'%' : " + "'100%'" + '}" style="width:{{f.progress}}%">' +
                                    //'</span>' +
                                '</div>' +
                        '</div>' +
                '</div>'              
            , link: function (scope, elem, attrs) {  
                             

            },
        }
    }]);   

})();