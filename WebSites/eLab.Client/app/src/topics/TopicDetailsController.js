angular.module('eLabApp').controller('TopicDetailsController', function ($scope, $rootScope, $state, topicService, $stateParams, $mdDialog, 
                                                                         topic, /*course,*/ Upload, API_PATH, signalR, chatHelper, $http) {

    $scope.activeParticipants = chatHelper.activeParticipantsGet();
    $scope.customFullscreen = false;
    $scope.activeParticipant = { id: '55', base64Image: '' };

    $scope.selectedColor = 0;
    $scope.colors = ['0,0,0', '128,128,128', '255,255,255', '255,0,0', '255,106,0', '255,216,0',
                        '182,255,0', '0,255,33', '0,148,255', '0,38,255', '255,0,110'];

    var vm = this;

    vm.topic = topic;
    vm.course = { teacherId: 15};//course;
    vm.user = {
        id: $scope.user.id,
        name: $scope.user.name
    };

    vm.isATeacherOrAdmin = ($scope.user.group === 'Admin' || $scope.user.id === course.teacherId);

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

    $scope.fullScreenWatch = function (ev, participant) {
        $scope.activeParticipant = participant;
        signalR.watchUserScreen($scope, participant.id, vm.topic.id);
        $mdDialog.show({
            controller: DialogController,
            contentElement: '#watchFullScreenDialog',
            parent: angular.element(document.body),
            targetEvent: ev,
            clickOutsideToClose: true,
            onRemoving: function (scope, element) {
                signalR.stopWatchingUserScreen($scope, vm.topic.id);
            }
        });
    };


    $scope.closeDialog = function() {
        alert("clicked");
    }

    function DialogController($scope, $mdDialog) {
        $scope.hide = function () {
            $mdDialog.hide();
        };

        $scope.cancel = function () {
            $mdDialog.cancel();
        };

        $scope.answer = function (answer) {
            $mdDialog.hide(answer);
        };
        $scope.closeDialog = function () {
            $mdDialog.cancel();
        };
    }
});
