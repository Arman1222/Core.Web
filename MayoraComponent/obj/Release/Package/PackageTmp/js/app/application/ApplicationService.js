(function () {
    window.app.factory('applicationService', applicationService);

    applicationService.$inject = ['$http', 'DataService','alerts'];
    function applicationService($http, DataService, alerts) {

        var controllerPath = 'Application/'
        var list = [];

        loadAll();

        var svc = {
            loadAll: loadAll,
            getPage: getPage,
            add: add,
            update: update,
            list: list,
            getData: getData
        };

        return svc;

        function loadAll() {
            return DataService.post(controllerPath + 'All/')
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

        function add(obj) {
            return DataService.post(controllerPath + 'Add', obj)
                .then(function (data) {
                    list.unshift(data);
            });        
        }

        function update(existingObj, updatedObj) {
            return DataService.post(controllerPath + 'Update', updatedObj)
				.then(function (obj) {
				    angular.extend(existingObj, obj);				    
				});
        }

        function getData(id) {
            for (var i = 0; i < list.length; i++) {
                if (list[i].Id == applicationId) return list[i];
            }

            return null;
        }
    }


    window.app.factory('rolemanageService', rolemanageService);

    rolemanageService.$inject = ['$http', 'DataService', 'alerts'];
    function rolemanageService($http, DataService, alerts) {

        var controllerPath = 'ApplicationRoles/'
        var list = [];

        loadAll();

        var svc = {
            loadAll: loadAll,
            getAll: getAll,
            getAllApplication: getAllApplication,
            dataDelete: dataDelete,
            getAllNavbar: getAllNavbar,
            linkRoleNavbar: linkRoleNavbar,
            unlinkRoleNavbar: unlinkRoleNavbar,
            getPage: getPage,
            add: add,
            update: update,
            list: list,
            getData: getData
        };

        return svc;

        function loadAll() {
            return DataService.post(controllerPath + 'All/')
				.then(function (response) {
				    list.addRange(response.data);
				    return response;
				});
        }

        function getAll() {
            return DataService.post(controllerPath + 'RoleGetAllRole')
				.then(function (response) {
				    return response;
				});
        }

        function getAllApplication(params) {
            return DataService.post(controllerPath + 'RoleGetAllApplication', params)
				.then(function (response) {
				    return response;
				});
        }

        function getAllNavbar(params) {
            return DataService.post(controllerPath + 'RoleGetAllNavbar', params)
				.then(function (response) {
				    return response;
				});
        }

        function linkRoleNavbar(params) {
            return DataService.post(controllerPath + 'LinkRoleNavbar', params)
				.then(function (response) {
				    return response;
				});
        }
        function unlinkRoleNavbar(params) {
            return DataService.post(controllerPath + 'UnlinkRoleNavbar', params)
				.then(function (response) {
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
            return DataService.post(controllerPath + 'RoleAdd', obj)
                .then(function (data) {
                    return data;
                });
        }

        function dataDelete(obj) {
            return DataService.post(controllerPath + 'RoleDelete', obj)
                .then(function (data) {
                    return data;
                });
        }

        function update(existingObj, updatedObj) {
            return DataService.post(controllerPath + 'Update', updatedObj)
				.then(function (obj) {
				    angular.extend(existingObj, obj);
				});
        }

        function getData(id) {
            for (var i = 0; i < list.length; i++) {
                if (list[i].Id == applicationId) return list[i];
            }

            return null;
        }
    }
})();