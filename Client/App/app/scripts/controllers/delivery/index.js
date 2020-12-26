'use strict';

angular.module('client')
  .controller('DeliveryIndexCtrl', function ($scope, $state, hotkeys, Delivery, Notification, ModalService, deliveries) {
      $scope.deliveryIndex = 0;
      $scope.deliveries = deliveries;

      hotkeys.bindTo($scope).add({
          combo: '+',
          description: 'Nueva lista de precios',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
              $state.go('DeliveryCreate');
              e.preventDefault();
          }
      })
      .add({
          combo: '-',
          description: 'Borrar envio',
          persistent: false,
          callback: function (e) {
              if ($scope.deliveryIndex < $scope.deliveries.length) {
                  $scope.delete($scope.deliveryIndex);
              }
              e.preventDefault();
          }
      })
      .add({
          combo: 'enter',
          description: 'Detalles',
          persistent: false,
          callback: function (e) {
            $state.go('DeliveryShow', { id: $scope.deliveries[$scope.deliveryIndex].id });
              e.preventDefault();
          }
      })
      .add({
          combo: '*',
          description: 'Editar envio',
          persistent: false,
          callback: function (e) {
            $state.go('DeliveryUpdate', { id: $scope.deliveries[$scope.deliveryIndex].id });
              e.preventDefault();
          }
      })
      .add({
          combo: 'up',
          description: 'Mover arriba',
          persistent: false,
          callback: function (e) {
              if ($scope.deliveryIndex > 0) {
                  $scope.deliveryIndex--;
                  e.preventDefault();
              }
          }
      })
      .add({
          combo: 'down',
          description: 'Mover abajo',
          persistent: false,
          callback: function (e) {
              if ($scope.deliveryIndex < deliveries.length - 1) {
                  $scope.deliveryIndex++;
                  e.preventDefault();
              }
          }
      });
      
      $scope.delete = function (index) {
          ModalService.show({ title: 'Lista de precios', message: 'Desea borrar el envio?' }).then(function (res) {
            Delivery.delete({ id: $scope.deliveries[index].id }, function (res) {
                  Notification.success('Envio borrado correctamente.');
                  $state.reload();
              }, function (err) {
                  Notification.error(err.data);
              });
          });
      };
  });
