angular.module('eLabApp').controller('CourseDetailsController', function ($scope, $rootScope, $state, courseService,
                                                                          $stateParams, $http, API_PATH,
                                                                          topicService, Upload, course) {
    var vm = this;
    vm.course = course;
    vm.activeTopics = [];
    vm.archivedTopics = [];
    angular.forEach(vm.course.topics, function (value, key) {
        if (value.isArchived) {
            vm.archivedTopics.push(value);
        } else {
            vm.activeTopics.push(value);
        }
    });
    vm.userEnrollmentKey = '';
    vm.isUserEnrolled = false;
    $http.get(API_PATH + 'participations/' + vm.course.id + '/' + $rootScope.user.id)
        .then(function (response) {
            vm.isUserEnrolled = response.data;
            console.log(response.data);
            console.log(vm.isUserEnrolled);
        }).catch(function (err) {
            console.log(err);
        });

    vm.updateCourse = function () {
        return courseService.updateCourse(vm.course).then(function () {
            $state.go('courseList')
        }).catch(function (err) {
            console.log(err);
        });
    };

    vm.deleteCourse = function (course) {
        return courseService.deleteCourse(course).then(function () {
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
                url: API_PATH + 'courses/' + vm.course.id + '/upload',
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

    // Based on an implementation here: web.student.tuwien.ac.at/~e0427417/jsdownload.html
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

    vm.enrollCourse = function () {
        if (vm.course.enrollmentKey == vm.userEnrollmentKey) {
            var participation = {
                courseId: vm.course.id,
                userId: $rootScope.user.id
            };
            console.log(participation);
            // [Route("api/participations/{CourseId}/{UserId}/enroll/{EnrollmentKey}")]
            return $http.put(API_PATH + 'participations/' + vm.course.id + '/' + $rootScope.user.id + '/enroll/' + vm.course.enrollmentKey, participation).then(function() {
                console.log('enroll successful');
                $state.reload();
            }).catch(function (err) {
                console.log(err);
            });
        }
    };

    vm.leaveCourse = function () {
            var participation = {
                courseId: vm.course.id,
                userId: $rootScope.user.id
            };
            console.log(participation);
            // [Route("api/participations/{CourseId}/{UserId}/enroll/{EnrollmentKey}")]
            return $http.put(API_PATH + 'participations/' + vm.course.id + '/' + $rootScope.user.id + '/leave/', participation).then(function () {
                console.log('leave successful');
                $state.go('courseListUser');
            }).catch(function (err) {
                console.log(err);
            });
      
    };

});

