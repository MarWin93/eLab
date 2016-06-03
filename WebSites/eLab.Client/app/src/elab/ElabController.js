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

        self.selectCourse = selectCourse;
        self.toggleSidenav = toggleSidenav;
        self.goToCourse = goToCourse;
        self.toggleChatWindow = toggleChatWindow;
        self.reloadTrianglify = reloadTrianglify;
    
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
    }
})();
