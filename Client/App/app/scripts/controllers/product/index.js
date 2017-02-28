'use strict';

angular.module('client')
  .controller('ProductIndexCtrl', function ($scope, $state, ModalService, hotkeys, Product, Notification, products) {
      $scope.productIndex = 0;
      $scope.products = products;

      hotkeys.bindTo($scope).add({
          combo: 'f2',
          description: 'Buscar producto',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
              $scope.$broadcast('SearchProduct');
              e.preventDefault();
          }
      })
      .add({
          combo: '+',
          description: 'Nuevo producto',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
              $state.go('ProductCreate');
              e.preventDefault();
          }
      })
      .add({
          combo: '-',
          description: 'Borrar producto',
          persistent: false,
          callback: function (e) {
              if ($scope.productIndex < $scope.products.length) {
                  $scope.delete($scope.productIndex);
              }
              e.preventDefault();
          }
      })
      .add({
          combo: '*',
          description: 'Editar producto',
          persistent: false,
          callback: function (e) {
              $state.go('ProductUpdate', { productId: $scope.products[$scope.productIndex].productId });
              e.preventDefault();
          }
      })
      .add({
          combo: 'up',
          description: 'Mover arriba',
          persistent: false,
          callback: function (e) {
              if ($scope.productIndex > 0) {
                  $scope.productIndex--;
                  e.preventDefault();
              }
          }
      })
      .add({
          combo: 'down',
          description: 'Mover abajo',
          persistent: false,
          callback: function (e) {
              if ($scope.productIndex < products.length - 1) {
                  $scope.productIndex++;
                  e.preventDefault();
              }
          }
      });

      $scope.init = function () {
          setTimeout(function () {
              $scope.$broadcast('InitProduct');
          }, 50);
      };

      $scope.init();

      $scope.getMatchingProducts = function ($viewValue) {
          return $.grep($scope.products, function (it) {
              return it.name.toLowerCase().indexOf($viewValue) != -1 || it.code.toLowerCase().indexOf($viewValue) == 0;
          });
      };

      $scope.goToProduct = function (product) {
          $state.go('ProductUpdate', { productId: product.productId });
      }
      
      $scope.delete = function (index) {
          ModalService.show({ title: 'Productos', message: 'Desea borrar el producto?' }).then(function (res) {
              Product.delete({ productId: $scope.products[index].productId }, function (res) {
                  Notification.success('Producto borrado correctamente.');
                  $state.reload();
              }, function (err) {
                  Notification.error(err.data);
              });
          });
      };
  });
