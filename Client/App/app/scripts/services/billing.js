'use strict';

angular.module('client').factory('Billing', function ($resource, ApiConfig) {
  return $resource(ApiConfig.host + 'billing/:id', null, {
    report: {
      url: ApiConfig.host + 'billing',
      method: 'POST'
    }
  });
});
