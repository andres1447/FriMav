'use strict';

angular.module('client').directive('blacklist', function () {
  return {
    require: 'ngModel',
    scope: {
      blacklist: '='
    },
    link: function (scope, elem, attrs, ngModel) {
      var blacklist = scope.blacklist;

      //For DOM -> model validation
      ngModel.$parsers.unshift(function (value) {
        var valid = blacklist.indexOf(value) === -1;
        ngModel.$setValidity('blacklist', valid);
        return valid ? value : undefined;
      });

      //For model -> DOM validation
      ngModel.$formatters.unshift(function (value) {
        ngModel.$setValidity('blacklist', blacklist.indexOf(value) === -1);
        return value;
      });
    }
  };
});
