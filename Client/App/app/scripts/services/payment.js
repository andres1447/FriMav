'use strict';

angular.module('client').factory('Payment', function ($resource, ApiConfig) {
    return $resource(ApiConfig.host + 'payment/:paymentId', { productId: '@id' }, {
        update: {
            method: 'PUT'
        },
        cancel: {
            method: 'POST',
            url: ApiConfig.host + 'payment/cancel'
        }
    });
});