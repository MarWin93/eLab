(function(){
    'use strict';

    angular.module('elab')
        .service('elabService', ['$q', '$http', 'API_PATH', ElabService]);

    function ElabService($q, $http, API_PATH){
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
