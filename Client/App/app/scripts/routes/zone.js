'use strict';

angular
  .module('client').config(function ($stateProvider, $urlRouterProvider) {
      $stateProvider
        .state('ZoneIndex', {
            url: '/zone/index',
            templateUrl: 'views/zone/index.html',
            controller: 'ZoneIndexCtrl',
            resolve: {
                zones: function (Zone) {
                    return Zone.query().$promise;
                }
            }
        })
        .state('ZoneCreate', {
            url: '/zone/create',
            templateUrl: 'views/zone/create.html',
            controller: 'ZoneCreateCtrl'
        })
        .state('ZoneUpdate', {
            url: '/zone/:zoneId/update',
            templateUrl: 'views/zone/update.html',
            controller: 'ZoneUpdateCtrl',
            resolve: {
                zone: function ($stateParams, Zone) {
                    return Zone.get({ zoneId: $stateParams.zoneId }).$promise;
                }
            }
        })
        .state('ZoneShow', {
            url: '/zone/:zoneId',
            templateUrl: 'views/zone/show.html',
            controller: 'ZoneShowCtrl',
            resolve: {
                zone: function (Zone, $stateParams) {
                    return Zone.get({ zoneId: $stateParams.zoneId }).$promise;
                },
                customers: function (Zone, $stateParams) {
                    return Zone.customers({ zoneId: $stateParams.zoneId }).$promise;
                }
            }
        });
  });
