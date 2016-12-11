angular.module('eLabApp').directive('pencilcase', ['drawerHelper', function (drawerHelper, signalR) {
    return {
        restrict: "E",
        scope: { model: '=', selected: '=' },
        templateUrl: function (element, attr) {
            return attr.templateUrl ? attr.templateUrl : 'templates/topics/pencil-case-template.html';
        },
        link: function ($scope, element) {
            var pc = this;
            $scope.changeColor = function (value) {
                drawerHelper.setColor(value);
            }
            $scope.changeTool = function (value) {
                drawerHelper.setTool(value);
            }
        }
    };
}]);


