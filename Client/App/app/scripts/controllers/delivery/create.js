'use strict';

angular.module('client')
  .controller('DeliveryCreateCtrl', function ($scope, $state, $filter, hotkeys, Notification, Delivery, employees, invoices) {

      hotkeys.bindTo($scope).add({
          combo: 'f5',
          description: 'Imprimir',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function () {
              $scope.create($scope.delivery);
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
        combo: 'up',
        description: 'Mover arriba',
        allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
        persistent: false,
        callback: function (e) {
          if ($scope.invoiceIndex > 0) {
            $scope.invoiceIndex--;
            e.preventDefault();
          }
        }
      })
      .add({
        combo: 'down',
        description: 'Mover abajo',
        allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
        persistent: false,
        callback: function (e) {
          if ($scope.invoiceIndex < invoices.length - 1) {
            $scope.invoiceIndex++;
            e.preventDefault();
          }
        }
      })
      .add({
        combo: 'space',
        description: 'Seleccionar',
        persistent: false,
        callback: function (e) {
          $scope.selectCurrentInvoice();
        }
      })
      .add({
        combo: 'enter',
        description: 'Seleccionar',
        persistent: false,
        callback: function (e) {
          $scope.selectCurrentInvoice();
        }
      })
      .add({
        combo: '*',
        description: 'Seleccionar Todo',
        persistent: false,
        callback: function (e) {
          $scope.selectAllInvoices();
        }
      });

      $scope.invoices = invoices;
      $scope.employees = orderByCode($filter, employees);
      $scope.invoiceIndex = -1;

      $scope.init = function () {
          $scope.delivery = {
              date: new Date(),
              invoices: [{}]
          };
          $scope.broadcast('DeliveryInit');
      };

      $scope.init();

      $scope.setEmployee = function (delivery) {
        $scope.delivery.employeeId = delivery.employee.id;
        $scope.invoiceIndex = 0;
      };

      $scope.selectCurrentInvoice = function () {
        if (invoices.length > 0 && invoiceIndex >= 0) {
          $scope.invoices[$scope.invoiceIndex].selected = !$scope.invoices[$scope.invoiceIndex].selected;
        }
      }

      $scope.selectAllInvoices = function () {
        if (invoices.length > 0) {
          $.each($scope.invoices, function (idx, it) { it.selected = !it.selected });
        }
      }

      $scope.create = function (delivery) {
          if (!$scope.sending) {
              $scope.sending = true;
              delivery.invoices = getSelectedInvoicesIds();
              Delivery.save(delivery, function (res) {
                  $scope.sending = false;
                  Notification.success('Envio creado correctamente');
                  $state.go('DeliveryIndex');
              });
          }
      };

      function getSelectedInvoicesIds() {
        return $.map($.grep($scope.invoices, function (it) { return it.selected; }), function (it) { return it.id; });
      }

    $scope.getMatchingEmployees = function ($viewValue) {
        var term = $viewValue.toLowerCase();
        return $.grep($scope.employees, function (it) {
          return it.code.toLowerCase().indexOf(term) != -1 || it.name.toLowerCase().indexOf(term) != -1;
        });
      };

    $scope.$watch(function () { return $scope.invoices }, function (newVal) {
      var productMap = {};
      var selected = $.map($.map($.grep($scope.invoices, function (it) { return it.selected }),
        function (select) {
          return select.products;
        }),
        function (product) {
          if (!productMap[product.name])
            productMap[product.name] = 0;
          productMap[product.name] += product.quantity;
        });

      $scope.deliveryProducts = $.map(Object.keys(productMap), function (key) { return { name: key, quantity: productMap[key] } });
    }, true);
  });
