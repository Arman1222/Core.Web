(function () {
    window.app.factory('navbarmanuService', navbarmanuService);

    navbarmanuService.$inject = ['$http', 'DataService', 'alerts'];
    function navbarmanuService($http, DataService, alerts) {

        var controllerPath = 'Navbar/'
        var list = [];       

        var svc = {
            loadAll: loadAll,
            getPage: getPage,
            add: add,
            update: update,
            datadelete: datadelete,
            getData: getData,
            getlistmenu: getlistmenu,
            gettreemenu: gettreemenu,
            gettreemenubyapp: gettreemenubyapp,
            moveup: moveup,
            movedown: movedown
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

        function gettreemenubyapp(params) {
            return DataService.post(controllerPath + 'Json_Gettreemenubyapp', params)
				.then(function (response) {
				    return response;
				});
        }

        function gettreemenu() {
            return DataService.post(controllerPath + 'Json_Gettreemenu')
				.then(function (response) {
				    return response;
				});
        }
        function getlistmenu() {
            return DataService.post(controllerPath + 'Json_Getmenu')
				.then(function (response) {
				    return response;
				});
        }
        function add(obj) {
            return DataService.post(controllerPath + 'Json_addmenu', obj)
                .then(function (data) {
                    return data;
            });        
        }

        function update(updatedObj) {
            return $http.post(controllerPath + 'Json_updatemenu', updatedObj)
				.then(function (obj) {
				    return obj;
				});
        }

        function datadelete(obj) {
            return DataService.post(controllerPath + 'Json_deletemenu', obj)
                .then(function (data) {
                    return data;
                });
        }

        function moveup(obj) {
            return DataService.post(controllerPath + 'Json_moveupmenu', obj)
                .then(function (data) {
                    return data;
                });
        }
        function movedown(obj) {
            return DataService.post(controllerPath + 'Json_movedownmenu', obj)
                .then(function (data) {
                    return data;
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