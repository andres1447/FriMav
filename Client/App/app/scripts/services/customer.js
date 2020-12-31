'use strict';

angular.module('client').factory('Customer', function ($resource, ApiConfig) {
  return $resource(ApiConfig.host + 'customer/:id', { id: '@id' }, {
        products: {
            method: 'GET',
            url: ApiConfig.host + 'customer/:id/products',
            isArray: true
        },
        update: {
            method: 'PUT'
        },
        codes: {
          url: ApiConfig.host + 'customer/codes',
          method: 'GET',
          isArray: true
        }
    });
});
