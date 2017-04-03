'use strict';

function hasValue(s) {
    return angular.isDefined(s) && s != null && s != 0;
}

function newDateUTC() {
    var now = new Date();
    return new Date(now.getUTCFullYear(), now.getUTCMonth(), now.getUTCDate(), now.getUTCHours(), now.getUTCMinutes(), now.getUTCSeconds());
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

if (!PrintHelper) {
    var PrintHelper;
    PrintHelper = {
        print: function (type, model) {
            console.log(model);
        }
    };
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


        if (typeof(ClientConfig) !== 'undefined') {
            $provide.constant('ApiConfig', ClientConfig);
        }

        $urlRouterProvider.otherwise('/invoice');
    })
    .run(function (hotkeys) {
        hotkeys.add({
            combo: 'ctrl+i',
            description: 'DevTools',
            allowIn: ['INPUT', 'SELECT', 'TEXTAREA'],
            callback: function () {
                CefHelper.showDevTools();
            }
        });
    });
