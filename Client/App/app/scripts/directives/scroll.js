'use strict';

angular.module('client')
  .directive('aeScroll', function ($timeout) {
    return function (scope, elem, attr) {
      scope.$on(attr.aeScroll, function (e) {
        $timeout(function () {
          elem[0].scrollIntoView({ behavior: "smooth" })
        });
      });
    };
  });
