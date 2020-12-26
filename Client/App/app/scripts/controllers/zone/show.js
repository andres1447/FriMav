'use strict';

angular.module('client')
  .controller('ZoneShowCtrl', function ($scope, $state, ModalService, hotkeys, Notification, Zone, zone, customers) {
      $scope.zone = zone;
      $scope.customers = customers;

      hotkeys.bindTo($scope)
      .add({
          combo: '-',
          description: 'Borrar zona',
          persistent: false,
          callback: function (e) {
            $scope.delete();
            e.preventDefault();
          }
      })
      .add({
          combo: 'esc',
          description: 'Volver',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
              $state.go('ZoneIndex');
              e.preventDefault();
          }
      });

      $scope.delete = function () {
          ModalService.show({ title: 'Zona', message: 'Desea borrar la zona?' }).then(function (res) {
            Zone.delete({ id: $scope.zone.id }, function (res) {
                  Notification.success('Zona borrada correctamente.');
                  $state.go('ZoneIndex');
              }, function (err) {
                  Notification.error(err.data);
              });
          });
      };
  });
