'use strict';

angular.module('client')
  .directive('aeAutofocus', function ($timeout) {
      return function (scope, elem, attr) {
        scope.$on(attr.aeAutofocus, function (e) {
          $timeout(function () {
            var $elem = $(elem);
            if (!$elem.is('input[type=text],textarea,select')) {
              var first = $elem.find('input[type=text],textarea,select').first();
              first.focus();
            }
            else
              $elem.focus();
          });
        });
      };
  });
