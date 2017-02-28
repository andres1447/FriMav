'use strict';

angular.module('client').factory('ProductFamily', function ($resource, ApiConfig) {
    return $resource(ApiConfig.host + 'family/:familyId', { familyId: '@familyId' }, {
        update: {
            method: 'PUT'
        },
        products: {
            url: ApiConfig.host + 'family/:familyId/products',
            method: 'GET',
            isArray: true
        }
    });
});