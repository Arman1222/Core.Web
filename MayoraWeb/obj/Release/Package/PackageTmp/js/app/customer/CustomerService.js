(function () {
    window.app.factory('customerService', customerService);

    customerService.$inject = ['$http','DataService','alerts'];
    function customerService($http, DataService, alerts) {

        var controllerPath = 'Customer/'
        var list = [];

        var svc = {
            getAll: getAll,
            getPage: getPage,
            exportWord: exportWord,
            exportPdf: exportPdf,
            add: add,
            update: update,
            list: list,
            getData: getData,
            uploadData: uploadData,
            testError: testError,
        };

        return svc;


        function getPage(params) {
            return DataService.post(controllerPath + 'GetPage', params)
				.then(function (response) {
				    return response;
				});
        }

        function getAll(params) {
            //if (typeof (pageNumber) === 'undefined') pageNumber = 1;
            //if (typeof (pageSize) === 'undefined') pageSize = 5;

            //return $http.post(MyApplication.rootPath + controllerPath + 'All')
			//	.then(function (response) {
			//	    list.addRange(response.data);
            //	});

            //return DataService.post(controllerPath + 'All/' + pageNumber + "/" + pageSize)

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

            //return $http.post(controllerPath + 'Add', obj)
			//	.success(function (obj) {
			//	    list.unshift(obj);
			//	});
        }

        function testError() {
            return DataService.post(controllerPath + 'TestShowError')
                .then(function (data) {
                    return data;
                });           
        }

        function exportWord() {
            return DataService.post('Report/Word')
                .then(function (data) {
                    return data;
                });
        }

        function exportPdf() {
            return DataService.post('Report/Pdf')
                .then(function (data) {
                    return data;
                });
        }

        function update(existingObj, updatedObj) {
            //return $http.post(MyApplication.rootPath + controllerPath + 'Update', updatedObj)
			//	.success(function (obj) {
			//	    angular.extend(existingObj, obj);
			//	});

            return DataService.post(controllerPath + 'Update', updatedObj)
               .then(function (obj) {
                   angular.extend(existingObj, obj);
               });
        }

        function uploadData(objList) {
            return DataService.post(controllerPath + 'UploadData', objList)
             .then(function (response) {

             });

            //return $http.post(MyApplication.rootPath + controllerPath + 'UploadData', objList)
			//	.success(function (response) {
				    
			//	});
        }
        
        function getData(id) {
            for (var i = 0; i < list.length; i++) {
                if (list[i].Id == id) return list[i];
            }

            return null;
        }
    }
})();