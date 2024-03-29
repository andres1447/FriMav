'use strict';

angular.module('client').factory('Invoice', function ($resource, ApiConfig) {
  return $resource(ApiConfig.host + 'invoice/:id', null, {
        update: {
            method: 'PUT'
        },
        undelivered: {
            method: 'GET',
            url: ApiConfig.host + 'invoice/undelivered',
            isArray: true
        },
        dontDeliver: {
          url: ApiConfig.host + 'invoice/:id/dontDeliver',
          method: 'POST'
        },
        updateExternalReferenceNumber: {
          url: ApiConfig.host + 'invoice/:id/externalReferenceNumber',
          method: 'POST'
        },
        ticket: {
          url: ApiConfig.host + 'invoice/ticket',
          method: 'POST'
        },
        cancelTicket: {
          url: ApiConfig.host + 'invoice/ticket/cancel',
          method: 'POST'
        }
    });
});
