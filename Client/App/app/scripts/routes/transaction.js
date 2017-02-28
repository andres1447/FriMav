'use strict';

angular
  .module('client').config(function ($stateProvider, $urlRouterProvider) {
      $stateProvider
        .state('TransactionRefund', {
            url: '/transaction/:transactionId/refund',
            templateUrl: 'views/transaction/refund.html',
            controller: 'TransactionRefundCtrl',
            resolve: {
                transaction: function (Transaction, $stateParams) {
                    return Transaction.get({ transactionId: $stateParams.transactionId }).$promise;
                }
            }
        });
  });
