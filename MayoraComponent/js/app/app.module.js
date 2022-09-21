(function () {
    'use strict';
    window.app = angular
       .module('MyWeb', ['ngAnimate',
        'ui.router',
        'ui.bootstrap',
        'ui.grid',
        'ui.grid.pagination',
        'ui.grid.edit',
        'ui.grid.autoResize',
        'ui.grid.selection',
        'ui.grid.resizeColumns',
        'ui.grid.exporter',
        'ui.grid.cellNav',
        'ui.grid.autoFitColumns',
        'ui.tree',
        'ngMessages',
        'ng-bs3-datepicker',
        'timepickerPop',
        'number-input',
        'ngFileUpload',
        'fcsa-number',
        'angularModalService',
        'ngSanitize',
        'blockUI',
        'ui.tinymce',
        'packmultipleSelect'
       ]);
})();