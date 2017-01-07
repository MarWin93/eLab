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
    vm.topic = topic;
    vm.course = course;
    vm.user = {
        id: $scope.user.id,
        name: $scope.user.name
    };

    vm.isATeacherOrAdmin = ($scope.user.group === 'Admin' || $scope.user.id == course.teacherId);

    signalR.signalRSetup($scope, vm.topic, vm.user);

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
        // Use an arraybuffer
        $http.get(API_PATH + 'courses/' + vm.course.id + '/download/' + file.id, { responseType: 'arraybuffer' })
        .success(function (data, status, headers) {

            var octetStreamMime = 'application/octet-stream';
            var success = false;
             
            // Get the headers
            headers = headers();

            // Get the filename from the x-filename header or default to "download.bin"
            var filename = headers['x-filename'] || file.name;

            // Determine the content type from the header or default to "application/octet-stream"
            var contentType = headers['content-type'] || octetStreamMime;

            try {
                // Try using msSaveBlob if supported
                console.log("Trying saveBlob method ...");
                var blob = new Blob([data], { type: contentType });
                if (navigator.msSaveBlob)
                    navigator.msSaveBlob(blob, filename);
                else {
                    // Try using other saveBlob implementations, if available
                    var saveBlob = navigator.webkitSaveBlob || navigator.mozSaveBlob || navigator.saveBlob;
                    if (saveBlob === undefined) throw "Not supported";
                    saveBlob(blob, filename);
                }
                console.log("saveBlob succeeded");
                success = true;
            } catch (ex) {
                console.log("saveBlob method failed with the following exception:");
                console.log(ex);
            }

            if (!success) {
                // Get the blob url creator
                var urlCreator = window.URL || window.webkitURL || window.mozURL || window.msURL;
                if (urlCreator) {
                    // Try to use a download link
                    var link = document.createElement('a');
                    if ('download' in link) {
                        // Try to simulate a click
                        try {
                            // Prepare a blob URL
                            console.log("Trying download link method with simulated click ...");
                            var blob = new Blob([data], { type: contentType });
                            var url = urlCreator.createObjectURL(blob);
                            link.setAttribute('href', url);

                            // Set the download attribute (Supported in Chrome 14+ / Firefox 20+)
                            link.setAttribute("download", filename);

                            // Simulate clicking the download link
                            var event = document.createEvent('MouseEvents');
                            event.initMouseEvent('click', true, true, window, 1, 0, 0, 0, 0, false, false, false, false, 0, null);
                            link.dispatchEvent(event);
                            console.log("Download link method with simulated click succeeded");
                            success = true;

                        } catch (ex) {
                            console.log("Download link method with simulated click failed with the following exception:");
                            console.log(ex);
                        }
                    }

                    if (!success) {
                        // Fallback to window.location method
                        try {
                            // Prepare a blob URL
                            // Use application/octet-stream when using window.location to force download
                            console.log("Trying download link method with window.location ...");
                            var blob = new Blob([data], { type: octetStreamMime });
                            var url = urlCreator.createObjectURL(blob);
                            window.location = url;
                            console.log("Download link method with window.location succeeded");
                            success = true;
                        } catch (ex) {
                            console.log("Download link method with window.location failed with the following exception:");
                            console.log(ex);
                        }
                    }

                }
            }

            if (!success) {
                // Fallback to window.open method
                console.log("No methods worked for saving the arraybuffer, using last resort window.open");
                window.open(httpPath, '_blank', '');
            }
        })
        .error(function (data, status) {
            console.log("Request failed with status: " + status);

            // Optionally write the error out to scope
            $scope.errorDetails = "Request failed with status: " + status;
        });
    };

    vm.present = function (pdfId) {
        $scope.newMessage({
            operation: 'changeFile',
            fileId: pdfId
        });
    };

    vm.isPdf = function (file) {
        return (file.name.slice(-3)=='pdf' || file.name.slice(-3)=='PDF' || file.name.slice(-3)=='Pdf');
    }

    vm.changePDF = function (pdfId) {
        $rootScope.$broadcast('changePDF', API_PATH + 'topics/' + topic.id + '/show/' + pdfId);
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