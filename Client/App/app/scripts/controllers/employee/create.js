'use strict';

angular.module('client')
  .controller('EmployeeCreateCtrl', function ($scope, $state, $timeout, hotkeys, Notification, Employee, codes) {
      hotkeys.bindTo($scope)
      .add({
          combo: 'f5',
          description: 'Guardar',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function () {
              $scope.create($scope.employee);
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

      $scope.employee = {
        joinDate: new Date()
      };

      $scope.codes = codes;

      $scope.init = function () {
          $scope.broadcast('InitForm');
      };

      $scope.init();
      
      $scope.create = function (employee) {
          if (!$scope.sending) {
              $scope.sending = true;
              Employee.save(employee, function (res) {
                  $scope.sending = false;
                  Notification.success('Empleado creado correctamente.');
                  $state.go('EmployeeIndex');
              }, function (err) {
                  $scope.sending = false;
                  Notification.error({ title: err.data.message, message: err.data.errors.join('</br>') });
              });
          }
      };
  });
