(function () {
    'use strict';

    //http://brianhann.com/easily-import-spreadsheets-into-ui-grid/
    window.app.directive("fileread", ['$filter', function ($filter) {
        return {
            scope: {
                opts: '=',
                onFinish: '&' 
            },
            link: function ($scope, $elm, $attrs) {
                $elm.on('change', function (changeEvent) {
                    var reader = new FileReader();
                    
                    //get file extension
                    var parts =  $elm.context.value.split('.');
                    var extension = parts[parts.length - 1];

                    //if xlsx
                    if (extension == 'xlsx') {
                        reader.onload = function (evt) {
                            $scope.$apply(function () {
                                var data = evt.target.result;

                                var workbook = XLSX.read(data, { type: 'binary' });

                                var headerNames = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[0]], { header: 1 })[0];

                                var data = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[0]]);

                                $scope.opts.columnDefs = [];
                                headerNames.forEach(function (h) {
                                    $scope.opts.columnDefs.push({ field: h });
                                });

                                $scope.opts.data = data;

                                $elm.val(null);
                                $scope.onFinish();
                            });
                        };
                    }
                    //if csv
                    else if (extension == 'csv') {
                        reader.onload = function (evt) {
                            $scope.$apply(function () {

                                var data = evt.target.result;
                                var obj = $filter('CsvToObj')(data);

                                $scope.opts.columnDefs = [];
                                obj.cols.forEach(function (h) {
                                    $scope.opts.columnDefs.push({ field: h.trim() });
                                });

                                $scope.opts.data = obj.rows;

                                $elm.val(null);
                                $scope.onFinish();
                            });
                        };
                    }
        
                    reader.readAsBinaryString(changeEvent.target.files[0]);
                });
            }
        }
    }]);

})();