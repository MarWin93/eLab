angular.module('eLabApp').controller('CourseDetailsController', function ($scope, $rootScope, $state, courseService,
                                                                          $stateParams, $http, API_PATH,
                                                                          topicService, Upload, course) {
    var vm = this;
    vm.course = course;
    vm.userEnrollmentKey = '';

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

    vm.fileDownload = function (file) {
        $http.get(API_PATH + 'courses/' + vm.course.id + '/download/' + file.id)
            .then(function (response) {

                var byteNumbers = new Array(response.data.length);
                for (var i = 0; i < response.data.length; i++) {
                    byteNumbers[i] = response.data.charCodeAt(i);
                }
                var byteArray = new Uint8Array(byteNumbers);

                var a = window.document.createElement('a');
                a.href = window.URL.createObjectURL(new Blob([byteArray], {type: 'octet/stream'}));
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

    vm.isEnrolled = function () {
        return false;
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

});

