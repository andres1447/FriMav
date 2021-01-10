'use strict';

angular.module('client')
  .controller('ProductCreateCtrl', function ($scope, $state, hotkeys, Notification, Product, productTypes, codes) {
      $scope.productTypes = productTypes;
      $scope.codes = codes;

      hotkeys.bindTo($scope).add({
          combo: 'f5',
          description: 'Guardar',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function () {
              $scope.create($scope.product);
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
          $scope.product = { active: true };
          $scope.broadcast('InitProductCreate');
      };

      $scope.init();
      
      $scope.create = function (product) {
          if (!$scope.sending) {
              $scope.sending = true;
              Product.save(product, function (res) {
                  $scope.sending = false;
                  Notification.success('Producto creado correctamente.');
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
      $scope.product.productTypeId = null;
      $scope.product.type = null;
    }
  });
