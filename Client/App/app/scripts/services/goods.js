'use strict';

angular.module('client').factory('GoodsSold', function ($resource, ApiConfig) {
  return $resource(ApiConfig.host + 'employee/goods/:id', { id: '@id' }, {
        update: {
            method: 'PUT'
        }
    });
});
