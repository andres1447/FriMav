'use strict';

angular.module('client')
  .controller('DeliveryCreateCtrl', function ($scope, $state, $filter, $timeout, hotkeys, Notification, Delivery, employees, invoices) {
      $scope.invoices = invoices;
      $scope.employees = employees;

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
      });

      $scope.init = function () {
          $scope.delivery = {
              date: new Date(),
              invoices: [{}]
          };
          $scope.broadcast('InitForm');
      };

      $scope.init();

      $scope.AddItem = function ($index) {
        if (hasValue($scope.delivery.invoices[$index].invoice.id)) {
              $scope.delivery.invoices.push({});
              return false;
          }
      };

      $scope.setEmployee = function (delivery) {
        $scope.delivery.employeeId = delivery.employee.id;
      };

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
        return $.map($.grep($scope.delivery.invoices, function (it) { return it.invoice ? it.invoice.id : it.invoice; }), function (it) { return it.invoice.id; });
      }

      $scope.getMatchingInvoices = function ($viewValue) {
          return $.grep($scope.invoices, function (it) {
            return it.personCode.toLowerCase().indexOf($viewValue) == 0 && getSelectedInvoicesIds().indexOf(it.id) == -1;
          });
      };

      $scope.getMatchingEmployees = function ($viewValue) {
          return $.grep($scope.employees, function (it) {
              return it.code.toLowerCase().indexOf($viewValue) == 0;
          });
      };
  });
