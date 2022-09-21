(function () {
    'use strict';

    window.app.directive('detailsMytask', detailsMytask);
    function detailsMytask() {
        return {
            scope: {
                data: '='
            },
            templateUrl: MyApplication.rootPath + '/mytask/template/details.tmpl.cshtml',
            controller: controller,
            controllerAs: 'vm'
        }
    }

    controller.$inject = ['$scope', '$modal'];
    function controller($scope, $modal) {
        var vm = this;

        vm.title = "Details Task";
        vm.mytask = $scope.data;
        vm.selectedView = 'details';
        vm.setView = setView;
        vm.edit = edit;             

        function setView(view) {
            vm.selectedView = view;
        }

        function edit(data) {
            $modal.open({
                template: '<edit-mytask data="data" />',
                scope: angular.extend($scope.$new(true), { data: data })
            });
        }     
    }
})();