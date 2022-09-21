(function () {
    "use strict";
    //o20170927
    window.app.directive('editMasteroffice', editMasteroffice);

    function editMasteroffice() {
        return {
            templateUrl: MyApplication.rootPath  + '/master_office/template/edit.tmpl.cshtml',
            controller: controller,
            controllerAs: 'vm'
        }
    }    

    controller.$inject = ['$scope', 'master_office_Service', 'alerts', '$timeout'];
    function controller($scope, master_office_Service, alerts, $timeout) {
        var vm = this;
        vm.save = save;
        vm.saving = false;
        vm.title = "Edit";
        vm.masteroffice = angular.copy($scope.data); //untukEdit
        
        vm.areaGetPage = master_office_Service.getArea;
        
        vm.onSelect_area = function () {
            vm.masteroffice.areaId = vm.selected_area == null ? null : vm.selected_area.areaId;
            vm.masteroffice.areaName = vm.selected_area == null ? null : vm.selected_area.areaName;
        }

        vm.selected_area = { areaId: vm.masteroffice.areaId, areaName: vm.masteroffice.areaName }; //untukTampilinPicklistFor

        //validation Cabang
        vm.is_area_valid = function () {
            return vm.masteroffice.areaId == null;
        }

        function save() {
            vm.saving = true;
            master_office_Service.update(vm.masteroffice)
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