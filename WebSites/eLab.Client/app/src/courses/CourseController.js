(function(){

    angular
        .module('courses')
        .controller('CourseController', [
            '$scope', 'courseService', '$routeParams', '$filter', '$timeout', '$q', '$http', 'API_PATH',
            CourseController
        ]);

    function CourseController($scope, courseService, $routeParams, $filter, $timeout, $q, $http, API_PATH) {
        var self = this;

        self.new = {'name': '', 'description': '', 'closed' :false};
        self.selected = null;
        self.courses = [ ];

        self.selectCourse = selectCourse;
        self.goToCourse = goToCourse;
        self.editCourse = editCourse;
        self.addCourse = addCourse;
        self.updateCourse = updateCourse;

        self.startClass = startClass;
        self.reloadTrianglify = reloadTrianglify;

        courseService.loadAllCourses()
            .then( function(courses) {
                self.courses = [].concat(courses);
                self.selected = $filter('filter')(self.courses, {id: $routeParams.courseId})[0];
            }).then( function(){
            $timeout(function () {
                self.reloadTrianglify();
            })
        });

        function selectCourse(course){
            self.selected = course;
        }

        function goToCourse(course){
            self.selectCourse(course);
            window.location = "#/courses/"+self.selected.id;
        }

        function editCourse(course){
            self.selectCourse(course);
            window.location = "#/courses/"+self.selected.id+'/edit';
        }

        function updateCourse(){
            var def = $q.defer();
            console.log('update');
            $http.put(API_PATH + 'courses/' + self.selected.id, self.selected)
                .success(function(data) {
                    window.location = "#/courses/";
                })
                .error(function() {
                    def.reject("Failed to update course");
                });
            return def.promise;
        }

        function addCourse(){
            var def = $q.defer();

            $http.post(API_PATH + 'courses', self.new)
                .success(function(data) {
                    window.location = "#/courses/";
                })
                .error(function() {
                    def.reject("Failed to create course");
                });
            return def.promise;
        }

        function startClass(topic_id, event){
            // TODO change hardcoded class ID
            window.location = "#/classes/1";
        }

        function reloadTrianglify(course){
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
        }

        $scope.$on('toggleChatWindow', function() {
            // TODO fix event sequence
            // self.reloadTrianglify();
        });
    }
})();
