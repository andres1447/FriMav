'use strict';

angular
  .module('client').config(function ($stateProvider, $urlRouterProvider) {
      $stateProvider
        .state('DeliveryIndex', {
            url: '/delivery',
            templateUrl: 'views/delivery/index.html',
            controller: 'DeliveryIndexCtrl',
            resolve: {
                deliveries: function (Delivery) {
                    return Delivery.query().$promise;
                }
            }
        })
        .state('DeliveryCreate', {
            url: '/delivery/create',
            templateUrl: 'views/delivery/create.html',
            controller: 'DeliveryCreateCtrl',
            resolve: {
                employees: function (Employee) {
                    return Employee.query().$promise;
                },
                invoices: function (Invoice) {
                    return Invoice.undelivered().$promise;
                }
            }
        });
  });
