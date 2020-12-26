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
          url: '/invoice/:id',
            templateUrl: 'views/invoice/show.html',
            controller: 'InvoiceShowCtrl',
            resolve: {
                invoice: function (Invoice, $stateParams) {
                  return Invoice.get({ id: $stateParams.id }).$promise;
                }
            }
        })
        .state('InvoiceRefund', {
          url: '/invoice/:id/refund',
            templateUrl: 'views/invoice/refund.html',
            controller: 'InvoiceRefundCtrl',
            resolve: {
                transaction: function (Transaction, $stateParams) {
                return Transaction.get({ id: $stateParams.id }).$promise;
                }
            }
        });
  });
