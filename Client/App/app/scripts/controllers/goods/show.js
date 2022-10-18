'use strict';

angular.module('client')
  .controller('GoodsSoldShowCtrl', function ($scope, $state, hotkeys, goodsSold) {
      $scope.goodsSold = goodsSold;

      hotkeys.bindTo($scope)
      .add({
          combo: 'esc',
          description: 'Volver a empleado',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
            $state.go('EmployeeLiquidated', { id: $scope.goodsSold.employeeId });
            e.preventDefault();
          }
      });

      $scope.init = function () {
          $scope.totals = { quantity: 0, total: $scope.goodsSold.total };
          angular.forEach($scope.goodsSold.items, function (item) {
              $scope.totals.quantity++;
          });
      };

      $scope.init();
  });
