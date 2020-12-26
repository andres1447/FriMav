'use strict';

angular.module('client').factory('Delivery', function ($resource, ApiConfig) {
  return $resource(ApiConfig.host + 'delivery/:id', null, {
        update: {
            method: 'PUT'
        }
    });
});
