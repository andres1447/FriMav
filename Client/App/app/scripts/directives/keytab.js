'use strict';

angular.module('client')
    .directive('aeKeyTab', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            element.bind('keydown', function (e) {
                var $element = angular.element(this);
                var codes = JSON.parse(attrs.aeKeyTab);
                if (codes.indexOf(e.keyCode) != -1 && e.target.type != 'submit') {
                    e.preventDefault();
                    var redo = true, rtnValue, index = -1, formElements, auxElem, auxKeyCodes;
                    while (redo) {
                        redo = false;
                        formElements = Array.from(this.form.elements).filter(e => e.type != 'button');
                        for (var i = 0; i < formElements.length; ++i) {
                            if (element[0] == formElements[i]) {
                                index = i;
                                break;
                            }
                        }
                        if (index == -1) {
                            formElements[0].focus();
                            formElements[0].select();
                        }
                        else {
                            if (angular.isDefined(attrs.aeTabValidator)) {
                                var valid;
                                scope.$apply(function () {
                                    valid = scope.$eval(attrs.aeTabValidator);
                                });

                                if (angular.isDefined(valid) && valid === false) {
                                    $element.focus();
                                    $element.select();
                                    return;
                                }
                            }
                            if (!$element.valid()) {
                              $element.focus();
                              $element.select();
                              return;
                            }
                            if (angular.isDefined(attrs.aeBeforeTab)) {
                                scope.$apply(function () {
                                    rtnValue = scope.$eval(attrs.aeBeforeTab);
                                    redo = angular.isDefined(rtnValue) ? rtnValue : false;
                                });
                                formElements = Array.from(this.form.elements).filter(e => e.type != 'button');
                            }
                            if (!redo && index < formElements.length - 1) {
                                for (var j = index + 1; j < formElements.length; ++j) {
                                    auxElem = angular.element(formElements[j]);
                                    auxKeyCodes = auxElem.attr("ae-key-tab");
                                    if (auxKeyCodes != null && JSON.parse(auxKeyCodes).indexOf(e.keyCode) != -1 && !auxElem[0].disabled) {
                                        $(element[0]).change();
                                        $(element[0]).blur();
                                        auxElem[0].focus();
                                        if (angular.isDefined(auxElem[0].select))
                                            auxElem[0].select();
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            });
        }
    };
});
