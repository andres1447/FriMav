'use strict';

angular.module('client')
  .controller('PrintSettingsCtrl', function ($scope, hotkeys, Notification) {
      $scope.printModes = [];
      $scope.printers = [];
      $scope.templates = [];
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
        load('printEntries', PrintHelper.getPrintEntries);
        $.each($scope.printEntries, function (idx, it) {
          it.name = getDescription(it.type)
        })
      }

      function getDescription(type) {
        switch (type) {
          case "Ticket": return "Ticket";
          case "Invoice": return "Factura";
          case "Delivery": return "Reparto";
          case "PriceList": return "Lista de precios";
          case "Absency": return "Falta";
          case "Advance": return "Adelanto";
          case "EmployeeTicket": return "Mercadería empleados";
          case "Loan": return "Préstamo";
          case "Payroll": return "Liquidación sueldo";
          case "CustomerAccount": return "Cuenta cliente";
          case "Vacation": return "Vacaciones";
          case "Bonus": return "Aguinaldo";
        }
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
