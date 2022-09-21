(function () {
    window.app.factory('companyService', companyService);

    companyService.$inject = ['$http', 'DataService', 'alerts'];
    function companyService($http, DataService, alerts) {

        var controllerPath = 'Company/'
        var list = [];

        var svc = {
            loadAll: loadAll,
            add: add,
            update: update,
            list: list,
            getData: getData
        };

        return svc;

        function loadAll(pageNumber, pageSize) {
            if (typeof (pageNumber) === 'undefined') pageNumber = 1;
            if (typeof (pageSize) === 'undefined') pageSize = 5;

            return DataService.post(controllerPath + 'All/' + pageNumber + "/" + pageSize)
				.then(function (response) {
				    list.addRange(response.data);
				    return response;
				});
        }

        function add(obj) {

            return DataService.post(controllerPath + 'Add', obj)
                .then(function (data) {
                    list.unshift(data);
                });         
        }

        function update(existingObj, updatedObj) {
            return $http.post(controllerPath + 'Update', updatedObj)
				.success(function (obj) {
				    angular.extend(existingObj, obj);
				});
        }

        function getData(id) {
            for (var i = 0; i < list.length; i++) {
                if (list[i].Id == id) return list[i];
            }

            return null;
        }
    }
})();