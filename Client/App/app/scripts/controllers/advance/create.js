'use strict';

angular.module('client')
  .controller('AdvanceCreateCtrl', function ($scope, $state, hotkeys, Notification, Advance, employees, $filter, Employee) {
      $scope.employees = orderByCode($filter, employees);
      $scope.balance = 0;

      $scope.advance = {
        date: new Date()
      };

      hotkeys.bindTo($scope)
      .add({
          combo: 'f5',
          description: 'Guardar',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function () {
            $scope.create($scope.advance);
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
           $scope.broadcast('InitAdvanceCreate');
      };

      $scope.init();
      
      $scope.create = function (advance) {
        if (!$scope.sending) {
          $scope.sending = true;
          Advance.save(advance, function (res) {
            $scope.sending = false;
            Notification.success('Adelanto creado correctamente.');
            PrintHelper.print('Advance', JSON.stringify(getPrintModel(advance)));
            $state.reload();
          }, function (err) {
            $scope.sending = false;
            Notification.error(err.data);
          });
        }
      };

    function getPrintModel(advance) {
      return {
        employeeCode: advance.employee.code,
        employeeName: advance.employee.name,
        date: advance.date,
        description: advance.description,
        amount: advance.amount
      }
    }

    $scope.getMatchingEmployees = function ($viewValue) {
      var term = $viewValue.toLowerCase();
      return $.grep($scope.employees, function (it) {
        return it.name.toLowerCase().indexOf(term) != -1 || it.code.toLowerCase().indexOf(term) == 0;
      });
    };

    $scope.setEmployee = function () {
      var id = $scope.advance.employee.id
      $scope.advance.employeeId = id;
      Employee.unliquidated({ id: id }).$promise.then(function (result) {
        $scope.balance = result.length > 0
          ? result[result.length - 1].balance
          : 0
      });
    };

    $scope.clearEmployee = function () {
      $scope.advance.employeeId = null;
      $scope.advance.employee = null;
    }
});
