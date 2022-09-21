(function() {
	'use strict';

	window.app.controller('noteandtaskController', ApplicationUsersController);

	ApplicationUsersController.$inject = ['$scope', '$modal', 'noteandtaskService', 'alerts', '$location'];
	function ApplicationUsersController($scope, $modal, noteandtaskService, alerts, $location) {
	    var vm = this;
	    vm.refreshNote = function () {
	        noteandtaskService.getPageNote().then(function (data) {
	            vm.allmynote = data.data;
	        });
	    }
	    vm.refreshNote();
	    vm.sliderVal = 10;
	    vm.addSlide = function () {
	        vm.sliderVal += 10;
	    }
	    vm.allmytask = noteandtaskService.getPageTask;
	    vm.addnote = addNote;
	    vm.addtask = addTask;
	    vm.removeNote = function (data) {
	        if (confirm('Are you sure you want to delete this thing from database . ?')) {
	            noteandtaskService.deleteNote(data)
				.then(function () {
				    //Close the modal
				    alert("Delete Succes");
				})
				.catch(function (response) {
				    if (response.data.errorMessage) {
				        vm.message.error("There was a problem saving the issue: <br/>" + response.data.errorMessage + "<br/>Please try again.");
				    } else {
				        vm.message.error("There was a problem saving the issue. Please try again.");
				    }
				})
				.finally(function () {
				    vm.refreshNote();
				});
	        }
	    }
	    vm.editNote = editNote;
	    vm.detailNote = detailNote;
	    function addNote() {
	        var modalInstance = $modal.open({
	            template: '<add-mynote />'
	        });
	        modalInstance.result.then(function () {
	            vm.refreshNote();
	        }, function () {
	            //$log.info('Modal dismissed at: ' + new Date());
	        });
	    }
	    function editNote(data) {
	        var modalInstance = $modal.open({
	            //windowClass: 'form-modal-window-1200',
	            template: '<edit-mynote data="data" />',
	            scope: angular.extend($scope.$new(true), { data: data })
	        });

	        modalInstance.result.then(function () {
	            vm.refreshNote();
	        }, function () {
	            //$log.info('Modal dismissed at: ' + new Date());
	        });
	    }
	    function detailNote(data) {
	        $modal.open({
	            windowClass: 'form-modal-window-1200',
	            template: '<details-mynote data="data" />',
	            scope: angular.extend($scope.$new(true), { data: data })
	        });
	    }
	    vm.tableActionTasks = {
	        onEdit: editTask,
	        onDetail: detailTask,
            onDelete: deleteTask
	    };

	    function deleteTask(data) {
	        if (confirm('Are you sure you want to delete this thing from database . ?')) {
	            noteandtaskService.deleteTask(data)
				.then(function () {
				    //Close the modal
				    alert("Delete Succes");
				})
				.catch(function (response) {
				    if (response.data.errorMessage) {
				        vm.message.error("There was a problem saving the issue: <br/>" + response.data.errorMessage + "<br/>Please try again.");
				    } else {
				        vm.message.error("There was a problem saving the issue. Please try again.");
				    }
				})
				.finally(function () {
				    vm.refreshTask();
				});
	        }
	    }

	    function addTask() {
	        var modalInstance = $modal.open({
	            windowClass: 'form-modal-window-1200',
	            template: '<add-mytask />'
	        });

	        modalInstance.result.then(function () {
	            vm.refreshTask();
	        }, function () {
	            //$log.info('Modal dismissed at: ' + new Date());
	        });
	    }

	    function editTask(data) {
	        var modalInstance = $modal.open({
	            windowClass: 'form-modal-window-1200',
	            template: '<edit-mytask data="data" />',
	            scope: angular.extend($scope.$new(true), { data: data })
	        });

	        modalInstance.result.then(function () {
	            vm.refreshTask();
	        }, function () {
	            //$log.info('Modal dismissed at: ' + new Date());
	        });
	    }

	    function detailTask(data) {
	        $modal.open({
	            windowClass: 'form-modal-window-1200',
	            template: '<details-mytask data="data" />',
	            scope: angular.extend($scope.$new(true), { data: data })
	        });
	    }
	}
})();