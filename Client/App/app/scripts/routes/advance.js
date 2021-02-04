'use strict';

angular
  .module('client').config(function ($stateProvider, $urlRouterProvider) {
      $stateProvider
        .state('AdvanceCreate', {
            url: '/advance/create',
            templateUrl: 'views/advance/create.html',
            controller: 'AdvanceCreateCtrl',
            resolve: {
                employees: function (Employee) {
                    return Employee.query().$promise;
                }
            }
        })
  });
