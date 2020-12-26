'use strict';

angular.module('client').factory('Zone', function ($resource, ApiConfig) {
  return $resource(ApiConfig.host + 'zone/:zoneId', { id: '@id' }, {
        update: {
            method: 'PUT'
        },
        customers: {
          url: ApiConfig.host + 'zone/:id/customers',
            method: 'GET',
            isArray: true
        }
    });
});
