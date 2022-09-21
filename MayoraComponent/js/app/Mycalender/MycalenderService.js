(function () {
    window.app.factory('mycalenderService', mycalenderService);

    mycalenderService.$inject = ['$http', 'DataService', 'alerts'];
    function mycalenderService($http, DataService, alerts) {

        var controllerPath = 'MyCalender/'
        var list = [];       

        var svc = {
            getPageNote: getPageNote,
            addNote: addNote,
            updateNote: updateNote,
            deleteNote: deleteNote,
            getPageTask: getPageTask,
            addTask: addTask,
            updateTask: updateTask,
            deleteTask: deleteTask
        };

        return svc;

        function getPageNote(params) {
            return DataService.post(controllerPath + 'GetPageNote', params)
				.then(function (response) {
				    return response;
				});
        }

        function addNote(params) {
            return DataService.post(controllerPath + 'AddNote', params)
				.then(function (response) {
				    return response;
				});
        }

        function updateNote(params) {
            return DataService.post(controllerPath + 'UpdateNote', params)
				.then(function (response) {
				    return response;
				});
        }

        function deleteNote(params) {
            return DataService.post(controllerPath + 'DeleteNote', params)
				.then(function (response) {
				    return response;
				});
        }

        function getPageTask(params) {
            return DataService.post(controllerPath + 'GetPageTask', params)
				.then(function (response) {
				    return response;
				});
        }

        function addTask(params) {
            return DataService.post(controllerPath + 'AddTask', params)
				.then(function (response) {
				    return response;
				});
        }

        function updateTask(params) {
            return DataService.post(controllerPath + 'UpdateTask', params)
				.then(function (response) {
				    return response;
				});
        }

        function deleteTask(params) {
            return DataService.post(controllerPath + 'DeleteTask', params)
				.then(function (response) {
				    return response;
				});
        }
    }
})();