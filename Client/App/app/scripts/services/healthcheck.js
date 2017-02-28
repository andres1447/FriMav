'use strict';

angular.module('client').factory('HealthCheck', function ($resource, ApiConfig) {
    return $resource(ApiConfig.host + 'healthcheck');
});