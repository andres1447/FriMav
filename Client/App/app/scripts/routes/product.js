'use strict';

angular
  .module('client').config(function ($stateProvider, $urlRouterProvider) {
      $stateProvider
        .state('ProductIndex', {
            url: '/product/index',
            templateUrl: 'views/product/index.html',
            controller: 'ProductIndexCtrl',
            resolve: {
                products: function (Product) {
                    return Product.query().$promise;
                }
            }
        })
        .state('ProductCreate', {
            url: '/product/create',
            templateUrl: 'views/product/create.html',
            controller: 'ProductCreateCtrl',
            resolve: {
                productFamilies: function (ProductFamily) {
                    return ProductFamily.query().$promise;
                }
            }
        })
        .state('ProductUpdate', {
            url: '/product/:productId/update',
            templateUrl: 'views/product/update.html',
            controller: 'ProductUpdateCtrl',
            resolve: {
                productFamilies: function (ProductFamily) {
                    return ProductFamily.query().$promise;
                },
                product: function ($stateParams, Product) {
                    return Product.get({ productId: $stateParams.productId }).$promise;
                }
            }
        });
  });
