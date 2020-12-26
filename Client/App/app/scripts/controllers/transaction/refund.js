'use strict';

angular.module('client')
  .controller('TransactionRefundCtrl', function ($scope, $state, $timeout, hotkeys, Notification, Transaction, transaction) {
      $scope.createRefund = {
          date: new Date(),
        id: transaction.id
      };

      $scope.transaction = transaction;

      hotkeys.bindTo($scope)
      .add({
          combo: 'f5',
          description: 'Guardar',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function () {
              $scope.refund($scope.createRefund);
          }
      })
      .add({
          combo: 'esc',
          description: 'Volver a cliente',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
            $state.go('CustomerShow', { id: transaction.personId });
              e.preventDefault();
          }
      });

      $scope.init = function () {
          $scope.broadcast('InitRefund');
      };

      $scope.init();
      
      $scope.getTransactionType = function (transaction) {
          switch (transaction.transactionType) {
              case 1: return 'Factura';
              case 2: return 'Pago';
              case 3: return 'Nota de credito';
              case 4: return 'Nota de debito';
          }
      };

      $scope.refund = function (createRefund) {
          if (!$scope.sending) {
              $scope.sending = true;
              Transaction.cancel(createRefund, function (res) {
                  $scope.sending = false;
                  Notification.success('Cancelacion creada correctamente.');
                $state.go('CustomerShow', { id: transaction.personId });
              }, function (err) {
                  $scope.sending = false;
                  Notification.error({ title: err.data.message, message: err.data.errors.join('</br>') });
              });
          }
      };
  });
