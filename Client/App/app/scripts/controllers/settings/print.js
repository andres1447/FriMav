'use strict';

angular.module('client')
  .controller('PrintSettingsCtrl', function ($scope, hotkeys, Notification) {
      $scope.printModes = [];
      $scope.printers = [];
      $scope.templates = [];
      $scope.types = [];
      $scope.printEntries = [];

      hotkeys.bindTo($scope).add({
          combo: 'f5',
          description: 'Guardar',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          callback: function () {
              $scope.save();
          }
      });

      $scope.init = function () {
        load('printModes', PrintHelper.getPrintModes);
        load('printers', PrintHelper.getPrinters);
        load('templates', PrintHelper.getTemplates);
        load('types', PrintHelper.getTypes);
        load('printEntries', PrintHelper.getPrintEntries);
      }

      $scope.init();

      function load(collection, fx) {
        var response = JSON.parse(fx());
        if (response.success)
          $scope[collection] = response.result || [];
        else
          Notification.error(response.errorMessage);
      }

      $scope.save = function () {
        var res = JSON.parse(PrintHelper.updatePrintEntries(JSON.stringify($scope.printEntries)));
        if (res.success)
          Notification.success('Se guardó la configuración de impresión');
        else
          Notification.error(res.errorMessage);
      };
  });
