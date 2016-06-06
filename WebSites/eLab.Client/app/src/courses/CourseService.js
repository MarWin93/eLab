(function(){
    'use strict';

    angular.module('courses')
        .service('courseService', ['$q', '$http', 'API_PATH', CourseService]);

    function CourseService($q, $http, API_PATH){
        var courses = [];

        return {
            loadAllCourses : function() {
                var def = $q.defer();

                $http.get(API_PATH + 'courses')
                    .success(function(data) {
                        courses = data;
                        def.resolve(data);
                    })
                    .error(function() {
                        def.reject("Failed to get courses");
                    });
                return def.promise;
            }
        };
    }

})();
