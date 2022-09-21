(function () {
    'use strict';

    window.app.directive('myCombobox', function ($timeout) {
        return {
            restrict: 'AEC',
            scope: {
                items: '=',
                title: '@',
                idColumn: '@',
                nameColumn: '@',
                ngModel: '=',
                onSelect: '&',
                ngDisabled: '=?'
            },
            link: function (scope, elem, attrs) {
                scope.handleSelection = function () {                    
                    $timeout(function () {
                        scope.onSelect();
                    }, 200);
                };

                scope.clear = function () {
                    scope.ngModel = null;
                    scope.handleSelection();
                };

            },
            template:
                '<div class="input-group">' +
                    '<span class="input-group-btn">' +
                        '<button class="btn btn-primary" type="button" ng-click="clear()" ng-disabled="ngDisabled"><i class="fa fa-times"></i></button>' +
                    '</span>' +
                    '<select class="form-control" ng-model="ngModel" ng-disabled="ngDisabled" ng-change="handleSelection()" '+
                        "data-ng-options='item as item[idColumn] + "+ "\" - \"" +"+ item[nameColumn] for item in items track by item[nameColumn]'> " +
                    '</select>'+
                '</div>'

            //template:
            //    '<select class="form-control" ng-model="ngModel" ng-disabled="ngDisabled" ng-change="handleSelection(model)" '+
            //        "data-ng-options='item as item[idColumn] + "+ "\" - \"" +"+ item[nameColumn] for item in items track by item[nameColumn]'> " +
            //    '</select>' 
            ////Jangan gunakan path karena element belum attached ke form pada waktu load pertama kali sehingga tidak dapat dilakukan custom validation
            ////templateUrl: MyApplication.rootPath + '/utility/template/combobox.tmpl.cshtml'
            ////Jangan gunakan path karena element belum attached ke form pada waktu load pertama kali sehingga tidak dapat dilakukan custom validation
        }
    });

})();

