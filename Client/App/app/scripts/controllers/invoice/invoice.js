'use strict';

angular.module('client')
  .controller('InvoiceCtrl', function ($scope, $state, $filter, hotkeys, Notification, Customer, Invoice, products, customers) {
      $scope.products = orderByCode($filter, products);
      $scope.baseProducts = products;
      $scope.customers = orderByCode($filter, customers);
      $scope.dontPrint = false;

      hotkeys.bindTo($scope).add({
          combo: 'f5',
          description: 'Imprimir',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function () {
              $scope.submit($scope.invoice);
          }
      })
      .add({
        combo: 'f8',
        description: 'Guardar sin imprimir',
        allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
        callback: function () {
          $scope.dontPrint = true;
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
      })
      .add({
        combo: 'del',
        allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
        description: 'Delete Row',
        callback: function (event, hotkey) {
          var $elem = $(event.srcElement).parents("tr:first");
          if (!$elem) return;

          event.preventDefault();
          if ($scope.invoice.items.length > 1) {
            var nextInput = $elem.closest('tr').nextAll('tr').filter(function (index, element) {
              return $(element).find(':text').length > 0;
            }).first().find(':text');

            // if next input exists go there, else go to the first one
            if (nextInput.length > 0) {
              nextInput.first().focus();
            }

            $scope.deleteItem($elem.index());
          }
        }
      })

      $scope.init = function () {
          $scope.invoice = {
              date: new Date(),
              shipping: 1,
              paymentMethod: 1,
              items: [{quantity: 1, price: 0}]
          };
          $scope.totals = {};
          $scope.dontPrint = false;

          $scope.broadcast('InitInvoice');
      };

      $scope.init();

      $scope.addItem = function () {
        var items = $scope.invoice.items;
        if (items.length === 0 || $scope.hasProduct(items[items.length - 1])) {
          $scope.invoice.items.push({ quantity: 1, price: 0 });
          return true;
        }
      };

    $scope.getMatchingCustomer = function ($viewValue) {
        var term = $viewValue.toLowerCase();
        return $.grep($scope.customers, function (it) {
          return it.name.toLowerCase().indexOf(term) != -1 || it.code.toLowerCase().indexOf(term) == 0;
        });
      };

      $scope.setCustomer = function (invoice) {
          Customer.products({ id: invoice.customer.id }).$promise.then(function (productList) {
              $scope.products = productList;
          });
          $scope.invoice.personId = invoice.customer.id;
          $scope.invoice.customerName = invoice.customer.name;
          $scope.invoice.deliveryAddress = invoice.customer.address;
          $scope.invoice.shipping = invoice.customer.shipping;
          $scope.invoice.paymentMethod = invoice.customer.paymentMethod;
      };

      $scope.clearCustomer = function () {
        $scope.invoice.personId = null;
        $scope.invoice.person = null;
        $scope.invoice.customerName = null;
        $scope.invoice.deliveryAddress = null;
        $scope.products = $scope.baseProducts;
      }

      $scope.setListPrice = function (item) {
        var res = $filter('filter')($scope.products, { id: item.product.id }, true);
        if (res.length > 0) {
          var product = res[0];
          item.price = product.price;
          item.basePrice = product.basePrice || product.price;
          item.productId = product.id;
        }
      };

      $scope.clearProduct = function (item) {
          item.product = null;
          item.productId = null;
          item.price = 0;
          item.basePrice = 0;
      }

      $scope.deleteItem = function (index) {
        $scope.invoice.items.splice(index, 1);
      }

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
        if (!$scope.sending && invoice.items.filter(function (x) { return hasValue(x.productId) }).length > 0) {
          $scope.sending = true;
          invoice.items = $.grep(invoice.items, function (it) {
            return hasValue(it.product) && hasValue(it.quantity) && hasValue(it.price);
          });
          Invoice.save(invoice, function (result) {
            $scope.sending = false;
            if ($scope.dontPrint)
              Notification.success('Factura guardada correctamente.');
            else
              print(invoice, result)
            $state.reload();
          });
        }
      };

      function print(invoice, result) {
        Notification.success('Imprimiendo factura...');
        var model = $scope.getPrintModel(invoice);
        model.number = result.number;
        model.total = result.total;
        model.balance = result.balance;
        PrintHelper.print('Invoice', JSON.stringify(model));
      }

      $scope.getMatchingProduct = function ($viewValue) {
        var term = $viewValue.toLowerCase();
        return $.grep($scope.products, function (it) {
          return it.name.toLowerCase().indexOf(term) != -1 || it.code.toLowerCase().indexOf(term) == 0;
        });
      };

      $scope.hasQuantity = function (item) {
          return hasValue(item.quantity);
      };

      $scope.hasProduct = function (item) {
          return hasValue(item.product) && hasValue(item.productId)
      };

      $scope.AddItem = function (item) {
          if ($scope.hasProduct(item) && $scope.hasQuantity(item) && hasValue(item.price)) {
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
