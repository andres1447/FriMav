'use strict';

angular.module('client')
  .directive('aeSelectOnClick', function () {
      return function (scope, elem, attr) {
          $(elem).click(function () {
              if (angular.isDefined(this.select)) {
                  this.select();
              }
          });
      };
  });