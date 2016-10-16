(function(){

    angular
        .module('elab')
        .controller('ElabController', [
            '$scope', '$rootScope', 'elabService', '$mdSidenav', '$timeout',
            ElabController
        ]);

    function ElabController($scope, $rootScope, elabService, $mdSidenav, $timeout) {
        var vm = this;
        vm.chat = true;
        vm.selected = null;
        vm.courses = [ ];
        vm.topics = [];
        
        vm.username = sessionStorage.getItem('userName');
        vm.user_id = sessionStorage.getItem('userId');
        vm.is_lecturer = null;

        vm.login_username = null;
        vm.login_password = null;
        vm.error = null;

        vm.selectCourse = selectCourse;
        vm.toggleSidenav = toggleSidenav;
        vm.goToCourse = goToCourse;
        vm.goToTopic = goToTopic;
        vm.toggleChatWindow = toggleChatWindow;
        vm.reloadTrianglify = reloadTrianglify;
    
        vm.login = login;
        vm.logout = logout;

        function updateCourses() {
            elabService.loadAllCourses()
                .then(function (courses) {
                    vm.courses = [].concat(courses);
                    vm.selected = courses[0];
                }).then(function () {
                    $timeout(function () {
                        vm.reloadTrianglify();
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
            vm.selected = course;
        }

        function goToCourse(course){
            vm.selectCourse(course);
            window.location = "#/courses/"+vm.selected.id;
        }
        
        function goToTopic(topic) {
            window.location = "#/topics/"+topic.id;
        }

        function toggleChatWindow(){
            vm.chat = !vm.chat;
            $scope.$broadcast('toggleChatWindow');
        }

        function reloadTrianglify(){
            var pattern;
            var icon;
            var id;
            for (var i=0; i<vm.courses.length; i++) {
                id = 'icon-' + vm.courses[i].id;
                icon = document.getElementById(id);
                if(icon){
                    pattern = Trianglify({
                        width: 30,
                        height: 30,
                        cell_size: 18,
                        seed: vm.courses[i].name,
                        x_colors: 'random'
                    });
                    icon.innerHTML = '';
                    icon.appendChild(pattern.canvas());
                }
            }
        }

        function login(){
            if (!vm.login_password){
                vm.error = 'Hasło jest wymagane.';
                return
            }
            var userLogin = {
                grant_type: 'password',
                username: vm.login_username,
                password: vm.login_password
            };
            var promiselogin = elabService.login(userLogin);
            promiselogin.then(function (response) {
                if (!response.data.username){
                    vm.error = 'Niepoprawny login lub hasło.';
                    return
                }
                sessionStorage.setItem('userName', response.data.username);
                sessionStorage.setItem('userId', response.data.id);
                sessionStorage.setItem('accessToken', response.data.access_token);
                sessionStorage.setItem('refreshToken', response.data.refresh_token);
                vm.username = sessionStorage.getItem('userName');
                vm.user_id = sessionStorage.getItem('userId');
                window.location = "#";
            }, function (err) {
                $scope.responseData="Error " + err.status;
            });
        }
        function logout(){
            vm.username = null;
            vm.user_id = null;
            sessionStorage.removeItem('userName');
            sessionStorage.removeItem('accessToken');
            window.location = "#";
        }
    }
})();
