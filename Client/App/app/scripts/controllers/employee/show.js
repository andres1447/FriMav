'use strict';

angular.module('client')
  .controller('EmployeeShowCtrl', function ($scope, $state, hotkeys, Notification, customer, accountEntries) {
      $scope.entryIndex = 0;
      $scope.accountEntries = accountEntries;
      $scope.customer = customer;

      hotkeys.bindTo($scope)
        .add({
          combo: '+',
          description: 'Nuevo pago',
          persistent: false,
          callback: function (e) {
              $state.go('PaymentCreate', { personId: customer.personId });
          }
        })
        .add({
          combo: '-',
          description: 'Anular la entrada',
          persistent: false,
          callback: function (e) {
              var entry = $scope.accountEntries[$scope.entryIndex];
              $scope.refund(entry);
              e.preventDefault();
          }
      })
      .add({
          combo: 'enter',
          description: 'Detalles',
          persistent: false,
          callback: function (e) {
              var entry = $scope.accountEntries[$scope.entryIndex];
              $scope.showEntry(entry)
              e.preventDefault();
          }
      })
      .add({
          combo: 'up',
          description: 'Mover arriba',
          persistent: false,
          callback: function (e) {
              if ($scope.entryIndex > 0) {
                  $scope.entryIndex--;
                  e.preventDefault();
              }
          }
      })
      .add({
          combo: 'down',
          description: 'Mover abajo',
          persistent: false,
          callback: function (e) {
              if ($scope.entryIndex < accountEntries.length - 1) {
                  $scope.entryIndex++;
                  e.preventDefault();
              }
          }
      })
      .add({
          combo: 'esc',
          description: 'Volver a cliente',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
              $state.go('CustomerIndex');
              e.preventDefault();
          }
      });

      $scope.description = function (entry) {
          switch (entry.transactionType) {
              case 1: return 'Factura #' + entry.entryId;
              case 2: return 'Pago #' + entry.entryId;
              case 3: return 'Reembolso de factura #' + entry.referenceId;
          }
      };

      $scope.showEntry = function (entry) {
          switch (entry.transactionType) {
              case 1: $state.go('InvoiceShow', { invoiceId: entry.entryId }); break;
              case 2: $state.go('PaymentShow', { paymentId: entry.entryId }); break;
              case 3: $state.go('PaymentShow', { paymentId: entry.entryId }); break;
          }
      };

      $scope.refund = function (entry) {
          switch (entry.transactionType) {
              case 1: $state.go('InvoiceRefund', { invoiceId: entry.entryId }); break;
              case 2: $state.go('PaymentCancel', { paymentId: entry.entryId }); break;
              case 3: $state.go('PaymentCancel', { paymentId: entry.entryId }); break;
          }
      };
  });
