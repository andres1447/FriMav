'use strict';

angular
  .module('client').config(function ($stateProvider, $urlRouterProvider) {
      $stateProvider
        .state('GoodsSoldCreate', {
            url: '/goods/create',
            templateUrl: 'views/goods/create.html',
            controller: 'GoodsSoldCreateCtrl',
            resolve: {
                employees: function (Employee) {
                    return Employee.query().$promise;
                },
                products: function (Product) {
                  return Product.active().$promise;
                }
            }
        })
  });
