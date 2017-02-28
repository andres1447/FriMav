'use strict';

angular.module('client').factory('Zone', function ($resource, ApiConfig) {
    return $resource(ApiConfig.host + 'zone/:zoneId', { zoneId: '@zoneId' }, {
        update: {
            method: 'PUT'
        },
        customers: {
            url: ApiConfig.host + 'zone/:zoneId/customers',
            method: 'GET',
            isArray: true
        }
    });
});