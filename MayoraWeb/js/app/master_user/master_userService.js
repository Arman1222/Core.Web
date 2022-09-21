(function () {
    window.app.factory('master_userService', master_userService);

    master_userService.$inject = ['$http', 'DataService', 'alerts'];
    function master_userService($http, DataService, alerts) {

        var controllerPath = 'master_user/'
        var list = [];

        var svc = {
            getPage: getPage,
            getOffice: getOffice,
            getClassification: getClassification,
            getModule: getModule,
            getUserModule: getUserModule,
            add: add,
            update: update,
            addModule: addModule,
            updatewithpass: updatewithpass,
            hapus: hapus,
            hapusModule: hapusModule,
            list: list,
            getData: getData,
            checker: checker
        };

        return svc;

        function getPage(params) {
            return DataService.post(controllerPath + 'GetPage', params)
				.then(function (response) {
				    return response;
				});
        }

        function getOffice(params) {
            return DataService.post(controllerPath + 'GetOffice', params)
				.then(function (response) {
				    return response;
				});
        }

        function getClassification(params) {
            return DataService.post(controllerPath + 'GetClassification', params)
				.then(function (response) {
				    return response;
				});
        }

        function getModule(params) {
            return DataService.post(controllerPath + 'GetModule', params)
				.then(function (response) {
				    return response;
				});
        }

        function getUserModule(params) {
            return DataService.post(controllerPath + 'GetUserModule', params)
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

        function update(obj) {
            return DataService.post(controllerPath + 'Update', obj)
                .then(function (data) {
                    list.unshift(data);
                    return data;
                });
        }

        function addModule(obj) {
            return DataService.post(controllerPath + 'AddModule', obj)
                .then(function (data) {
                    list.unshift(data);
                    return data;
                });
        }

        function updatewithpass(obj) {
            return DataService.post(controllerPath + 'UpdateWithPass', obj)
                .then(function (data) {
                    list.unshift(data);
                    return data;
                });
        }

        function hapus(obj) {
            return DataService.post(controllerPath + 'Hapus', obj)
                .then(function (data) {
                    list.unshift(data);
                    return data;
                });
        }

        function hapusModule(obj) {
            return DataService.post(controllerPath + 'HapusModule', obj)
                .then(function (data) {
                    list.unshift(data);
                    return data;
                });
        }

        function getData(id) {
            for (var i = 0; i < list.length; i++) {
                if (list[i].Id == id) return list[i];
            }

            return null;
        }

        function checker(params) {
            return DataService.post(controllerPath + 'Checker', params)
				.then(function (response) {
				    return response;
				});
        }
    }
})();