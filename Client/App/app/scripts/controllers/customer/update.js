'use strict';

angular.module('client')
  .controller('CustomerUpdateCtrl', function ($scope, $state, $timeout, hotkeys, Notification, Customer, customer, zones, codes) {
      $scope.zones = zones;
      $scope.codes = codes.remove(customer.code);
      $scope.customer = customer;

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

      $scope.init = function () {
          $timeout(function () {
              $scope.$broadcast('InitCustomerCreate');
          }, 50);
      };

      $scope.init();
      
      $scope.update = function (customer) {
          if (!$scope.sending) {
              $scope.sending = true;
              Customer.update(customer, function (res) {
                  $scope.sending = false;
                  Notification.success('Cliente editado correctamente.');
                  $state.go('CustomerIndex');
              }, function (err) {
                  $scope.sending = false;
                  Notification.error({ title: err.data.message, message: err.data.errors.join('</br>') });
              });
          }
      };

      $scope.setZoneId = function (customer) {
        customer.zoneId = customer.zone.id;
      };
  });
