'use strict';

angular.module('client')
  .controller('PrintSettingsCtrl', function ($scope, $state, hotkeys, Notification) {
      $scope.printEntries = JSON.parse(PrintHelper.getPrintEntries());

      $scope.printModes = JSON.parse(PrintHelper.getPrintModes());
      $scope.printers = JSON.parse(PrintHelper.getPrinters());
      $scope.templates = JSON.parse(PrintHelper.getTemplates());
      $scope.types = JSON.parse(PrintHelper.getTypes())

      hotkeys.bindTo($scope).add({
          combo: 'f5',
          description: 'Guardar',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function () {
              $scope.save();
          }
      });

      $scope.save = function () {
          PrintHelper.updatePrintEntries(JSON.stringify($scope.printEntries));
      };
  });
