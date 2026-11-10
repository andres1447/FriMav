'use strict';

angular.module('client').factory('ProductMeasure', function ($resource, ApiConfig) {
    return $resource(ApiConfig.host + 'productmeasure/:id', { id: '@id' }, {
        update: {
            method: 'PUT'
        }
    });
});
