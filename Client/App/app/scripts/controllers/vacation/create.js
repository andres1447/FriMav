'use strict';

angular.module('client')
  .controller('VacationCreateCtrl', function ($scope, $state, hotkeys, Notification, employees, $filter) {
      $scope.employees = orderByCode($filter, employees);

      $scope.startDate = function() {
        var date = new Date();
        var diff = date.getDate() - date.getDay() + 8;
        return new Date(date.setDate(diff));
      }

      $scope.vacation = {
        date: $scope.startDate()
      };

      hotkeys.bindTo($scope)
      .add({
          combo: 'f5',
          description: 'Imprimir',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function () {
            $scope.create($scope.vacation);
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
           $scope.broadcast('InitVacationCreate');
      };

      $scope.init();
      
      $scope.create = function (vacation) {
        if (!$scope.sending) {
          $scope.sending = true;
          Notification.success('Vacaciones creada correctamente.');
          PrintHelper.print('Vacation', JSON.stringify(getPrintModel(vacation)));
          $state.reload();
        }
      };

    function getPrintModel(vacation) {
      return {
        employeeCode: vacation.employee.code,
        employeeName: vacation.employee.name,
        date: vacation.date,
        weeks: vacation.weeks,
        salary: vacation.employee.salary,
        description: vacation.description
      }
    }

    $scope.getMatchingEmployees = function ($viewValue) {
      var term = $viewValue.toLowerCase();
      return $.grep($scope.employees, function (it) {
        return it.name.toLowerCase().indexOf(term) != -1 || it.code.toLowerCase().indexOf(term) == 0;
      });
    };

    $scope.setEmployee = function () {
      $scope.vacation.employeeId = $scope.vacation.employee.id;
    };

    $scope.clearEmployee = function () {
      $scope.vacation.employeeId = null;
      $scope.vacation.employee = null;
    }
});
