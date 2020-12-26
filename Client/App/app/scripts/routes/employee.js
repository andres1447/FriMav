'use strict';

angular
  .module('client').config(function ($stateProvider, $urlRouterProvider) {
      $stateProvider
        .state('EmployeeIndex', {
            url: '/employee/index',
            templateUrl: 'views/employee/index.html',
            controller: 'EmployeeIndexCtrl',
            resolve: {
                employees: function (Employee) {
                    return Employee.query().$promise;
                }
            }
        })
        .state('EmployeeCreate', {
            url: '/employee/create',
            templateUrl: 'views/employee/create.html',
            controller: 'EmployeeCreateCtrl'
        })
        .state('EmployeeUpdate', {
          url: '/employee/:id/update',
            templateUrl: 'views/employee/update.html',
            controller: 'EmployeeUpdateCtrl',
            resolve: {
                employee: function ($stateParams, Employee) {
                return Employee.get({ id: $stateParams.id }).$promise;
                }
            }
        })
        .state('EmployeeShow', {
          url: '/employee/:id/show',
            templateUrl: 'views/employee/show.html',
            controller: 'EmployeeShowCtrl',
            resolve: {
                employee: function ($stateParams, Employee) {
                return Employee.get({ id: $stateParams.id }).$promise;
                },
                accountEntries: function (customer, AccountEntry) {
                  return AccountEntry.query({ id: customer.id }).$promise;
                }
            }
        })
  });
