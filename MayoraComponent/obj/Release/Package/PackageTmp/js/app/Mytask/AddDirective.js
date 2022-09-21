(function () {
    "use strict";

    window.app.directive('addMytask', addMytask);

    function addMytask() {
        return {
            templateUrl: MyApplication.rootPath  + '/Mytask/template/add.tmpl.cshtml',
            controller: controller,
            controllerAs: 'vm'
        }
    }    

    controller.$inject = ['$scope', 'noteandtaskService', 'alerts', '$timeout', 'MessageBox', 'blockUI'];
    function controller($scope, noteandtaskService, alerts, $timeout, MessageBox, blockUI) {
        var vm = this;
        vm.title = "Add Task";
        vm.save = save;
        vm.saving = false;
        
        function save() {
            if (vm.mytask.startdate != null || vm.mytask.enddate != null) {
                if (vm.mytask.startdate > vm.mytask.enddate) {
                    alert("Start date tidak boleh lebih dari end date");
                    return false;
                }
                if (vm.mytask.enddate < vm.mytask.startdate) {
                    alert("End date tidak boleh kurang dari start date");
                    return false;
                }
            }
            
            noteandtaskService.addTask(vm.mytask)
                        .then(function (data) {
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