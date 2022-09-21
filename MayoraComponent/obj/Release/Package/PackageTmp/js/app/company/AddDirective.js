(function () {
    "use strict";

    window.app.directive('addCompany', addCompany);

    function addCompany() {
        return {
            templateUrl: MyApplication.rootPath + '/company/template/add.tmpl.cshtml',
            controller: controller,
            controllerAs: 'vm'
        }
    }

    controller.$inject = ['$scope', 'companyService','alerts'];
    function controller($scope, companyService, alerts) {
        var vm = this;
        vm.save = save;
        vm.title = "Add New Company";
        vm.saving = false;
        vm.company = {};   

        function save() {
            vm.saving = true;            
            companyService.add(vm.company)
                    .then(function (data) {
                        //Close the modal
                        $scope.$close();
                    })
                    .catch(function (response) {
                        if (response.data.errorMessage) {
                            vm.message.error("There was a problem saving data: <br/>" + response.data.errorMessage + "<br/>Please try again.");
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