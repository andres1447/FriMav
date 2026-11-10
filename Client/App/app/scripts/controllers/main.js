'use strict';

angular.module('client').controller('MainCtrl', function ($scope, $state, hotkeys, $timeout, HealthCheck, PendingDeliveryCheck) {
    $scope.$state = $state;
    $scope.backendAlive = false;
    $scope.deliveries = PendingDeliveryCheck

    $scope.shippingOptions = [
        { name: 'Retiro', id: 1 },
        { name: 'Reparto', id: 2 }
    ];

    $scope.paymentMethodOptions = [
        { name: 'Efectivo', id: 1 },
        { name: 'Cuenta', id: 2 }
    ];

    $scope.shippingView = function ($model) {
        var text;
        angular.forEach($scope.shippingOptions, function (it, index) {
            if (it.id === $model) {
                text = it.id + ' - ' + it.name;
            }
        });
        return text;
    };

    $scope.paymentView = function ($model) {
        var text;
        angular.forEach($scope.paymentMethodOptions, function (it, index) {
            if (it.id === $model) {
                text = it.id + ' - ' + it.name;
            }
        });
        return text;
    };
    
    hotkeys.bindTo($scope).add({
        combo: 'f12',
        description: 'DevTools',
        allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
        callback: function () {
            CefHelper.showDevTools();
        }
    })
    .add({
      combo: 'f1',
      description: 'Ticket',
      allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
      callback: function () {
        $state.go("TicketCreate");
      }
    })
    .add({
      combo: 'f2',
      description: 'Factura',
      allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
      callback: function () {
        $state.go("InvoiceCreate");
      }
    })
    .add({
      combo: 'f3',
      description: 'Pago',
      allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
      callback: function () {
        $state.go("PaymentCreate");
      }
    })
    .add({
      combo: 'f4',
      description: 'Reparto',
      allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
      callback: function () {
        $state.go("DeliveryIndex");
      }
    });

    $scope.reload = function () {
      $state.reload();
    };

    $scope.broadcast = function (ev) {
        $timeout(function () {
            $scope.$broadcast(ev);
        }, 100);
    };

    function RecursiveCheckBackendAlive() {
        $scope.backendAlive = false;
        HealthCheck.query().$promise.then(function (res) {
          $scope.backendAlive = true;
          $timeout(RecursiveCheckBackendAlive, 300000);
        }, function () {
          $timeout(RecursiveCheckBackendAlive, 5000);
        });
    }

    $scope.init = function () {
      RecursiveCheckBackendAlive();
      PendingDeliveryCheck.schedule();
    };

    $scope.init();
});
