'use strict';

angular
  .module('client').config(function ($stateProvider, $urlRouterProvider) {
      $stateProvider
        .state('PaymentCreate', {
          url: '/payment',
            templateUrl: 'views/payment/create.html',
            controller: 'PaymentCreateCtrl',
            resolve: {
                customers: function (Customer) {
                  return Customer.query().$promise;
                }
            }
        })
        .state('CustomerPaymentCreate', {
          url: '/customer/:id/payment',
          templateUrl: 'views/payment/customer-payment-create.html',
          controller: 'CustomerPaymentCreateCtrl',
          resolve: {
            customer: function ($stateParams, Customer) {
              return Customer.get({ id: $stateParams.id }).$promise;
            }
          }
        })
        .state('PaymentCancel', {
          url: '/payment/:id/cancel',
            templateUrl: 'views/payment/cancel.html',
            controller: 'PaymentCancelCtrl',
            resolve: {
                transaction: function ($stateParams, Transaction) {
                return Transaction.get({ id: $stateParams.id }).$promise;
                }
            }
        });
  });
