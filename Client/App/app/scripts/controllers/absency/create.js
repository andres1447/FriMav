'use strict';

angular.module('client')
  .controller('AbsencyCreateCtrl', function ($scope, $state, hotkeys, Notification, Absency, employees, $filter) {
      $scope.employees = orderByCode($filter, employees);

      $scope.absency = {
        date: new Date()
      };

      hotkeys.bindTo($scope)
      .add({
          combo: 'f5',
          description: 'Guardar',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function () {
            $scope.create($scope.absency);
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
           $scope.broadcast('InitAbsencyCreate');
      };

      $scope.init();
      
      $scope.create = function (absency) {
          if (!$scope.sending) {
              $scope.sending = true;
              Absency.save(absency, function (res) {
                $scope.sending = false;
                Notification.success('Falta creada correctamente.');
                PrintHelper.print('Absency', JSON.stringify(getPrintModel(absency)));
                $state.reload();
              }, function (err) {
                $scope.sending = false;
                Notification.error(err.data);
              });
          }
      };

    function getPrintModel(absency) {
      return {
        employeeCode: absency.employee.code,
        employeeName: absency.employee.name,
        date: absency.date,
        description: absency.description
      }
    }

    $scope.getMatchingEmployees = function ($viewValue) {
      var term = $viewValue.toLowerCase();
      return $.grep($scope.employees, function (it) {
        return it.name.toLowerCase().indexOf(term) != -1 || it.code.toLowerCase().indexOf(term) == 0;
      });
    };

    $scope.setEmployee = function () {
      $scope.absency.employeeId = $scope.absency.employee.id;
    };

    $scope.clearEmployee = function () {
      $scope.absency.employeeId = null;
      $scope.absency.employee = null;
    }
});
