'use strict';

angular.module('client')
  .controller('CustomerPaymentCreateCtrl', function ($scope, $state, hotkeys, Notification, Payment, customer) {
      $scope.payment = {
        personId: customer.id
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
            $state.go('CustomerShow', { id: customer.id });
              e.preventDefault();
          }
      });

      $scope.init = function () {
          $scope.broadcast('InitCustomerPaymentCreate');
      };

      $scope.init();
      
      $scope.create = function (payment) {
          if (!$scope.sending) {
              $scope.sending = true;
              Payment.save(payment, function (res) {
                  $scope.sending = false;
                  Notification.success('Pago creado correctamente.');
                $state.go('CustomerShow', { id: payment.personId });
              }, function (err) {
                  $scope.sending = false;
                  Notification.error(err.data);
              });
          }
      };
  });
