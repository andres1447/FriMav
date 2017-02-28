'use strict';

angular
  .module('client').config(function ($stateProvider) {
      $stateProvider
        .state('PrintSettings', {
            url: '/settings/print',
            templateUrl: 'views/settings/print.html',
            controller: 'PrintSettingsCtrl'
        });
  });
