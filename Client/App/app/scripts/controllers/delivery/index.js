'use strict';

angular.module('client')
  .controller('DeliveryIndexCtrl', function ($scope, $state, hotkeys, Delivery, Notification, ModalService, PendingDeliveryCheck) {
      $scope.deliveryIndex = 0;
      $scope.deliveries = [];
      $scope.pageNumber = 0
      $scope.itemsPerPage = 20;
      $scope.totalCount = 0;
      $scope.totalPages = 0;
      $scope.closedDeliveries = false;

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
        description: 'Cerrar envio',
        persistent: false,
        callback: function (e) {
            if ($scope.deliveryIndex < $scope.deliveries.length) {
              $scope.close($scope.deliveryIndex);
            }
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
            if ($scope.deliveryIndex < $scope.deliveries.length - 1) {
              $scope.deliveryIndex++;
              e.preventDefault();
            }
          }
      })
      .add({
        combo: 'pageup',
        description: 'Cargar envios anteriores',
        allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
        persistent: false,
        callback: function (e) {
          e.preventDefault();
          $scope.prevPage();
        }
      })
      .add({
        combo: 'pagedown',
        description: 'Cargar envios mas recientes',
        allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
        persistent: false,
        callback: function (e) {
          e.preventDefault();
          $scope.nextPage();
        }
      });

      $scope.prevPage = function () {
        if ($scope.pageNumber >= $scope.totalPages - 1)
          return;

        $scope.pageNumber++;
        $scope.loadDeliveries();
      }

      $scope.nextPage = function () {
        if ($scope.pageNumber <= 0)
          return;

        $scope.pageNumber--;
        $scope.loadDeliveries();
      }

    $scope.loadDeliveries = function () {
        Delivery.get({ closed: $scope.closedDeliveries, offset: $scope.pageNumber, count: $scope.itemsPerPage }).$promise.then(function (response) {
          $scope.deliveries = response.items;
          $scope.totalCount = response.totalCount;
          $scope.totalPages = Math.ceil($scope.totalCount / $scope.itemsPerPage);
          if ($scope.deliveryIndex >= $scope.deliveries.length)
            $scope.deliveryIndex = 0;
        });
      }

      $scope.loadDeliveries();
      
      $scope.delete = function (index) {
        ModalService.show({ title: 'Envío', message: 'Desea borrar el envio?' }).then(function (res) {
          Delivery.delete({ id: $scope.deliveries[index].id }, function (res) {
                Notification.success('Envio borrado correctamente.');
                $state.reload();
            }, function (err) {
                Notification.error(err.data);
            });
        }, function () { });
    };

    $scope.close = function (index) {
      ModalService.show({ title: 'Envío', message: 'Desea cerrar el envio?' }).then(function (res) {
        Delivery.close({ id: $scope.deliveries[index].id }, { id: $scope.deliveries[index].id, payments: [] }, function (res) {
          Notification.success('Envio cerrado correctamente.');
          PendingDeliveryCheck.schedule();
          $state.reload();
        }, function (err) {
          Notification.error(err.data);
        });
      }, function () { });
    };
  });
