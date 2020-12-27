'use strict';

angular.module('client')
  .controller('CustomerShowCtrl', function ($scope, $state, hotkeys, Notification, customer, transactions) {
      $scope.transactionIndex = 0;
      $scope.transactions = transactions;
      $scope.customer = customer;

      hotkeys.bindTo($scope)
        .add({
          combo: '+',
          description: 'Nuevo pago',
          persistent: false,
          callback: function (e) {
            $state.go('CustomerPaymentCreate', { id: customer.id });
          }
        })
        .add({
          combo: '-',
          description: 'Anular la entrada',
          persistent: false,
          callback: function (e) {
              var entry = $scope.transactions[$scope.transactionIndex];
              $scope.refund(entry);
              e.preventDefault();
          }
      })
      .add({
          combo: 'enter',
          description: 'Detalles',
          persistent: false,
          callback: function (e) {
              var entry = $scope.transactions[$scope.transactionIndex];
              $scope.showEntry(entry)
              e.preventDefault();
          }
      })
      .add({
          combo: 'up',
          description: 'Mover arriba',
          persistent: false,
          callback: function (e) {
              if ($scope.transactionIndex > 0) {
                  $scope.transactionIndex--;
                  e.preventDefault();
              }
          }
      })
      .add({
          combo: 'down',
          description: 'Mover abajo',
          persistent: false,
          callback: function (e) {
              if ($scope.transactionIndex < transactions.length - 1) {
                  $scope.transactionIndex++;
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

      function transactionTypeDescription(type) {
          switch (type) {
              case 1: return 'Factura';
              case 2: return 'Pago';
              case 3: return 'N/C';
              case 4: return 'N/D';
          }
      }

      $scope.description = function (transaction) {
          switch (transaction.transactionType) {
              case 1: return 'Factura #' + transaction.number;
              case 2: return 'Pago #' + transaction.number;
              case 3: return 'N/C #{0} - Reembolso de {1} #{2}'.format(transaction.number, transactionTypeDescription(transaction.reference.transactionType), transaction.reference.number);
              case 4: return 'N/D #{0} - Anulacion de {1} #{2}'.format(transaction.number, transactionTypeDescription(transaction.reference.transactionType), transaction.reference.number);
          }
      };

      $scope.showEntry = function (transaction) {
          if (transaction.transactionType == 1) {
              $state.go('InvoiceShow', { id: transaction.id });
          }
      };

      $scope.refund = function (transaction) {
          $state.go('TransactionRefund', { id: transaction.id });
      };
  });
