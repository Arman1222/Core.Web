(function () {
    window.app.factory('portalService', portalService);

    portalService.$inject = ['$http', 'DataService', 'alerts'];
    function portalService($http, DataService, alerts) {

        var controllerPath = 'Portal/'
        var list = [];       

        var svc = {
            loadAll: loadAll,
            myApplicationList: myApplicationList
        };

        return svc;

        function loadAll() {
            return DataService.post(controllerPath + 'All')
				.then(function (response) {
				    list.addRange(response.data);
				    return response;
				});
        }

        function myApplicationList() {
            return DataService.post(controllerPath + 'MyListApplication')
				.then(function (response) {
				    return response;
				});
        }
    }
})();