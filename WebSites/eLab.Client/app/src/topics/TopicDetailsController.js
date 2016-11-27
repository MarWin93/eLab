angular.module('eLabApp').controller('TopicDetailsController', function ($scope, $rootScope, $state, topicService, $stateParams, 
                                                                         topic, courses, Upload, API_PATH, signalR, chatHelper, $http) {

    $scope.activeParticipants = chatHelper.activeParticipantsGet();
    var vm = this;

    vm.topic = topic;
    vm.courses = courses;
    vm.user = {
        id: $scope.user.id,
        name: $scope.user.name
    };

    signalR.signalRSetup($scope, vm.topic, vm.user);

    console.log(topic);
    vm.updateTopic = function () {
        return topicService.updateTopic(vm.topic).then(function () {
            $state.go('courseDetails', {courseId: $stateParams.courseId})
        }).catch(function (err) {
            console.log(err);
        });
    };

    vm.deleteTopic = function (course) {
        return topicService.deleteTopic(course).then(function () {
            $state.go('courseList');
        }).catch(function (err) {
            console.log(err);
        });
    };

    vm.fileUpload = function (file, errFiles) {
        vm.file = file;
        vm.errFile = errFiles && errFiles[0];
        if (file) {
            console.log(file);
            Upload.upload({
                url: API_PATH + 'topics/' + vm.topic.id + '/upload',
                data: {file: file}  
            }).then(function (response) {
                console.log(response);
                $state.reload();
            }).catch(function (error) {
                console.log("Failed to upload file.");
                console.log(error)
            });
        }
    };

    ////Stream sharing
    //vm.activeParticipants = [
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

    //$scope.streamingHub = null; // holds the reference to hub
    //$.connection.hub.url = 'http://localhost:8089/signalr';
    //$.connection.hub.jsonp = true;
    //$.connection.hub.logging = true; //debuging purpose

    //$scope.streamingHub = $.connection.streamingHub; // initializes hub

    ////client side function declarations 
    //$scope.streamingHub.client.updateUserThumbImage = function (userId, base64Image) {
    //    for (var i = 0; i < vm.activeParticipants.length; i++) {
    //        if (vm.activeParticipants[i].id == userId) {
    //            vm.activeParticipants[i].base64Image = base64Image;
    //            vm.activeParticipants[i].loop = vm.activeParticipants[i].loop + 1;
    //        }
    //    }
    //};

    //// starts hub
    //$.connection.hub.start({ jsonp: true }).done(function () {
    //    //server (hub) side function declarations
    //    $scope.streamingHub.server.joinGroup(vm.topic.id, "3");
    //});

});
