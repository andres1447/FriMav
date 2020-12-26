'use strict';

angular.module('client').factory('Catalog', function ($resource, ApiConfig) {
  return $resource(ApiConfig.host + 'catalog/:id', { id: '@id' }, {
        update: {
            method: 'PUT'
        }
    });
});
