'use strict';

angular
  .module('client').config(function ($stateProvider) {
    $stateProvider
      .state('ConfigValuesUpdate', {
        url: '/parameters/index',
        templateUrl: 'views/configValues/update.html',
        controller: 'ConfigValuesUpdateCtrl',
        resolve: {
          configValues: function (ConfigValues) {
            return ConfigValues.query().$promise;
          }
        }
      });
  });
