'use strict';

angular.module('client').factory('Absency', function ($resource, ApiConfig) {
  return $resource(ApiConfig.host + 'employee/absency/:id', { id: '@id' }, {
        update: {
            method: 'PUT'
        }
    });
});
