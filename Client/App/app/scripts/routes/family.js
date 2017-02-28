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
        .state('FamilyShow', {
            url: '/family/:familyId',
            templateUrl: 'views/family/show.html',
            controller: 'FamilyShowCtrl',
            resolve: {
                family: function (ProductFamily, $stateParams) {
                    return ProductFamily.get({ familyId: $stateParams.familyId }).$promise;
                },
                products: function (ProductFamily, $stateParams) {
                    return ProductFamily.products({ familyId: $stateParams.familyId }).$promise;
                }
            }
        })
        .state('FamilyCreate', {
            url: '/family/create',
            templateUrl: 'views/family/create.html',
            controller: 'FamilyCreateCtrl'
        })
        .state('FamilyUpdate', {
            url: '/family/:familyId/update',
            templateUrl: 'views/family/update.html',
            controller: 'FamilyUpdateCtrl',
            resolve: {
                family: function ($stateParams, ProductFamily) {
                    return ProductFamily.get({ familyId: $stateParams.familyId }).$promise;
                }
            }
        });
  });
