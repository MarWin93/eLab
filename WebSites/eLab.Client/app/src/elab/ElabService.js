angular.module('eLabApp').factory('elabService', function ElabService($http, API_PATH){
    return {
        loadAllCourses : function() {
            return $http.get(API_PATH + 'courses')
        },
        login : function(userlogin){
            return $http.post(
                API_PATH + 'TOKEN',
                {grant_type: 'password', username: userlogin.username, password: userlogin.password}
            )
        },
        register : function(userInfo){
            return $http({
                url: API_PATH + "account/register",
                method: "POST",
                data: userInfo
            });
        }
    };
});