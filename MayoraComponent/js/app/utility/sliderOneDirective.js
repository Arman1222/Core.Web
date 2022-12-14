(function () {
    'use strict';

    window.app.directive('ngSliderTwo', ['$timeout', function ($timeout) {
        return {
            restrict: "A",
            scope: {
                min: "=?",
                max: "=?",
                from: "=",
                to: "=",
                disable: "=?",
                values: "=?",

                type: "@",
                step: "@",
                minInterval: "@",
                maxInterval: "@",
                dragInterval: "@",
                fromFixed: "@",
                fromMin: "@",
                fromMax: "@",
                fromShadow: "@",
                toFixed: "@",
                toMax: "@",
                toShadow: "@",
                prettifyEnabled: "@",
                prettifySeparator: "@",
                forceEdges: "@",
                keyboard: "@",
                keyboardStep: "@",
                grid: "@",
                gridMargin: "@",
                gridNum: "@",
                gridSnap: "@",
                hideMinMax: "@",
                hideFromTo: "@",
                prefix: "@",
                postfix: "@",
                maxPostfix: "@",
                decorateBoth: "@",
                valuesSeparator: "@",
                inputValuesSeparator: "@",

                prettify: "&",
                onChange: "&",
                onFinish: "&",
            },
            replace: true,
            link: function ($scope, $element, attrs) {
                if (typeof $scope.min == 'undefined' || $scope.min === null) {
                    $scope.min = 0
                }
                if (typeof $scope.max == 'undefined' || $scope.max === null) {
                    $scope.max = 100
                }

                var configIon = {
                    min: $scope.min,
                    max: $scope.max,
                    from: $scope.from,
                    disable: $scope.disable,
                    step: $scope.step,
                    min_interval: $scope.minInterval,
                    max_interval: $scope.maxInterval,
                    drag_interval: $scope.dragInterval,
                    values: $scope.values,
                    from_fixed: $scope.fromFixed,
                    from_min: $scope.fromMin,
                    from_max: $scope.fromMax,
                    from_shadow: $scope.fromShadow,
                    to_fixed: $scope.toFixed,
                    to_max: $scope.toMax,
                    to_shadow: $scope.toShadow,
                    prettify_enabled: $scope.prettifyEnabled,
                    prettify_separator: $scope.prettifySeparator,
                    force_edges: $scope.forceEdges,
                    keyboard: $scope.keyboard,
                    keyboard_step: $scope.keyboardStep,
                    grid: $scope.grid,
                    grid_margin: $scope.gridMargin,
                    grid_num: $scope.gridNum,
                    grid_snap: $scope.gridSnap,
                    hide_min_max: $scope.hideMinMax,
                    hide_from_to: $scope.hideFromTo,
                    prefix: $scope.prefix,
                    postfix: $scope.postfix,
                    max_postfix: $scope.maxPostfix,
                    decorate_both: $scope.decorateBoth,
                    values_separator: $scope.valuesSeparator,
                    input_values_separator: $scope.inputValuesSeparator,
                    prettify: function (value) {
                        if (!attrs.prettify) {
                            return value;
                        }
                        return $scope.prettify({
                            value: value
                        });
                    },
                    onChange: function (a) {
                        $scope.$apply(function () {
                            //$scope.caretPosition = true;
                            if (typeof $scope.from != 'undefined') $scope.from = a.from;
                            if (typeof $scope.to != 'undefined') $scope.to = a.to;
                            $scope.onChange && $scope.onChange({
                                a: a
                            });
                        });
                    },
                    onFinish: function () {
                        $timeout(function () {
                            $scope.$apply($scope.onFinish); 
                        });
                    },
                };

                if ($scope.to != null) configIon.to = $scope.to;
                if ($scope.type != null && $scope.type != 'text') configIon.type = $scope.type;

                $element.ionRangeSlider(configIon);
                var watchers = [];
                watchers.push($scope.$watch("min", function (value) {
                    $element.data("ionRangeSlider").update({
                        min: value
                    });
                }));
                watchers.push($scope.$watch('max', function (value) {
                    $element.data("ionRangeSlider").update({
                        max: value
                    });
                }));
                watchers.push($scope.$watch('from', function (value) {
                    var slider = $element.data("ionRangeSlider");
                    if (slider.old_from !== value) {
                        slider.update({
                            from: value
                        });
                    }
                }));
                watchers.push($scope.$watch('to', function (value) {
                    var slider = $element.data("ionRangeSlider");
                    if (slider.old_to !== value) {
                        slider.update({
                            to: value
                        });
                    }
                }));
                watchers.push($scope.$watch('disable', function (value) {
                    $element.data("ionRangeSlider").update({
                        disable: value
                    });
                }));
            }
        };

    }]);

})();