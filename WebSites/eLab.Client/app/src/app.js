var eLabApp = angular.module('eLabApp', [
    'ngMaterial',
    'ngRoute',
    'courses',
    'classes',
    'elab',
    'topics',
    'dndLists'])
    .config(function($mdThemingProvider, $mdIconProvider){
        $mdIconProvider
            .defaultIconSet('./assets/svg/avatars.svg', 128);
        $mdThemingProvider.theme('default')
            .primaryPalette('teal')
            .accentPalette('red');
    });

eLabApp.value('API_PATH', 'http://localhost:8089/api/');

eLabApp.config(['$routeProvider',
    function($routeProvider) {
        $routeProvider.
        // home page
            // last added courses and classes
        when('/', {
            templateUrl: 'templates/home.html',
            controller: 'CourseController',
            controllerAs: 'cc'
        }).
        // courses
            // all courses list
        when('/courses', {
            templateUrl: 'templates/courses/course-list.html',
            controller: 'CourseController',
            controllerAs: 'cc'
        }).
            // course view (topics list)
        when('/courses/:courseId', {
            templateUrl: 'templates/courses/course-detail.html',
            controller: 'CourseController',
            controllerAs: 'cc'
        }).
            // add new course
        when('/courses/add', {
            templateUrl: 'templates/courses/course-edit.html',
            controller: 'CourseController',
            controllerAs: 'cc'
        }).
            // edit course
        when('/courses/:courseId/edit', {
            templateUrl: 'templates/courses/course-edit.html',
            controller: 'CourseController',
            controllerAs: 'cc'
        }).
            // edit certificate for course - optional
        //when('/courses/certificate/:courseId', {
        //    templateUrl: 'templates/courses/course-certificate.html',
        //    controller: 'CourseController'
        //}).
        
        // classes
            // add new class
        when('/classes/add', {
            templateUrl: 'templates/classes/class-edit.html',
            controller: 'ClassController',
            controllerAs: 'clc'
        }).
            // drafts, archives
        when('/classes', {
            templateUrl: 'templates/classes/class-list.html',
            controller: 'ClassController',
            controllerAs: 'clc'
        }).
            // classes
        when('/classes/:classId', {
            templateUrl: 'templates/classes/class-detail.html',
            controller: 'ClassController',
            controllerAs: 'clc'
        }).
            // edit
        when('/classes/:classId/edit', {
            templateUrl: 'templates/classes/class-edit.html',
            controller: 'ClassController',
            controllerAs: 'clc'
        }).
            // screens preview
        when('/classes/:classId/screens', {
            templateUrl: 'templates/screens/screen-list.html',
            controller: 'ScreensController',
            controllerAs: 'sc'
        }).
            // one screen live preview
        when('/classes/:classId/screens/:computerId', {
            templateUrl: 'templates/screens/screen-detail.html',
            controller: 'ScreensController',
            controllerAs: 'sc'
        }).
            // add test to class - optional
        //when('/classes/:classId/tests/add', {
        //    templateUrl: 'templates/courses/course-certificate.html',
        //    controller: 'CourseController'
        //}).
            // edit test - optional
        //when('/classes/:classId/tests/:testId', {
        //    templateUrl: 'templates/courses/course-certificate.html',
        //    controller: 'CourseController'
        //}).
        // groups
            // show groups, add new, edit, add student to group
        // when('/classes/:classId/groups/', {
        //     templateUrl: 'templates/groups/group-edit.html',
        //     controller: 'GroupController',
        //     controllerAs: 'gc'
        // }).
        
        // other
            // settings
        when('/settings', {
            templateUrl: 'templates/settings.html'
        }).
            // help
        when('/help', {
            templateUrl: 'templates/help.html'
        }).
        otherwise({
            redirectTo: '/'
        });
    }]);