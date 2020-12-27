'use strict';

angular.module('client')
  .controller('ProductUpdateCtrl', function ($scope, $state, $timeout, hotkeys, Notification, Product, productTypes, product) {
      $scope.productTypes = productTypes;
      $scope.product = product;

      hotkeys.bindTo($scope).add({
          combo: 'f5',
          description: 'Guardar',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function () {
              $scope.submit($scope.product);
          }
      })
      .add({
          combo: 'esc',
          description: 'Volver al indice',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
              $state.go('ProductIndex');
              e.preventDefault();
          }
      });

      $scope.init = function () {
           $scope.broadcast('InitProductCreate');
      };

      $scope.init();
      
      $scope.update = function (product) {
          if (!$scope.sending) {
              $scope.sending = true;
              Product.update(product, function (res) {
                  $scope.sending = false;
                  Notification.success('Producto editado correctamente.');
                  $state.go('ProductIndex');
              }, function (err) {
                  $scope.sending = false;
                  Notification.error({ title: err.data.message, message: err.data.errors.join('</br>') });
              });
          }
      };

      $scope.setProductTypeId = function (product) {
        product.productTypeId = product.type.id;
      };

      $scope.clearProductType = function () {
        product.productTypeId = null;
        product.type = null;
      }
  });
