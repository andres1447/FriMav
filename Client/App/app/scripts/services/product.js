'use strict';

angular.module('client').factory('Product', function ($resource, ApiConfig) {
  return $resource(ApiConfig.host + 'product/:id', { id: '@id' }, {
        update: {
            method: 'PUT'
        },
        active: {
            url: ApiConfig.host + 'product/active',
            method: 'GET',
            isArray: true
        },
        codes: {
          url: ApiConfig.host + 'product/codes',
          method: 'GET',
          isArray: true
        },
        pricelist: {
          url: ApiConfig.host + 'product/pricelist',
          method: 'GET',
          isArray: true
        }
    });
});
