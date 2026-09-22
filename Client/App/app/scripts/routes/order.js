'use strict';

angular
  .module('client').config(function ($stateProvider) {
      $stateProvider
        .state('OrderCreate', {
            url: '/order',
            templateUrl: 'views/order/order.html',
            controller: 'OrderCtrl',
            resolve: {
                products: function (Product) {
                    return Product.active().$promise;
                },
                customers: function (Customer) {
                    return Customer.query().$promise;
                }
            }
        });
  });
