'use strict';

angular.module('client')
  .controller('UpdateExternalReferenceNumberCtrl', function (Invoice, transactionDocument, $uibModalInstance, Notification) {
    var $ctrl = this;

    $ctrl.transactionDocument = transactionDocument
    $ctrl.externalDocumentNumber = transactionDocument.externalDocumentNumber

    $ctrl.submit = function () {
      Invoice.updateExternalReferenceNumber({ id: $ctrl.transactionDocument.id }, { number: $ctrl.externalDocumentNumber }, function () {
        $ctrl.sending = false;
        Notification.success('Se guardó el número de ticket.');
        transactionDocument.externalDocumentNumber = $ctrl.externalDocumentNumber;
        $uibModalInstance.close();
      });
    };

    $ctrl.cancel = function () {
      $uibModalInstance.dismiss();
    };
  });
