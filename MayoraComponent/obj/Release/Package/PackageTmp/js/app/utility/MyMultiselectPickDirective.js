(function () {
    'use strict';
    window.app.directive("multipleAutocompletePick", [
        '$filter',
        '$http',
        '$modal',
        function ($filter, $http, $modal) {
            return {
                restrict: 'EA',
                scope: {
                    title: '@',
                    listService: '=?',
                    ngModel: '=',
                    apiUrl: '@',
                    ngDisabled: '=?',
                    beforeSelectItem: '=?',
                    afterSelectItem: '=?',
                    beforeRemoveItem: '=?',
                    afterRemoveItem: '=?',
                    name: '@',
                    message: '=',
                    sortBy: '@',
                    searchParams: '=?',
                    refreshData: '=?',
                    totalItems: '=?',
                    response: '=?',
                    showColumns: '@',
                    hideColumns: '@'
                },
                template: '<div class="ng-ms form-item-container">'+                            '    <ul class="list-inline">'+                            '        <li ng-repeat="item in ngModel">'+                            '            <span ng-if="objectProperty == undefined || objectProperty == \'\'">'+                            '                {{item}} <span class="remove" ng-click="removeAddedValues(item)">'+                            '                    <i class="glyphicon glyphicon-remove"></i>'+                            '                </span>&nbsp;'+                            '            </span>'+                            '            <span ng-if="objectProperty != undefined && objectProperty != \'\'">'+                            '                {{item[objectProperty]}} <span class="remove" ng-click="removeAddedValues(item)">'+                            '                    <i class="glyphicon glyphicon-remove"></i>'+                            '                </span>&nbsp;'+                            '            </span>'+                            '        </li>'+                            '        <li>'+                            '            <div class="btn btn-sm btn-success" ng-disabled="ngDisabled" ng-click="show()">Add</div>'+                            '        </li>'+                            '    </ul>'+                            '</div>',
                link: function (scope, element, attr) {
                    scope.objectProperty = attr.objectProperty;
                    scope.selectedItemIndex = 0;
                    scope.name = attr.name;
                    scope.isRequired = attr.required;
                    scope.errMsgRequired = attr.errMsgRequired;
                    scope.isHover = false;
                    scope.isFocused = false;
                    if (scope.ngModel === null || !(scope.ngModel instanceof Array)) {
                        scope.ngModel = [];
                    }

                    var isDuplicate = function (arr, item) {
                        var duplicate = false;
                        if (arr == null || arr == "")
                            return duplicate;

                        for (var i = 0; i < arr.length; i++) {
                            duplicate = angular.equals(arr[i], item);
                            if (duplicate)
                                break;
                        }
                        return duplicate;
                    };

                    scope.alreadyAddedValues = function (item) {
                        var isAdded = true;
                        isAdded = !isDuplicate(scope.ngModel, item);
                        //if(scope.ngModel != null && scope.ngModel != ""){
                        //    isAdded = scope.ngModel.indexOf(item) == -1;
                        //    console.log("****************************");
                        //    console.log(item);
                        //    console.log(scope.ngModel);
                        //    console.log(isAdded);
                        //}
                        return isAdded;
                    };

                    scope.removeAddedValues = function (item) {
                        if (scope.ngModel != null && scope.ngModel != "") {
                            var itemIndex = scope.ngModel.indexOf(item);
                            if (itemIndex != -1) {
                                if (scope.beforeRemoveItem && typeof (scope.beforeRemoveItem) == 'function')
                                    scope.beforeRemoveItem(item);

                                scope.ngModel.splice(itemIndex, 1);

                                if (scope.afterRemoveItem && typeof (scope.afterRemoveItem) == 'function')
                                    scope.afterRemoveItem(item);
                            }
                        }
                    };

                    scope.mouseEnterOnItem = function (index) {
                        scope.selectedItemIndex = index;
                    };

                    //picklist Area

                    var modalInstance = {};
                    scope.show = function () {
                        if (scope.ngModel !== undefined && scope.ngModel !== null)
                            scope.rowselected = angular.copy(scope.ngModel);
                        modalInstance = $modal.open({
                            template:'<div class="modal-header">' +                                    '    <h3><i class="fa fa-laptop"></i>Search {{title}}</h3>    ' +                                    '</div>' +                                    '<div class="modal-body">' +                                    '    <div style="margin-bottom: 5px;">' +                                    '        <a class="btn btn-sm btn-danger" ng-click="close()"> <span class="glyphicon glyphicon-chevron-left"></span>  Close</a>' +                                    '    </div>' +                                    '    <modal-grid grid-data="listService"' +                                    '                title="{{title}}"' +                                    '                message="message"' +                                    '                enable-select="True"' +                                    '                sort-by="sortBy"' +                                    '                selected-rows="rowselected"' +                                    '                search-params="searchParams"' +                                    '                refresh-data="refreshData"' +                                    '                show-columns="showColumns"' +                                    '                hide-columns="hideColumns" />' +                                    '</div>' +                                    '<div class="modal-footer">' +                                    '    <div class="btn btn-primary" ng-click="select(rowselected)">Select</div>' +                                    '    <div class="btn btn-warning" ng-click="close()">Cancel</div>' +                                    '</div>',
                            scope: scope,
                            windowClass: 'app-modal-window'
                        });
                    };

                    scope.sort_by = function (predicate) {
                        scope.predicate = predicate;
                        scope.reverse = !scope.reverse;
                    };

                    scope.close = function () {
                        scope.rowselected = [];
                        modalInstance.dismiss("Close");
                    };


                    var isDuplicate = function (arr, item) {
                        var duplicate = false;
                        if (arr == null || arr == "")
                            return duplicate;

                        for (var i = 0; i < arr.length; i++) {
                            duplicate = angular.equals(arr[i], item);
                            if (duplicate)
                                break;
                        }
                        return duplicate;
                    };

                    scope.select = function (selectedItem) {
                        selectedItem = selectedItem.filter(function (val) {
                            return !isDuplicate(scope.ngModel, val);
                        });

                        //selectedItem = function () {
                        //    return selectedItem.filter(function (letter) {
                        //        return scope.ngModel.indexOf(letter) !== -1;
                        //    });
                        //};

                        if (selectedItem.length > 0) {
                            if (scope.beforeSelectItem && typeof (scope.beforeSelectItem) == 'function')
                                scope.beforeSelectItem(selectedItem);
                            if (scope.ngModel === null || !(scope.ngModel instanceof Array)) {
                                scope.ngModel = [];
                            }
                            scope.ngModel.push.apply(scope.ngModel, selectedItem);

                            if (scope.afterSelectItem && typeof (scope.afterSelectItem) == 'function')
                                scope.afterSelectItem(selectedItem);
                        }
                        if (selectedItem !== undefined && selectedItem !== null) {
                            modalInstance.close(selectedItem);
                        }
                    };
                    //picklist End Area
                }
            };
        }
    ]);


    

})();