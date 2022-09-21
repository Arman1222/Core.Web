(function() {
	'use strict';

	window.app.controller('ApplicationUsersController', ApplicationUsersController);

	ApplicationUsersController.$inject = ['$scope', '$modal', 'applicationusersService', 'alerts', '$location'];
	function ApplicationUsersController($scope, $modal, applicationusersService, alerts, $location) {
	    var vm = this;
	    var uriPhoto = 'http://10.100.2.29/MyPeople';
		//vm.add = add;
		//vm.detail = detail;
		vm.edit = edit;        
		vm.applicationusersGetPage = applicationusersService.getPage;
		vm.applicationusersGetEmployee = applicationusersService.getEmployee;
		vm.applicationusersGetDepartment = applicationusersService.getDepartment;
		vm.applicationusersGetArea = applicationusersService.getArea;
		vm.applicationusersGetBranch = applicationusersService.getBranch;
		vm.message = alerts;

		vm.applicationusersGetEmployee()
                        .then(function (data) {
                            vm.employee = data;
                            vm.employee.photo = vm.employee.photo.replace("~", uriPhoto);
                        });
	    vm.applicationusersGetDepartment()
                        .then(function (data) {
                            vm.department = data;
                        });
	    vm.applicationusersGetArea()
                        .then(function (data) {
                            vm.area = data;
                        });
	    vm.applicationusersGetBranch()
                       .then(function (data) {
                           vm.branch = data;
                       });

		vm.tableActions = {
		    onEdit: edit,
		};
		vm.addnote = addNote;
		function addNote() {
		    var modalInstance = $modal.open({
		        template: '<add-mynote />'
		    });
		    modalInstance.result.then(function () {
		        //vm.refreshNote();
		    }, function () {
		        //$log.info('Modal dismissed at: ' + new Date());
		    });
		}
		vm.changeUseRole = function (item) {
		    if (item.used) {
		        applicationusersService.userRoleAdd({userId: vm.selectedUser.id, roleId: item.id}).then( function (result) {
		            vm.refreshListRole();
		        } );
		    } else {
		        applicationusersService.userRoleDelete({ userId: vm.selectedUser.id, roleId: item.id }).then(function (result) {
		            vm.refreshListRole();
		        });
		    }
		};
        
		vm.refreshListRole = function () {
		    applicationusersService.userRoleGetAll({ userId: vm.selectedUser.id }).then(function (result) {
		        vm.listRole = result.data;
		    });
		}


		function edit(data) {
		    vm.selectedUser = data;
		    vm.refreshListRole();
		    //window.location = MyApplication.rootPath + "ApplicationUsers/Edit/" + data.id;
		    //var modalInstance = $modal.open({
		    //    windowClass: 'form-modal-window-1200',
		    //    template: '<edit-usersapp data="data" />',
		    //    scope: angular.extend($scope.$new(true), { data: data })
		    //});

		    //modalInstance.result.then(function () {
		    //    vm.refreshNote();
		    //}, function () {
		    //    //$log.info('Modal dismissed at: ' + new Date());
		    //});
		    //$window.location()
		    //if (!$scope.$$phase) $scope.$apply();
		}

	}
})();