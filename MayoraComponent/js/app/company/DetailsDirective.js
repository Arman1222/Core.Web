(function () {
    'use strict';

    window.app.directive('detailsCompany', detailsCompany);
    function detailsCompany() {
        return {
            scope: {
                data: '='
            },
            templateUrl: MyApplication.rootPath + '/company/template/details.tmpl.cshtml',
            controller: controller,
            controllerAs: 'vm'
        }
    }

    controller.$inject = ['$scope', '$modal'];
    function controller($scope, $modal) {
        var vm = this;

        vm.company = $scope.data;
        vm.selectedView = 'details';
        vm.setView = setView;
        vm.edit = edit;             

        function setView(view) {
            vm.selectedView = view;
        }

        function edit(data) {
            $modal.open({
                template: '<edit-company data="data" />',
                scope: angular.extend($scope.$new(true), { data: data })
            });
        }     
    }
})();