'use strict';

angular.module('client')
  .controller('GoodsSoldCreateCtrl', function ($scope, $state, $filter, hotkeys, Notification, products, employees, GoodsSold) {
      $scope.products = orderByCode($filter, products);
      $scope.employees = orderByCode($filter, employees);
      $scope.discount = 10;

      hotkeys.bindTo($scope).add({
          combo: 'f5',
          description: 'Imprimir',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function () {
            $scope.print($scope.goodsSold);
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
        if ($scope.goodsSold.items.length > 1) {
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
          $scope.goodsSold = {
              date: new Date(),
              items: [{quantity: 1, price: 0}]
          };
          $scope.totals = {};

          $scope.broadcast('InitGoodsSoldCreate');
      };

      $scope.init();

      $scope.addItem = function () {
        var items = $scope.goodsSold.items;
        if (items.length === 0 || $scope.hasProduct(items[items.length - 1])) {
          $scope.goodsSold.items.push({ quantity: 1, price: 0 });
          return true;
        }
      };

      $scope.setListPrice = function (item) {
        var res = $filter('filter')($scope.products, { id: item.product.id }, true);
        if (res.length > 0) {
          var product = res[0];
          item.price = product.price * (1 - $scope.discount / 100);
          item.productId = product.id;
        }
      };

      $scope.updatePrices = function (goods) {
        $.each(goods.items, function (_, item) {
          $scope.setListPrice(item)
        });
      }

      $scope.clearProduct = function (item) {
        item.product = null;
        item.productId = null;
        item.price = 0;
        item.basePrice = 0;
      }

      $scope.deleteItem = function (index) {
        $scope.goodsSold.items.splice(index, 1);
      }

      $scope.submit = function (goodsSold) {
        goodsSold.items = $.grep(goodsSold.items, function (it) {
          return hasValue(it.product) && hasValue(it.quantity) && hasValue(it.price);
        });
        GoodsSold.save(goodsSold, function (res) {
          PrintHelper.print('EmployeeTicket', JSON.stringify($scope.getPrintModel(goodsSold)));
          Notification.success('Imprimiendo ticket...');
          $state.reload();
        })
      };

      $scope.getPrintModel = function (goodsSold) {
          var printModel = {
              date: goodsSold.date,
              employeeCode: goodsSold.employee.code,
              employeeName: goodsSold.employee.name,
              items: []
          };
          angular.forEach(goodsSold.items, function (item) {
              printModel.items.push({ product: item.product.name, quantity: item.quantity, price: item.price });
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

      $scope.getMatchingEmployees = function ($viewValue) {
        var term = $viewValue.toLowerCase();
        return $.grep($scope.employees, function (it) {
          return it.name.toLowerCase().indexOf(term) != -1 || it.code.toLowerCase().indexOf(term) == 0;
        });
      };

      $scope.setEmployee = function () {
        $scope.goodsSold.employeeId = $scope.goodsSold.employee.id;
      };

      $scope.clearEmployee = function () {
        $scope.goodsSold.employeeId = null;
        $scope.goodsSold.employee = null;
      }

      $scope.$watch(function () { return $scope.goodsSold; }, function (newVal) {
          $scope.totals = { quantity: 0, count: 0, subtotal: 0 };
          angular.forEach(newVal.items, function (item) {
              $scope.totals.subtotal += item.quantity && item.price ? item.quantity * item.price : 0;
              $scope.totals.count += item.quantity && item.price ? 1 : 0;
          });
          $scope.totals.price = $scope.totals.weight ? $scope.totals.subtotal / $scope.totals.weight : 0;
      }, true);
  });
