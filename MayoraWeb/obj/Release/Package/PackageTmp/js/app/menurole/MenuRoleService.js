(function () {
    window.app.factory('menuroleService', menuroleService);

    menuroleService.$inject = ['$http', 'DataService'];
    function menuroleService($http, DataService) {

        var controllerPath = '/MenuRole/'
        var list = [];
        var menutree = '';       

        var svc = {
            controllerPath: controllerPath,
            add: add,
            update: update,
            list: list,
            getMenusByRole: getMenusByRole,
            menuPicklist: menuPicklist,
            rolePicklist: rolePicklist,
            getPageMenu: getPageMenu,
            getPageRole: getPageRole,
            getPageApplication: getPageApplication
        };

        return svc;        

        function getPageApplication(params) {
            return DataService.post('/Application/GetPage', params)
				.then(function (response) {
				    return response;
				});
        }

        function getPageMenu(params) {
            return DataService.post('/Navbar/GetPage', params)
				.then(function (response) {				    
				    return response;
				});
        }

        function getPageRole(params) {
            return DataService.post('/ApplicationRoles/GetPage', params)
				.then(function (response) {
				    return response;
				});
        }

        function menuPicklist() {
            return DataService.post('/Navbar/All').then(function (data) {
                return _generateTable(data);
            });
        };

        function rolePicklist () {
            return DataService.post('/ApplicationRoles/All').then(function (data) {
                return _generateTable(data);
            });
        };        

        function add(obj) {
            return DataService.post(controllerPath + 'Add', obj)
                .then(function (data) {
                    return data;
            });         
        }

        function update(existingObj, updatedObj) {
            return $http.post(controllerPath + 'Update', updatedObj)
				.success(function (obj) {
				    angular.extend(existingObj, obj);
				});
        }

        function getMenusByRole(obj) {
            return DataService.post(controllerPath + 'GetMenusByRole', obj)
               .then(function (data) {
                   return data;
               });
        }
    }
})();