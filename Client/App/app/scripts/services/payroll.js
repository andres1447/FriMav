'use strict';

angular.module('client').factory('Payroll', function ($resource, ApiConfig) {
  return $resource(ApiConfig.host + 'employee/payroll/:id', { id: '@id' }, {
        update: {
            method: 'PUT'
        }
    });
});
