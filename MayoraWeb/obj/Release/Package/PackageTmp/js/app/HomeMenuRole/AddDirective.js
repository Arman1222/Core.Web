(function () {
    "use strict";

    window.app.directive('addHomemenurole', addHomemenurole);

    function addHomemenurole() {
        return {
            templateUrl: MyApplication.rootPath + '/HomeMenuRole/template/add.tmpl.cshtml',
            controller: controller,
            controllerAs: 'vm'
        }
    }    

    controller.$inject = ['$scope', 'homemenuroleService', 'imageclassService','applicationusersService', 'alerts', 'Upload', '$timeout','MessageBox','blockUI', 'menuroleService'];
    function controller($scope, homemenuroleService, imageclassService, applicationusersService, alerts, Upload, $timeout, MessageBox, blockUI, menuroleService) {
        var vm = this;
        vm.save = save;
        vm.homemenurole = {};
        //picklist area Menu Role
        vm.selectedRole = { roleId: 1 };
        vm.getPageRole = menuroleService.getPageRole;
        vm.getPageApplication = menuroleService.getPageApplication;

        vm.onSelectRole = function () {
            vm.homemenurole.roleId = vm.selectedRole.id;
        };
        vm.onSelectApplication = function () {
            vm.homemenurole.apilcationId = vm.selectedApplication.applicationId;
        };
        //picklist End area Menu Role
        function save() {            
            //blockUI.start('Saving...');
                homemenuroleService.add(vm.homemenurole)
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
                            //blockUI.stop();
                        });
        }
    }
})();