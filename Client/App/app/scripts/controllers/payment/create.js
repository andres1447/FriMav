'use strict';

angular.module('client')
  .controller('PaymentCreateCtrl', function ($scope, $state, hotkeys, Notification, Payment, customers, $filter) {
      $scope.customers = orderByCode($filter, customers);

      $scope.payment = {
        date: new Date()
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
           $scope.broadcast('InitPaymentCreate');
      };

      $scope.init();
      
      $scope.create = function (payment) {
          if (!$scope.sending) {
              $scope.sending = true;
              Payment.save(payment, function (res) {
                $scope.sending = false;
                Notification.success('Pago creado correctamente.');
                $state.reload();
              }, function (err) {
                $scope.sending = false;
                Notification.error(err.data);
              });
          }
    };

    $scope.getMatchingCustomer = function ($viewValue) {
      var term = $viewValue.toLowerCase();
      return $.grep($scope.customers, function (it) {
        return it.name.toLowerCase().indexOf(term) != -1 || it.code.toLowerCase().indexOf(term) == 0;
      });
    };

    $scope.setPerson = function () {
      $scope.payment.personId = $scope.payment.person.id;
    };

    $scope.clearPerson = function () {
      $scope.payment.personId = null;
      $scope.payment.person = null;
    }
});
