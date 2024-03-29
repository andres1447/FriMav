'use strict';

angular.module('client')
  .controller('CustomerShowCtrl', function ($scope, $state, hotkeys, customer, Transaction, Notification, $uibModal) {
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
        combo: 'f5',
        description: 'Imprimir',
        persistent: false,
        callback: function (e) {
          $scope.print();
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
              if ($scope.transactionIndex < $scope.transactions.length - 1) {
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
          $scope.prevPage();
        }
      })
      .add({
        combo: 'pagedown',
        description: 'Cargar transacciones mas recientes',
        allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
        persistent: false,
        callback: function (e) {
          e.preventDefault();
          $scope.nextPage();
        }
      });

      $scope.prevPage = function () {
        if ($scope.pageNumber >= $scope.totalPages - 1)
          return;

        $scope.pageNumber++;
        $scope.loadTransactions();
      }

      $scope.nextPage = function () {
        if ($scope.pageNumber <= 0)
          return;

        $scope.pageNumber--;
        $scope.loadTransactions();
      }

      $scope.loadTransactions = function () {
        Transaction.account({ id: customer.id, offset: $scope.pageNumber, count: $scope.itemsPerPage }).$promise.then(function (response) {
          $scope.transactions = response.items;
          $scope.totalCount = response.totalCount;
          $scope.totalPages = Math.ceil($scope.totalCount / $scope.itemsPerPage);
          if ($scope.transactionIndex >= $scope.transactions.length)
            $scope.transactionIndex = 0;
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
        return 'Factura #' + transaction.number
          + (transaction.ExternalReferenceNumber ? ' [Ticket: ' + ExternalReferenceNumber + ']' : '')
          + (transaction.isRefunded ? ' (Cancelada)' : '');
      }

      function paymentDescription(transaction) {
          return 'Pago #' + transaction.number + (transaction.isRefunded ? ' (Anulado)' : '');
      }

      function creditNoteDescription(transaction) {
        return 'Nota de crédito #{0}'.format(transaction.number) +
          (transaction.refundedDocument ? ' ({0} #{1})'.format(
          transactionTypeDescription(transaction.refundedDocument.transactionType),
          transaction.refundedDocument.number) : '')
      }

      function debitNoteDescription(transaction) {
        return 'Nota de débito #{0}'.format(transaction.number) +
          (transaction.refundedDocument ? ' ({0} #{1})'.format(
            transactionTypeDescription(transaction.refundedDocument.transactionType),
            transaction.refundedDocument.number) : '')
      }

      $scope.print = function () {
        PrintHelper.print('CustomerAccount', JSON.stringify(getPrintModel()));
        Notification.success('Factura guardada correctamente.');
      }

      function getPrintModel() {
        return {
          customerCode: customer.code,
          customerName: customer.name,
          deliveryAddress: customer.deliveryAddress,
          balance: customer.balance,
          transactions: $.map($scope.transactions, function (tran) {
            return {
              date: tran.date,
              description: $scope.description(tran) + " " + (tran.description ? tran.description : ''),
              externalDocumentNumber: tran.externalDocumentNumber,
              total: tran.total,
              balance: tran.balance
            }
          })
        }
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

      $scope.updateExternalReferenceNumber = function (transaction) {
        $uibModal.open({
          templateUrl: 'views/transaction/updateExternalReferenceNumber.html',
          controller: 'UpdateExternalReferenceNumberCtrl',
          controllerAs: '$ctrl',
          resolve: {
            transactionDocument: function () {
              return transaction;
            }
          }
        }).result.then(function () {
            $scope.loadTransactions();
        }, function () { });
        $scope.broadcast('InitUpdateExternalReferenceNumber')
      }
  });
