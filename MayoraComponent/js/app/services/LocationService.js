(function () {
    window.app.factory('locationService', locationService);

    locationService.$inject = ['$http', 'DataService', 'alerts', '$window'];
    function locationService($http, DataService, alerts, $window) {

        var svc = {
            search: search          
        };

        return svc;

        function search(params,isNewTab) {
            var url = $window.location.origin + $window.location.pathname; // "/?startDate="+new Date().toISOString()+"&endDate="+new Date().toISOString()+"&area=100";

            if (params !== undefined && params !== null) {
                url += "?";
                for (var key in params) {
                    //copy all the fields
                    if (params[key] !== undefined) {
                        if (params[key] instanceof Date) {
                            url += key + '=' + params[key].toISOString() + '&';
                        } else {
                            url += key + '=' + params[key] + '&';
                        }
                    }
                }
            }
            if (isNewTab) {
                $window.open(
                url,
                '_blank' // <- This is what makes it open in a new window.
                );
            } else {
                $window.location.href = url;
            }
        }
    }
})();