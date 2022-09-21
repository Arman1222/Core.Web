(function () {
    //http://brianhann.com/ui-grid-the-easiest-customization-youll-ever-write/
    //http://stackoverflow.com/questions/29092831/how-to-use-angular-ui-grid-with-server-side-paging
    //http://www.advancesharp.com/blog/1116/ng-grid-search-sort-paging-with-angularjs-webapi-and-mvc-asp-net
    //http://stackoverflow.com/questions/28353514/not-able-to-display-ui-grid-in-ui-bootstrap-modal-windown
    //The issue is related to modal sizing, if you resize the window the grid will be displayed.
    //Adding auto-resizing directive solved the problem.

    //Dynamic height based on row count
    //https://github.com/angular-ui/ui-grid/issues/3031
    //https://github.com/angular-ui/ui-grid/issues/2500

	window.app.directive('modalGrid', modalGrid);
	function modalGrid() {
		return {
		    scope: {
                init: '=?',
		        gridData: '=',
		        gridDataAll: '=',
		        gridDataUrl: '@',
                searchParams: '=?',
				title: '@',
                tableActions: '=',
                columns: '@?',
                loading: '=',
                message: '=',
                selectColumn: '@',                
                sortBy: '=?',
                refreshData: '=?',
                onBlurEditedCell: '=?',
                selectedRows: '=?',
                totalItems: '=?',
                scopeParams: '=?',
                showColumns: '=?',
                hideColumns: '=?',
                onLoad: '=?',
                response: '=?',
                ngDisabled: '=?',
                autoLoad: '@?',
                enableSelect: '@?',
                enableFilter: '@?',
                rowStyle: '=?',
                csvFilename: '@?',
                hideSearch: '@?'
			},
			template:
				'<div>' +
					'<h4 ng-show="vm.title"><i class="fa fa-pie-chart fa-fw"></i> {{vm.title}}</h4>' +
                    '<div ng-hide="hideSearch" class="input-group col-lg-4">' +
                        '<input type="text"'+
                            'name="searchText"'+
                            'ng-model="vm.filterOptions.filterText"' +
                            'ng-disabled="ngDisabled"' +
                            'my-enter="refreshData()"' +
                            'class="form-control"'+
                            'placeholder="Search..." />'+
                        '<span class="input-group-addon" ng-click="refreshData()"><i class="fa fa-search"></i></span>' +
                    '</div>' +
                    '</br>' +
					'<div>' +
						//'<p ng-if="vm.loading">Loading...</p>' +
						'<div external-scopes="vm.externalScopes" ui-grid="vm.gridOptions" ng-disabled="ngDisabled" ui-grid-pagination ui-grid-auto-resize ui-grid-selection ui-grid-resize-columns ui-grid-exporter ng-style="{height: (vm.gridOptions.data.length*30)+(vm.gridOptions.enableFiltering ? 115 : 80)+\'px\'}" loading="vm.loading">' +
                            '<div class="grid-msg-overlay" ng-hide="!vm.loading">' +
                                '<div class="msg">' +
                                    '<span>Loading Data...<i class="fa fa-spinner fa-spin"></i></span>' +
                                '</div>' +
                            '</div>' +
                            '<div class="grid-msg-overlay" ng-hide="vm.loading || vm.gridOptions.data.length  || vm.gridOptions.enableFiltering">' +
                              '<div class="msg">' +
                                '<span>No Data</span>' +
                              '</div>' +
                            '</div>' +
                        '</div>' +
					'</div>' +
				'</div>',
			controllerAs: 'vm',
			controller: controller
		}
	}

	controller.$inject = ['$scope', '$http', 'MessageBox', '$timeout', 'uiGridExporterConstants', 'uiGridExporterService'];
	function controller($scope, $http, MessageBox, $timeout, uiGridExporterConstants, uiGridExporterService) {
		var vm = this;

		vm.title = $scope.title;		

		vm.paginationOptions = {
		    pageNumber: 1,
		    pageSize: 10,
		    sortField: null,
		    sortDirection: null
		};
		vm.paramColumn = [];
	    vm.filterOptions = {
	            filterText: '',
	            externalFilter: 'searchText',
	            useExternalFilter: true
	    };

	    if ($scope.sortBy == "undefined")
	    {
	        $scope.sortBy = null;
	    }

	    $scope.defaultCsvFilename = "myFile.csv";
	    if ($scope.csvFilename !== undefined && $scope.csvFilename !== '') {
	        $scope.defaultCsvFilename = $scope.csvFilename;
	    }

		vm.gridOptions = {
		    paginationPageSizes: [10, 20, 50],
		    paginationPageSize: 10,
		    enableFiltering: $scope.enableFilter === 'True',
		    enableRowSelection: $scope.enableSelect === 'True',
		    enableSelectAll: $scope.enableSelect === 'True',
		    enableRowHeaderSelection: $scope.enableSelect === 'True',
		    useExternalPagination: true,
		    enableVerticalScrollbar: 0, //NEVER
		    enableHorizontalScrollbar: 0, //NEVER
		    enablePaginationControls: false,
		    exporterMenuPdf: false, //Remove export to pdf option in Angular ui-grid
		    exporterMenuCsv: false,
		    exporterCsvFilename: $scope.defaultCsvFilename,
		    exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
		    enableGridMenu: true,
		    gridMenuCustomItems: [ 
                      {
                          title: 'Export All Visible Data as CSV',
                          order: 1,
                          action: function ($event) {
                              generateCsv(vm.gridOptions.data);
                          }
                      }
		    ],
		    //useExternalSorting: true,
		    enableColumnMenus: false,
		    onRegisterApi: function (gridApi) {
		        vm.gridApi = gridApi;
		        vm.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
		            if (sortColumns.length == 0) {
		                vm.paginationOptions.sortDirection = null;
		                vm.paginationOptions.sortField = null;
		            } else {
		                _removeArray(sortColumns, "field", "Action");
		                _removeArray(sortColumns, "field", "Select");
		                if (sortColumns.length > 0 && sortColumns[0].field !== 'Action' && sortColumns[0].field !== 'Select') {
		                    vm.paginationOptions.sortField = sortColumns[sortColumns.length - 1].field;
		                    vm.paginationOptions.sortDirection = sortColumns[sortColumns.length - 1].sort.direction;		                    
		                }
		            }
		            getPage();
		        });
		        vm.gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
		            vm.paginationOptions.pageNumber = newPage;
		            vm.paginationOptions.pageSize = pageSize;
		            getPage();
		        });
		        vm.gridApi.selection.on.rowSelectionChanged($scope, function (rows) {
                    //untuk select one by one
		            $scope.selectedRows = vm.gridApi.selection.getSelectedRows();
		        });
		        vm.gridApi.selection.on.rowSelectionChangedBatch($scope, function (rows) {
                    //untuk selection header on click
		            $scope.selectedRows = vm.gridApi.selection.getSelectedRows();
		        });
		        vm.gridApi.core.on.filterChanged($scope, function () {
		            if (typeof $scope.gridData === "function") {
		                var grid = this.grid;
		                vm.paramColumn = [];
		                for (var i = 0; i < grid.columns.length; i++) {
		                    if (grid.columns[i].enableFiltering) {
		                        vm.paramColumn.push({ field: grid.columns[i].field, term: grid.columns[i].filters[0].term });
		                    }
		                }
		            }
                    getPage();
		        });
		        //vm.gridApi.edit.on.afterCellEdit($scope, function (rowEntity, colDef, newValue, oldValue) {
		        //    var result = { rowEntity: rowEntity, colName: colDef.name, newValue: newValue, oldValue: oldValue };
		        //    returnEditedCell(result);
		        //});
		          
		    },
		    rowTemplate: '<div ng-init="rowStyle=grid.appScope.getRowStyle(row.entity)" ng-dblclick="grid.appScope.tableActions.onDblClick(row.entity)" ng-click="grid.appScope.tableActions.onClick(row.entity)" ng-mouseover="rowStyle={\'background-color\': \'#dff2f7\'}; grid.appScope.onRowHover(this);" ng-mouseleave="rowStyle=grid.appScope.getRowStyle(row.entity)">' +                         '    <div ng-style="rowStyle" ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.uid" ui-grid-one-bind-id-grid="rowRenderIndex + \'-\' + col.uid + \'-cell\'"' +                         '         class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader }" role="{{col.isRowHeader ? \'rowheader\' : \'gridcell\'}}" ui-grid-cell>' +                         '    </div>' +                         '</div>'
		};	    	    

		function returnEditedCell(result) {
		    if (typeof $scope.onBlurEditedCell === "function") {
		        $scope.onBlurEditedCell(result);
		    }
		}

		function generateCsv(data) {
		    //http://stackoverflow.com/questions/34189494/angular-ui-grid-change-behavior-of-export-all-data-as-csv
		    var uiExporter = uiGridExporterService;
		    var grid = vm.gridApi.grid;
		    var exportColumnHeaders = uiExporter.getColumnHeaders(grid, uiGridExporterConstants.VISIBLE);
		    var selectionData = [];
		    _removeArray(exportColumnHeaders, "displayName", "Action");

		    if (exportColumnHeaders !== undefined && exportColumnHeaders.length > 0) {
		        data.forEach(function (entry) {
		            var innerData = [];
		            for (var i = 0; i < exportColumnHeaders.length; i++) {
		                var selectObj = { value: entry[exportColumnHeaders[i]["name"]] };
		                innerData.push(selectObj);
		            }
		            selectionData.push(innerData);
		        });
		        var csvContent = uiExporter.formatAsCsv(exportColumnHeaders, selectionData, grid.options.exporterCsvColumnSeparator);
		        uiExporter.downloadFile($scope.defaultCsvFilename, csvContent, grid.options.exporterOlderExcelCompatibility);
		    }
		}

        //jika ada function download all data maka tampilkan menu Export All Data as CSV
		if ($scope.gridDataAll) {
		    vm.gridOptions.gridMenuCustomItems.push(generateExportAllCsv());
		}

		function generateExportAllCsv() {
		    return {
		        title: 'Export All Data as CSV',
		        order: 0,
		        action: function ($event) {                              

		            if (typeof $scope.gridDataAll === "function")
		            {
		                $scope.gridDataAll(getParams())
                        .then(function (response) {
                            if (response !== undefined) {

                                generateCsv(response.data);                                          
                            }
                        });
		            }
		            else if ($scope.gridData instanceof Array)
		            {
		                generateCsv($scope.gridData);
		            }                                                                                                                                                             
		        }
		    }
		}

		vm.getSelectColumn = function () {
		    return { name: 'Select', cellTemplate: "<div align='center'><a href='' ng-click='grid.appScope.tableActions.onSelect(row.entity)'><i class='fa fa-edit fa-lg'></i></a></div>" };
		};

		function setColumns() {
		    if ($scope.columns) {
		        vm.gridOptions.columnDefs = angular.fromJson($scope.columns);
		        if ($scope.selectColumn) { //jika ada select column tambah column select di depan array
		            vm.gridOptions.columnDefs.unshift(vm.getSelectColumn());
		        }
		    }
		}
		setColumns();

		$scope.getRowStyle = function (entity) {
		    return {};
		};

		if ($scope.rowStyle) {
		    $scope.getRowStyle = $scope.rowStyle;
		}

		vm.loading = true;

		$scope.$watch('loading', function () {
		    vm.loading = $scope.loading;
		});

	    //https://github.com/angular-ui/ui-grid/issues/565
	    //http://plnkr.co/edit/96o8ZemeM1IG8CpaXwV2?p=preview
		$scope.charToPixelRatio = 10; //change this value if u change the font size
		$scope.autoColWidth = function (colDefs, rows) {
		    var totalChars = {};
		    for (var i = 0, n = rows.length; i < n; i++) {
		        var tempTotalChars = {};		        
		        for (var colName in rows[i]) {	    
		            if (rows[i][colName] === undefined || rows[i][colName] === null) {
		                tempTotalChars[colName] = 1;
		            } else {
		                if (rows[i][colName] instanceof Date) {
		                    tempTotalChars[colName] = 10; //jika date default 10 char
		                } else {
		                    tempTotalChars[colName] = rows[i][colName].toString().length;
		                }
		            }
		            if (i == 0 || tempTotalChars[colName] > totalChars[colName]) {
		                totalChars[colName] = tempTotalChars[colName]
		            }
		        }
		    }
		    for (var i = 0, n = colDefs.length; i < n; i++) {
		        //get string length column header
		        var colummHeaderLength = 0;
		        if (colDefs[i].name !== undefined) {
		            colummHeaderLength = colDefs[i].name.toString().length;
		        } else {
		            colummHeaderLength = colDefs[i].field.toString().length;
		        }
		        var columnName = colDefs[i].name;
		        var columnField = colDefs[i].field;
		        //if (colDefs[i].field !== undefined) {
		            if (colummHeaderLength >= totalChars[colDefs[i].field]) {
		                totalChars[colDefs[i].field] = colummHeaderLength + 1 ; 
		            } else {
		                totalChars[colDefs[i].field] = totalChars[colDefs[i].field];// + (totalChars[colDefs[i].field] - colummHeaderLength);
		            }

		            if (colDefs[i].width === undefined) {
		                if (colDefs[i].name === 'Select')
		                {
		                        colDefs[i].width = 70;
		                }
		                else if (colDefs[i].name === 'Action')
		                {
		                    colDefs[i].width = 80;
		                    if ($scope.ngDisabled) //jika ngDisabled
		                        colDefs[i].visible = false;
		                }
		                else
		                {
		                    colDefs[i].width = (totalChars[colDefs[i].field] * $scope.charToPixelRatio); //+ "px";  Error jika pake px : Cannot parse column width 'px' for column named '' 
		                }
		            }
		        //}
		    };
		    return colDefs;
		}
   
		$scope.$watch('gridData', function () {
		    $scope.refreshData();
		});

	    //http://stackoverflow.com/questions/17270109/how-to-refresh-ng-grid-when-griddata-has-different-number-of-columns-from-previo
		$scope.$watch('vm.gridOptions.data', function () {
		    if (!$scope.columns) {
		        vm.gridOptions.columnDefs = [];
		        
		            if ($scope.selectColumn) { //jika ada select column
		                vm.gridOptions.columnDefs.push(vm.getSelectColumn());
		            }		            

		            //HideColumns
		            var hideColumnArr = [];
		            if ($scope.hideColumns !== undefined && $scope.hideColumns !== null) {
		                hideColumnArr = $scope.hideColumns.split(",");
		            }		                    

		            //ShowColumns
		            var showColumnArr = [];
		            if ($scope.showColumns !== undefined && $scope.showColumns !== null) {
		                showColumnArr = $scope.showColumns.split(",");
		            }
                    
		            if (vm.gridOptions.data !== undefined && vm.gridOptions.data.length > 0) {
		                if (showColumnArr !== undefined && showColumnArr !== null && showColumnArr.length > 0) {
		                    angular.forEach(Object.keys(vm.gridOptions.data["0"]), function (key) {
		                        if (showColumnArr !== undefined && showColumnArr.indexOf(key) > -1) {
		                            //jika ada di showcolumn maka visible true
		                            vm.gridOptions.columnDefs.push({ field: key, visible: true });
		                        } else {
		                            vm.gridOptions.columnDefs.push({ field: key, visible: false });
		                        }
		                    });
		                } else {		                    
		                        angular.forEach(Object.keys(vm.gridOptions.data["0"]), function (key) {
		                            if (hideColumnArr !== undefined && hideColumnArr.indexOf(key) > -1) {
		                                //jika ada di hidecolumn maka visible false
		                                vm.gridOptions.columnDefs.push({ field: key, visible: false });
		                            } else {
		                                vm.gridOptions.columnDefs.push({ field: key });
		                            }
		                        });		                    
		                }
		            }
		    }
		    if (vm.gridOptions.data.length) {
		        vm.gridOptions.columnDefs = $scope.autoColWidth(vm.gridOptions.columnDefs, vm.gridOptions.data);
		        vm.gridOptions.enablePaginationControls = true;
		        vm.gridOptions.enableHorizontalScrollbar = 1; //ALWAYS
		    }

		});

		function getParams(){
		    var params = {
		        searchText: vm.filterOptions.filterText,
		        pageNumber: vm.paginationOptions.pageNumber,
		        pageSize: vm.paginationOptions.pageSize,
		        sortBy: vm.paginationOptions.sortField,
		        sortDirection: vm.paginationOptions.sortDirection,
		    };
		    for (var i = 0; i < vm.paramColumn.length; i++) {
		        params[vm.paramColumn[i].field] = vm.paramColumn[i].term;
		    }
		    if ($scope.searchParams) {
		        //params = Object.assign(params, $scope.searchParams); ga bisa di ie
		        _copyObject($scope.searchParams, params);
		    }
		    return params;
		}

		function loadDataArray(dataArr) {
		    vm.gridOptions.useExternalSorting = false;
		    var gridDataArr = [];
		    var firstRow = (vm.paginationOptions.pageNumber - 1) * vm.paginationOptions.pageSize;

		    if (vm.filterOptions.filterText !== undefined && vm.filterOptions.filterText !== '') { // jika search
		        var gridDataArrSearch = dataArr.inArray(vm.filterOptions.filterText);
		        $scope.totalItems = gridDataArrSearch.length;
		        vm.gridOptions.totalItems = gridDataArrSearch.length;
		        if (gridDataArrSearch !== undefined && gridDataArrSearch instanceof Array)
		            gridDataArr = gridDataArrSearch.slice(firstRow, firstRow + vm.paginationOptions.pageSize);
		    } else {
		        $scope.totalItems = dataArr.length;
		        vm.gridOptions.totalItems = dataArr.length;
		        gridDataArr = dataArr.slice(firstRow, firstRow + vm.paginationOptions.pageSize);
		    }

		    vm.gridOptions.data = gridDataArr;
		    if (typeof $scope.onLoad === "function") {
		        $scope.onLoad();
		    }
		}

		var getPage = function() {
		    if ($scope.init !== undefined && typeof $scope.init === "function") {
		        $scope.init()
                       .then(function (response) {
                           loadGrid();
                       });
		    } else {
		        loadGrid();
		    }
		    
		};

		var loadGrid = function () {

		    if (typeof $scope.gridData === "function") {
		        vm.gridOptions.useExternalSorting = true;
		        $scope.gridData(getParams())
                     .then(function (response) {
                         if (response.data !== undefined) {
                             $scope.response = response;
                             vm.gridOptions.totalItems = response.totalItems; // data.length;
                             $scope.totalItems = response.totalItems;
                             vm.gridOptions.data = response.data;

                             //tidak digunakan karena dilakukan saat grid data berubah
                             if (typeof $scope.onLoad === "function") {
                                 $scope.onLoad();
                             }
                         }
                     })
                     .catch(function (response) {
                         $scope.response = response;
                         MessageBox.show(response);
                     })
                    .finally(function () {
                        vm.loading = false;
                    });
		    }
		    else if ($scope.gridData instanceof Array) {
		        loadDataArray($scope.gridData);
		    }
		};

		$scope.refreshData = function () {
		    vm.gridOptions.paginationCurrentPage = 1; //jika searching set pagenumber mulai dari 1
		    vm.paginationOptions.pageNumber = 1; //jika searching set pagenumber mulai dari 1
		    getPage();
		};
	}

})();
