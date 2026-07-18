'use strict';

angular
  .module('client').config(function ($stateProvider, $urlRouterProvider) {
      $stateProvider
        .state('TransferenceCreate', {
            url: '/transference/create',
            templateUrl: 'views/transference/create.html',
            controller: 'TransferenceCreateCtrl',
            resolve: {
                employees: function (Employee) {
                    return Employee.query().$promise;
                }
            }
        })
  });
