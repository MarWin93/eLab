angular.module('eLabApp').directive('drawing', ['drawerHelper', 'signalR', function(drawerHelper, signalR) {
    return {
        restrict: "A",
        link: function ($scope, element) {
            var vm = this;

            //PASS topic here, god knows how?
            vm.topic = {
                id: $scope.ctrl.topic.id
            };
            vm.user = {
                id: $scope.user.id,
                name: $scope.user.name
            };

            console.log("Root from drawer:" + $scope.user.id);

            signalR.signalRSetup($scope, vm.topic, vm.user);
            
            var ctx = element[0].getContext('2d');

            // variable that decides if something should be drawn on mousemove
            var drawing = false;
            // the last coordinates before the current move
            var currentX;
            var currentY;

            $scope.$watch(
                function () {
                    return [element[0].offsetWidth, element[0].offsetHeight].join('x');
                },
                function (value) {
                    element[0].height = element[0].offsetHeight;
                    element[0].width = element[0].offsetWidth;
                }
            );

            element.bind('mousedown', function (event) {
                currentX = event.offsetX;
                currentY = event.offsetY;

                $scope.newMessage({
                    operation: 'beginPath',
                    x: event.offsetX / element[0].width,
                    y: event.offsetY / element[0].height
                });

                // begins new line
                drawing = true;
            });

            element.bind('mousemove', function (event) {
                //console.log(event.offsetX);
                if (drawing && drawerHelper.getTool() != 'highlighter') {
                    //drawerHelper.drawLine(currentX, currentY, event.offsetX, event.offsetY);
                    draw();
                }
            });

            element.bind('mouseup', function (event) {
                if (drawing && drawerHelper.getTool() == 'highlighter')
                    highlight();

                stopDrawing();
            });

            element.bind('mouseout', function (event) {
                stopDrawing();
            });

            function draw() {
                // get current mouse position
                currentX = event.offsetX;
                currentY = event.offsetY;

                $scope.newMessage({
                    operation: 'draw',
                    tool: drawerHelper.getTool(),
                    lineWidth: drawerHelper.getLineWidth(),
                    color: drawerHelper.getColor(),
                    x: currentX / element[0].width,
                    y: currentY / element[0].height
                });
            }

            function highlight() {
                $scope.newMessage({
                    operation: 'highlight',
                    tool: drawerHelper.getTool(),
                    lineWidth: drawerHelper.getLineWidth(),
                    color: drawerHelper.getColor(),
                    x: currentX / element[0].width,
                    y: currentY / element[0].height,
                    x2: event.offsetX / element[0].width,
                    y2: event.offsetY / element[0].height
                });

                // get current mouse position
                currentX = event.offsetX;
                currentY = event.offsetY;
            }

            function stopDrawing() {
                if (drawerHelper.getTool() != 'highlighter')
                    $scope.newMessage({
                        operation: 'endPath'
                    });
                drawing = false;
            }
        }
    };
}]);
    

