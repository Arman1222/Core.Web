(function () {
    "use strict";

    window.app.directive('addApplication', addApplication);

    function addApplication() {
        return {
            templateUrl: MyApplication.rootPath  + '/application/template/add.tmpl.cshtml',
            controller: controller,
            controllerAs: 'vm'
        }
    }

    controller.$inject = ['$scope', 'applicationService'];
    function controller($scope, applicationService) {
        var vm = this;
        vm.save = save;
        vm.title = "Add New Application";
        vm.saving = false;
        vm.application = {};        
     
        function save() {           
            vm.saving = true;            
            applicationService.add(vm.application)
                    .then(function (data) {
                        //Close the modal
                        $scope.$close();
                    })
                    .catch(function (response) {  
                        if (response.data.errorMessage) {
                            vm.message.error("There was a problem saving data : <br/>" + response.data.errorMessage + "<br/>Please try again.");                            
                        } else {
                            vm.message.error("There was a problem saving data. Please try again.");
                        }
                    })
                    .finally(function () {
                        vm.saving = false;
                    });   
        }
    }
})();