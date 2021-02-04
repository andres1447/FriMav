'use strict';

angular.module('client')
  .controller('EmployeeShowCtrl', function ($scope, $state, hotkeys, Notification, employee, unliquidatedDocuments, Advance, Absency, GoodsSold, Loan, ModalService) {
      $scope.entryIndex = 0;
      $scope.unliquidatedDocuments = unliquidatedDocuments;
      $scope.employee = employee;

      hotkeys.bindTo($scope)
        .add({
          combo: '-',
          description: 'Anular la entrada',
          persistent: false,
          callback: function (e) {
              var entry = $scope.unliquidatedDocuments[$scope.entryIndex];
              $scope.delete(entry);
              e.preventDefault();
          }
      })
      .add({
          combo: 'enter',
          description: 'Detalles',
          persistent: false,
          callback: function (e) {
              var entry = $scope.unliquidatedDocuments[$scope.entryIndex];
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
              if ($scope.entryIndex < $scope.unliquidatedDocuments.length - 1) {
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
      });

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
        return index == $scope.unliquidatedDocuments.length - 1;
      }

      $scope.canShow = function (entry) {
        return entry.type == 5;
      }

      $scope.canCancel = function (entry) {
        return entry.type > 1 && entry.type < 5;
      }

      $scope.delete = function (entry) {
        var api = getApi(entry);
        ModalService.show({ title: getTitle(entry), message: getConfirmMessage(entry) }).then(function (res) {
          api.delete({ id: entry.id }, function (res) {
            Notification.success(getSuccessMessage(entry));
            $state.reload();
          }, function (err) {
            Notification.error(err.data);
          });
        }, function () { });
      };

      function getApi(entry) {
        switch (entry.type) {
          case 2: return Advance;
          case 3: return Absency;
          case 4: return GoodsSold;
          case 5: return Loan;
        }
      }

      function getTitle(entry) {
        switch (entry.type) {
          case 2: return 'Eliminar adelanto';
          case 3: return 'Eliminar ausencia';
          case 4: return 'Eliminar meradería vendida';
          case 5: return 'Eliminar prestamo';
        }
      }

      function getConfirmMessage(entry) {
        switch (entry.type) {
          case 2: return '¿Desea eliminar el adelanto?';
          case 3: return '¿Desea eliminar la ausencia?';
          case 4: return '¿Desea eliminar la mercadería vendida?';
          case 5: return '¿Desea eliminar el préstamo?';
        }
      }

      function getSuccessMessage(entry) {
        switch (entry.type) {
          case 2: return 'Adelanto eliminado correctamente';
          case 3: return 'Ausencia eliminada correctamente';
          case 4: return 'Mercadería vendida eliminada correctamente';
          case 5: return 'Préstamo eliminado correctamente';
        }
      }
  });
