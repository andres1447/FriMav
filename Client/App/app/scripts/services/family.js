'use strict';

angular.module('client').factory('ProductFamily', function ($resource, ApiConfig) {
    return $resource(ApiConfig.host + 'producttype/:id', { id: '@id' }, {
        update: {
            method: 'PUT'
        },
        products: {
            url: ApiConfig.host + 'producttype/:id/products',
            method: 'GET',
            isArray: true
        }
    });
});
