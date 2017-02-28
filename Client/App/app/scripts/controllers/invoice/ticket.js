'use strict';

angular.module('client')
  .controller('TicketCtrl', function ($scope, $state, $filter, $timeout, hotkeys, Notification, products) {
      $scope.products = products;

      hotkeys.bindTo($scope).add({
          combo: 'f5',
          description: 'Imprimir',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function () {
              $scope.print($scope.ticket);
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
          $scope.ticket = {
              date: new Date(),
              items: [{quantity: 0, price: 0}]
          };
          $scope.totals = {};

          $scope.broadcast('InitTicket');
      };

      $scope.init();

      $scope.addItem = function () {
          var items = $scope.ticket.items;
          if (items.length === 0 || hasValue(items[items.length - 1].quantity)) {
              $scope.ticket.items.push({ quantity: 0, price: 0 });
              return true;
          }
      };

      $scope.setListPrice = function ($index) {
          var it = $scope.ticket.items[$index];
          if (it) {
              var res = $filter('filter')($scope.products, { productId: it.product.productId }, true);
              if (res.length > 0) {
                  it.price = res[0].price;
              }
          }
      };

      $scope.print = function (ticket) {
          ticket.items = $.grep(ticket.items, function (it) {
              return hasValue(it.product) && hasValue(it.quantity) && hasValue(it.price);
          });
          PrintHelper.print('Ticket', JSON.stringify($scope.getPrintModel(ticket)));
          Notification.success('Imprimiendo ticket...');
          $state.reload();
      };

      $scope.getPrintModel = function (ticket) {
          var printModel = {
              date: ticket.date,
              items: []
          };
          angular.forEach(ticket.items, function (item) {
              printModel.items.push({ product: item.product.name, quantity: item.quantity, price: item.price });
          });
          return printModel;
      };

      $scope.validateProduct = function ($index) {
          if (!hasValue($scope.ticket.items[$index].product.productId)) {
              return -1;
          }
      };

      $scope.validateQuantity = function ($index) {
          if (!hasValue($scope.ticket.items[$index].quantity)) {
              return false;
          }
      }

      $scope.lineProductValidation = function ($index) {
          if (hasValue($scope.ticket.items[$index].product) && hasValue($scope.ticket.items[$index].product.productId))
              return true;
          return false;
      }

      $scope.AddItem = function ($index) {
          if (hasValue($scope.ticket.items[$index].product) && hasValue($scope.ticket.items[$index].product.productId) && hasValue($scope.ticket.items[$index].quantity) && hasValue($scope.ticket.items[$index].price)) {
              $scope.addItem();
              return false;
          }
      }

      $scope.getMatchingProduct = function ($viewValue) {
          return $.grep($scope.products, function (it) {
              return it.name.toLowerCase().indexOf($viewValue) != -1 || it.code.toLowerCase().indexOf($viewValue) == 0;
          });
      }

      $scope.$watch(function () { return $scope.ticket; }, function (newVal) {
          $scope.totals = { quantity: 0, count: 0, subtotal: 0 };
          angular.forEach(newVal.items, function (item) {
              $scope.totals.subtotal += item.quantity && item.price ? item.quantity * item.price : 0;
              $scope.totals.count += item.quantity && item.price ? 1 : 0;
          });
          $scope.totals.price = $scope.totals.weight ? $scope.totals.subtotal / $scope.totals.weight : 0;
      }, true);
  });
