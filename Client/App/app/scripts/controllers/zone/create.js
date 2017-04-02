'use strict';

angular.module('client')
  .controller('ZoneCreateCtrl', function ($scope, $state, $timeout, hotkeys, Notification, Zone) {
      
      hotkeys.bindTo($scope)
      .add({
          combo: 'f5',
          description: 'Guardar',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function () {
              $scope.create($scope.zone);
          }
      })
      .add({
          combo: 'esc',
          description: 'Volver al indice',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
              $state.go('ZoneIndex');
              e.preventDefault();
          }
      });

      $scope.init = function () {
          $scope.zone = {};

          $scope.broadcast('InitZoneCreate');
      };

      $scope.init();
      
      $scope.create = function (zone) {
          if (!$scope.sending) {
              $scope.sending = true;
              Zone.save(zone, function (res) {
                  $scope.sending = false;
                  Notification.success('Zona creada correctamente.');
                  $state.go('ZoneIndex');
              }, function (err) {
                  $scope.sending = false;
                  Notification.error(err.data);
              });
          }
      };
  });
