(function () {
    'use strict';

    window.app.directive('deleteHomemenurole', detailsHomemenurole);
    function detailsHomemenurole() {
        return {
            scope: {
                data: '='
            },
            templateUrl: MyApplication.rootPath + '/homemenurole/template/delete.tmpl.cshtml',
            controller: controller,
            controllerAs: 'vm'
        }
    }

    controller.$inject = ['$scope', '$modal', 'homemenuroleService'];
    function controller($scope, $modal, homemenuroleService) {
        var vm = this;

        vm.homemenurole = $scope.data;
        vm.selectedView = 'details'; 
        vm.deleteData = deleteData;

        function deleteData() {
            homemenuroleService.deleteData(vm.homemenurole)
                    .then(function (data) {
                        $scope.$parent.$close();
                    })
                    .catch(function (response) {
                        if (response.data.errorMessage) {
                            vm.message.error("There was a problem delete data : <br/>" + response.data.errorMessage + "<br/>Please try again.");
                        } else {
                            vm.message.error("There was a problem delete data. Please try again.");
                        }
                    })
                    .finally(function () {

                    });
        }
    }
})();