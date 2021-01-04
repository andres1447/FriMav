'use strict';

angular.module('client').factory('Transaction', function ($resource, ApiConfig) {
  return $resource(ApiConfig.host + 'transaction/:id', null, {
        account: {
            url: ApiConfig.host + 'transaction/person/:id',
            method: 'GET'
        },
        cancel: {
            url: ApiConfig.host + 'transaction/cancel',
            method: 'POST'
        }
    });
});
