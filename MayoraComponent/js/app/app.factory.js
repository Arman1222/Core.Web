(function () {
    'use strict';
window.app.factory("DataService", ['$http', '$location', function ($http, $q, $location) {

    var serviceBase = MyApplication.rootPath;
    var obj = {};

    obj.get = function (q) {
        return $http.get(serviceBase + q).then(function (results) {
            return results.data;
        });
    };

    obj.post = function (q, object) {
        return $http.post(serviceBase + q, object).then(function (results) {
            return results.data;
        });
    };

    obj.put = function (q, object) {
        return $http.put(serviceBase + q, object).then(function (results) {
            return results.data;
        });
    };

    obj.delete = function (q) {
        return $http.delete(serviceBase + q).then(function (results) {
            return results.data;
        });
    };

    return obj;

}]);

window.app.factory("DateService", ["$http", function ($http) {
    // Expect input as d/m/y
    var getDatepickerOptions = function (s) {
        return {
            format: 'dd-MM-yyyy'
            , language: 'id'
            , autoclose: true
            , weekStart: 0
        }
    };

    return {
        getDatepickerOptions: getDatepickerOptions

    }
}]);

//http://www.dwmkerr.com/the-only-angularjs-modal-service-youll-ever-need/
//http://www.bennadel.com/blog/2806-creating-a-simple-modal-system-in-angularjs.htm
window.app.factory("MessageBox", ["ModalService", function (ModalService) {

    var show = function (response) {
        if (response != undefined) {
            if (typeof response === 'string') {
                success("INFO", response, false);
            }
            else if (response.data != undefined) {
                if (response.data.errorMessages != undefined && response.data.errorMessages.length > 1) {
                    var errorMessages = "";
                    for (var i = 0; i < response.data.errorMessages.length; i++) {
                        errorMessages += response.data.errorMessages[i] + '</br>';
                    }
                    error("ERROR", errorMessages);
                } else if (response.data.errorMessage != undefined) {
                    error("ERROR", response.data.errorMessage);
                }
            } else if (response.statusText != undefined) {
                success("INFO", response.statusText, false);
            }

        }
    };

    var confirm = function (title, message, callbackFunction) {
        success(title, message, true, callbackFunction);
        //success("INFO", "test");
    };

    var success = function (title, message, isConfirm, callbackFunction) {
        ModalService.showModal({
            templateUrl: 'modalsuccess.html',
            controller: "ModalController",
            inputs: {
                title: title,
                message: message,
                isConfirm: isConfirm
            }
        }).then(function (modal) {
            modal.element.modal();
            modal.close.then(function (result) {
                //$scope.message = "You said " + result;
                if (isConfirm && result && callbackFunction !== undefined) {
                    callbackFunction();
                }
            });
        });
    };

    var error = function (title, message) {
        ModalService.showModal({
            templateUrl: 'modalerror.html',
            controller: "ModalController",
            inputs: {
                title: title,
                message: message,
                isConfirm: false
            }
        }).then(function (modal) {
            modal.element.modal();
            modal.close.then(function (result) {
                //$scope.message = "You said " + result;
            });
        });
    };

    return {
        show: show,
        confirm: confirm,
        success: success,
        error: error

    }
}]);

window.app.controller('ModalController', function ($scope, title, message, isConfirm, close) {
    $scope.title = title;
    $scope.message = message;
    $scope.isConfirm = isConfirm;
    $scope.close = function (result) {
        close(result, 500); // close, but give 500ms for bootstrap to animate
    };

});

window.app.factory('$exceptionHandler', ['$log', function ($log) {
    return function myExceptionHandler(exception, cause) {
        //logErrorsToBackend(exception, cause);
        $log.warn(exception, cause);
    };
}]);
})();