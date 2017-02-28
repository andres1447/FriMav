'use strict';

angular.module('client').factory('Catalog', function ($resource, ApiConfig) {
    return $resource(ApiConfig.host + 'catalog/:catalogId', { catalogId: '@catalogId' }, {
        update: {
            method: 'PUT'
        }
    });
});