angular.module('eLabApp').controller('TopicDetailsController', function ($scope, $rootScope, $state, topicService, $stateParams, $mdDialog, 
                                                                         topic, course, courses, Upload, API_PATH, signalR, chatHelper, $mdToast, $http) {

    $scope.activeParticipants = chatHelper.activeParticipantsGet();
    $scope.customFullscreen = false;
    $scope.activeParticipant = { id: '55', base64Image: '' };

    $scope.selectedColor = 0;
    $scope.colors = ['0,0,0', '128,128,128', '255,255,255', '255,0,0', '255,106,0', '255,216,0',
                        '182,255,0', '0,255,33', '0,148,255', '0,38,255', '255,0,110'];

    var vm = this;
    vm.courses = courses;
    vm.pdfURL = '/app/src/firstPage.pdf';
    vm.topic = topic;
    vm.course = course;
    vm.user = {
        id: $scope.user.id,
        name: $scope.user.name
    };

    vm.isATeacherOrAdmin = ($scope.user.group === 'Admin' || $scope.user.id == course.teacherId);

    signalR.signalRSetup($scope, vm.topic, vm.user);

    console.log(topic);
    vm.updateTopic = function () {
        return topicService.updateTopic(vm.topic).then(function () {
            $state.go('courseDetails', {courseId: $stateParams.courseId})
        }).catch(function (err) {
            console.log(err);
        });
    };

    vm.updateStreamUrl = function () {
        return topicService.updateTopic(vm.topic).then(function (res) {
            console.log('updated url!');
        }).catch(function (e) {
            console.log(e);
        })
    };

    $scope.closeTopic = function () {
        return topicService.closeTopic(vm.topic).then(function () {
            $state.go('courseDetails', { "courseId": vm.course.id });
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

    vm.fileDownload = function (file) {
        $http.get(API_PATH + 'topics/' + vm.topic.id + '/download/' + file.id)
            .then(function (response) {

                var byteNumbers = new Array(response.data.length);
                for (var i = 0; i < response.data.length; i++) {
                    byteNumbers[i] = response.data.charCodeAt(i);
                }
                var byteArray = new Uint8Array(byteNumbers);

                var a = window.document.createElement('a');
                a.href = window.URL.createObjectURL(new Blob([byteArray], { type: 'octet/stream' }));
                a.download = file.name;
                document.body.appendChild(a);
                a.click();
                document.body.removeChild(a);
            })
            .catch(function (error) {
                console.log("Failed to delete download file");
                console.log(error);
            });
    };

    vm.present = function (pdfId) {
        $scope.newMessage({
            operation: 'changeFile',
            fileId: pdfId
        });
    };

    vm.changePDF = function (pdfId) {
        $scope.ctrl.pdfURL = API_PATH + 'topics/' + topic.id + '/show/' + pdfId;
        $rootScope.$broadcast('gotoPage', 1);
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
    };

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

    $scope.showActionToast = function () {
        if (!vm.isATeacherOrAdmin) {
            var toast = $mdToast.simple()
                .textContent('Włącz agenta!')
                .hideDelay(0)
                .action('Zamknij')
                .highlightAction(true)
                .highlightClass('md-warn')
                .position('bottom right');

            $mdToast.show(toast)
                .then(function(response) {
                    if (response == 'ok') {
                    }
                });
        }
    };

    $scope.closeActionToast = function (userId) {
        if (userId == vm.user.id) {
            $mdToast.cancel();
        }

    };

});


angular.module('eLabApp').config(function ($sceDelegateProvider) {
    API_PATH = 'http://elab-pg.azurewebsites.net/api/';
    //API_PATH = window.location.protocol + '//' + window.location.hostname + ':8089/';
    console.log(API_PATH + '**');
    $sceDelegateProvider.resourceUrlWhitelist([
      'self',
      API_PATH + '**'
    ]);
});