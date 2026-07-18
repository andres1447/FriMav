'use strict';

angular.module('client').factory('Transference', function ($resource, ApiConfig) {
  return $resource(ApiConfig.host + 'employee/transference/:id', { id: '@id' }, {
        update: {
            method: 'PUT'
        }
    });
});
