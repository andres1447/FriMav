'use strict';

angular
  .module('client').config(function ($stateProvider, $urlRouterProvider) {
      $stateProvider
        .state('BonusCreate', {
            url: '/bonus/create',
            templateUrl: 'views/bonus/create.html',
            controller: 'BonusCreateCtrl',
            resolve: {
                employees: function (Employee) {
                    return Employee.query().$promise;
                }
            }
        })
  });
