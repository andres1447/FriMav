'use strict';

angular.module('client')
  .controller('ProductMeasureIndexCtrl', function ($scope, $state, ModalService, hotkeys, ProductMeasure, Notification, measures) {
      $scope.measureIndex = 0;
      $scope.measures = measures;

      hotkeys.bindTo($scope)
      .add({
          combo: '+',
          description: 'Nueva medida',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
              $state.go('ProductMeasureCreate');
              e.preventDefault();
          }
      })
      .add({
          combo: '-',
          description: 'Borrar medida',
          persistent: false,
          callback: function (e) {
              $scope.delete($scope.measureIndex);
              e.preventDefault();
          }
      })
      .add({
          combo: '*',
          description: 'Editar medida',
          persistent: false,
          callback: function (e) {
              $state.go('ProductMeasureUpdate', { id: $scope.measures[$scope.measureIndex].id });
              e.preventDefault();
          }
      })
      .add({
          combo: 'up',
          description: 'Mover arriba',
          persistent: false,
          callback: function (e) {
              if ($scope.measureIndex > 0) {
                  $scope.measureIndex--;
                  e.preventDefault();
              }
          }
      })
      .add({
          combo: 'down',
          description: 'Mover abajo',
          persistent: false,
          callback: function (e) {
              if ($scope.measureIndex < measures.length - 1) {
                  $scope.measureIndex++;
                  e.preventDefault();
              }
          }
      });

      $scope.delete = function (index) {
          ModalService.show({ title: 'Medida', message: 'Desea borrar la medida?' }).then(function () {
              ProductMeasure.delete({ id: $scope.measures[index].id }, function () {
                  Notification.success('Medida borrada correctamente.');
                  $state.reload();
              }, function (err) {
                  Notification.error(err.data);
              });
          }, function () { });
      };
  });
