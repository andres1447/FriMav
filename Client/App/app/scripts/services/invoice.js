'use strict';

angular.module('client').factory('Invoice', function ($resource, ApiConfig) {
  return $resource(ApiConfig.host + 'invoice/:id', null, {
        update: {
            method: 'PUT'
        },
        undelivered: {
            method: 'GET',
            url: ApiConfig.host + 'invoice/undelivered',
            isArray: true
        },
        dontDeliver: {
          url: ApiConfig.host + 'invoice/:id/dontDeliver',
          method: 'POST'
        }
    });
});
