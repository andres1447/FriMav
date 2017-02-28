'use strict';

angular.module('client')
  .controller('CustomerUpdateCtrl', function ($scope, $state, $timeout, hotkeys, Notification, Customer, customer, zones) {
      $scope.zones = zones;

      hotkeys.bindTo($scope)
      .add({
          combo: 'f5',
          description: 'Guardar',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function () {
              $scope.update($scope.customer);
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

      $scope.customer = customer;

      $scope.init = function () {
          $timeout(function () {
              $scope.$broadcast('InitCustomerCreate');
          }, 50);
      };

      $scope.init();
      
      $scope.update = function (customer) {
          Customer.update(customer, function (res) {
              Notification.success('Cliente editado correctamente.');
              $state.go('CustomerIndex');
          }, function (err) {
              Notification.error({ title: err.data.message, message: err.data.errors.join('</br>') });
          });
      };

      $scope.setZoneId = function (customer) {
          customer.zoneId = customer.zone.zoneId;
      };
  });
