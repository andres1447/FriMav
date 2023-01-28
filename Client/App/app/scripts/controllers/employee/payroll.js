'use strict';

angular.module('client')
  .controller('PayrollCtrl', function ($scope, $state, hotkeys, Notification, payrolls, Employee, Advance, Absency, GoodsSold, Loan, ModalService) {

      function setCurrentPayroll() {
        $scope.entryIndex = 0;
        if ($scope.payrollIndex < $scope.payrolls.length)
          $scope.currentPayroll = $scope.payrolls[$scope.payrollIndex];
      }

      $scope.entryIndex = 0;
      $scope.payrollIndex = 0;
      $scope.payrolls = payrolls;
      $scope.closing = false;

      setCurrentPayroll();

      hotkeys.bindTo($scope)
        .add({
          combo: '-',
          description: 'Anular la entrada',
          persistent: false,
          callback: function (e) {
              var entry = $scope.currentPayroll.liquidation[$scope.entryIndex];
              $scope.delete(entry);
              e.preventDefault();
          }
      })
      .add({
          combo: 'enter',
          description: 'Detalles',
          persistent: false,
          callback: function (e) {
              var entry = $scope.currentPayroll.liquidation[$scope.entryIndex];
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
              if ($scope.entryIndex < $scope.currentPayroll.liquidation.length - 1) {
                  $scope.entryIndex++;
                  e.preventDefault();
              }
          }
      })
      .add({
        combo: 'left',
        description: 'Anterior empleado',
        persistent: false,
        callback: function (e) {
          $scope.prevEmployee();
          e.preventDefault();
        }
      })
      .add({
        combo: 'right',
        description: 'Siguiente empleado',
        persistent: false,
        callback: function (e) {
          $scope.nextEmployee();
          e.preventDefault();
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
        combo: 'f5',
        description: 'Liquidar sueldo',
        allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
        persistent: false,
        callback: function (e) {
          $scope.closeCurrentPayroll();
          e.preventDefault();
        }
      })
      .add({
        combo: 'f8',
        description: 'Liquidar todo',
        allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
        persistent: false,
        callback: function (e) {
          $scope.closePayrolls();
          e.preventDefault();
        }
      });

      $scope.prevEmployee = function () {
        if (--$scope.payrollIndex < 0) {
          $scope.payrollIndex = $scope.payrolls.length - 1;
        }
        setCurrentPayroll();
      }

      $scope.nextEmployee = function () {
        if (++$scope.payrollIndex == $scope.payrolls.length) {
          $scope.payrollIndex = 0;
        }
        setCurrentPayroll()
      }

      $scope.description = function (entry) {
        switch (entry.type) {
            case 0: return 'Saldo anterior';
            case 1: return 'Sueldo';
            case 2: return 'Adelanto';
            case 3: return 'Ausencia';
            case 4: return 'Mercadería';
            case 5: return 'Cuota prestamo';
            case 6: return 'Presentismo';
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
        return index == $scope.currentPayroll.liquidation.length - 1;
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

      $scope.closeCurrentPayroll = function () {
        $scope.closing = true;
        Employee.closePayroll({ id: $scope.currentPayroll.employeeId }, function (res) {
          Notification.success('Sueldo liquidado correctamente');
          printPayroll($scope.currentPayroll);
          $state.reload();
        }, function (err) {
          $scope.closing = false;
          Notification.error(err.data);
        });
      };

      function printPayroll(payroll) {
        PrintHelper.print('Payroll', JSON.stringify(getPrintModel(payroll)));
      }

      function getPrintModel(payroll) {
        return {
          date: payroll.date,
          employeeCode: payroll.employeeCode,
          employeeName: payroll.employeeName,
          salary: payroll.salary,
          balance: payroll.balance,
          total: payroll.total,
          hasAttendBonus: payroll.hasAttendBonus,
          liquidation: $.map(payroll.liquidation, function (it) {
            return {
              date: it.date,
              name: $scope.description(it),
              amount: it.amount,
              description: it.description,
              balance: it.balance
            }
          })
        }
      }

      $scope.closePayrolls = function () {
          $scope.closing = true;
          Employee.closePayrolls(function (res) {
              Notification.success('Sueldo liquidado correctamente');
              $.each($scope.payrolls, function (_, it) {
                printPayroll(it);
              });
              $state.reload();
          }, function (err) {
              $scope.closing = false;
              Notification.error(err.data);
          });
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
