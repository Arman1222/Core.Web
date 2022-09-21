(function () {
    "use strict";
    //o20170927
    window.app.directive('addMasteroffice', addMasteroffice);

    function addMasteroffice() {
        return {
            templateUrl: MyApplication.rootPath  + '/master_office/template/add.tmpl.cshtml',
            controller: controller,
            controllerAs: 'vm'
        }
    }    

    controller.$inject = ['$scope', 'master_office_Service', 'alerts', '$timeout'];
    function controller($scope, master_office_Service, alerts, $timeout) {
        var vm = this;
        vm.save = save;
        vm.saving = false;
        vm.title = "Add";
        vm.masteroffice = {}; //untukAdd
        
        vm.areaGetPage = master_office_Service.getArea;
        
        vm.onSelect_area = function () {
            vm.masteroffice.areaId = vm.selected_area == null ? null : vm.selected_area.areaId;
            vm.masteroffice.areaName = vm.selected_area == null ? null : vm.selected_area.areaName;
        }

        //validation Cabang
        vm.is_area_valid = function () {
            return vm.masteroffice.areaId == null;
        }

        function save() {
            vm.saving = true;
            var params = { search: vm.masteroffice.officeId };
            master_office_Service.checker(params).then(function (value) {
                vm.check = value;
                if (vm.check == "Y") {
                    alert("Office Id Sudah Ada!");
                    vm.saving = false;
                    return false;
                }
                else {
                    insert();
                }
            });
        }
        
        function insert() {
            vm.saving = true;
            var params = { search: vm.masteroffice.officeT24_KD_Cabang };
            master_office_Service.checkerT24(params).then(function (value) {
                vm.check = value;
                if (vm.check == "Y") {
                    alert("Branch Code T24 Sudah Ada!");
                    vm.saving = false;
                    return false;
                }
                else {
                    add();
                }
            });
        }

        function add() {
            vm.saving = true;
            master_office_Service.add(vm.masteroffice)
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