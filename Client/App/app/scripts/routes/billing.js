'use strict';

angular
  .module('client').config(function ($stateProvider) {
      $stateProvider
        .state('BillingReport', {
            url: '/billing/report',
            templateUrl: 'views/billing/report.html',
            controller: 'BillingReportCtrl'
        });
  });
