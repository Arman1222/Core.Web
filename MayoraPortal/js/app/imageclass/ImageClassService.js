(function() {
    window.app.factory('imageclassService', imageclassService);

    imageclassService.$inject = ['$http','DataService'];
    function imageclassService($http, DataService) {

        var controllerPath = 'ImageClass/'
        var list = [];

        var picklist = function () {
            return DataService.post(controllerPath + 'list')
            .then(function (data) {
                return _generateTable(data);              
            });
        };

        var combobox = function () {
            return DataService.post(controllerPath + 'list').then(function (data) {
                return data;
            });
        };

        var svc = {
            loadAll: loadAll,
			add: add,
			update: update,
			list: list,
			getData: getData,
			picklist: picklist,
            combobox: combobox
		};

		return svc;

		function loadAll(pageNumber, pageSize) {
		    if (typeof (pageNumber) === 'undefined') pageNumber = 1;
		    if (typeof (pageSize) === 'undefined') pageSize = 5;

		    return DataService.post(controllerPath + 'All/' + pageNumber + "/" + pageSize)
				.then(function (response) {
				    list.addRange(response.data);
				    return response;
				});
		}

		function add(obj) {
		    return $http.post(MyApplication.rootPath + controllerPath + 'Add', obj)
				.success(function (obj) {
				    list.unshift(obj);
				});
		}

		function update(existingObj, updatedObj) {
		    return $http.post(MyApplication.rootPath + controllerPath + 'Update', updatedObj)
				.success(function(obj) {
				    angular.extend(existingObj, obj);
				});
		}

		function getData(id) {
		    for (var i = 0; i < list.length; i++) {
		        if (list[i].imageClassId == id) return list[i];
			}

			return null;
		}
	}
})();