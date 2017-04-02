'use strict';

angular.module('client')
  .controller('FamilyCreateCtrl', function ($scope, $state, $timeout, hotkeys, Notification, ProductFamily) {
      
      hotkeys.bindTo($scope)
      .add({
          combo: 'f5',
          description: 'Guardar',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function () {
              $scope.create($scope.family);
          }
      })
      .add({
          combo: 'esc',
          description: 'Volver al indice',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
              $state.go('FamilyIndex');
              e.preventDefault();
          }
      });

      $scope.init = function () {
          $scope.family = {};

          $scope.broadcast('InitFamilyCreate');
      };

      $scope.init();
      
      $scope.create = function (family) {
          if (!$scope.sending) {
              $scope.sending = true;
              ProductFamily.save(family, function (res) {
                  $scope.sending = false;
                  Notification.success('Familia de productos creada correctamente.');
                  $state.go('FamilyIndex');
              }, function (err) {
                  $scope.sending = false;
                  Notification.error(err.data);
              });
          }
      };
  });
