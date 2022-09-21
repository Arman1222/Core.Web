(function () {
    'use strict';

    window.app.directive('detailsApplication', detailsApplication);
    function detailsApplication() {
        return {
            scope: {
                data: '='
            },
            templateUrl: MyApplication.rootPath + '/application/template/details.tmpl.cshtml',
            controller: controller,
            controllerAs: 'vm'
        }
    }

    controller.$inject = ['$scope', '$modal'];
    function controller($scope, $modal) {
        var vm = this;

        vm.application = $scope.data;
        vm.selectedView = 'details';
        vm.setView = setView;
        vm.edit = edit;             

        function setView(view) {
            vm.selectedView = view;
        }

        function edit(data) {
            $modal.open({
                template: '<edit-application data="data" />',
                scope: angular.extend($scope.$new(true), { data: data })
            });
        }     
    }
})();