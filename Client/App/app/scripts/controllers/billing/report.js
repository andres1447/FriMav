'use strict';

angular.module('client')
  .controller('BillingReportCtrl', function ($scope, hotkeys, Billing, Notification) {
      $scope.request = {
        mode: 0,
        date: new Date(),
        month: new Date().getMonth() + 1,
        year: new Date().getFullYear()
      };

      hotkeys.bindTo($scope)
      .add({
          combo: 'f5',
          description: 'Buscar',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function () {
            $scope.search($scope.request);
          }
      });

      $scope.init = function () {
          $scope.broadcast('InitBillingReport');
      };

      $scope.init();
      
      $scope.submit = function (request) {
          if (!$scope.sending) {
            $scope.sending = true;
            
            Billing.report(getModel(request), function (res) {
                $scope.sending = false;
                $scope.report = res;
              }, function (err) {
                $scope.sending = false;
                Notification.error(err.data);
              });
          }
      };

    function getModel(request) {
      if (request.mode === 0)
        return { from: request.date, to: request.date }
      if (request.mode === 1)
        return { from: new Date(request.year, request.month - 1, 1), to: new Date(request.year, request.month, 0) }
    }
});
