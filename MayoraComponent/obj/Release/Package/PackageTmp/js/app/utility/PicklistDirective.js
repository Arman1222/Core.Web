(function () {
    'use strict';

    window.app.directive('myPicklist', ["$modal", "$timeout", "$templateCache", function ($modal, $timeout, $templateCache) {
        return {
            restrict: 'AE',
            scope: {
                title: '@',
                ngModel: '=',
                idColumn: '@',
                nameColumn: '@',
                name: '@',
                onSelect: '&',
                changeable: '@',
                message: '=',
                picklist: '=',
                listService: '=',
                selectedId: '@',
                sortBy: '@',
                ngDisabled: '=?',
                searchParams: '=?',
                refreshData: '=?',
                totalItems: '=?',
                onLoad: '=?',
                response: '=?',
                showColumns: '@',
                hideColumns: '@'
            },
            link: function (scope, elem, attrs) {                  

                scope.setPage = function (pageNo) {
                    scope.currentPage = pageNo;
                };
                scope.filter = function () {
                    $timeout(function () {
                        scope.filteredItems = scope.filtered.length;
                    }, 10);
                };
                scope.sort_by = function (predicate) {
                    scope.predicate = predicate;
                    scope.reverse = !scope.reverse;
                };

                scope.getViewValue = function (selectedItem) {
                    //return selectedItem[scope.idColumn] + ' - ' + selectedItem[scope.nameColumn];
                    return selectedItem[scope.nameColumn];
                };

                scope.select = function (selectedItem) {
                    scope.ngModel = selectedItem;
                    scope.viewValue = "";
                    if (selectedItem !== undefined && selectedItem !== null) {
                        scope.viewValue = scope.getViewValue(selectedItem);
                        modalInstance.close(selectedItem);
                    }                    
                    $timeout(function () {
                        scope.onSelect();
                    }, 200);
                };

                scope.$watch('ngModel', function () {
                    if (scope.ngModel !== undefined
                        && scope.ngModel !== null
                        && scope.ngModel[scope.idColumn] !== undefined
                        && scope.ngModel[scope.nameColumn] !== undefined) {
                        scope.viewValue = scope.getViewValue(scope.ngModel);
                    } else {
                        scope.viewValue = '';
                    }
                });

                var modalInstance = {};
                scope.close = function () {
                    modalInstance.dismiss("Close");
                };

                scope.show = function () {
                    modalInstance = $modal.open({
                        template:       '<div class="modal-header">' +                                        '    <h3><i class="fa fa-laptop"></i>Search {{title}}</h3>    ' +                                        '</div>' +                                        '<div class="modal-body">' +                                        '    <div style="margin-bottom: 5px;">' +                                        '        <a class="btn btn-sm btn-danger" ng-click="close()"> <span class="glyphicon glyphicon-chevron-left"></span> Close</a>' +                                        '    </div>' +                                        '    <modal-grid grid-data="listService"' +                                        '               title="{{title}}"' +                                        '               message="vm.message"' +                                        '               select-column="true"' +                                        '               table-actions="tableActions"' +                                        '               sort-by="sortBy"' +                                        '               search-params="searchParams"' +                                        '               refresh-data="refreshData"' +                                        '               show-columns="showColumns"' +                                        '               hide-columns="hideColumns" />' +                                        '</div>',
                        scope: scope,
                        windowClass: 'app-modal-window'
                    });
                };

                scope.clear = function () {
                    scope.select(null);
                };

                scope.tableActions = {
                    onSelect: function (selectedItem) {    
                        scope.select(selectedItem);
                    }
                };

                //scope.list = [];
                //scope.picklist()
                //    .then(function (data) {
                //            scope.list = data;
                //            scope.currentPage = 1; //current page
                //            scope.entryLimit = 20; //max no of items to display in a page
                //            scope.filteredItems = scope.list.rows.length; //Initially for no filter
                    
                //            //set selected
                //            if (scope.selectedId != null && scope.selectedId != '') {
                //                for (var i = 0; i < scope.list.rows.length; i++) {
                //                    if (scope.list.rows[i][scope.idColumn] == scope.selectedId) {
                //                        scope.model = scope.list.rows[i];
                //                        $timeout(function () {
                //                            scope.onSelect();
                //                        }, 200);
                //                    }
                //                }
                //            }
                //    })
                //    .catch(function (response) { 
                //        scope.message.error(response.data.errorMessage);
                //    });  

            },
            template:
               '<div class="input-group">' +
                    '<span class="input-group-btn">' +
                        '<button class="btn btn-primary" type="button" ng-click="clear()" ng-disabled="ngDisabled"><i class="fa fa-times"></i></button>' +
                    '</span>' +
                    '<input type="text" ng-click="show()" ng-model="viewValue" ng-disabled="ngDisabled" class="form-control" readonly="readonly" />' +
                    '<span class="input-group-btn">' +
                        '<button class="btn btn-primary" type="button" ng-click="show()" ng-disabled="ngDisabled"><i class="fa fa-search"></i></button>' +
                    '</span>' +
               '</div>'

            //template:
            //    '<div class="inner-addon right-addon">'+
            //    '<i class="glyphicon glyphicon-search"></i>'+
            //    '<input type="text" ng-click="show()" ng-model="viewValue" ng-disabled="ngDisabled" class="form-control" readonly="readonly" />'+
            //    '</div>'
            ////Jangan gunakan path karena element belum attached ke form pada waktu load pertama kali sehingga tidak dapat dilakukan custom validation
            ////templateUrl: MyApplication.rootPath + '/utility/template/picklist.tmpl.cshtml',
            ////Jangan gunakan path karena element belum attached ke form pada waktu load pertama kali sehingga tidak dapat dilakukan custom validation
            ////transclude: false
            ////replace: true
        };

    }]);

})();