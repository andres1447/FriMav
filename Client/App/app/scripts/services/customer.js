'use strict';

angular.module('client').factory('Customer', function ($resource, ApiConfig) {
    return $resource(ApiConfig.host + 'customer/:personId', { personId: '@personId' }, {
        products: {
            method: 'GET',
            url: ApiConfig.host + 'customer/:personId/products',
            isArray: true
        },
        update: {
            method: 'PUT'
        }
    });
});