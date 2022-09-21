(function () {
    window.app.factory('master_office_Service', master_office_Service);
    //o20170927
    master_office_Service.$inject = ['$http', 'DataService', 'alerts'];
    function master_office_Service($http, DataService, alerts) {

        var controllerPath = 'master_office_/'
        var list = [];

        var svc = {
            getPage: getPage,
            getArea: getArea,
            add: add,
            edit: edit,
            insert: insert,
            update: update,
            list: list,
            getData: getData,
            checker: checker,//o20170928
            checkerT24: checkerT24//o20170928
        };

        return svc;

        function getPage(params) {
            return DataService.post(controllerPath + 'GetPage', params)
				.then(function (response) {
				    return response;
				});
        }

        function getArea(params) {
            return DataService.post(controllerPath + 'GetArea', params)
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

        function insert(obj) {
            return DataService.post(controllerPath + 'Insert', obj)
                .then(function (data) {
                    list.unshift(data);
                    return data;
                });
        }

        function edit(obj) {
            return DataService.post(controllerPath + 'Edit', obj)
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

        function getData(id) {
            for (var i = 0; i < list.length; i++) {
                if (list[i].Id == id) return list[i];
            }

            return null;
        }
        //o20170928
        function checker(params) {
            return DataService.post(controllerPath + 'Checker', params)
				.then(function (response) {
				    return response;
				});
        }
        //o20170928
        function checkerT24(params) {
            return DataService.post(controllerPath + 'CheckerT24', params)
				.then(function (response) {
				    return response;
				});
        }
    }
})();