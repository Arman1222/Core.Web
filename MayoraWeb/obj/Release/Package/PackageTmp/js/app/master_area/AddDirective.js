(function () {
    "use strict";
    //o20170928
    window.app.directive('addMasterarea', addMasterarea);

    function addMasterarea() {
        return {
            templateUrl: MyApplication.rootPath + '/master_area/template/add.tmpl.cshtml',
            controller: controller,
            controllerAs: 'vm'
        }
    }    

    controller.$inject = ['$scope', 'master_area_Service', 'alerts', '$timeout'];
    function controller($scope, master_area_Service, alerts, $timeout) {
        var vm = this;
        vm.save = save;
        vm.saving = false;
        vm.title = "Add";
        vm.masterarea = {}; //untukAdd
        
        vm.areaGetPage = master_area_Service.getArea;
        
        //validation Cabang
        vm.is_area_valid = function () {
            return vm.masterarea.areaId == null;
        }

        function save() {
            vm.saving = true;
            var params = { search: vm.masterarea.areaCode };
            master_area_Service.checkerCode(params).then(function (value) {
                vm.check = value;
                if (vm.check == "Y") {
                    alert("Area Code Sudah Ada!");
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
            master_area_Service.add(vm.masterarea)
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