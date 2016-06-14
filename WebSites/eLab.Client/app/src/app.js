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
            // last added courses and topics
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
        // add new course
        when('/courses/add', {
            templateUrl: 'templates/courses/course-edit.html',
            controller: 'CourseController',
            controllerAs: 'cc'
        }).
            // course view (topics list)
        when('/courses/:courseId', {
            templateUrl: 'templates/courses/course-detail.html',
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
        
        // topics
            // add new topic
        when('/topics/add', {
            templateUrl: 'templates/topics/topic-edit.html',
            controller: 'TopicController',
            controllerAs: 'tc'
        }).
            // drafts, archives
        when('/topics', {
            templateUrl: 'templates/topics/topic-list.html',
            controller: 'TopicController',
            controllerAs: 'tc'
        }).
            // topics details
        when('/topics/:topicId', {
            templateUrl: 'templates/topics/topic-detail.html',
            controller: 'ClassController',
            controllerAs: 'tc'
        }).
            // edit
        when('/topics/:topicId/edit', {
            templateUrl: 'templates/topics/topic-edit.html',
            controller: 'TopicController',
            controllerAs: 'tc'
        }).
            // screens preview
        when('/topics/:topicId/screens', {
            templateUrl: 'templates/screens/screen-list.html',
            controller: 'ScreensController',
            controllerAs: 'sc'
        }).
            // one screen live preview
        when('/topics/:topicId/screens/:computerId', {
            templateUrl: 'templates/screens/screen-detail.html',
            controller: 'ScreensController',
            controllerAs: 'sc'
        }).
        // other
            // settings
        when('/settings', {
            templateUrl: 'templates/settings.html'
        }).
            // help
        when('/help', {
            templateUrl: 'templates/help.html'
        }).
        when('/login', {
            templateUrl: 'templates/login.html'
        }).
        otherwise({
            redirectTo: '/'
        });
    }]);