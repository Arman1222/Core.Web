(function () {
    'use strict';

    window.app.directive('detailsMynote', detailsMynote);
    function detailsMynote() {
        return {
            scope: {
                data: '='
            },
            templateUrl: MyApplication.rootPath + '/mynote/template/details.tmpl.cshtml',
            controller: controller,
            controllerAs: 'vm'
        }
    }

    controller.$inject = ['$scope', '$modal'];
    function controller($scope, $modal) {
        var vm = this;

        vm.mynote = $scope.data;
        vm.selectedView = 'details';
        vm.setView = setView;
        vm.edit = edit;             

        function setView(view) {
            vm.selectedView = view;
        }

        function edit(data) {
            $modal.open({
                template: '<edit-mynote data="data" />',
                scope: angular.extend($scope.$new(true), { data: data })
            });
        }     
    }
})();