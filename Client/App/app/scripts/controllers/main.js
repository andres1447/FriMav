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
    
    hotkeys.add({
        combo: 'ctrl+i',
        description: 'DevTools',
        allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
        callback: function () {
            CefHelper.showDevTools();
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

    function checkBackendAlive() {
        $scope.backendAlive = false;
        HealthCheck.query().$promise.then(function (res) {
            $scope.backendAlive = true;
        });
    }

    function RecursiveCheckBackendAlive() {
        checkBackendAlive();
        $timeout(checkBackendAlive, 300000);
    }

    $scope.init = function () {
        RecursiveCheckBackendAlive();
    };

    $scope.init();
    
    $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams, options) {
        checkBackendAlive();
    });
});
