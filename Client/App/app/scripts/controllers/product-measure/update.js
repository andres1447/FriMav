'use strict';

angular.module('client')
  .controller('ProductMeasureUpdateCtrl', function ($scope, $state, hotkeys, Notification, ProductMeasure, measure) {
      $scope.measure = measure;

      hotkeys.bindTo($scope)
      .add({
          combo: 'f5',
          description: 'Guardar',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function () {
              $scope.update($scope.measure);
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
          $scope.broadcast('InitProductMeasureCreate');
      };

      $scope.init();

      $scope.update = function (measure) {
          if (!$scope.sending) {
              $scope.sending = true;
              ProductMeasure.update(measure, function () {
                  $scope.sending = false;
                  Notification.success('Medida editada correctamente.');
                  $state.go('ProductMeasureIndex');
              }, function (err) {
                  $scope.sending = false;
                  Notification.error(err.data);
              });
          }
      };
  });
