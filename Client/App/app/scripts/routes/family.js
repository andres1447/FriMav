'use strict';

angular
  .module('client').config(function ($stateProvider, $urlRouterProvider) {
      $stateProvider
        .state('FamilyIndex', {
            url: '/family/index',
            templateUrl: 'views/family/index.html',
            controller: 'FamilyIndexCtrl',
            resolve: {
                families: function (ProductFamily) {
                    return ProductFamily.query().$promise;
                }
            }
        })
        .state('FamilyCreate', {
            url: '/family/create',
            templateUrl: 'views/family/create.html',
            controller: 'FamilyCreateCtrl'
        })
        .state('FamilyShow', {
            url: '/family/:id',
            templateUrl: 'views/family/show.html',
            controller: 'FamilyShowCtrl',
            resolve: {
                family: function (ProductFamily, $stateParams) {
                    return ProductFamily.get({ id: $stateParams.id }).$promise;
                },
                products: function (ProductFamily, $stateParams) {
                    return ProductFamily.products({ id: $stateParams.id }).$promise;
                }
            }
        })
        .state('FamilyUpdate', {
            url: '/family/:id/update',
            templateUrl: 'views/family/update.html',
            controller: 'FamilyUpdateCtrl',
            resolve: {
                family: function ($stateParams, ProductFamily) {
                    return ProductFamily.get({ id: $stateParams.id }).$promise;
                }
            }
        });
  });
