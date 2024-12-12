'use strict';

angular.module('client')
  .controller('LoanCreateCtrl', function ($scope, $state, hotkeys, Notification, Loan, employees, $filter, $timeout) {
      $scope.employees = orderByCode($filter, employees);

      $scope.loan = {
        date: new Date()
      };

      $scope.template = {
        startFromDate: new Date().firstDayOfWeek().addDays(6 + 7)
      };

      hotkeys.bindTo($scope)
      .add({
          combo: 'f5',
          description: 'Guardar',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function () {
            $scope.create($scope.loan);
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
        combo: 'f8',
        description: 'Generar',
        allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
        callback: function () {
          $scope.generate($scope.loan);
        }
      });

      $scope.init = function () {
           $scope.broadcast('InitLoanCreate');
      };

      $scope.init();

      $scope.generate = function (template) {
        if (!template.feeCount && !template.totalAmount) return;

        if (!template.startFromDate)
          template.startFromDate = new Date();

        var feeAmount = template.totalAmount / template.feeCount
        $scope.loan.fees = [];
        var date;
        for (var i = 0; i < template.feeCount; ++i) {
          date = template.startFromDate.addDays(i * 7);
          $scope.loan.fees.push({ date: date, amount: feeAmount });
        }
        $timeout(function () {
          $scope.broadcast('LoanGenerated');
        })
      }
      
      $scope.create = function (loan) {
          if (!$scope.sending) {
              $scope.sending = true;
              Loan.save(loan, function (res) {
                $scope.sending = false;
                Notification.success('Préstamo creado correctamente.');
                PrintHelper.print('Loan', JSON.stringify(getPrintModel(loan)));
                $state.reload();
              }, function (err) {
                $scope.sending = false;
                Notification.error(err.data);
              });
          }
      };

    function getPrintModel(loan) {
      return {
        employeeCode: loan.employee.code,
        employeeName: loan.employee.name,
        description: loan.description,
        fees: $.map(loan.fees, function (fee) {
          return { date: fee.date, amount: fee.amount }
        }),
      };
    }

    $scope.getMatchingEmployees = function ($viewValue) {
      var term = $viewValue.toLowerCase();
      return $.grep($scope.employees, function (it) {
        return it.name.toLowerCase().indexOf(term) != -1 || it.code.toLowerCase().indexOf(term) == 0;
      });
    };

    $scope.setEmployee = function () {
      $scope.loan.employeeId = $scope.loan.employee.id;
    };

    $scope.clearEmployee = function () {
      $scope.loan.employeeId = null;
      $scope.loan.employee = null;
    }
});
