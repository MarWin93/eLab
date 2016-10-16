(function () {

    angular
        .module('courses', ['ngFileUpload'])
        .controller('CourseController', [
            '$scope', '$rootScope', 'courseService', '$routeParams', '$filter', '$timeout', '$q', '$http', 'API_PATH',
            'topicService', 'Upload',
            CourseController
        ]);

    function CourseController($scope, $rootScope, courseService, $routeParams, $filter, $timeout, $q, $http, API_PATH,
                              topicService, Upload) {
        var vm = this;

        vm.new = {
            'name': '',
            'description': '',
            'closed': false,
            'prowadzacyId': vm.user_id = sessionStorage.getItem('userId')
        };
        vm.selected = null;
        vm.courses = [];
        vm.topicsNavigation = topicService.navigation;
        var selected_id = $routeParams.courseId;

        if (selected_id) {
            courseService.loadCourse(selected_id)
                .then(function (course) {
                    vm.selected = course;
                });
        }

        courseService.loadAllCourses()
            .then(function (courses) {
                vm.courses = [].concat(courses);
            }).then(function () {
            $timeout(function () {
                vm.reloadTrianglify();
            })
        });

        vm.selectCourse = function (course) {
            vm.selected = course;
        };

        vm.goToCourse = function (course) {
            vm.selectCourse(course);
            window.location = "#/courses/" + vm.selected.id;
        };

        vm.goToUpdateCourse = function (course) {
            vm.selectCourse(course);
            window.location = "#/courses/" + vm.selected.id + '/edit';
        };

        vm.goToCreateCourse = function () {
            window.location = "#/courses/add";
        };

        vm.goToCreateCourse = function () {
            window.location = "#/courses/add";
        };

        vm.updateCourse = function () {
            var def = $q.defer();
            if (selected_id)
                $http.put(API_PATH + 'courses/' + vm.selected.id, vm.selected)
                    .success(function (data) {
                        window.location = "#/courses/";
                        $scope.$emit('updateCourses');
                    })
                    .error(function () {
                        def.reject("Failed to update course");
                    });
            else
                $http.post(API_PATH + 'courses', vm.selected)
                    .success(function (data) {
                        window.location = "#/courses/";
                        $scope.$emit('updateCourses');
                    })
                    .error(function () {
                        def.reject("Failed to create course");
                    });

            return def.promise;
        };

        vm.createCourse = function () {
            $http.post(API_PATH + 'courses', vm.new)
                .success(function (data) {
                    window.location = "#/courses/";
                    $scope.$emit('updateCourses');
                })
                .error(function () {
                    def.reject("Failed to create course");
                });
        };
        vm.deleteCourse = function (course) {
            var def = $q.defer();

            $http.delete(API_PATH + 'courses/' + course.id)
                .success(function (data) {
                    window.location = "#/courses/";
                    $scope.$emit('updateCourses');
                })
                .error(function () {
                    def.reject("Failed to delete course");
                });

            return def.promise;
        };

        vm.canEdit = function (course) {
            // return course.prowadzacyId == sessionStorage.getItem('userId');
        };

        vm.fileUpload = function (file, errFiles) {
            vm.file = file;
            vm.errFile = errFiles && errFiles[0];
            if (file) {
                console.log(file);
                Upload.upload({
                    url: API_PATH + 'courses/' + selected_id + '/upload',
                    data: {file: file}
                }).then(function (response) {
                    courseService.loadCourse(selected_id)
                        .then(function (course) {
                            vm.selected = course;
                        });
                }).catch(function (error) {
                    console.log("Failed to upload file.");
                    console.log(error)
                });
            }
        };

        vm.fileDownload = function (file) {
            $http.get(API_PATH + 'courses/' + selected_id + '/download/' + file.id)
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

        //this.startTopic = function (class_id, event) {
        //    // TODO change hardcoded class ID
        //    window.location = "#/topics/1";
        //}

        //this.editTopic = function (class_id, event) {
        //    console.log('siema');
        //    window.location = "#/topics/" + class_id + "/edit";
        //}

        // TODO delete it if unnecessary
        //this.createTopic = function (course){
        //    window.location = '#/topics/add';
        //}

        this.reloadTrianglify = function (course) {
            var pattern;
            var dimensions;
            var cover;
            var id;
            if (course) {
                id = 'cover-' + course.id;
                cover = document.getElementById(id);
                if (cover) {
                    dimensions = cover.getClientRects()[0];
                    pattern = Trianglify({
                        width: dimensions.width,
                        height: dimensions.height,
                        cell_size: 60,
                        seed: course.name,
                        x_colors: 'random'
                    });
                    cover.innerHTML = '';
                    cover.appendChild(pattern.canvas());
                }
            }
            else {
                for (var i = 0; i < vm.courses.length; i++) {
                    id = 'cover-' + vm.courses[i].id;
                    cover = document.getElementById(id);
                    if (cover) {
                        dimensions = cover.getClientRects()[0];
                        pattern = Trianglify({
                            width: dimensions.width,
                            height: dimensions.height,
                            cell_size: 75,
                            seed: vm.courses[i].name,
                            x_colors: 'random'
                        });
                        cover.innerHTML = '';
                        cover.appendChild(pattern.canvas());
                    }
                }
            }
        };

        $scope.$on('toggleChatWindow', function () {
            // TODO fix event sequence
            // self.reloadTrianglify();
        });


    }
})();
