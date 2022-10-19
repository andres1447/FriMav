'use strict';

angular
  .module('client').config(function ($stateProvider, $urlRouterProvider) {
      $stateProvider
        .state('VacationCreate', {
            url: '/vacation/create',
            templateUrl: 'views/vacation/create.html',
            controller: 'VacationCreateCtrl',
            resolve: {
                employees: function (Employee) {
                    return Employee.query().$promise;
                }
            }
        })
  });
