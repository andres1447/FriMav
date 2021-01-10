'use strict';

angular.module('client').service('PendingDeliveryCheck', function (Delivery, $timeout) {
  var self = this;

  function getTimeForPendingDeliveriesCheck() {
    const now = new Date();
    const time = new Date(now.getFullYear(), now.getMonth(), now.getDate(), 12, 0, 0);
    return time - now;
  }

  function delayCheckPendingDeliveries(time) {
    if (self.pendingDeliveriesTimer) $timeout.cancel(self.pendingDeliveriesTimer);
    self.pendingDeliveriesTimer = $timeout(fetchHasPending, time)
  }

  function fetchHasPending() {
    Delivery.pending().$promise.then(function (res) {
      self.hasPending = res.hasPending;
    });
  }

  this.hasPending = false;

  this.schedule = function () {
    var time = getTimeForPendingDeliveriesCheck();
    if (time <= 0)
      fetchHasPending()
    else
      delayCheckPendingDeliveries(time);
  }
});
