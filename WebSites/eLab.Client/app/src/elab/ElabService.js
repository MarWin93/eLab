(function(){
    'use strict';

    angular.module('elab')
        .service('elabService', ['$q', '$http', 'API_PATH', ElabService]);

    function ElabService($q, $http, API_PATH){
        var self = this;
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
            },
            login : function(userlogin){
                console.log(userlogin);
                return $http({
                    url: API_PATH + "TOKEN",
                    method: "POST",
                    data: {grant_type: 'password', username: userlogin.username, password: userlogin.password},
                    headers: {'Content-Type': 'application/x-www-form-urlencoded'}
                });
            },
            register : function(userInfo){
                return $http({
                    url: API_PATH + "account/register",
                    method: "POST",
                    data: userInfo
                });
            }
        };
    }

})();
