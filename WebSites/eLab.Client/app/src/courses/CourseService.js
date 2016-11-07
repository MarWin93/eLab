angular.module('eLabApp').service('courseService', function ($http, API_PATH){
    return {
        getCourses: function() {
            return $http.get(API_PATH + 'courses');
        },
        
        getCourse: function (course_id) {
            return $http.get(API_PATH + 'courses/' + course_id);
        },
        
        createCourse: function (course) {
            return $http.post(API_PATH + 'courses', course);
        },

        deleteCourse: function (course) {
            return $http.delete(API_PATH + 'courses/' + course.id)
        },
        
        updateCourse: function (course) {
            return $http.put(API_PATH + 'courses/' + course.id, course)
        }
    };
});
