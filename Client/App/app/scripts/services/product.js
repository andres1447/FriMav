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
        }
    });
});
