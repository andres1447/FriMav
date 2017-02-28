'use strict';

angular
  .module('client').config(function ($stateProvider, $urlRouterProvider) {
      $stateProvider
        .state('PaymentCreate', {
            url: '/payment/create?personId',
            templateUrl: 'views/payment/create.html',
            controller: 'PaymentCreateCtrl',
            resolve: {
                customer: function ($stateParams, Customer) {
                    return Customer.get({ personId: $stateParams.personId }).$promise;
                }
            }
        })
        .state('PaymentCancel', {
            url: '/payment/:transactionId/cancel',
            templateUrl: 'views/payment/cancel.html',
            controller: 'PaymentCancelCtrl',
            resolve: {
                transaction: function ($stateParams, Transaction) {
                    return Transaction.get({ transactionId: $stateParams.transactionId }).$promise;
                }
            }
        });
  });
