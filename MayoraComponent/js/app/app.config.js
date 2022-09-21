(function () {
    'use strict';
    //http://aboutcode.net/2013/07/27/json-date-parsing-angularjs.html
    window.app.config(["$httpProvider", "$provide", "$urlRouterProvider", "$stateProvider", function ($httpProvider, $provide, $urlRouterProvider, $stateProvider) {
        //$httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
        //$httpProvider.defaults.headers.common = {};
        //$httpProvider.defaults.headers.post = {};
        //$httpProvider.defaults.headers.put = {};
        //$httpProvider.defaults.headers.patch = {};
        //$httpProvider.defaults.headers.get = {};
        $httpProvider.defaults.headers.common['Access-Control-Allow-Headers'] = '*';
        $httpProvider.defaults.useXDomain = true;
        delete $httpProvider.defaults.headers.common['X-Requested-With'];
        $httpProvider.defaults.transformResponse.push(function (responseData) {
            _convertDateStringsToDates(responseData);
            return responseData;
        });
        //https://github.com/angular-ui/ui-grid/issues/4214
        $provide.decorator('uiGridExporterService', function ($delegate, $filter) {
            $delegate.formatFieldAsCsv = function (field) {
                if (field.value == null) { // we want to catch anything null-ish, hence just == not ===
                    return '';
                }
                if (typeof (field.value) === 'number') {
                    return field.value;
                }
                if (typeof (field.value) === 'boolean') {
                    return (field.value ? 'TRUE' : 'FALSE');
                }
                if (typeof (field.value) === 'string') {
                    //Anyone using this decorator with text values which may have commas will want to surround text fields with quotes.
                    if (!isNaN(field.value)) //if numeric then add single quote
                        return '"\'' + field.value + '"';

                    return '"' + field.value + '"';
                    //return field.value;
                }
                if (field.value instanceof Date) {
                    //return $filter('date')(field.value, 'dd/MM/yyyy');
                    return $filter('dateFormat')(field.value);
                }

                return JSON.stringify(field.value);
            };

            return $delegate
        });

        $urlRouterProvider.otherwise('/home');

        $stateProvider
            // State managing 
            .state('home', {
                url: '/home',
                templateUrl: 'Dashboard',
                itemdesc: {
                    name: "home",
                    text: "Dashboard",
                    description: "AING KASEP"
                }
            })
             // nested list with data
            .state('changeprofile', {
                url: '/changeprofile',
                templateUrl: 'Manage/ChangeProfile'
            })
            .state('customer', {
                url: '/customer',
                templateUrl: 'Customer'
            })
            .state('mycalender', {
                url: '/mycalender',
                templateUrl: 'Dashboard/mycalender',
                itemdesc: {
                    name: "mycalender",
                    text: "My Calender",
                    description: "Art your activity"
                }
            })
            .state('mytask', {
                url: '/mytask',
                templateUrl: 'Dashboard/mytask',
                itemdesc: {
                    name: "mytask",
                    text: "My Task",
                    description: "A little more action"
                }
            })
            .state('my-note', {
                url: '/my-note',
                templateUrl: 'Dashboard/mynote',
                itemdesc: {
                    name: "my-note",
                    text: "My Note",
                    description: "Write your important thing"
                }

            });
        //var serviceBase = MyApplication.rootPath;
        //$http.get(serviceBase + 'Json_Getmenu').then(function (results) {
        //    console.log(results.data);
        //});
    }]);
    window.app.config(function($sceDelegateProvider) {  
        $sceDelegateProvider.resourceUrlWhitelist([
            // Allow same origin resource loads.
            'self',
            // Allow loading from our assets domain. **.
            //'http://10.10.3.222:88/MayoraComponent/**',
            //'http://10.10.2.20:10012/**',
            'http://10.10.2.199/MayoraComponent/**'
        ]);
    });

    window.app.provider('$dashboardState', function($stateProvider){
        this.$get = function($state){
            return {
                addState: function(title, templateurl, itemdesc) {
                    $stateProvider.state(title, {
                        url: '/' + title,
                        templateUrl: templateurl,
                        itemdesc: itemdesc
                    });
                }
            }
        }
    });
})();