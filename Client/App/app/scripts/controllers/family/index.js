'use strict';

angular.module('client')
  .controller('FamilyIndexCtrl', function ($scope, $state, ModalService, hotkeys, ProductFamily, Notification, families) {
      $scope.familyIndex = 0;
      $scope.families = families;

      hotkeys.bindTo($scope)
      .add({
          combo: 'enter',
          description: 'Detalles',
          persistent: false,
          callback: function (e) {
            $state.go('FamilyShow', { id: $scope.families[$scope.familyIndex].id });
              e.preventDefault();
          }
      })
      .add({
          combo: '+',
          description: 'Nueva familia de productos',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
              $state.go('FamilyCreate');
              e.preventDefault();
          }
      })
      .add({
          combo: '-',
          description: 'Borrar familia de productos',
          persistent: false,
          callback: function (e) {
              $scope.delete($scope.familyIndex);
              e.preventDefault();
          }
      })
      .add({
          combo: '*',
          description: 'Editar familia de productos',
          persistent: false,
          callback: function (e) {
              $state.go('FamilyUpdate', { id: $scope.families[$scope.familyIndex].id });
              e.preventDefault();
          }
      })
      .add({
          combo: 'up',
          description: 'Mover arriba',
          persistent: false,
          callback: function (e) {
              if ($scope.familyIndex > 0) {
                  $scope.familyIndex--;
                  e.preventDefault();
              }
          }
      })
      .add({
          combo: 'down',
          description: 'Mover abajo',
          persistent: false,
          callback: function (e) {
              if ($scope.familyIndex < families.length - 1) {
                  $scope.familyIndex++;
                  e.preventDefault();
              }
          }
      });
      
      $scope.delete = function (index) {
          ModalService.show({ title: 'Familia de productos', message: 'Desea borrar la familia de productos?' }).then(function (res) {
              ProductFamily.delete({ id: $scope.families[index].id }, function (res) {
                  Notification.success('Familia borrada correctamente.');
                  $state.reload();
              }, function (err) {
                  Notification.error(err.data);
              });
          });
      };
  });
