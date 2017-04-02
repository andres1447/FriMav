'use strict';

angular.module('client')
  .controller('EmployeeUpdateCtrl', function ($scope, $state, $timeout, hotkeys, Notification, Employee, employee) {
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
              $state.go('CustomerIndex');
              e.preventDefault();
          }
      });

      $scope.employee = employee;

      $scope.init = function () {
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
