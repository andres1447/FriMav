'use strict';

angular.module('client')
  .controller('OrderCtrl', function ($scope, $state, $filter, hotkeys, Notification, products, customers) {
      $scope.products = orderByCode($filter, products);
      $scope.customers = orderByCode($filter, customers);

      hotkeys.bindTo($scope).add({
          combo: 'f5',
          description: 'Imprimir',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function () {
              $scope.print($scope.order);
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
          if ($scope.order.items.length > 1) {
            var nextInput = $elem.closest('tr').nextAll('tr').filter(function (index, element) {
              return $(element).find(':text').length > 0;
            }).first().find(':text');

            if (nextInput.length > 0) {
              nextInput.first().focus();
            }

            $scope.deleteItem($elem.index());
          }
        }
      });

      $scope.init = function () {
          $scope.order = {
              items: [emptyItem()]
          };
          $scope.broadcast('InitOrder');
      };

      $scope.init();

      function emptyItem() {
          return { quantity: 1, measure: 0 };
      }

      $scope.addItem = function () {
          var items = $scope.order.items;
          if (items.length === 0 || $scope.hasProduct(items[items.length - 1])) {
              $scope.order.items.push(emptyItem());
              return true;
          }
      };

      $scope.getMatchingCustomer = function ($viewValue) {
          var term = $viewValue.toLowerCase();
          return $.grep($scope.customers, function (it) {
              return it.name.toLowerCase().indexOf(term) != -1 || it.code.toLowerCase().indexOf(term) == 0;
          });
      };

      $scope.setCustomer = function (order) {
          $scope.order.personId = order.customer.id;
      };

      $scope.clearCustomer = function () {
          $scope.order.personId = null;
          $scope.order.customer = null;
      };

      $scope.setProduct = function (item) {
          var res = $filter('filter')($scope.products, { id: item.product.id }, true);
          if (res.length > 0) {
              var product = res[0];
              item.productId = product.id;
              item.measure = product.measure != null ? product.measure : 0;
          }
      };

      $scope.clearProduct = function (item) {
          item.product = null;
          item.productId = null;
          item.measure = 0;
      };

      $scope.deleteItem = function (index) {
          $scope.order.items.splice(index, 1);
      };

      $scope.print = function (order) {
          if ($scope.sending || !order.customer) return;
          var items = $.grep(order.items, function (it) {
              return hasValue(it.product) && hasValue(it.quantity);
          });
          if (items.length === 0) return;

          $scope.sending = true;
          Notification.success('Imprimiendo pedido...');
          PrintHelper.print('Order', JSON.stringify($scope.getPrintModel(order, items)));
          $scope.sending = false;
          $state.reload();
      };

      $scope.getPrintModel = function (order, items) {
          var model = {
              date: new Date(),
              customerCode: order.customer.code,
              customerName: order.customer.name,
              items: []
          };
          angular.forEach(items, function (item) {
              model.items.push({
                  product: item.product.name,
                  quantity: item.quantity,
                  measure: Number(item.measure) === 1 ? 'Un' : 'Kg'
              });
          });
          return model;
      };

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
          return hasValue(item.product) && hasValue(item.productId);
      };

      $scope.AddItem = function (item) {
          if ($scope.hasProduct(item) && $scope.hasQuantity(item)) {
              $scope.addItem();
              return false;
          }
      };
  });
