'use strict';

angular.module('client')
    .directive('aeKeyPrev', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            element.bind('keydown', function (e) {
                var $element = angular.element(this);
                var codes = JSON.parse(attrs.aeKeyPrev);
                if (codes.indexOf(e.keyCode) != -1 && e.target.type != 'submit') {
                    e.preventDefault();
                    var index = 0;
                    var formElements = this.form.elements;
                    for (var i = 0; i < formElements.length; ++i) {
                        if (element[0] == formElements[i]) {
                            index = i;
                            break;
                        }
                    }
                    if (index <= 0) {
                        formElements[0].focus();
                        formElements[0].select();
                    }
                    else {
                        var auxElem = angular.element(formElements[index - 1]);
                        if (!auxElem[0].disabled) {
                            $(element[0]).change();
                            $(element[0]).blur();
                            auxElem[0].focus();
                            if (angular.isDefined(auxElem[0].select))
                                auxElem[0].select();
                        }
                    }
                }
            });
        }
    };
});