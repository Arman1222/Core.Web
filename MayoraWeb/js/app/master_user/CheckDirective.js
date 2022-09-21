(function () {
    'use strict';

    window.app.directive('checkMasteruser', checkMasteruser);
    function checkMasteruser() {
        return {
            scope: {
                data: '='
            },
            templateUrl: MyApplication.rootPath + '/master_user/template/check.tmpl.cshtml',
            controller: controller,
            controllerAs: 'vm'
        }
    }

    controller.$inject = ['$scope', '$modal', 'master_userService'];
    function controller($scope, $modal, master_userService) {
        var vm = this;
        vm.save = save;
        vm.masteruser = $scope.data;
        vm.usermodule = {};
        vm.loading = true;

        vm.tableActions = {
            onHapus: hapus,
        };

        vm.searchParams = {
            searchText: vm.masteruser.name
        };
        //vm.searchParams.searchText = vm.masteruser.name;
        vm.usermodulGetPage = master_userService.getUserModule;

        //Form
        vm.moduleGetPage = master_userService.getModule;

        vm.onSelect_module = function () {
            vm.usermodule.module = vm.selected_module == null ? null : vm.selected_module.module;
        }

        vm.classGetPage = master_userService.getClassification;

        vm.onSelect_class = function () {
            vm.usermodule.class_id = vm.selected_class == null ? null : vm.selected_class.classificationId;
        }

        function save() {
            if (vm.usermodule.module == null) {
                vm.message.error("Module Harap Diisi !");
                return false;
            }
            if (vm.usermodule.class_id == null) {
                vm.message.error("Classification Harap Diisi !");
                return false;
            }
            vm.usermodule.username = vm.masteruser.name;
            master_userService.addModule(vm.usermodule)
                        .then(function (data) {
                            alert("Insert Success!");
                            vm.refreshData();
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

        function hapus(data) {
            var modalInstance = $modal.open({
                //windowClass: 'form-modal-window-1200',
                backdrop: 'static',
                keyboard: false,
                template: '<hapus-module data="data" />',
                scope: angular.extend($scope.$new(true), { data: data })
            });

            modalInstance.result.then(function () {
                alert("Delete Success!");
                vm.refreshData();
            }, function () {
                //$log.info('Modal dismissed at: ' + new Date());
            });
        }
    }
})();