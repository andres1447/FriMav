'use strict';

angular.module('client')
  .controller('EmployeeIndexCtrl', function ($scope, $state, ModalService, hotkeys, Employee, Notification, employees, $filter) {
      $scope.index = 0;
      $scope.employees = orderByCode($filter, employees);

      hotkeys.bindTo($scope).add({
          combo: '+',
          description: 'Nuevo empleado',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
              $state.go('EmployeeCreate');
              e.preventDefault();
          }
      })
      .add({
        combo: 'f5',
        description: 'Falta',
        allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
        persistent: false,
        callback: function (e) {
          $state.go('AbsencyCreate');
          e.preventDefault();
        }
      })
      .add({
        combo: 'f6',
        description: 'Adelanto',
        allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
        persistent: false,
        callback: function (e) {
          $state.go('AdvanceCreate');
          e.preventDefault();
        }
      })
      .add({
        combo: 'f7',
        description: 'Mercadería',
        allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
        persistent: false,
        callback: function (e) {
          $state.go('GoodsSoldCreate');
          e.preventDefault();
        }
      })
      .add({
        combo: 'f8',
        description: 'Préstamo',
        allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
        persistent: false,
        callback: function (e) {
          $state.go('LoanCreate');
          e.preventDefault();
        }
      })
      .add({
        combo: 'f9',
        description: 'Liquidar sueldos',
        allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
        persistent: false,
        callback: function (e) {
          $state.go('Payroll');
          e.preventDefault();
        }
      })
      .add({
          combo: '-',
          description: 'Borrar empleado',
          persistent: false,
          callback: function (e) {
                $scope.delete($scope.index);
                e.preventDefault();
          }
      })
      .add({
          combo: 'enter',
          description: 'Detalles',
          persistent: false,
          callback: function (e) {
            $state.go('EmployeeShow', { id: $scope.employees[$scope.index].id });
              e.preventDefault();
          }
      })
      .add({
          combo: '*',
          description: 'Editar empleado',
          persistent: false,
          callback: function (e) {
            $state.go('EmployeeUpdate', { id: $scope.employees[$scope.index].id });
              e.preventDefault();
          }
      })
      .add({
          combo: 'up',
          description: 'Mover arriba',
          persistent: false,
          callback: function (e) {
              if ($scope.index > 0) {
                  $scope.index--;
                  e.preventDefault();
              }
          }
      })
      .add({
          combo: 'down',
          description: 'Mover abajo',
          persistent: false,
          callback: function (e) {
              if ($scope.index < employees.length - 1) {
                  $scope.index++;
                  e.preventDefault();
              }
          }
      });

      $scope.init = function () {
          setTimeout(function () {
              $scope.$broadcast('InitForm');
          }, 50);
      };

      $scope.init();
      
      $scope.delete = function (index) {
          ModalService.show({ title: 'Empleados', message: 'Desea borrar al empleado?' }).then(function (res) {
            Employee.delete({ id: $scope.employees[index].id }, function (res) {
                  Notification.success('Empleado borrado correctamente.');
                  $state.reload();
              }, function (err) {
                  Notification.error(err.data);
              });
          }, function () { });
      };
  });
