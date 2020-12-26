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
          var products = {};
          $scope.invoiceTotals = { quantity: 0, amount: 0 };
          angular.forEach($scope.delivery.invoices, function (invoice) {
              $scope.invoiceTotals.quantity++;
              $scope.invoiceTotals.amount += invoice.total;
              angular.forEach(invoice.items, function (it) {
                if (!products[it.product.id]) {
                  products[it.product.id] = { productId: it.productId, name: it.product.name, quantity: 0 };
                  }
                products[it.product.id].quantity += it.quantity;
              });
          });
          $scope.products = [];
          angular.forEach(products, function (it) {
              $scope.products.push(it);
          });
      };

      $scope.init();

      $scope.print = function () {
          Notification.success('Imprimiendo envio...');
          PrintHelper.print('Delivery', JSON.stringify($scope.getPrintModel()));
      };

      $scope.getPrintModel = function () {
          var model = {
              date: $scope.delivery.date,
              employee: { code: $scope.delivery.employee.code, name: $scope.delivery.employee.name },
              invoices: [],
              items: []
          };
          angular.forEach($scope.delivery.invoices, function (item) {
              model.invoices.push({ customer: { code: item.person.code, name: item.person.name }, total: item.total, number: item.number });
          });
          angular.forEach($scope.products, function (item) {
              model.items.push({ product: item.name, quantity: item.quantity });
          });
          return model;
      };

      $scope.delete = function () {
          ModalService.show({ title: 'Envio', message: 'Desea borrar el envio?' }).then(function (res) {
            Delivery.delete({ id: delivery.id }, function (res) {
                  Notification.success('Envio borrado correctamente.');
                  $state.go('DeliveryIndex');
              }, function (err) {
                  Notification.error(err.data);
              });
          });
      };
  });
