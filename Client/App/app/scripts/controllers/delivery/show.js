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
          angular.forEach($.map($scope.delivery.invoices, function (i) { return i.items; }), function (it) {
              if (!products[it.product.productId]) {
                  products[it.product.productId] = { productId: it.productId, name: it.product.name, quantity: 0 };
              }
              products[it.product.productId].quantity += it.quantity;
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
              employeeCode: $scope.delivery.employee.code,
              employeeName: $scope.delivery.employee.name,
              invoices: [],
              items: []
          };
          angular.forEach($scope.delivery.invoices, function (item) {
              model.invoices.push({ code: item.person.code, customer: item.person.name, total: item.total, number: item.number });
          });
          angular.forEach($scope.products, function (item) {
              model.items.push({ product: item.name, quantity: item.quantity });
          });
          return model;
      };

      $scope.delete = function () {
          ModalService.show({ title: 'Envio', message: 'Desea borrar el envio?' }).then(function (res) {
              Delivery.delete({ deliveryId: delivery.deliveryId }, function (res) {
                  Notification.success('Envio borrado correctamente.');
                  $state.go('DeliveryIndex');
              }, function (err) {
                  Notification.error(err.data);
              });
          });
      };
  });
