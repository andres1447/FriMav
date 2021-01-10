'use strict';

angular.module('client')
  .controller('DeliveryCreateCtrl', function ($scope, $state, $filter, hotkeys, Notification, Delivery, employees, invoices, Invoice, PendingDeliveryCheck, $anchorScroll) {
    hotkeys.bindTo($scope).add({
        combo: 'f5',
        description: 'Imprimir',
        allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
        callback: function () {
            $scope.create($scope.delivery);
        }
    })
    .add({
        combo: 'esc',
        description: 'Reiniciar',
        allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
        callback: function () {
            $scope.reload();
        }
    })
    .add({
      combo: 'up',
      description: 'Mover arriba',
      allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
      persistent: false,
      callback: function (e) {
        $scope.backInvoiceIndex();
        e.preventDefault();
      }
    })
    .add({
      combo: 'down',
      description: 'Mover abajo',
      allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
      persistent: false,
      callback: function (e) {
        $scope.advanceInvoiceIndex();
        e.preventDefault();
      }
    })
    .add({
      combo: 'space',
      description: 'Seleccionar',
      persistent: false,
      callback: function (e) {
        $scope.selectCurrentInvoice();
        e.preventDefault();
      }
    })
    .add({
      combo: 'enter',
      description: 'Seleccionar',
      persistent: false,
      callback: function (e) {
        $scope.selectCurrentInvoice();
        $scope.advanceInvoiceIndex();
        e.preventDefault();
      }
    })
    .add({
      combo: '-',
      description: 'No enviar',
      persistent: false,
      callback: function (e) {
        $scope.removeCurrentInvoice();
      }
    })
    .add({
      combo: '*',
      description: 'Seleccionar Todo',
      persistent: false,
      callback: function (e) {
        $scope.selectAllInvoices();
      }
    });

    $scope.invoices = invoices;
    $scope.employees = orderByCode($filter, employees);
    $scope.invoiceIndex = -1;

    $scope.init = function () {
        $scope.delivery = {
            date: new Date(),
            invoices: [{}]
        };
        $scope.broadcast('DeliveryInit');
    };

    $scope.init();

    $scope.setEmployee = function (delivery) {
      $scope.delivery.employeeId = delivery.employee.id;
      $scope.invoiceIndex = 0;
    };

    $scope.selectCurrentInvoice = function () {
      if (invoices.length > 0 && $scope.invoiceIndex >= 0) {
        $scope.invoices[$scope.invoiceIndex].selected = !$scope.invoices[$scope.invoiceIndex].selected;
      }
    }

    $scope.removeCurrentInvoice = function () {
      if (invoices.length > 0 && $scope.invoiceIndex >= 0) {
        $scope.removeInvoice($scope.invoiceIndex);
      }
    }

    $scope.removeInvoice = function (index) {
      var invoice = $scope.invoices[index];
      $scope.invoices.splice($scope.invoiceIndex, 1);
      Invoice.dontDeliver({ id: invoice.id }, null);
    }

    $scope.selectAllInvoices = function () {
      if (invoices.length > 0) {
        $.each($scope.invoices, function (idx, it) { it.selected = !it.selected });
      }
    }

    $scope.backInvoiceIndex = function () {
      if ($scope.invoiceIndex > 0) {
        $scope.invoiceIndex--;
        $scope.scrollTo($scope.invoiceIndex);
      }
    }

    $scope.advanceInvoiceIndex = function () {
      if ($scope.invoiceIndex < $scope.invoices.length - 1) {
        $scope.invoiceIndex++;
        $scope.scrollTo($scope.invoiceIndex);
      }
    }

    $scope.create = function (delivery) {
      if ($scope.sending) return;

      $scope.sending = true;
      delivery.invoices = getSelectedInvoicesIds();
      Delivery.save(delivery, function (res) {
        $scope.sending = false;
        var model = getDeliveryModel();
        PrintHelper.print('Delivery', JSON.stringify(model));
        PendingDeliveryCheck.schedule();
        Notification.success('Envio creado correctamente');
        $state.go('DeliveryIndex');
      }, function (err) {
        $scope.sending = false;
        Notification.error('Ocurrió un error al guardar el envío');
      });
    };

    $scope.getMatchingEmployees = function ($viewValue) {
      return filterByCodeOrName($scope.employees, $viewValue);
    };

    $scope.$watch(function () { return $scope.invoices }, function (newVal) {
      var productMap = groupDeliveryProducts();
      $scope.deliveryProducts = $.map(Object.keys(productMap), function (key) {
        return { name: key, quantity: productMap[key] }
      });
    }, true);

    $scope.scrollTo = function (index) {
      var newHash = 'invoice_' + $scope.invoices[index].id;
      var prevOffset = $anchorScroll.yOffset;
      $anchorScroll.yOffset = 150;
      $anchorScroll(newHash);
      $anchorScroll.yOffset = prevOffset;
    }

    function getSelectedInvoicesIds() {
      return $.map(getSelectedInvoices(), function (it) { return it.id; });
    }

    function getSelectedInvoices() {
      return $.grep($scope.invoices, function (it) { return it.selected; });
    }

    function getProductsToDeliver() {
      return $.map(getSelectedInvoices(), function (select) { return select.products; });
    }

    function groupDeliveryProducts() {
      var productMap = {};
      $.each(getProductsToDeliver(), function (idx, product) {
        if (!productMap[product.name]) productMap[product.name] = 0;
        productMap[product.name] += product.quantity;
      });
      return productMap;
    }

    function getDeliveryModel() {
      return {
        date: $scope.delivery.date,
        number: $scope.delivery.number,
        employeeCode: $scope.delivery.employee.code,
        employeeName: $scope.delivery.employee.name,
        invoices: getSelectedInvoices(),
        products: $scope.deliveryProducts
      };
    }
  });
