(function () {
    "use strict";

    window.app.directive('addMynote', addMynote);

    function addMynote() {
        return {
            templateUrl: MyApplication.rootPath + '/Mynote/template/add.tmpl.cshtml',
            controller: controller,
            controllerAs: 'vm'
        }
    }    

    controller.$inject = ['$scope', 'noteandtaskService', 'alerts', '$timeout', 'MessageBox', 'blockUI'];
    function controller($scope, noteandtaskService, alerts, $timeout, MessageBox, blockUI) {
        var vm = this;
        vm.save = save;
        vm.title = "Add Note";
        vm.mynote = {};
        vm.mynote.note_class = 'box box-default';
        vm.saving = false;
        function save() {
            noteandtaskService.addNote(vm.mynote)
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