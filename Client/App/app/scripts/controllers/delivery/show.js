'use strict';

angular.module('client')
  .controller('DeliveryShowCtrl', function ($scope, $state, hotkeys, Notification, delivery) {
      $scope.delivery = delivery;

      hotkeys.bindTo($scope)
      .add({
          combo: 'esc',
          description: 'Volver a envios',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
              $state.go('DeliveryIndex');
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

      $scope.init = function () {
          $scope.products = {};
          angular.forEach($.map($scope.delivery.invoices, function (i) { return i.items; }), function (it) {
              if ($scope.products[it.product.productId]) {

              }
          });
      };

      $scope.init();

      $scope.print = function () {
          Notification.success('Imprimiendo envio...');
          PrintHelper.print('Factura', JSON.stringify($scope.getPrintModel($scope.invoice)));
      };

      $scope.getPrintModel = function (invoice) {
          var model = {
              date: invoice.date,
              deliveryAddress: invoice.deliveryAddress,
              customerCode: invoice.person.code,
              customerName: invoice.person.name,
              number: invoice.number,
              balance: 0,
              items: []
          };
          angular.forEach(invoice.items, function (item) {
              model.items.push({ product: item.product.name, quantity: item.quantity, price: item.price });
          });
          return model;
      };
  });
