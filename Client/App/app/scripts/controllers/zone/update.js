'use strict';

angular.module('client')
  .controller('ZoneUpdateCtrl', function ($scope, $state, $timeout, hotkeys, Notification, Zone, zone) {
      $scope.zone = zone;

      hotkeys.bindTo($scope)
      .add({
          combo: 'f5',
          description: 'Guardar',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function () {
              $scope.update($scope.zone);
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
           $scope.broadcast('InitZoneCreate');
      };

      $scope.init();
      
      $scope.update = function (zone) {
          Zone.update(zone, function (res) {
              Notification.success('Zona creada correctamente.');
              $state.go('ZoneIndex');
          }, function (err) {
              Notification.error(err.data);
          });
      };
  });
