'use strict';

angular.module('client')
  .controller('CatalogCreateCtrl', function ($scope, $state, $filter, $timeout, hotkeys, Notification, Catalog, products) {
      $scope.products = products;

      hotkeys.bindTo($scope).add({
          combo: 'f5',
          description: 'Imprimir',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function () {
              $scope.create($scope.catalog);
          }
      })
      .add({
          combo: 'esc',
          description: 'Reiniciar',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function () {
              $scope.reload();
          }
      });

      $scope.init = function () {
          $scope.catalog = {
              products: [{}]
          };
          $scope.broadcast('InitForm');
      };

      $scope.init();

      $scope.AddItem = function ($index) {
          if (hasValue($scope.catalog.products[$index].product) && hasValue($scope.catalog.products[$index].product.productId)) {
              $scope.catalog.products.push({ quantity: 0, price: 0 });
              return false;
          }
      };

      $scope.submit = function (catalog) {
          catalog.products = $.grep(catalog.products, function (it) {
              return hasValue(it.product);
          });
          catalog.products = $.map(catalog.products, function (it) {
              return it.product.productId;
          });
          Catalog.save(catalog, function (res) {
              Notification.success('Lista de precios creada correctamente');
              $state.go('CatalogIndex');
          });
      };

      $scope.validateProduct = function ($index) {
          if (!hasValue($scope.catalog.products[$index].product.productId)) {
              return -1;
          }
      };

      $scope.getMatchingProduct = function ($viewValue) {
          return $.grep($scope.products, function (it) {
              return it.name.toLowerCase().indexOf($viewValue) != -1 || it.code.toLowerCase().indexOf($viewValue) == 0;
          });
      }
  });
