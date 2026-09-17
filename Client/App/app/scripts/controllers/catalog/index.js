'use strict';

angular.module('client')
  .controller('CatalogIndexCtrl', function ($scope, $state, hotkeys, Catalog, Product, Notification, ModalService, catalogs) {
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
            $state.go('CatalogShow', { id: $scope.catalogs[$scope.catalogIndex].id });
              e.preventDefault();
          }
      })
      .add({
          combo: '*',
          description: 'Editar lista de precios',
          persistent: false,
          callback: function (e) {
            $state.go('CatalogUpdate', { id: $scope.catalogs[$scope.catalogIndex].id });
              e.preventDefault();
          }
      })
      .add({
          combo: 'f5',
          description: 'Publicar lista de precios',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
              $scope.publishPriceList();
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

      $scope.publishPriceList = function () {
          Product.pricelist(function (groups) {
              Notification.success('Generando lista de precios...');
              var model = {
                  groups: $.map(groups, function (group) {
                      return {
                          name: group.name,
                          products: $.map(group.products || [], function (product) {
                              return {
                                  name: product.name,
                                  price: product.price
                              };
                          })
                      };
                  })
              };
              PrintHelper.print('StorePriceList', JSON.stringify(model));
          }, function (err) {
              Notification.error(err.data || 'No se pudo obtener la lista de precios.');
          });
      };

      $scope.delete = function (index) {
          ModalService.show({ title: 'Lista de precios', message: 'Desea borrar la lista de precios?' }).then(function (res) {
            Catalog.delete({ id: $scope.catalogs[index].id }, function (res) {
                  Notification.success('Lista de precios borrada correctamente.');
                  $state.reload();
              }, function (err) {
                  Notification.error(err.data);
              });
          }, function () { });
      };
  });
