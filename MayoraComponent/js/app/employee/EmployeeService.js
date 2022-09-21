(function () {
    window.app.factory('employeeService', employeeService);

    employeeService.$inject = ['$http', 'DataService', 'alerts'];
    function employeeService($http, DataService, alerts) {

        var controllerPath = 'Employee/'
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

        function add(obj) {
            return DataService.post(controllerPath + 'Add', obj)
                .then(function (data) {
                    list.unshift(data);
                    return data;
                });
        }

        function update(existingObj, updatedObj) {
            return DataService.post(controllerPath + 'Update', updatedObj)
                .then(function (data) {
                    angular.extend(existingObj, updatedObj);
                    return data;
                });
        }

        function deleteData(obj) {
            return DataService.post(controllerPath + 'Delete', obj)
                .then(function (data) {
                    return data;
                });
        }

        function getData(nik) {
            for (var i = 0; i < list.length; i++) {
                if (list[i].nik == nik) return list[i];
            }

            return null;
        }
    }
})();