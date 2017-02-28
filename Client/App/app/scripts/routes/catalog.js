'use strict';

angular
  .module('client').config(function ($stateProvider, $urlRouterProvider) {
      $stateProvider
        .state('CatalogCreate', {
            url: '/catalog/create',
            templateUrl: 'views/catalog/create.html',
            controller: 'CatalogCreateCtrl',
            resolve: {
                products: function (Product) {
                    return Product.active().$promise;
                }
            }
        })
        .state('CatalogIndex', {
            url: '/catalog',
            templateUrl: 'views/catalog/index.html',
            controller: 'CatalogIndexCtrl',
            resolve: {
                catalogs: function (Catalog) {
                    return Catalog.query().$promise;
                }
            }
        })
        .state('CatalogShow', {
            url: '/catalog/:catalogId',
            templateUrl: 'views/catalog/show.html',
            controller: 'CatalogShowCtrl',
            resolve: {
                catalog: function (Catalog, $stateParams) {
                    return Catalog.get({ catalogId: $stateParams.catalogId }).$promise;
                }
            }
        });
  });
