'use strict';

angular
  .module('client').config(function ($stateProvider, $urlRouterProvider) {
      $stateProvider
        .state('CustomerIndex', {
            url: '/customer/index',
            templateUrl: 'views/customer/index.html',
            controller: 'CustomerIndexCtrl',
            resolve: {
                customers: function (Customer) {
                    return Customer.query().$promise;
                }
            }
        })
        .state('CustomerCreate', {
            url: '/customer/create',
            templateUrl: 'views/customer/create.html',
            controller: 'CustomerCreateCtrl',
            resolve: {
                zones: function (Zone) {
                    return Zone.query().$promise;
                }
            }
        })
        .state('CustomerUpdate', {
          url: '/customer/:id/update',
            templateUrl: 'views/customer/update.html',
            controller: 'CustomerUpdateCtrl',
            resolve: {
                customer: function ($stateParams, Customer) {
                return Customer.get({ id: $stateParams.id }).$promise;
                },
                zones: function (Zone) {
                    return Zone.query().$promise;
                }
            }
        })
        .state('CustomerShow', {
          url: '/customer/:id/show',
            templateUrl: 'views/customer/show.html',
            controller: 'CustomerShowCtrl',
            resolve: {
                customer: function ($stateParams, Customer) {
                return Customer.get({ id: $stateParams.id }).$promise;
                },
                transactions: function (customer, Transaction) {
                  return Transaction.account({ id: customer.id }).$promise;
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
  });
