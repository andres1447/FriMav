'use strict';

angular.module('client').controller('MainCtrl', function ($rootScope, $scope, $state, hotkeys, $timeout, HealthCheck) {
    $scope.$state = $state;
    $scope.backendAlive = false;

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
        combo: 'ctrl+i',
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
        $state.go("DeliveryCreate");
      }
    });

    $scope.hideContent = false;

    $scope.reload = function () {
      return $state.transitionTo($state.current, $stateParams, {
        reload: true
      }).then(function () {
        $scope.hideContent = true;
        return $timeout(function () {
          return $scope.hideContent = false;
        }, 1);
      });
    };

    $scope.broadcast = function (ev) {
        $timeout(function () {
            $scope.$broadcast(ev);
        }, 100);
    };

    function checkBackendAlive() {
        $scope.backendAlive = false;
        HealthCheck.query().$promise.then(function (res) {
            $scope.backendAlive = true;
        });
    }

    function RecursiveCheckBackendAlive() {
        checkBackendAlive();
        $timeout(RecursiveCheckBackendAlive, 300000);
    }

    $scope.init = function () {
        RecursiveCheckBackendAlive();
    };

    $scope.init();
    
    $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams, options) {
        checkBackendAlive();
    });
});
