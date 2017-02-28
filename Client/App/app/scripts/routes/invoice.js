'use strict';

angular
  .module('client').config(function ($stateProvider, $urlRouterProvider) {
      $stateProvider
        .state('TicketCreate', {
            url: '/ticket',
            templateUrl: 'views/invoice/ticket.html',
            controller: 'TicketCtrl',
            resolve: {
                products: function (Product) {
                    return Product.active().$promise;
                }
            }
        })
        .state('InvoiceCreate', {
            url: '/invoice',
            templateUrl: 'views/invoice/invoice.html',
            controller: 'InvoiceCtrl',
            resolve: {
                products: function (Product) {
                    return Product.active().$promise;
                },
                customers: function (Customer) {
                    return Customer.query().$promise;
                }
            }
        })
        .state('InvoiceShow', {
            url: '/invoice/:transactionId',
            templateUrl: 'views/invoice/show.html',
            controller: 'InvoiceShowCtrl',
            resolve: {
                invoice: function (Transaction, $stateParams) {
                    return Transaction.get({ transactionId: $stateParams.transactionId }).$promise;
                }
            }
        })
        .state('InvoiceRefund', {
            url: '/invoice/:transactionId/refund',
            templateUrl: 'views/invoice/refund.html',
            controller: 'InvoiceRefundCtrl',
            resolve: {
                transaction: function (Transaction, $stateParams) {
                    return Transaction.get({ transactionId: $stateParams.transactionId }).$promise;
                }
            }
        });
  });
