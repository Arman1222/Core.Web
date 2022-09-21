(function() {
	'use strict';

	window.app.controller('ImageClassController', ImageClassController);

	ImageClassController.$inject = ['$modal', 'imageclassService','alerts'];
	function ImageClassController($modal, imageclassService, alerts) {
		var vm = this;
		vm.add = add;
        
	    imageclassService
            .loadAll()
            .then(function (data) {
                vm.imageclasses = imageclassService.list;
            })
            .catch(function (response) {
                alerts.error(response.errorMessage);
            })
            .finally(function () {
                vm.loading = false;
            });

	    vm.imageclassLoadAll = imageclassService.loadAll;

		vm.message = alerts;

		function add() {
			$modal.open({
			    template: '<add-imageclass />'
			});
		}
	}
})();