'use strict';

angular
  .module('client').config(function ($stateProvider) {
      $stateProvider
        .state('ProductMeasureIndex', {
            url: '/product-measure/index',
            templateUrl: 'views/product-measure/index.html',
            controller: 'ProductMeasureIndexCtrl',
            resolve: {
                measures: function (ProductMeasure) {
                    return ProductMeasure.query().$promise;
                }
            }
        })
        .state('ProductMeasureCreate', {
            url: '/product-measure/create',
            templateUrl: 'views/product-measure/create.html',
            controller: 'ProductMeasureCreateCtrl'
        })
        .state('ProductMeasureUpdate', {
            url: '/product-measure/:id/update',
            templateUrl: 'views/product-measure/update.html',
            controller: 'ProductMeasureUpdateCtrl',
            resolve: {
                measure: function ($stateParams, ProductMeasure) {
                    return ProductMeasure.get({ id: $stateParams.id }).$promise;
                }
            }
        });
  });
