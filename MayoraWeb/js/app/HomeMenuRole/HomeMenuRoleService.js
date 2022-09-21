(function () {
    window.app.factory('homemenuroleService', homemenuroleService);

    homemenuroleService.$inject = ['$http','DataService','alerts'];
    function homemenuroleService($http, DataService, alerts) {

        var controllerPath = 'HomeMenuRole/'
        var list = [];

        var svc = {
            getAll: getAll,
            getPage: getPage,
            add: add,
            update: update,
            list: list,
            deleteData: deleteData,
            getData: getData
        };

        return svc;


        function deleteData(obj) {
            return DataService.post(controllerPath + 'Delete', obj)
                .then(function (data) {
                    return data;
                });
        }

        function getPage(params) {
            return DataService.post(controllerPath + 'GetPage', params)
				.then(function (response) {
				    return response;
				});
        }

        function getAll(params) {
            return DataService.post(controllerPath + 'All', params)
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
            return DataService.post(controllerPath + 'Update', updatedObj)
               .then(function (obj) {
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