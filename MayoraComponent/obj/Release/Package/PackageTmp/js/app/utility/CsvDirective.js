(function () {
    'use strict';
    //http://stackoverflow.com/questions/28070374/parsing-a-csv-file-provided-by-an-input-in-angular
    //http://stackoverflow.com/questions/24080602/parsing-csv-in-javascript-and-angularjs
    window.app.filter('CsvToObj', function () {
        return function (input) {
            var rows = input.split('\n');
            if (rows.length > 0) {
                var cols = rows[0].split(';');
                var obj = {};
                obj.cols = cols;
                obj.rows = [];
                angular.forEach(rows, function (val, key) {
                    if (key > 0) {
                        var o = val.split(';');
                        var objItem = {};
                        for (var i = 0; i < o.length; i++) {
                            objItem[cols[i].trim()] = o[i];
                        }                       
                        obj.rows.push(objItem);
                    } 
                    //obj.push({
                    //    o[0]: o[0],
                    //    o[1]: o[1],
                    //    o[2]: "not sent"
                    //});
                });
            }
            return obj;
        };
    });

})();