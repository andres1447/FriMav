'use strict';

angular.module('client')
  .controller('PaymentCreateCtrl', function ($scope, $state, $timeout, hotkeys, Notification, Payment, customer) {
      $scope.payment = {
          date: new Date(),
          personId: customer.personId
      };

      hotkeys.bindTo($scope)
      .add({
          combo: 'f5',
          description: 'Guardar',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function () {
              $scope.create($scope.payment);
          }
      })
      .add({
          combo: 'esc',
          description: 'Volver a cliente',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
              $state.go('CustomerShow', { personId: customer.personId });
              e.preventDefault();
          }
      });

      $scope.init = function () {
           $scope.broadcast('InitPaymentCreate');
      };

      $scope.init();
      
      $scope.create = function (payment) {
          if (!$scope.sending) {
              $scope.sending = true;
              Payment.save(payment, function (res) {
                  $scope.sending = false;
                  Notification.success('Pago creado correctamente.');
                  $state.go('CustomerShow', { personId: payment.personId });
              }, function (err) {
                  $scope.sending = false;
                  Notification.error(err.data);
              });
          }
      };
  });
