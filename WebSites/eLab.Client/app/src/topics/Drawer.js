angular.module('eLabApp').directive('drawing', ['drawerHelper', function(drawerHelper) {
    return {
        restrict: "A",
        link: function ($scope, element) {
            var vm = this;
            
            vm.signalRSetup = function (topic, user) {
                $scope.topicsHub = null; // holds the reference to hub
                $.connection.hub.url = 'http://localhost:8089/signalr';
                $.connection.hub.jsonp = true;
                $.connection.hub.logging = true; //debuging purpose

                $scope.topicsHub = $.connection.topicsHub; // initializes hub

                //client side function declarations
                $scope.topicsHub.client.broadcastMessage = function (message) {
                    drawerHelper.fromSignalR(angular.fromJson(message));
                };

                // starts hub
                $.connection.hub.start({ jsonp: true }).done(function () {
                    //server (hub) side function declarations
                    $scope.topicsHub.server.joinGroup(topic.id, user.id, user.name);
                    console.log("Topic id:" + topic.id);
                    $scope.newMessage = function (parameters) { //- funkcja podpieta pod przycisk dodaj plik w widoku warsztatu
                        // sends a new message to the server
                        parameters = angular.toJson(parameters);
                        $scope.topicsHub.server.lock(parameters, topic.id);
                    };
                });
            };

            //PASS topic here, god knows how?
            vm.topic = {
              id: 1
            };
            vm.user = {
                id: $scope.user.id,
                name: $scope.user.name
            };

            console.log("Root from drawer:" + $scope.user.id);

            vm.signalRSetup(vm.topic, vm.user);
            
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
    

