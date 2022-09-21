(function () {
    window.app.factory('master_area_Service', master_area_Service);
    //o20170928
    master_area_Service.$inject = ['$http', 'DataService', 'alerts'];
    function master_area_Service($http, DataService, alerts) {

        var controllerPath = 'master_area_/'
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
            checkerId: checkerId,
            checkerCode: checkerCode
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
        
        function checkerId(params) {
            return DataService.post(controllerPath + 'CheckerId', params)
				.then(function (response) {
				    return response;
				});
        }
        
        function checkerCode(params) {
            return DataService.post(controllerPath + 'CheckerCode', params)
				.then(function (response) {
				    return response;
				});
        }
    }
})();