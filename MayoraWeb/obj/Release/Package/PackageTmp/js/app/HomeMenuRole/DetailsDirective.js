(function () {
    'use strict';

    window.app.directive('detailsHomemenurole', detailsHomemenurole);
    function detailsHomemenurole() {
        return {
            scope: {
                data: '='
            },
            templateUrl: MyApplication.rootPath + '/homemenurole/template/details.tmpl.cshtml',
            controller: controller,
            controllerAs: 'vm'
        }
    }

    controller.$inject = ['$scope', '$modal'];
    function controller($scope, $modal) {
        var vm = this;

        vm.homemenurole = $scope.data;
        vm.selectedView = 'details';
        vm.setView = setView;
        vm.edit = edit;             

        function setView(view) {
            vm.selectedView = view;
        }

        function edit(data) {
            $modal.open({
                template: '<edit-homemenurole data="data" />',
                scope: angular.extend($scope.$new(true), { data: data })
            });
        }     
    }
})();