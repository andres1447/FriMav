'use strict';

angular.module('client')
  .directive('formGroup', function ($timeout) {
    return {
      template: '<div class="form-group" ng-class="{ \'has-error\': hasError }"><label class="control-label" for="{{for}}">{{ label }}</label><div ng-transclude=""></div></div>',
      restrict: 'E',
      replace: true,
      transclude: true,
      require: "^form",
      scope: {
        label: "@",
        input: '='
      },
      link: function (scope, elem, attrs, ctrl) {
        $timeout(function () {
          if (!hasValue(scope.input)) return;

          var id = scope.input.$$attr.id;
          var watchBar = attrs.input + '.$viewValue';
          scope.for = id;
          scope.hasError = scope.input.$invalid;
          scope.$parent.$watch(watchBar, function (hasError) {
            scope.hasError = scope.input.$invalid && scope.input.$dirty;
          });
        })
      }
    };
  });
