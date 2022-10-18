'use strict';

angular
  .module('client').config(function ($stateProvider, $urlRouterProvider) {
      $stateProvider
        .state('DeliveryIndex', {
            url: '/delivery',
            templateUrl: 'views/delivery/index.html',
            controller: 'DeliveryIndexCtrl',
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
        })
        .state('DeliveryShow', {
          url: '/delivery/:id',
            templateUrl: 'views/delivery/show.html',
            controller: 'DeliveryShowCtrl',
            resolve: {
                delivery: function (Delivery, $stateParams) {
                return Delivery.get({ id: $stateParams.id }).$promise;
                }
            }
        });
  });
