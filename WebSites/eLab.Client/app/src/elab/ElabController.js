(function(){

    angular
        .module('elab')
        .controller('ElabController', [
            '$scope', '$rootScope', 'elabService', '$mdSidenav', '$timeout',
            ElabController
        ]);

    function ElabController($scope, $rootScope, elabService, $mdSidenav, $timeout) {
        var self = this;
        self.chat = true;
        self.selected = null;
        self.courses = [ ];

        self.username = sessionStorage.getItem('userName');
        self.user_id = sessionStorage.getItem('userId');
        self.is_lecturer = null;

        self.login_username = null;
        self.login_password = null;

        self.selectCourse = selectCourse;
        self.toggleSidenav = toggleSidenav;
        self.goToCourse = goToCourse;
        self.toggleChatWindow = toggleChatWindow;
        self.reloadTrianglify = reloadTrianglify;

        self.login = login;
        self.logout = logout;
    
        function updateCourses() {
            elabService.loadAllCourses()
                .then(function (courses) {
                    self.courses = [].concat(courses);
                    self.selected = courses[0];
                }).then(function () {
                    $timeout(function () {
                        self.reloadTrianglify();
                    })
                });
        }
        updateCourses();
        $scope.$on('updateCourses', function () {
            updateCourses();
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
            promiselogin.then(function (response) {
                console.log('login successful!');
                console.log(response);
                self.username = response.data.userName;
                //Store the token information in the SessionStorage
                //So that it can be accessed for other views
                sessionStorage.setItem('userName', response.data.userName);
                sessionStorage.setItem('userId', response.data.userId);
                sessionStorage.setItem('accessToken', response.data.access_token);
                sessionStorage.setItem('refreshToken', response.data.refresh_token);
                this.username = sessionStorage.getItem('userName');
                window.location = "#";
            }, function (err) {
                console.log('login failed!');
                $scope.responseData="Error " + err.status;
            });
        }
        function logout(){
            this.username = null;
            this.user_id = null;
            sessionStorage.removeItem('userName');
            sessionStorage.removeItem('accessToken');
            window.location = "#";
        }
    }
})();
