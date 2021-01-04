'use strict';

function groupBy(collection, property) {
  var i = 0, val, index,
    values = [], result = [];
  for (; i < collection.length; i++) {
    val = collection[i][property];
    index = values.indexOf(val);
    if (index > -1)
      result[index].push(collection[i]);
    else {
      values.push(val);
      result.push([collection[i]]);
    }
  }
  return result;
}

$.validator.setDefaults({
  errorPlacement: function (error, element) { }
});

function hasValue(s) {
    return angular.isDefined(s) && s != null && s != 0;
}

function newDateUTC() {
    var now = new Date();
    return new Date(now.getUTCFullYear(), now.getUTCMonth(), now.getUTCDate(), now.getUTCHours(), now.getUTCMinutes(), now.getUTCSeconds());
}

if (!Date.prototype.toLocal) {
  Date.prototype.toLocal = function () {
    return new Date(this.getTime() - this.getTimezoneOffset() * 60 * 1000);
  }
}


if (!String.prototype.format) {
    String.prototype.format = function () {
        var args = arguments;
        return this.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined'
              ? args[number]
              : match
            ;
        });
    };
}

if (!Array.prototype.remove) {
  Array.prototype.remove = function (item) {
    var index = this.indexOf(item);
    if (index > -1) {
      this.splice(index, 1);
    }
    return this;
  }
}

if (!PrintHelper) {
    var PrintHelper;
    PrintHelper = {
        print: function (type, model) {
            console.log(model);
        }
    };
}

if (!CefHelper) {
  var CefHelper = {
    showDevTools: function () { }
  }
}

if (!String.prototype.padStart) {
  String.prototype.padStart = function (count, char) {
    return (Array(count).join(char) + this).slice(-10);
  }
}

if (!String.prototype.padEnd) {
  String.prototype.padEnd = function (count, char) {
    return (Array(count).join(char) + this).slice(10);
  }
}

function orderByCode($filter, collection) {
  return $filter('orderBy')(collection, function (it) { return it.code.padStart(10, '0'); })
}

angular.module('client', [
  'ngAnimate',
  'ngCookies',
  'ngResource',
  'ngRoute',
  'ngSanitize',
  'ngTouch',
  'ui.router',
  'ui.bootstrap',
  'cfp.hotkeys',
  'ui-notification'
])
  .config(function ($stateProvider, $urlRouterProvider, hotkeysProvider, $provide) {
    hotkeysProvider.cheatSheetHotkey = '?';

    $stateProvider
      .state('Main', {
        url: '/',
        templateUrl: 'views/main.html',
        controller: 'MainCtrl'
      });


    if (typeof (ClientConfig) !== 'undefined') {
      $provide.constant('ApiConfig', ClientConfig);
    }

    $urlRouterProvider.otherwise('/invoice');
  });
