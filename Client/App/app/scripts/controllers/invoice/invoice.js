'use strict';

angular.module('client')
  .controller('InvoiceCtrl', function ($scope, $state, $filter, $timeout, hotkeys, Notification, Customer, Invoice, products, customers) {
      $scope.products = products;
      $scope.customers = customers;

      hotkeys.bindTo($scope).add({
          combo: 'f5',
          description: 'Imprimir',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function () {
              $scope.submit($scope.invoice);
          }
      })
      .add({
          combo: 'esc',
          description: 'Reiniciar',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function () {
              $scope.reload();
          }
      });

      $scope.init = function () {
          $scope.invoice = {
              date: new Date(),
              shipping: 1,
              paymentMethod: 1,
              items: [{quantity: 0, price: 0}]
          };
          $scope.totals = {};

          $scope.broadcast('InitInvoice');
      };

      $scope.init();

      $scope.addItem = function () {
          var items = $scope.invoice.items;
          if (items.length === 0 || hasValue(items[items.length - 1].quantity)) {
              $scope.invoice.items.push({ quantity: 0, price: 0 });
              return true;
          }
      };

      $scope.getMatchingCustomer = function ($viewValue) {
          return $.grep($scope.customers, function (it) {
              return it.name.toLowerCase().indexOf($viewValue) != -1 || it.code.toLowerCase().indexOf($viewValue) == 0;
          });
      };

      $scope.setCustomer = function (invoice) {
          Customer.products({ personId: invoice.customer.personId }).$promise.then(function (productList) {
              $scope.products = productList;
          });
          $scope.invoice.personId = invoice.customer.personId;
          $scope.invoice.deliveryAddress = invoice.customer.address;
          $scope.invoice.shipping = invoice.customer.shipping;
          $scope.invoice.paymentMethod = invoice.customer.paymentMethod;
      };

      $scope.setListPrice = function ($index) {
          var it = $scope.invoice.items[$index];
          if (it) {
              var res = $filter('filter')($scope.products, { productId: it.product.productId }, true);
              if (res.length > 0) {
                  var product = res[0];
                  it.price = product.priceForCustomer;
                  it.basePrice = product.price;
                  it.productId = product.productId;
              }
          }
      };

      $scope.getPrintModel = function (invoice) {
          var model = {
              date: invoice.date,
              deliveryAddress: invoice.deliveryAddress,
              customerCode: invoice.customer.code,
              customerName: invoice.customer.name,
              balance: 0,
              items: []
          };
          angular.forEach(invoice.items, function (item) {
              model.items.push({ product: item.product.name, quantity: item.quantity, price: item.price });
          });
          return model;
      };

      $scope.submit = function (invoice) {
          if (!$scope.sending) {
              $scope.sending = true;
              invoice.items = $.grep(invoice.items, function (it) {
                  return hasValue(it.product) && hasValue(it.quantity) && hasValue(it.price);
              });
              Invoice.save(invoice, function (result) {
                  $scope.sending = false;
                  Notification.success('Imprimiendo factura...');
                  var model = $scope.getPrintModel(invoice);
                  model.number = result.number;
                  model.total = result.total;
                  model.balance = result.balance;
                  PrintHelper.print('Invoice', JSON.stringify(model));
                  $state.reload();
              });
          }
      };

      $scope.getMatchingProduct = function ($viewValue) {
          return $.grep($scope.products, function (it) {
              return it.name.toLowerCase().indexOf($viewValue) != -1 || it.code.toLowerCase().indexOf($viewValue) == 0;
          });
      };

      $scope.validateProduct = function ($index) {
          if (!hasValue($scope.invoice.items[$index].product.productId)) {
              return -1;
          }
      };

      $scope.validateQuantity = function ($index) {
          if (!hasValue($scope.invoice.items[$index].quantity)) {
              return false;
          }
      };

      $scope.lineProductValidation = function ($index) {
          if (hasValue($scope.invoice.items[$index].product) && hasValue($scope.invoice.items[$index].product.productId))
              return true;
          return false;
      };

      $scope.AddItem = function ($index) {
          if (hasValue($scope.invoice.items[$index].product) && hasValue($scope.invoice.items[$index].product.productId) && hasValue($scope.invoice.items[$index].quantity) && hasValue($scope.invoice.items[$index].price)) {
              $scope.addItem();
              return false;
          }
      };

      $scope.$watch(function () { return $scope.invoice; }, function (newVal) {
          $scope.totals = { quantity: 0, count: 0, subtotal: 0 };
          angular.forEach(newVal.items, function (item) {
              $scope.totals.subtotal += item.quantity && item.price ? item.quantity * item.price : 0;
              $scope.totals.count += item.quantity && item.price ? 1 : 0;
          });
          $scope.totals.price = $scope.totals.weight ? $scope.totals.subtotal / $scope.totals.weight : 0;
      }, true);
  });
