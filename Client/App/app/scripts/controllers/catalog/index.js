'use strict';

angular.module('client')
  .controller('CatalogIndexCtrl', function ($scope, $state, hotkeys, Catalog, Notification, ModalService, catalogs) {
      $scope.catalogIndex = 0;
      $scope.catalogs = catalogs;

      hotkeys.bindTo($scope).add({
          combo: '+',
          description: 'Nueva lista de precios',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
              $state.go('CatalogCreate');
              e.preventDefault();
          }
      })
      .add({
          combo: '-',
          description: 'Borrar lista de precios',
          persistent: false,
          callback: function (e) {
              if ($scope.catalogIndex < $scope.catalogs.length) {
                  $scope.delete($scope.catalogIndex);
              }
              e.preventDefault();
          }
      })
      .add({
          combo: 'enter',
          description: 'Detalles',
          persistent: false,
          callback: function (e) {
              $state.go('CatalogShow', { catalogId: $scope.catalogs[$scope.catalogIndex].catalogId });
              e.preventDefault();
          }
      })
      .add({
          combo: '*',
          description: 'Editar lista de precios',
          persistent: false,
          callback: function (e) {
              $state.go('CatalogUpdate', { catalogId: $scope.catalogs[$scope.catalogIndex].catalogId });
              e.preventDefault();
          }
      })
      .add({
          combo: 'up',
          description: 'Mover arriba',
          persistent: false,
          callback: function (e) {
              if ($scope.catalogIndex > 0) {
                  $scope.catalogIndex--;
                  e.preventDefault();
              }
          }
      })
      .add({
          combo: 'down',
          description: 'Mover abajo',
          persistent: false,
          callback: function (e) {
              if ($scope.catalogIndex < catalogs.length - 1) {
                  $scope.catalogIndex++;
                  e.preventDefault();
              }
          }
      });
      
      $scope.delete = function (index) {
          ModalService.show({ title: 'Lista de precios', message: 'Desea borrar la lista de precios?' }).then(function (res) {
              Catalog.delete({ catalogId: $scope.catalogs[index].catalogId }, function (res) {
                  Notification.success('Lista de precios borrada correctamente.');
                  $state.reload();
              }, function (err) {
                  Notification.error(err.data);
              });
          });
      };
  });
