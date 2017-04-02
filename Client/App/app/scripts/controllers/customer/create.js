'use strict';

angular.module('client')
  .controller('CustomerCreateCtrl', function ($scope, $state, $timeout, hotkeys, Notification, Customer, zones) {
      $scope.customer = { shipping: 2, paymentMethod: 2 };
      $scope.zones = zones;

      hotkeys.bindTo($scope)
      .add({
          combo: 'f5',
          description: 'Guardar',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function () {
              $scope.create($scope.customer);
          }
      })
      .add({
          combo: 'esc',
          description: 'Volver al indice',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
              $state.go('CustomerIndex');
              e.preventDefault();
          }
      });

      $scope.init = function () {
          $scope.broadcast('InitCustomerCreate');
      };

      $scope.init();
      
      $scope.create = function (customer) {
          if (!$scope.sending) {
              $scope.sending = true;
              Customer.save(customer, function (res) {
                  $scope.sending = false;
                  Notification.success('Cliente creado correctamente.');
                  $state.go('CustomerIndex');
              }, function (err) {
                  $scope.sending = false;
                  Notification.error({ title: err.data.message, message: err.data.errors.join('</br>') });
              });
          }
      };

      $scope.setZoneId = function (customer) {
          customer.zoneId = customer.zone.zoneId;
      };
  });
