'use strict';

angular.module('client').factory('Payment', function ($resource, ApiConfig) {
  return $resource(ApiConfig.host + 'payment/:id', { id: '@id' }, {
        update: {
            method: 'PUT'
        },
        cancel: {
            method: 'POST',
            url: ApiConfig.host + 'payment/cancel'
        }
    });
});
