'use strict';

angular.module('client')
  .controller('ConfigValuesUpdateCtrl', function ($scope, Notification, ConfigValues, configValues) {
      $scope.configValues = configValues;
      
      $scope.update = function (configValue) {
          if (!$scope.sending) {
              $scope.sending = true;
            ConfigValues.update(configValue, function (res) {
                  $scope.sending = false;
                  Notification.success('Parámetro guardado correctamente.');
              }, function (err) {
                  $scope.sending = false;
                  Notification.error(err.data);
              });
          }
      };
  });
