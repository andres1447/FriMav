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
        },
        unliquidated: {
          url: ApiConfig.host + 'employee/:id/unliquidated',
          method: 'GET',
          isArray: true
        },
        liquidated: {
          url: ApiConfig.host + 'employee/:id/liquidated',
          method: 'GET'
        },
        loanFees: {
          url: ApiConfig.host + 'employee/:id/loanFees',
          method: 'GET',
          isArray: true
        },
        closePayroll: {
          method: 'POST',
          url: ApiConfig.host + '/employee/:id/payroll'
        },
        closePayrolls: {
          method: 'POST',
          url: ApiConfig.host + '/employee/payroll'
        }
    });
});
