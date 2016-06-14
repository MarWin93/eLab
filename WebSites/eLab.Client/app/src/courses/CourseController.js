(function(){

    angular
        .module('courses')
        .controller('CourseController', [
            '$scope', 'courseService', '$routeParams', '$filter', '$timeout', '$q', '$http', 'API_PATH', 'topicService',
            CourseController
        ]);

    function CourseController($scope, courseService, $routeParams, $filter, $timeout, $q, $http, API_PATH, topicService) {
        var self = this;

        self.new = {
            'name': '',
            'description': '',
            'closed': false,
            'prowadzacyId': 1
        };
        self.selected = null;
        self.courses = [];
        self.topicsNavigation = topicService.navigation;

        courseService.loadAllCourses()
            .then( function(courses) {
                self.courses = [].concat(courses);
                self.selected = $filter('filter')(self.courses, {id: $routeParams.courseId})[0];
            }).then( function(){
            $timeout(function () {
                self.reloadTrianglify();
            })
        });

        this.selectCourse = function (course) {
            self.selected = course;
        };

        this.goToCourse = function (course) {
            self.selectCourse(course);
            window.location = "#/courses/"+self.selected.id;
        };

        this.goToUpdateCourse = function (course){
            self.selectCourse(course);
            window.location = "#/courses/"+self.selected.id+'/edit';
        };

        this.updateCourse = function (){
            var def = $q.defer();

            $http.put(API_PATH + 'courses/' + self.selected.id, self.selected)
                .success(function(data) {
                    window.location = "#/courses/";
                })
                .error(function() {
                    def.reject("Failed to update course");
                });
            return def.promise;
        };

        this.deleteCourse = function (course){
            var def = $q.defer();

            $http.delete(API_PATH + 'courses/' + course.id)
                .success(function(data) {
                    window.location = "#/courses/";
                })
                .error(function() {
                    def.reject("Failed to delete course");
                });
            return def.promise;
        };

        this.goToCreateCourse = function (){
            window.location = "#/courses/add";
        };
        
        this.createCourse = function (){
            var def = $q.defer();

            $http.post(API_PATH + 'courses', self.new)
                .success(function(data) {
                    window.location = "#/courses/";
                })
                .error(function() {
                    def.reject("Failed to create course");
                });
            return def.promise;
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
            if (course){
                id = 'cover-' + course.id;
                cover = document.getElementById(id);
                if(cover){
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
            else{
                for (var i=0; i<self.courses.length; i++) {
                    id = 'cover-' + self.courses[i].id;
                    cover = document.getElementById(id);
                    if(cover){
                        dimensions = cover.getClientRects()[0];
                        pattern = Trianglify({
                            width: dimensions.width,
                            height: dimensions.height,
                            cell_size: 75,
                            seed: self.courses[i].name,
                            x_colors: 'random'
                        });
                        cover.innerHTML = '';
                        cover.appendChild(pattern.canvas());
                    }
                }
            }
        };

        $scope.$on('toggleChatWindow', function() {
            // TODO fix event sequence
            // self.reloadTrianglify();
        });
    }
})();
