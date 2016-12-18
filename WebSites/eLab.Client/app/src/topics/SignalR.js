angular.module('eLabApp').service('signalR', ['drawerHelper', 'chatHelper', function (drawerHelper, chatHelper, $scope) {

    this.signalRSetup = function ($scope, topic, user) {
        $scope.topicsHub = null; // holds the reference to hub
        $.connection.hub.url = 'http://localhost:8089/signalr';
        $.connection.hub.jsonp = true;
        $.connection.hub.logging = true; //debuging purpose

        $scope.topicsHub = $.connection.topicsHub; // initializes hub
        var me = {
            user: { id: $scope.user.id, name: $scope.user.name, group: $scope.user.group },
            topic: { id: topic.id }
        }

        chatHelper.registerClientMethods($scope.topicsHub, me);

        //client side function declarations
        $scope.topicsHub.client.broadcastMessage = function (message) {
            message = angular.fromJson(message);
            drawerHelper.fromSignalR(message);

            if (message.operation == 'changeFile')
                $scope.ctrl.changePDF(message.fileId);
        };

        $scope.topicsHub.client.updateUserThumbImage = function (userId, base64Image) {
            angular.forEach($scope.activeParticipants, function (value, key) {
                if (key == userId) {
                    value.base64Image = base64Image;
                    $scope.$apply();
                }
            });
        };

        $scope.topicsHub.client.updateUserFullScreenImage = function (base64Image) {
            $scope.activeParticipant.base64Image = base64Image;
            $scope.$apply();
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

    this.watchUserScreen = function($scope, userId, topicId) {
        $scope.topicsHub.server.watchUserScreen(userId, topicId);
    }

    this.stopWatchingUserScreen = function ($scope, topicId) {
        $scope.topicsHub.server.stopWatchingUserScreen(topicId);
    }

    return this;

}]);