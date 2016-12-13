angular.module('eLabApp').directive('pdfDraw', ['drawerHelper', 'signalR', function (drawerHelper, signalR) {
    return {
        restrict: "E",
        scope: false,
        templateUrl: 'templates/topics/pdf-viewer-template.html',
        link: function ($scope, element) {
            element = angular.element(element).find('canvas');

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

            //signalR.signalRSetup($scope, vm.topic, vm.user);
            
            var ctx = element[0].getContext('2d');

            // variable that decides if something should be drawn on mousemove
            var drawing = false;

            var currentX;
            var currentY;

            element.bind('mousedown', function (event) {
                currentX = event.offsetX;
                currentY = event.offsetY;

                //$scope.newMessage({
                //    operation: 'beginPath',
                //    x: event.offsetX / element[0].width,
                //    y: event.offsetY / element[0].height
                //});

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

            element.bind('contextmenu', function (event) {
                stopDrawing();
            });

            element.bind('mouseout', function (event) {
                stopDrawing();
            });

            function draw(operation) {
                if (!operation)
                    operation = 'draw'
                // get current mouse position
                $scope.newMessage({
                    operation: operation,
                    tool: drawerHelper.getTool(),
                    lineWidth: drawerHelper.getLineWidth(),
                    color: drawerHelper.getColor(),
                    x: currentX / element[0].offsetWidth,
                    y: currentY / element[0].offsetHeight,
                    x2: event.offsetX / element[0].offsetWidth,
                    y2: event.offsetY / element[0].offsetHeight
                });

                currentX = event.offsetX;
                currentY = event.offsetY;
            }

            function highlight() {
                draw('highlight');
            }

            function stopDrawing() {
                //if (drawerHelper.getTool() != 'highlighter')
                //    $scope.newMessage({
                //        operation: 'endPath'
                //    });
                drawing = false;
            }
        }
    };
}]);
    

