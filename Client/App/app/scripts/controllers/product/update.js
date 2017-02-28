'use strict';

angular.module('client')
  .controller('ProductUpdateCtrl', function ($scope, $state, $timeout, hotkeys, Notification, Product, productFamilies, product) {
      $scope.productFamilies = productFamilies;
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
          if (hasValue(product.family))
              product.familyId = product.family.familyId;
          Product.update(product, function (res) {
              Notification.success('Producto editado correctamente.');
              $state.go('ProductIndex');
          }, function (err) {
              Notification.error({ title: err.data.message, message: err.data.errors.join('</br>') });
          });
      };

      $scope.setFamilyId = function (product) {
          product.familyId = product.family.familyId;
      };
  });
