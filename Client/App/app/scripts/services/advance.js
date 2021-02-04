'use strict';

angular.module('client').factory('Advance', function ($resource, ApiConfig) {
  return $resource(ApiConfig.host + 'employee/advance/:id', { id: '@id' }, {
        update: {
            method: 'PUT'
        }
    });
});
