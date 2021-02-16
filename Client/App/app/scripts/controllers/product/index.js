'use strict';

angular.module('client')
  .controller('ProductIndexCtrl', function ($scope, $state, ModalService, hotkeys, Product, Notification, products, $filter) {
      $scope.productIndex = 0;
      $scope.products = orderByCode($filter, products)

      hotkeys.bindTo($scope).add({
          combo: 'f12',
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
            $state.go('ProductUpdate', { id: $scope.products[$scope.productIndex].id });
              e.preventDefault();
          }
      })
      .add({
          combo: 'up',
          description: 'Mover arriba',
          persistent: false,
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function (e) {
            if ($scope.typeaheadOpen) return;
            if ($scope.productIndex > 0 && !$scope.typeaheadOpen) {
                $scope.productIndex--;
                e.preventDefault();
            }
          }
      })
      .add({
          combo: 'down',
          description: 'Mover abajo',
          persistent: false,
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function (e) {
            if ($scope.typeaheadOpen) return;
            if ($scope.productIndex < products.length - 1 && !$scope.typeaheadOpen) {
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
          var term = $viewValue.toLowerCase();
          return $.grep($scope.products, function (it) {
              return it.name.toLowerCase().indexOf(term) != -1 || it.code.toLowerCase().indexOf(term) == 0;
          });
      };

      $scope.goToProduct = function (product) {
        $state.go('ProductUpdate', { id: product.id });
      }
      
      $scope.delete = function (index) {
          ModalService.show({ title: 'Productos', message: 'Desea borrar el producto?' }).then(function (res) {
            Product.delete({ id: $scope.products[index].id }, function (res) {
                  Notification.success('Producto borrado correctamente.');
                  $state.reload();
              }, function (err) {
                  Notification.error(err.data);
              });
          }, function () { });
      };
  });
