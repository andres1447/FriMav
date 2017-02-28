'use strict';

angular.module('client')
  .controller('PaymentCancelCtrl', function ($scope, $state, $timeout, hotkeys, Notification, Transaction, transaction) {
      $scope.cancelation = {
          date: new Date(),
          transactionId: transaction.transactionId
      };

      hotkeys.bindTo($scope)
      .add({
          combo: 'f5',
          description: 'Guardar',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function () {
              $scope.cancel($scope.cancelation);
          }
      })
      .add({
          combo: 'esc',
          description: 'Volver a cliente',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
              $state.go('CustomerShow', { personId: transaction.personId });
              e.preventDefault();
          }
      });

      $scope.init = function () {
           $scope.broadcast('InitPaymentCreate');
      };

      $scope.init();
      
      $scope.cancel = function (cancelation) {
          Transaction.cancel(cancelation, function (res) {
              Notification.success('Pago cancelado correctamente.');
              $state.go('CustomerShow', { personId: transaction.personId });
          }, function (err) {
              Notification.error(err.data);
          });
      };
  });
