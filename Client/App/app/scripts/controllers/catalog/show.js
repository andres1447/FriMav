'use strict';

angular.module('client')
  .controller('CatalogShowCtrl', function ($scope, $state, ModalService, hotkeys, Notification, Catalog, catalog) {
      $scope.catalog = catalog;

      hotkeys.bindTo($scope)
      .add({
          combo: '-',
          description: 'Borrar lista de precios',
          persistent: false,
          callback: function (e) {
            $scope.delete($scope.catalog);
            e.preventDefault();
          }
      })
      .add({
          combo: 'esc',
          description: 'Volver',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
              $state.go('CatalogIndex');
              e.preventDefault();
          }
      })
      .add({
          combo: 'f5',
          description: 'Imprimir',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
              $scope.print($scope.catalog);
              e.preventDefault();
          }
      });

      $scope.print = function (catalog) {
          Notification.success('Imprimiendo lista de precios...');
          PrintHelper.print('PriceList', JSON.stringify($scope.getPrintModel(catalog)));
      };

      $scope.getPrintModel = function (catalog) {
          var model = {
              name: catalog.name,
              products: []
          };
          angular.forEach(catalog.products, function (item) {
              model.products.push({ name: item.name, price: item.price });
          });
          return model;
      };

      $scope.delete = function (catalog) {
          ModalService.show({ title: 'Lista de precios', message: 'Desea borrar la lista de precios?' }).then(function (res) {
            Catalog.delete({ id: catalog.id }, function (res) {
                  Notification.success('Lista de precios borrada correctamente.');
                  $state.go('CatalogIndex');
              }, function (err) {
                  Notification.error(err.data);
              });
          });
      };
  });
