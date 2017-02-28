'use strict';

angular.module('client')
  .directive('aeAutofocus', function () {
      return function (scope, elem, attr) {
        scope.$on(attr.aeAutofocus, function (e) {
            elem[0].focus();
        });
      };
  });