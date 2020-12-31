'use strict';

angular.module('client').factory('Employee', function ($resource, ApiConfig) {
  return $resource(ApiConfig.host + 'employee/:id', { id: '@id' }, {
        update: {
            method: 'PUT'
        },
        codes: {
          url: ApiConfig.host + 'employee/codes',
          method: 'GET',
          isArray: true
        }
    });
});
