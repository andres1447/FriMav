'use strict';

angular
  .module('client').config(function ($stateProvider, $urlRouterProvider) {
      $stateProvider
        .state('AbsencyCreate', {
            url: '/absency/create',
            templateUrl: 'views/absency/create.html',
            controller: 'AbsencyCreateCtrl',
            resolve: {
                employees: function (Employee) {
                    return Employee.query().$promise;
                }
            }
        })
  });
