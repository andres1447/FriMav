'use strict';

angular.module('client')
  .directive('aeAutofocus', function ($timeout) {
      return function (scope, elem, attr) {
        scope.$on(attr.aeAutofocus, function (e) {
          $timeout(function () {
            elem[0].focus();
          });
        });
      };
  });
