angular.module('eLabApp').service('signalR', ['drawerHelper', 'chatHelper', function (drawerHelper, chatHelper) {

    this.signalRSetup = function ($scope, topic, user) {
        $scope.topicsHub = null; // holds the reference to hub
        $.connection.hub.url = 'http://localhost:8089/signalr';
        $.connection.hub.jsonp = true;
        $.connection.hub.logging = true; //debuging purpose

        $scope.topicsHub = $.connection.topicsHub; // initializes hub
        var me = {
            user: { id: $scope.user.id, name: $scope.user.name },
            topic: { id: topic.id }
        } 

        //$scope.activeParticipants = [
        //    {
        //        id: '3',
        //        base64Image: '',
        //        loop: 1
        //    },
        //    {
        //        id: '4',
        //        base64Image: '',
        //        loop: 1
        //    }
        //];

        chatHelper.registerClientMethods($scope.topicsHub, me);

        //client side function declarations
        $scope.topicsHub.client.broadcastMessage = function (message) {
            drawerHelper.fromSignalR(angular.fromJson(message));
        };

        $scope.topicsHub.client.updateUserThumbImage = function (userId, base64Image) {
            //$scope.activeParticipants = $scope.activeParticipants.map(function (element) {
            //    element.id == userId ? element.base64Image = base64Image : console.log("user: " + userId + " not found");
            //});
            //for (var i = 0; i < Object.keys($scope.activeParticipants).length; i++) {
            //    if ($scope.activeParticipants[i].id == userId) {
            //        $scope.activeParticipants[i].base64Image = base64Image;
            //    }
            //}
            angular.forEach($scope.activeParticipants, function (value, key) {
                if (key == userId) {
                    value.base64Image = base64Image;
                }
            });
            //for (var i = 0; i < vm.activeParticipants.length; i++) {
            //    if (vm.activeParticipants[i].id == userId) {
            //        vm.activeParticipants[i].base64Image = base64Image;
            //        vm.activeParticipants[i].loop = vm.activeParticipants[i].loop + 1;
            //    }
            //}
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