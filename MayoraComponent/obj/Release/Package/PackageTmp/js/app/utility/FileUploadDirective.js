(function () {
    'use strict';
    window.app.directive("fileUpload", ["$timeout", "Upload", function () {
        return {
            restrict: 'AE',
            scope: {
                title: '@',
                accept: '@',
                response: '=?',
                error: '=?',
                folder: '@',
                maxSize: '@',
                ngModel: '=?',
                ngChange: '&',
                ngDisabled: '=?',
                setFile: '=?'
            },
            controller: function ($scope, $timeout, Upload, $http, DataService) {

                var controllerPath = 'UploadFile/'
                $scope.tempngModel = $scope.ngModel;
                if ($scope.labelSize === undefined) {
                    $scope.labelSize = 'col-sm-2';
                }
                if ($scope.maxSize === undefined) {
                    $scope.maxSize = '4MB';
                }
                //if (!(typeof($scope.folder) === undefined)) {
                //    var params = { folder: $scope.folder };
                //    return DataService.post(controllerPath + 'setFolder', params)
                //        .then(function (response) {
                //            if (response == "Succes") {
                //                console.log(response);
                //            }
                //        });
                //}
                if ($scope.inputSize === undefined) {
                    $scope.inputSize = 'col-sm-10';
                }

                $scope.resetFile = function () {
                    var param = { namaFile: $scope.ngModel, folder: $scope.folder };
                    $scope.resetData(param)
                    .then(function (data) {
                        if (data == "Succes") {
                            $scope.ngModel = null;
                            $scope.files = [];
                        }
                    });
                }

                $scope.setFile = function () {
                    if ($scope.ngModel != $scope.tempngModel) {
                        if ($scope.ngModel == null || $scope.ngModel == '') {
                            var params = { namaFile: $scope.tempngModel, folder: $scope.folder };
                            return DataService.post(controllerPath + 'ClearImage', params)
                           .then(function (response) {
                               if (!(typeof ($scope.singleFile) === "undefined")) $scope.singleFile.progress = null;
                               $scope.tempngModel = null;
                               return response;
                           });
                        } else {
                            if ($scope.tempngModel != null) {
                                var param = { namaFile: $scope.tempngModel, folder: $scope.folder };
                                return DataService.post(controllerPath + 'ClearImage', param)
                               .then(function (response) {
                                   var param2 = { namaFile: $scope.ngModel, folder: $scope.folder };
                                   return DataService.post(controllerPath + 'SetImage', param2)
                                  .then(function (response) {
                                      if (!(typeof ($scope.singleFile) === "undefined")) $scope.singleFile.progress = null;
                                      return response;
                                  });
                               });
                            }
                            var params = { namaFile: $scope.ngModel, folder: $scope.folder };
                            return DataService.post(controllerPath + 'SetImage', params)
                           .then(function (response) {
                               if (!(typeof ($scope.singleFile) === "undefined")) $scope.singleFile.progress = null;
                               $scope.tempngModel = $scope.ngModel;
                               return response;
                           });
                        }
                    }
                }

                $scope.viewOrDownload = function () {
                    if ($scope.tempngModel == $scope.ngModel) {
                        window.open(
                            MyApplication.rootPath + "Files/" + $scope.folder + "/" + $scope.ngModel,
                            '_blank'
                        );
                    } else {
                        window.open(
                            MyApplication.rootPath + "Files/" + $scope.folder + "/Temp/" + $scope.ngModel,
                            '_blank'
                        );
                    }
                }

                $scope.resetData = function (params) {
                    return DataService.post(controllerPath + 'ResetData', params)
                        .then(function (response) {
                            if (!(typeof ($scope.singleFile) === "undefined")) $scope.singleFile.progress = null;
                            return response;
                        });
                }

                $scope.uploadFiles = function (files, errFiles) {
                    $scope.files = files;
                    $scope.errFiles = errFiles; 
                    if (files != null && errFiles.length == 0) {
                        if (!(typeof ($scope.singleFile) === "undefined")) $scope.singleFile.progress = null;
                        for (var file in files) {
                            $scope.singleFile = files[file];
                            break;
                        }
                        var param = { namaFile: $scope.ngModel, folder: $scope.folder };
                        $scope.resetData(param)
                        .then(function (data) {
                            if (data == "Succes") {
                                angular.forEach(files, function (file) {
                                    $scope.file = file;
                                    $scope.ngModel = null;
                                    file.upload = Upload.upload({
                                        url: MyApplication.rootPath + 'UploadFile/Upload',
                                        data: { file: file, folder: $scope.folder },
                                        fields: { 'username': 'Username' }
                                    });

                                    file.upload.then(function (response) {
                                        $timeout(function () {
                                            $scope.ngModel = response.data;
                                            $scope.ngChange();
                                        });
                                    }, function (response) {
                                        if (response.status > 0) {
                                            $scope.error = response.status + ': ' + response.data;
                                        }
                                    }, function (evt) {
                                        file.progress = Math.min(100, parseInt(100.0 *
                                                     evt.loaded / evt.total));
                                    });
                                });
                            }
                        });
                    }
                }
            },
            //http://stackoverflow.com/questions/11235206/twitter-bootstrap-form-file-element-upload-button
            template:
                //'<div class="form-group">' +
                //        '<label for="Upload" class="control-label {{labelSize}} ng-binding">{{title}}</label>' +
                //        '<div class="{{inputSize}}">' +
                            '<div class="input-group">' +
                                '<span class="input-group-btn">' +
                                    '<span class="btn btn-primary btn-default" ngf-select="uploadFiles($files, $invalidFiles)"  ng-disabled="ngDisabled"' +
                                            'accept="{{accept}}"' +

                                            'ngf-max-size="{{maxSize}}">' +
                                     '...' +
                                     '</span>' +
                                '</span>' +
                                '<div class="form-control">' +
                                    '<div class="progress-bar progress-bar-info progress-bar-striped" role="progressbar" aria-valuemin="0" aria-valuemax="100" style="width: {{singleFile.progress == null ? 0 : singleFile.progress; }}%;">' +
                                    '</div>' +
                                    '<span class="progress-value">{{singleFile.progress == 100 || singleFile.progress == null ? ngModel.substr(37, ngModel.length - 37) : (singleFile.progress).toString() + "% Complete"}}</span>' +
                                '</div>' +
                                '<span class="input-group-btn">' +
                                    '<button class="btn btn-primary" type="button" title="reset" ng-click="resetFile()" ng-disabled="ngModel == null || ngDisabled"><i class="glyphicon glyphicon-remove"></i></button>' +
                                    '<button class="btn btn-warning" type="button" title="view" ng-click="viewOrDownload()" ng-disabled="ngModel == null"><i class="glyphicon glyphicon-eye-open"></i></button>' +
                                '</span>' +
                            '</div>' //+
                 //           '<div class="help-block" ng-show="errFiles.length > 0">' +
                 //             '<p style="color:#a94442;" ng-repeat="file in errFiles" class="">{{file.name}} : {{file.$error}} {{file.$errorParam}}</p>' +
                 //           '</div>' +
                 //       '</div>' +
                 //'</div>'
            , link: function (scope, elem, attrs) {
            },
        }
    }]);

})();