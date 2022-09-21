(function() {
	'use strict';

	window.app.controller('navbarmanuController', navbarmanuController);

	navbarmanuController.$inject = ['$scope', '$modal', 'navbarmanuService', 'alerts', '$location', '$state', '$dashboardState'];
	function navbarmanuController($scope, $modal, navbarmanuService, alerts, $location, $state, $dashboardState) {
	    var vm = this;
	    navbarmanuService.gettreemenu().then(
            function (data) {
                vm.navbarlist = data.data;
                //console.log(vm.navbarlist);
            });
	    vm.linkapp = MyApplication.rootPath + 'AdminLTE-2.3.11/index.html';
	    navbarmanuService.getlistmenu().then(
                function (data) {
                    for (var i = 0; i < data.data.length; i++) {
                        if (data.data[i].controller != null || data.data[i].action != null) {
                            $dashboardState.addState(data.data[i].name, data.data[i].controller + '/' + data.data[i].action, data.data[i]);
                        }
                    }
                }
            );
	}

	window.app.controller('headerController', headerController);

	headerController.$inject = ['$rootScope','$scope', '$state'];
	function headerController($rootScope, $scope, $state) {
	    var vm = this;
	    $rootScope.$on('$stateChangeSuccess', 
            function (event, toState, toParams, fromState, fromParams) {
                vm.curstate = toState;
        })
	}

})();