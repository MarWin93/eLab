angular.module('eLabApp').service('signalR', ['drawerHelper', 'chatHelper', function (drawerHelper, chatHelper) {

    this.signalRSetup = function ($scope, topic, user) {
        $scope.topicsHub = null; // holds the reference to hub
        $.connection.hub.url = 'http://localhost:8089/signalr';
        $.connection.hub.jsonp = true;
        $.connection.hub.logging = true; //debuging purpose

        $scope.topicsHub = $.connection.topicsHub; // initializes hub

        var me = {
            user: { id: $scope.user.id, name: $scope.user.name },
            topic: { id: $scope.ctrl.topic.id }
        }

        chatHelper.registerClientMethods($scope.topicsHub, me);

        //client side function declarations
        $scope.topicsHub.client.broadcastMessage = function (message) {
            drawerHelper.fromSignalR(angular.fromJson(message));
        };

        // starts hub
        $.connection.hub.start({ jsonp: true }).done(function () {
            //server (hub) side function declarations
            $scope.topicsHub.server.joinGroup(topic.id, user.id, user.name);
            chatHelper.registerEvents($scope.topicsHub);
            console.log("Topic id:" + topic.id);
            $scope.newMessage = function (parameters) { //- funkcja podpieta pod przycisk dodaj plik w widoku warsztatu
                // sends a new message to the server
                parameters = angular.toJson(parameters);
                $scope.topicsHub.server.lock(parameters, topic.id);
            };
        });
    };

    return this;

}]);