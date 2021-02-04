'use strict';

angular
  .module('client').config(function ($stateProvider, $urlRouterProvider) {
    $stateProvider
      .state('Payroll', {
        url: '/employee/payroll',
        templateUrl: 'views/employee/payroll.html',
        controller: 'PayrollCtrl',
        resolve: {
          payrolls: function (Payroll) {
            return Payroll.query().$promise;
          }
        }
      });
  });
