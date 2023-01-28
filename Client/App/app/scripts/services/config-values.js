'use strict';

angular.module('client').factory('ConfigValues', function ($resource, ApiConfig) {
  return $resource(ApiConfig.host + 'configValues', {}, {
        update: {
            method: 'PUT'
        }
    });
});
