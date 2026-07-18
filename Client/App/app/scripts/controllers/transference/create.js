'use strict';

angular.module('client')
  .controller('TransferenceCreateCtrl', function ($scope, $state, hotkeys, Notification, Transference, employees, $filter, Employee) {
      $scope.employees = orderByCode($filter, employees);
      $scope.balance = 0;

      $scope.transference = {
        date: new Date()
      };

      hotkeys.bindTo($scope)
      .add({
          combo: 'f5',
          description: 'Guardar',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function () {
            $scope.create($scope.transference);
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
           $scope.broadcast('InitTransferenceCreate');
      };

      $scope.init();

      $scope.create = function (transference) {
        if (!$scope.sending) {
          $scope.sending = true;
          Transference.save(transference, function (res) {
            $scope.sending = false;
            Notification.success('Transferencia creada correctamente.');
            PrintHelper.print('Transference', JSON.stringify(getPrintModel(transference)));
            $state.reload();
          }, function (err) {
            $scope.sending = false;
            Notification.error(err.data);
          });
        }
      };

    function getPrintModel(transference) {
      return {
        employeeCode: transference.employee.code,
        employeeName: transference.employee.name,
        date: transference.date,
        description: transference.description,
        amount: transference.amount
      }
    }

    $scope.getMatchingEmployees = function ($viewValue) {
      var term = $viewValue.toLowerCase();
      return $.grep($scope.employees, function (it) {
        return it.name.toLowerCase().indexOf(term) != -1 || it.code.toLowerCase().indexOf(term) == 0;
      });
    };

    $scope.setEmployee = function () {
      var id = $scope.transference.employee.id
      $scope.transference.employeeId = id;
      Employee.unliquidated({ id: id }).$promise.then(function (result) {
        $scope.balance = result.length > 0
          ? result[result.length - 1].balance
          : 0
      });
    };

    $scope.clearEmployee = function () {
      $scope.transference.employeeId = null;
      $scope.transference.employee = null;
    }
});
