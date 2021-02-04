'use strict';

angular.module('client').factory('Loan', function ($resource, ApiConfig) {
  return $resource(ApiConfig.host + 'employee/loan/:id', { id: '@id' }, {
        update: {
            method: 'PUT'
        }
    });
});
