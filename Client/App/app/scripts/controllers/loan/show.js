'use strict';

angular.module('client')
  .controller('LoanShowCtrl', function ($scope, $state, hotkeys, Notification, ModalService, Loan, loan) {
      $scope.loan = loan;
      $scope.canCancel = $.grep(loan.fees, function (f) { return f.isLiquidated }).length === 0;

      hotkeys.bindTo($scope)
      .add({
          combo: '-',
          description: 'Eliminar préstamo',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function () {
            $scope.delete($scope.loan);
          }
      })
      .add({
          combo: 'esc',
          description: 'Volver a empleados',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
            $state.go('EmployeeIndex');
            e.preventDefault();
          }
      });

      $scope.init = function () {
          $scope.broadcast('LoanShow');
      };

      $scope.init();

      $scope.delete = function (loan) {
        if (!$scope.canCancel) return;

        ModalService.show({ title: 'Eliminar préstamo', message: 'Desea eliminar el préstamo?' }).then(function (res) {
          Loan.delete({ id: loan.id }, function (res) {
            Notification.success('Préstamo eliminado correctamente.');
            $state.go('EmployeeIndex');
          }, function (err) {
            Notification.error(err.data);
          });
        }, function () { });
      };
});
