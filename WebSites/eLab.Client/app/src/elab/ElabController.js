(function(){

    angular
        .module('elab')
        .controller('ElabController', [
            '$scope', 'elabService', '$mdSidenav', '$timeout',
            ElabController
        ]);

    function ElabController($scope, elabService, $mdSidenav, $timeout) {
        var self = this;
        self.chat = true;
        self.selected = null;
        self.courses = [ ];

        self.username = null;
        self.user_id = null;
        self.is_lecturer = null;

        self.login_username = null;
        self.login_password = null;

        self.selectCourse = selectCourse;
        self.toggleSidenav = toggleSidenav;
        self.goToCourse = goToCourse;
        self.toggleChatWindow = toggleChatWindow;
        self.reloadTrianglify = reloadTrianglify;
        self.login = login;

        elabService.loadAllCourses()
            .then( function(courses) {
                self.courses = [].concat(courses);
                self.selected = courses[0];
            }).then( function(){
            $timeout(function () {
                self.reloadTrianglify();
            })
        });

        function toggleSidenav() {
            $mdSidenav('left').toggle();
        }

        function selectCourse(course){
            self.selected = course;
        }

        function goToCourse(course){
            self.selectCourse(course);
            window.location = "#/courses/"+self.selected.id;
        }

        function toggleChatWindow(){
            self.chat = !self.chat;
            $scope.$broadcast('toggleChatWindow');
        }

        function reloadTrianglify(){
            var pattern;
            var icon;
            var id;
            for (var i=0; i<self.courses.length; i++) {
                id = 'icon-' + self.courses[i].id;
                icon = document.getElementById(id);
                if(icon){
                    pattern = Trianglify({
                        width: 30,
                        height: 30,
                        cell_size: 18,
                        seed: self.courses[i].name,
                        x_colors: 'random'
                    });
                    icon.innerHTML = '';
                    icon.appendChild(pattern.canvas());
                }
            }
        }

        function login(){
            var userLogin = {
                grant_type: 'password',
                username: self.login_username,
                password: self.login_password
            };
            var promiselogin = elabService.login(userLogin);
            promiselogin.then(function (resp) {
                console.log('login successful!');
                self.username = resp.data.userName;
                //Store the token information in the SessionStorage
                //So that it can be accessed for other views
                sessionStorage.setItem('userName', resp.data.userName);
                sessionStorage.setItem('accessToken', resp.data.access_token);
                sessionStorage.setItem('refreshToken', resp.data.refresh_token);
            }, function (err) {
                console.log('login failed!');
                $scope.responseData="Error " + err.status;
            });
        }
    }
})();
