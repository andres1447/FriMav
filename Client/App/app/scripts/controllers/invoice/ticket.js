'use strict';

angular.module('client')
  .controller('TicketCtrl', function ($scope, $state, $filter, hotkeys, Notification, products, $stateParams, Invoice) {
      $scope.products = orderByCode($filter, products);
      $scope.previousTicket = $stateParams.previousTicket;

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
      })
      .add({
      combo: 'del',
      allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
      description: 'Delete Row',
      callback: function (event, hotkey) {
        var $elem = $(event.srcElement).parents("tr:first");
        if (!$elem) return;

        event.preventDefault();
        if ($scope.ticket.items.length > 1) {
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
          $scope.ticket = {
              date: new Date(),
              items: [{quantity: 1, price: 0}]
          };
          $scope.totals = {};

          $scope.broadcast('InitTicket');
      };

      $scope.init();

      $scope.addItem = function () {
          var items = $scope.ticket.items;
          if (items.length === 0 || $scope.hasProduct(items[items.length - 1])) {
              $scope.ticket.items.push({ quantity: 1, price: 0 });
              return true;
          }
      };

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
        $scope.ticket.items.splice(index, 1);
      }

      $scope.print = function (ticket) {
        if ($scope.printing) return;

        $scope.printing = true;
        ticket.items = $.grep(ticket.items, function (it) {
            return hasValue(it.product) && hasValue(it.quantity) && hasValue(it.price);
        });
        Notification.success('Imprimiendo ticket...');
        var model = $scope.getPrintModel(ticket);
        Invoice.ticket(model, function () {

        });
        $scope.sendToPrinter(model);
        $scope.printing = false;
        $state.go($state.current, { previousTicket: model }, {
          reload: true, inherit: false, notify: true
        });
      };

      $scope.sendToPrinter = function (model) {
        PrintHelper.print('Ticket', JSON.stringify(model));
      }

      $scope.cancel = function (model) {
        if ($scope.canceling) return;
        Notification.info('Anulando ticket...');
        $scope.canceling = true;
        Invoice.cancelTicket(model, function (res) {
          $scope.canceling = false;
          $state.go($state.current, { }, {
            reload: true, inherit: false, notify: true
          });
        });
      }

      $scope.getPrintModel = function (ticket) {
          var printModel = {
              date: ticket.date,
              total: 0,
              items: []
          };
          angular.forEach(ticket.items, function (item) {
            printModel.items.push({ productId: item.product.id, product: item.product.name, quantity: item.quantity, price: item.price });
            printModel.total += item.quantity * item.price;
          });
          return printModel;
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

      $scope.getMatchingProduct = function ($viewValue) {
        var term = $viewValue.toLowerCase();
        return $.grep($scope.products, function (it) {
            return it.name.toLowerCase().indexOf(term) != -1 || it.code.toLowerCase().indexOf(term) == 0;
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
