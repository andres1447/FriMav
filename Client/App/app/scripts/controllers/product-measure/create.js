'use strict';

angular.module('client')
  .controller('ProductMeasureCreateCtrl', function ($scope, $state, hotkeys, Notification, ProductMeasure) {

      hotkeys.bindTo($scope)
      .add({
          combo: 'f5',
          description: 'Guardar',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function () {
              $scope.create($scope.measure);
          }
      })
      .add({
          combo: 'esc',
          description: 'Volver al indice',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
              $state.go('ProductMeasureIndex');
              e.preventDefault();
          }
      });

      $scope.init = function () {
          $scope.measure = {};
          $scope.broadcast('InitProductMeasureCreate');
      };

      $scope.init();

      $scope.create = function (measure) {
          if (!$scope.sending) {
              $scope.sending = true;
              ProductMeasure.save(measure, function () {
                  $scope.sending = false;
                  Notification.success('Medida creada correctamente.');
                  $state.go('ProductMeasureIndex');
              }, function (err) {
                  $scope.sending = false;
                  Notification.error(err.data);
              });
          }
      };
  });
