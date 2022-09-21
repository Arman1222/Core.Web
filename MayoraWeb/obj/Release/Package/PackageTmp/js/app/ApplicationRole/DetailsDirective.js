(function () {
    'use strict';

    window.app.directive('detailsNavbar', detailsNavbar);
    function detailsNavbar() {
        return {
            scope: {
                data: '='
            },
            templateUrl: MyApplication.componentPath + '/navbar/template/details.tmpl.cshtml',
            controller: controller,
            controllerAs: 'vm'
        }
    }

    controller.$inject = ['$scope', '$modal'];
    function controller($scope, $modal) {
        var vm = this;

        vm.navbar = $scope.data;
        vm.selectedView = 'details';
        vm.setView = setView;
        vm.edit = edit;             

        function setView(view) {
            vm.selectedView = view;
        }

        function edit(data) {
            $modal.open({
                template: '<edit-navbar data="data" />',
                scope: angular.extend($scope.$new(true), { data: data })
            });
        }     
    }
})();