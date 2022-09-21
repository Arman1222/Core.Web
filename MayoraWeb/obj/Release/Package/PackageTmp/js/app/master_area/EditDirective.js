(function () {
    "use strict";
    //o20170928
    window.app.directive('editMasterarea', editMasterarea);

    function editMasterarea() {
        return {
            templateUrl: MyApplication.rootPath + '/master_area/template/edit.tmpl.cshtml',
            controller: controller,
            controllerAs: 'vm'
        }
    }    

    controller.$inject = ['$scope', 'master_area_Service', 'alerts', '$timeout'];
    function controller($scope, master_area_Service, alerts, $timeout) {
        var vm = this;
        vm.save = save;
        vm.saving = false;
        vm.title = "Edit";
        vm.masterarea = angular.copy($scope.data); //untukEdit
        
        vm.areaGetPage = master_area_Service.getArea;
        
        //validation Cabang
        vm.is_area_valid = function () {
            return vm.masterarea.areaId == null;
        }

        function save() {
            vm.saving = true;
            master_area_Service.update(vm.masterarea)
                        .then(function (data) {
                            //Close the modal
                            $scope.$close();
                        })
                        .catch(function (response) {
                            if (response.data.errorMessage) {
                                vm.message.error("There was a problem saving the issue: <br/>" + response.data.errorMessage + "<br/>Please try again.");
                                vm.saving = false;
                            } else {
                                vm.message.error("There was a problem saving the issue. Please try again.");
                                vm.saving = false;
                            }
                        })
                        .finally(function () {
                        });
        }


    }
})();