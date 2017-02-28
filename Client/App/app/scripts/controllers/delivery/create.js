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
          if (hasValue($scope.delivery.invoices[$index].invoice.transactionId)) {
              $scope.delivery.invoices.push({});
              return false;
          }
      };

      $scope.setEmployee = function (delivery) {
          $scope.delivery.employeeId = delivery.employee.personId;
      };

      $scope.create = function (delivery) {
          delivery.invoices = getSelectedInvoicesIds();
          Delivery.save(delivery, function (res) {
              Notification.success('Envio creado correctamente');
              $state.go('DeliveryIndex');
          });
      };

      function getSelectedInvoicesIds() {
          return $.map($.grep($scope.delivery.invoices, function (it) { return it.invoice ? it.invoice.transactionId : it.invoice; }), function (it) { return it.invoice.transactionId; });
      }

      $scope.getMatchingInvoices = function ($viewValue) {
          return $.grep($scope.invoices, function (it) {
              return it.person.code.toLowerCase().indexOf($viewValue) == 0 && getSelectedInvoicesIds().indexOf(it.transactionId) == -1;
          });
      };

      $scope.getMatchingEmployees = function ($viewValue) {
          return $.grep($scope.employees, function (it) {
              return it.code.toLowerCase().indexOf($viewValue) == 0;
          });
      };
  });
