(function () {
    "use strict";

    window.app.directive('addMasteruser', addMasteruser);

    function addMasteruser() {
        return {
            templateUrl: MyApplication.rootPath  + '/master_user/template/add.tmpl.cshtml',
            controller: controller,
            controllerAs: 'vm'
        }
    }    

    controller.$inject = ['$scope', 'master_userService', 'alerts', '$timeout'];
    function controller($scope, master_userService, alerts, $timeout) {
        var vm = this;
        vm.save = save;
        vm.saving = false;
        vm.title = "Add";
        vm.masteruser = {};

        vm.officeGetPage = master_userService.getOffice;

        vm.onSelect_office = function () {
            vm.masteruser.officeId = vm.selected_office == null? null : vm.selected_office.officeId;
        }

        //validation Cabang
        vm.is_office_valid = function () {
            return vm.masteruser.officeId == null;
        }

        //function save() {
        //    if (vm.masteruser.password == null || vm.masteruser.password == '') {
        //        vm.message.error("Password Harap Diisi !");
        //        return false;
        //    }
        //    else if (vm.masteruser.maxPassWrong == 0) {
        //        vm.message.error("Max Pass Wrong Harap Diisi !");
        //        return false;
        //    }
        //    vm.saving = true;
        //    master_userService.add(vm.masteruser)
        //                .then(function (data) {
        //                    //Close the modal
        //                    $scope.$close();
        //                })
        //                .catch(function (response) {
        //                    if (response.data.errorMessage) {
        //                        vm.message.error("There was a problem saving the issue: <br/>" + response.data.errorMessage + "<br/>Please try again.");
        //                    } else {
        //                        vm.message.error("There was a problem saving the issue. Please try again.");
        //                    }
        //                })
        //                .finally(function () {
        //                });
        //}

        function save() {
            if (vm.masteruser.password == null || vm.masteruser.password == '') {
                vm.message.error("Password Harap Diisi !");
                return false;
            }
            else if (vm.masteruser.maxPassWrong == 0) {
                vm.message.error("Max Pass Wrong Harap Diisi !");
                return false;
            }
            vm.saving = true;
            var params = { search: vm.masteruser.name };
            master_userService.checker(params).then(function (value) {
                vm.check = value;
                if (vm.check == "E") {
                    alert("User Name Sudah Ada!");
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
            master_userService.add(vm.masteruser)
                        .then(function (data) {
                            //Close the modal
                            $scope.$close();
                        })
                        .catch(function (response) {
                            if (response.data.errorMessage) {
                                vm.message.error("There was a problem saving the issue: <br/>" + response.data.errorMessage + "<br/>Please try again.");
                            } else {
                                vm.message.error("There was a problem saving the issue. Please try again.");
                            }
                        })
                        .finally(function () {
                        });
        }

    }
})();