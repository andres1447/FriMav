'use strict';

angular.module('client').service('ModalService', function ($uibModal) {

    var modalDefaults = {
        backdrop: true,
        keyboard: true,
        modalFade: true,
        template: '<div class="modal-header"><h4>{{ modalOptions.title }}</h4></div><div class="modal-body"><p>{{ modalOptions.message }}</p></div><div class="modal-footer"><button class="btn btn-success" data-ng-click="modalOptions.ok();"><i class="glyphicon glyphicon-ok"></i> Si<span class="key-shortcut">&crarr;</span></button><button type="button" class="btn btn-danger" data-ng-click="modalOptions.close()"><i class="glyphicon glyphicon-remove"></i> No<span class="key-shortcut">&minus;</span></button></div>'
    };

    var modalOptions = {
        title: 'Proceder',
        message: 'Desea realizar esta accion?'
    };

    this.showModal = function (customModalDefaults, customModalOptions) {
        if (!customModalDefaults) customModalDefaults = {};
        customModalDefaults.backdrop = 'static';
        return this.show(customModalDefaults, customModalOptions);
    };

    this.show = function (customModalOptions, customModalDefaults) {
        //Create temp objects to work with since we're in a singleton service
        var tempModalDefaults = {};
        var tempModalOptions = {};

        //Map angular-ui modal custom defaults to modal defaults defined in service
        angular.extend(tempModalDefaults, modalDefaults, customModalDefaults ? customModalDefaults : {});

        //Map modal.html $scope custom properties to defaults defined in service
        angular.extend(tempModalOptions, modalOptions, customModalOptions ? customModalOptions : {});

        if (!tempModalDefaults.controller) {
          tempModalDefaults.controller = ['$scope', '$uibModalInstance', 'hotkeys', function ($scope, $uibModalInstance, hotkeys) {
                $scope.modalOptions = tempModalOptions;
                $scope.previousHotkeys = angular.copy(hotkeys.get());

                function recoverHotkeys() {
                    hotkeys.purgeHotkeys();
                    for (var b = $scope.previousHotkeys.length; b--;)
                        hotkeys.bindTo($scope.$parent).add($scope.previousHotkeys[b])
                }

                hotkeys.purgeHotkeys();

                hotkeys.bindTo($scope).add({
                    combo: "enter",
                    description: "Aceptar",
                    callback: function (e) {
                        $scope.modalOptions.ok();
                        e.preventDefault();
                    }
                })
                .add({
                    combo: "-",
                    description: "Cancelar",
                    callback: function (e) {
                        $scope.modalOptions.close();
                        e.preventDefault();
                    }
                });

                $scope.modalOptions.ok = function (result) {
                    recoverHotkeys();
                    $uibModalInstance.close(result);
                };
                $scope.modalOptions.close = function (result) {
                    recoverHotkeys();
                    $uibModalInstance.dismiss('cancel');
                };
            }];
        }
        return $uibModal.open(tempModalDefaults).result;
    };
});
