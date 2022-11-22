'use strict';

angular.module('client')
  .controller('BonusCreateCtrl', function ($scope, $state, hotkeys, Notification, employees, $filter) {
      $scope.employees = orderByCode($filter, employees);

      $scope.startDate = function() {
        var date = new Date();
        var diff = date.getDate() - date.getDay() + 8;
        return new Date(date.setDate(diff));
      }

      $scope.bonus = {
        date: $scope.startDate()
      };

      hotkeys.bindTo($scope)
      .add({
          combo: 'f5',
          description: 'Imprimir',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function () {
            $scope.create($scope.bonus);
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
           $scope.broadcast('InitBonusCreate');
      };

      $scope.init();
      
      $scope.create = function (bonus) {
        if (!$scope.sending) {
          $scope.sending = true;
          Notification.success('Aguinaldo creado correctamente.');
          PrintHelper.print('Bonus', JSON.stringify(getPrintModel(bonus)));
          $state.reload();
        }
      };

    function getPrintModel(bonus) {
      return {
        employeeCode: bonus.employee.code,
        employeeName: bonus.employee.name,
        date: bonus.date,
        salary: bonus.employee.salary,
        description: bonus.description
      }
    }

    $scope.getMatchingEmployees = function ($viewValue) {
      var term = $viewValue.toLowerCase();
      return $.grep($scope.employees, function (it) {
        return it.name.toLowerCase().indexOf(term) != -1 || it.code.toLowerCase().indexOf(term) == 0;
      });
    };

    $scope.setEmployee = function () {
      $scope.bonus.employeeId = $scope.bonus.employee.id;
    };

    $scope.clearEmployee = function () {
      $scope.bonus.employeeId = null;
      $scope.bonus.employee = null;
    }
});
