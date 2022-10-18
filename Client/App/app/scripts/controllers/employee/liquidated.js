'use strict';

angular.module('client')
  .controller('EmployeeLiquidatedCtrl', function ($scope, $state, hotkeys, employee, Employee) {
      $scope.entryIndex = 0;
      $scope.liquidatedDocuments = [];
      $scope.pageNumber = 0
      $scope.itemsPerPage = 20;
      $scope.totalCount = 0;
      $scope.totalPages = 0;
      $scope.employee = employee;

      hotkeys.bindTo($scope)
      .add({
          combo: 'enter',
          description: 'Detalles',
          persistent: false,
          callback: function (e) {
            var entry = $scope.liquidatedDocuments[$scope.entryIndex];
              $scope.showEntry(entry)
              e.preventDefault();
          }
      })
      .add({
          combo: 'up',
          description: 'Mover arriba',
          persistent: false,
          callback: function (e) {
              if ($scope.entryIndex > 0) {
                  $scope.entryIndex--;
                  e.preventDefault();
              }
          }
      })
      .add({
          combo: 'down',
          description: 'Mover abajo',
          persistent: false,
          callback: function (e) {
              if ($scope.entryIndex < $scope.liquidatedDocuments.length - 1) {
                  $scope.entryIndex++;
                  e.preventDefault();
              }
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
      })
      .add({
        combo: 'pageup',
        description: 'Cargar transacciones anteriores',
        allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
        persistent: false,
        callback: function (e) {
          e.preventDefault();
          $scope.prevPage();
        }
      })
      .add({
        combo: 'pagedown',
        description: 'Cargar transacciones mas recientes',
        allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
        persistent: false,
        callback: function (e) {
          e.preventDefault();
          $scope.nextPage();
        }
      });

      $scope.nextPage = function () {
        if ($scope.pageNumber <= 0)
          return;

        $scope.pageNumber--;
        $scope.loadLiquidated();
      }

      $scope.prevPage = function () {
        if ($scope.pageNumber >= $scope.totalPages - 1)
          return;

        $scope.pageNumber++;
        $scope.loadLiquidated();
      }

      $scope.loadLiquidated = function () {
        Employee.liquidated({ id: employee.id, offset: $scope.pageNumber, count: $scope.itemsPerPage }).$promise.then(function (response) {
          $scope.liquidatedDocuments = response.items;
          $scope.totalCount = response.totalCount;
          $scope.totalPages = Math.ceil($scope.totalCount / $scope.itemsPerPage);
          if ($scope.entryIndex >= $scope.liquidatedDocuments.length)
            $scope.entryIndex = 0;
        });
      }

      $scope.loadLiquidated();

      $scope.description = function (entry) {
        switch (entry.type) {
            case 0: return 'Saldo anterior';
            case 1: return 'Sueldo';
            case 2: return 'Adelanto';
            case 3: return 'Ausencia';
            case 4: return 'Mercadería';
            case 5: return 'Cuota prestamo';
          }
      };

      $scope.showEntry = function (entry) {
          switch (entry.type) {
            case 2: $state.go('AdvanceShow', { id: entry.id }); break;
            case 3: $state.go('AbsencyShow', { id: entry.id }); break;
            case 4: $state.go('GoodsSoldShow', { id: entry.id }); break;
            case 5: $state.go('LoanShow', { id: entry.loanId }); break;
          }
      };

      $scope.isLastTransaction = function (index) {
        return index == $scope.liquidatedDocuments.length - 1 && $scope.pageNumber == 0;
      }

      $scope.canShow = function (entry) {
        return entry.type == 5 || entry.type == 4;
      }
  });
