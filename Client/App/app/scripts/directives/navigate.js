'use strict';

angular.module('client')
    .directive('aeNavigate', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var $element = $(element[0]);
            var arrow = { left: 37, up: 38, right: 39, down: 40 };

            function moveVertical(tr, td, e) {
                var moveTo = null;

                var pos = td[0].cellIndex;

                var moveToRow = null;
                if (e.which == arrow.down) {
                    moveToRow = tr.next('tr');
                }
                else if (e.which == arrow.up) {
                    moveToRow = tr.prev('tr');
                }

                if (moveToRow.length) {
                    moveTo = $(moveToRow[0].cells[pos]);
                }

                return moveTo;
            }

            // select all on focus
            $element.find('input,select,textarea').keydown(function (e) {
                var td = $(e.target).closest('td');
                var tr = td.closest('tr');

                var prev, children;
                var input = this;

                if (e.which == arrow.up && tr[0].rowIndex == 1 && td[0].cellIndex == 0) {
                    var elems = $(this).closest('form').find('input,select,textarea');
                    var index = elems.index($(this));
                    if (index > 0) {
                        setTimeout(function () {
                            $(input).change();
                            $(input).blur();
                            $(elems[index - 1]).focus().select();
                        }, 0)
                        return;
                    }
                }

                var self = angular.element(this);

                // shortcut for key other than arrow keys
                if ($.inArray(e.which, [arrow.left, arrow.up, arrow.right, arrow.down]) < 0) {
                    return;
                }

                if (angular.isDefined(self.attr('ae-tab-validator'))) {
                    var valid;
                    scope.$apply(function () {
                        valid = scope.$eval(self.attr('ae-tab-validator'));
                    });

                    // Not advancing if model is not valid
                    if (angular.isDefined(valid) && valid === false && $.inArray(e.which, [arrow.right, arrow.down]) > -1) {
                        self.focus();
                        self.select();
                        return;
                    }
                }

                var moveTo = null;
                var ctrl = angular.element(input).data('$ngModelController');

                switch (e.which) {

                    case arrow.left:
                    {
                        moveTo = td.prev('td:has(input,textarea)');
                        break;
                    }
                    case arrow.right:
                    {
                        moveTo = td.next('td:has(input,textarea)');
                        break;
                    }

                    case arrow.up:
                    case arrow.down:
                    {
                        moveTo = moveVertical(tr, td, e);
                        break;
                    }
                }

                if (moveTo && moveTo.length) {
                    e.preventDefault();
                    moveTo.find('input,textarea').each(function (i, input) {
                        input.focus();
                        input.select();
                    });
                }
            });
        }
    }
});