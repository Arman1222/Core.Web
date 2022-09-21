(function () {
    window.app.factory('applicationusersService', applicationusersService);

    applicationusersService.$inject = ['$http', 'DataService', 'alerts'];
    function applicationusersService($http, DataService, alerts) {

        var controllerPath = 'ApplicationUsers/'
        var list = [];       

        var svc = {
            loadAll: loadAll,
            getPage: getPage,
            getEmployee: getEmployee,
            getRoleNames: getRoleNames,
            getDepartment: getDepartment,
            getArea: getArea,
            getDivision: getDivision,
            getJabatan: getJabatan,
            getBranch: getBranch,
            add: add,
            update: update,
            list: list,
            getData: getData,
            userRoleGetAll: userRoleGetAll,
            userRoleAdd: userRoleAdd,
            userRoleDelete: userRoleDelete,
            getMypeopleDashBoardData: getMypeopleDashBoardData
        };

        return svc;

        function loadAll() {
            return DataService.post(controllerPath + 'All')
				.then(function (response) {
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
        function getMypeopleDashBoardData() {
            return DataService.post(controllerPath + 'getMypeopleDashBoardData')
				.then(function (response) {
				    return response;
				});
        }
        function userRoleGetAll(params) {
            return DataService.post(controllerPath + 'UserRoleGetAll', params)
				.then(function (response) {
				    return response;
				});
        }

        function userRoleAdd(params) {
            return DataService.post(controllerPath + 'UserRoleAdd', params)
				.then(function (response) {
				    return response;
				});
        }
        function userRoleDelete(params) {
            return DataService.post(controllerPath + 'UserRoleDelete', params)
				.then(function (response) {
				    return response;
				});
        }

        function getEmployee() {
            return DataService.post(controllerPath + 'GetEmployee')
				.then(function (response) {
				    return response;
				});
        }

        function getRoleNames() {
            return DataService.post(controllerPath + 'GetRoleNames')
				.then(function (response) {
				    return response;
				});
        }

        function getDepartment(params) {
            return DataService.post(controllerPath + 'GetDepartment')
				.then(function (response) {
				    return response;
				});
        }
        function getDivision(params) {
            return DataService.post(controllerPath + 'GetDivision')
				.then(function (response) {
				    return response;
				});
        }
        function getJabatan(params) {
            return DataService.post(controllerPath + 'GetJabatan')
				.then(function (response) {
				    return response;
				});
        }
        function getArea(params) {
            return DataService.post(controllerPath + 'GetArea')
				.then(function (response) {
				    return response;
				});
        }
        function getBranch(params) {
            return DataService.post(controllerPath + 'GetBranch')
				.then(function (response) {
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
                if (list[i].id == id) return list[i];
            }

            return null;
        }
    }
})();