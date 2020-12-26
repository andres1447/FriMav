'use strict';

angular.module('client').directive('clearOnBackspace', function ($timeout) {
  return {
    restrict: 'A',
    scope: {
      onClear: '&'
    },
    link: function(scope, element, attrs) {
      element.on('keydown', function(e) {
        var key = e.keyCode || e.charCode;
        if (key == 8) {
          element[0].value = '';
          $timeout(function () {
            scope.onClear();
          });
        }
      });
    }
  }
});
