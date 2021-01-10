'use strict';

angular.module('client').factory('Delivery', function ($resource, ApiConfig) {
  return $resource(ApiConfig.host + 'delivery/:id', null, {
    update: {
      method: 'PUT'
    },
    pending: {
      url: ApiConfig.host + 'delivery/pending',
      method: 'GET',
    },
    close: {
      url: ApiConfig.host + 'delivery/:id/close',
      method: 'POST',
    }
  });
});
