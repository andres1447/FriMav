'use strict';

angular.module('client')
  .controller('FamilyShowCtrl', function ($scope, $state, ModalService, hotkeys, Notification, ProductFamily, family, products) {
      $scope.family = family;
      $scope.products = products;

      hotkeys.bindTo($scope)
      .add({
          combo: '-',
          description: 'Borrar familia de productos',
          persistent: false,
          callback: function (e) {
            $scope.delete();
            e.preventDefault();
          }
      })
      .add({
          combo: 'esc',
          description: 'Volver',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
              $state.go('FamilyIndex');
              e.preventDefault();
          }
      })
      .add({
          combo: 'f5',
          description: 'Imprimir',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
              $scope.print();
              e.preventDefault();
          }
      });

      $scope.print = function () {
          Notification.success('Imprimiendo familia de productos...');
          var model = {
              products: []
          };
          angular.forEach($scope.products, function (item) {
              model.products.push({ name: item.name, price: item.price });
          });
          PrintHelper.print('PriceList', JSON.stringify(model));
      };

      $scope.delete = function () {
          ModalService.show({ title: 'Familia de productos', message: 'Desea borrar la familia de productos?' }).then(function (res) {
              ProductFamily.delete({ familyId: $scope.family.familyId }, function (res) {
                  Notification.success('Familia de productos borrada correctamente.');
                  $state.go('FamilyIndex');
              }, function (err) {
                  Notification.error(err.data);
              });
          });
      };
  });
