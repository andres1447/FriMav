'use strict';

angular.module('client')
  .controller('CustomerIndexCtrl', function ($scope, $state, ModalService, hotkeys, Customer, Notification, customers) {
      $scope.customerIndex = 0;
      $scope.customers = customers;

      hotkeys.bindTo($scope).add({
          combo: 'f2',
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
          persistent: false,
          callback: function (e) {
                $scope.delete($scope.customerIndex);
                e.preventDefault();
          }
      })
      .add({
          combo: 'enter',
          description: 'Detalles',
          persistent: false,
          callback: function (e) {
            $state.go('CustomerShow', { id: $scope.customers[$scope.customerIndex].id });
              e.preventDefault();
          }
      })
      .add({
          combo: '*',
          description: 'Editar cliente',
          persistent: false,
          callback: function (e) {
            $state.go('CustomerUpdate', { id: $scope.customers[$scope.customerIndex].id });
              e.preventDefault();
          }
      })
      .add({
          combo: 'up',
          description: 'Mover arriba',
          persistent: false,
          callback: function (e) {
              if ($scope.customerIndex > 0) {
                  $scope.customerIndex--;
                  e.preventDefault();
              }
          }
      })
      .add({
          combo: 'down',
          description: 'Mover abajo',
          persistent: false,
          callback: function (e) {
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
          return $.grep($scope.customers, function (it) {
              return it.name.toLowerCase().indexOf($viewValue) != -1 || it.code.toLowerCase().indexOf($viewValue) == 0;
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
          });
      };
  });
