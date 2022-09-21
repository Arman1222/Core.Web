(function() {
	'use strict';

	window.app.controller('CustomerController', CustomerController);

	CustomerController.$inject = ['$scope','$modal', 'customerService','alerts','customertemenosService'];
	function CustomerController($scope, $modal, customerService, alerts, customertemenosService) {
		var vm = this;
		vm.add = add;
		vm.detail = detail;
		vm.edit = edit;
		vm.customersFunction = customerService.getPage;
		vm.customersGetAll = customerService.getAll;
		vm.customerstemenosGetPage = customertemenosService.getPage;
		vm.message = alerts;
		vm.loading = true;

		vm.onBlurEditedCell = function (val) {
		    alert("merubah : " + val.oldValue + " menjadi " + val.newValue + " pada Column " + val.colName);
		    console.log(val);
		}
		//throw new Error("test");
		vm.exportWord = function () {
		    customerService.exportWord()
                .then(function (response) {
                    var file = _base64toBlob(response.data, response.contentType); //new Blob(response.data, { type: response.contentType });
                    var fileURL = URL.createObjectURL(file);
                    window.open(fileURL);                    
                });
		}

		vm.exportPdf = function () {
		    customerService.exportPdf()
                .then(function (response) {
                    var file = _base64toBlob(response.data, response.contentType); //new Blob(response.data, { type: response.contentType });
                    var fileURL = URL.createObjectURL(file);
                    window.open(fileURL);
                });
		}

		vm.rowStyle = function (entity) {
		    if (entity && entity.customerType === 'Corporate') {
		        return { background: 'green' };
		    }
		    return {};
		}
	   
		vm.onLoadTableCustomer =
            function () {
                return new Promise(function (resolve, reject) {
                    var a = 10
                    if (a == 10)
                        resolve(a);
                    else
                        reject(a);
                })
                .then(function (e) {
                    console.log('done', e);
                })
                .catch(function (e) {
                    console.log('catch: ', e);
                });
            };          


		$scope.$watch('vm.totalItems', function (value) {
		    if (value) {
		        console.log(value);
		        var totalItems = vm.totalItems;
		    }
		});

		$scope.$watch('vm.response', function (value) {
		    if (value) {
		        console.log(value);
		        var response = vm.response;
		    }
		});

		vm.tableActions = {
		    onEdit: edit,
		    onDetail: detail,
		    onDelete: function (value) {
		        alert('Delete : ' + value);
		    },
		    onClick: function (value) {
		        //alert('Click : ' + value);
		    },
		    onDblClick: function (value) {
		        //alert('DblClick : ' + value);
		    }
		};

		function add() {
		    var modalInstance = $modal.open({
		        windowClass: 'form-modal-window-1200',
				template: '<add-customer />'
		    });

		    modalInstance.result.then(function () {
		        vm.refreshData();
		    }, function () {
		        //$log.info('Modal dismissed at: ' + new Date());
		    });
		}

		function edit(data) {
		    var modalInstance = $modal.open({
		        template: '<edit-customer data="data" />',
		        scope: angular.extend($scope.$new(true), { data: data })
		    });

		    modalInstance.result.then(function () {
		        vm.refreshData();
		    }, function () {
		        //$log.info('Modal dismissed at: ' + new Date());
		    });
		}

		function detail(data) {
		    $modal.open({
		        template: '<details-customer data="data" />',
		        scope: angular.extend($scope.$new(true), { data: data })
		    });
		}
	}
})();