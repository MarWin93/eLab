angular.module('eLabApp').directive('pencilcase', ['drawerHelper', function (drawerHelper, signalR) {
    return {
        restrict: "E",
        scope: { model: '=', selected: '=' },
        templateUrl: 'templates/topics/pencil-case-template.html',
        link: function ($scope, element) {
            $scope.changeColor = function (node, value) {
                //angular.element(element).find('span.selected').removeClass('selected');
                angular.element(node).addClass('selected');
                drawerHelper.setColor(value);
            }
            $scope.changeTool = function (value) {
                drawerHelper.setTool(value);
            }
        }
    };
}]);


