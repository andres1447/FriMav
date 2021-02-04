'use strict';

angular.module('client')
  .controller('EmployeeUpdateCtrl', function ($scope, $state, $timeout, hotkeys, Notification, Employee, employee, codes) {
      hotkeys.bindTo($scope)
      .add({
          combo: 'f5',
          description: 'Guardar',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function () {
              $scope.update($scope.employee);
          }
      })
      .add({
          combo: 'esc',
          description: 'Volver al indice',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
              $state.go('EmployeeIndex');
              e.preventDefault();
          }
      });

      $scope.codes = codes.remove(employee.code);

      $scope.init = function () {
          employee.joinDate = new Date(employee.joinDate);
          $scope.employee = employee;
          $scope.broadcast('InitForm');
      };

      $scope.init();
      
      $scope.update = function (employee) {
          if (!$scope.sending) {
              $scope.sending = true;
              Employee.update(employee, function (res) {
                  $scope.sending = false;
                  Notification.success('Empleado editado correctamente.');
                  $state.go('EmployeeIndex');
              }, function (err) {
                  $scope.sending = false;
                  Notification.error({ title: err.data.message, message: err.data.errors.join('</br>') });
              });
          }
      };
  });
