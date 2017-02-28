'use strict';

angular.module('client').factory('Employee', function ($resource, ApiConfig) {
    return $resource(ApiConfig.host + 'employee/:personId', { personId: '@personId' }, {
        update: {
            method: 'PUT'
        }
    });
});