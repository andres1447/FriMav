'use strict';

angular.module('client')
  .controller('CustomerShowCtrl', function ($scope, $state, hotkeys, customer, Transaction) {
      $scope.transactionIndex = 0;

      $scope.transactions = [];
      $scope.pageNumber = 0
      $scope.itemsPerPage = 20;
      $scope.totalCount = 0;
      $scope.totalPages = 0;

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
      })
      .add({
        combo: 'pageup',
        description: 'Cargar transacciones anteriores',
        allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
        persistent: false,
        callback: function (e) {
          e.preventDefault();
          if ($scope.pageNumber >= $scope.totalPages - 1)
            return;

          $scope.pageNumber++;
          $scope.loadTransactions();
        }
      })
      .add({
        combo: 'pagedown',
        description: 'Cargar transacciones mas recientes',
        allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
        persistent: false,
        callback: function (e) {
          e.preventDefault();
          if ($scope.pageNumber <= 0)
            return;

          $scope.pageNumber--;
          $scope.loadTransactions();
        }
      });

      $scope.loadTransactions = function () {
        Transaction.account({ id: customer.id, offset: $scope.pageNumber, count: $scope.itemsPerPage }).$promise.then(function (response) {
          $scope.transactions = response.items;
          $scope.totalCount = response.totalCount;
          $scope.totalPages = Math.ceil($scope.totalCount / $scope.itemsPerPage);
        });
      }

      $scope.isLastTransaction = function (index) {
        return index == $scope.transactions.length - 1 && $scope.pageNumber == 0;
      }

      $scope.loadTransactions();

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
            case 1: return invoiceDescription(transaction);
            case 2: return paymentDescription(transaction);
            case 3: return creditNoteDescription(transaction);
            case 4: return debitNoteDescription(transaction);
          }
      };

      function invoiceDescription(transaction) {
          return 'Factura #' + transaction.number + (transaction.isRefunded ? ' (Cancelada)' : '');
      }

      function paymentDescription(transaction) {
          return 'Pago #' + transaction.number + (transaction.isRefunded ? ' (Anulado)' : '');
      }

      function creditNoteDescription(transaction) {
        return 'Nota de crédito #{0}'.format(transaction.number) +
          (transaction.refundedDocument ? ' - Cancelación de {0} #{1}'.format(
          transactionTypeDescription(transaction.refundedDocument.transactionType),
          transaction.refundedDocument.number) : '')
      }

      function debitNoteDescription(transaction) {
        return 'Nota de débito #{0}'.format(transaction.number) +
          (transaction.refundedDocument ? ' - Anulación de {0} #{1}'.format(
            transactionTypeDescription(transaction.refundedDocument.transactionType),
            transaction.refundedDocument.number) : '')
      }

      $scope.showEntry = function (transaction) {
          if (transaction.transactionType == 1) {
              $state.go('InvoiceShow', { id: transaction.id });
          }
      };

      $scope.refund = function (transaction) {
        if (!transaction.isRefunded)
            $state.go('TransactionRefund', { id: transaction.id });
      };
  });
