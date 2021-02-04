'use strict';

angular.module('client')
  .controller('InvoiceShowCtrl', function ($scope, $state, hotkeys, Notification, invoice) {
      $scope.invoice = invoice;

      hotkeys.bindTo($scope)
      .add({
          combo: '-',
          description: 'Reembolzar factura',
          persistent: false,
          callback: function (e) {
            $state.go('InvoiceRefund', { id: $scope.invoice.id });
              e.preventDefault();
          }
      })
      .add({
          combo: 'esc',
          description: 'Volver a cliente',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
            $state.go('CustomerShow', { id: $scope.invoice.personId });
            e.preventDefault();
          }
      })
      .add({
          combo: 'f5',
          description: 'Reimprimir',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
              $scope.print();
              e.preventDefault();
          }
      });

      $scope.init = function () {
          $scope.totals = { quantity: 0, total: $scope.invoice.total };
          angular.forEach($scope.invoice.items, function (item) {
              $scope.totals.quantity++;
          });
      };

      $scope.init();

      $scope.print = function () {
          Notification.success('Imprimiendo factura...');
          PrintHelper.print('Invoice', JSON.stringify($scope.getPrintModel($scope.invoice)));
      };

      $scope.getPrintModel = function (invoice) {
          var model = {
              date: invoice.date,
              deliveryAddress: invoice.deliveryAddress,
              customerCode: invoice.person.code,
              customerName: invoice.customerName,
              number: invoice.number,
              balance: 0,
              items: []
          };
          angular.forEach(invoice.items, function (item) {
              model.items.push({ product: item.productName, quantity: item.quantity, price: item.price });
          });
          return model;
      };
  });
