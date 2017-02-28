'use strict';

angular.module('client')
  .controller('ZoneIndexCtrl', function ($scope, $state, ModalService, hotkeys, Zone, Notification, zones) {
      $scope.zoneIndex = 0;
      $scope.zones = zones;

      hotkeys.bindTo($scope)
      .add({
          combo: 'enter',
          description: 'Detalles',
          persistent: false,
          callback: function (e) {
              $state.go('ZoneShow', { zoneId: $scope.zones[$scope.zoneIndex].zoneId });
              e.preventDefault();
          }
      })
      .add({
          combo: '+',
          description: 'Nueva zona',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
              $state.go('ZoneCreate');
              e.preventDefault();
          }
      })
      .add({
          combo: '-',
          description: 'Borrar zona',
          persistent: false,
          callback: function (e) {
              $scope.delete($scope.zoneIndex);
              e.preventDefault();
          }
      })
      .add({
          combo: '*',
          description: 'Editar zona',
          persistent: false,
          callback: function (e) {
              $state.go('ZoneUpdate', { zoneId: $scope.zones[$scope.zoneIndex].zoneId });
              e.preventDefault();
          }
      })
      .add({
          combo: 'up',
          description: 'Mover arriba',
          persistent: false,
          callback: function (e) {
              if ($scope.zoneIndex > 0) {
                  $scope.zoneIndex--;
                  e.preventDefault();
              }
          }
      })
      .add({
          combo: 'down',
          description: 'Mover abajo',
          persistent: false,
          callback: function (e) {
              if ($scope.zoneIndex < zones.length - 1) {
                  $scope.zoneIndex++;
                  e.preventDefault();
              }
          }
      });
      
      $scope.delete = function (index) {
          ModalService.show({ title: 'Zona', message: 'Desea borrar la zona?' }).then(function (res) {
              Zone.delete({ zoneId: $scope.zones[index].zoneId }, function (res) {
                  Notification.success('Zona borrada correctamente.');
                  $state.reload();
              }, function (err) {
                  Notification.error(err.data);
              });
          });
      };
  });
