'use strict';

angular.module('client').factory('Transaction', function ($resource, ApiConfig) {
    return $resource(ApiConfig.host + 'transaction/:transactionId', null, {
        account: {
            url: ApiConfig.host + 'transaction/person/:personId',
            method: 'GET',
            isArray: true
        },
        cancel: {
            url: ApiConfig.host + 'transaction/cancel',
            method: 'POST'
        }
    });
});