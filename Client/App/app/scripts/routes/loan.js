'use strict';

angular
  .module('client').config(function ($stateProvider, $urlRouterProvider) {
      $stateProvider
        .state('LoanCreate', {
            url: '/loan/create',
            templateUrl: 'views/loan/create.html',
            controller: 'LoanCreateCtrl',
            resolve: {
                employees: function (Employee) {
                    return Employee.query().$promise;
                }
            }
        })
        .state('LoanShow', {
          url: '/loan/:id/show',
          templateUrl: 'views/loan/show.html',
          controller: 'LoanShowCtrl',
          resolve: {
            loan: function (Loan, $stateParams) {
              return Loan.get({ id: $stateParams.id }).$promise;
            }
          }
        })
  });
