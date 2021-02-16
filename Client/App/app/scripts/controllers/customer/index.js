'use strict';

angular.module('client')
  .controller('CustomerIndexCtrl', function ($scope, $state, ModalService, hotkeys, Customer, Notification, customers, $filter) {
      $scope.customerIndex = 0;
      $scope.customers = orderByCode($filter, customers);

      hotkeys.bindTo($scope).add({
          combo: 'f12',
          description: 'Buscar cliente',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
              $scope.$broadcast('SearchCustomer');
              e.preventDefault();
          }
      }).add({
          combo: '+',
          description: 'Nuevo cliente',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
              $state.go('CustomerCreate');
              e.preventDefault();
          }
      })
      .add({
          combo: '-',
          description: 'Borrar cliente',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
                $scope.delete($scope.customerIndex);
                e.preventDefault();
          }
      })
      .add({
          combo: 'enter',
          description: 'Detalles',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
            if ($scope.customerSearch != null) return;
            $state.go('CustomerShow', { id: $scope.customers[$scope.customerIndex].id });
              e.preventDefault();
          }
      })
      .add({
          combo: '*',
          description: 'Editar cliente',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
            $state.go('CustomerUpdate', { id: $scope.customers[$scope.customerIndex].id });
              e.preventDefault();
          }
      })
      .add({
          combo: 'up',
          description: 'Mover arriba',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
            if ($scope.typeaheadOpen) return;
              if ($scope.customerIndex > 0) {
                  $scope.customerIndex--;
                  e.preventDefault();
              }
          }
      })
      .add({
          combo: 'down',
          description: 'Mover abajo',
          allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
          persistent: false,
          callback: function (e) {
            if ($scope.typeaheadOpen) return;
              if ($scope.customerIndex < customers.length - 1) {
                  $scope.customerIndex++;
                  e.preventDefault();
              }
          }
      });

      $scope.init = function () {
          setTimeout(function () {
              $scope.$broadcast('InitCustomer');
          }, 50);
      };

      $scope.init();

    $scope.getMatchingCustomer = function ($viewValue) {
        var term = $viewValue.toLowerCase();
        return $.grep($scope.customers, function (it) {
          return it.name.toLowerCase().indexOf(term) != -1 || it.code.toLowerCase().indexOf(term) == 0;
        });
      };

      $scope.goToCustomer = function (customer) {
        $state.go('CustomerShow', { id: customer.id });
      }
      
      $scope.delete = function (index) {
          ModalService.show({ title: 'Clientes', message: 'Desea borrar al cliente?' }).then(function (res) {
            Customer.delete({ id: $scope.customers[index].id }, function (res) {
                  Notification.success('Cliente borrado correctamente.');
                  $state.reload();
              }, function (err) {
                  Notification.error(err.data);
              });
          }, function () { });
      };
  });
