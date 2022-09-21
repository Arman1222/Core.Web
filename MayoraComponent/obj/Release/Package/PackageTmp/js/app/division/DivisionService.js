(function () {
    window.app.factory('divisionService', divisionService);

    divisionService.$inject = ['$http', 'DataService', 'alerts'];
    function divisionService($http, DataService, alerts) {

        var controllerPath = 'Division/'
        var list = [];        

        var svc = {
            loadAll: loadAll,
            getPage: getPage,
            add: add,
            update: update,
            deleteData: deleteData,
            list: list,
            getData: getData
        };

        return svc;

        function loadAll() {
            return DataService.post(controllerPath + 'All')
				.then(function (response) {
				    list = [];
				    list.addRange(response.data);
				    return response;
				});
        }

        function getPage(params) {
            return DataService.post(controllerPath + 'GetPage', params)
				.then(function (response) {
				    return response;
				});
        }

        function getData(id) {
            for (var i = 0; i < list.length; i++) {
                if (list[i].id == id) return list[i];
            }

            return null;
        }
    }
})();