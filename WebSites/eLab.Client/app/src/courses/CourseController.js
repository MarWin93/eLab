angular.module('eLabApp').controller('CourseController', function ($scope, $rootScope, $state, $http, courses,
                                                                   courseService, topics) {
    var vm = this;
    vm.courses = courses;
    vm.topics = topics;
    
    vm.course = {
        'name': '',
        'description': '',
        'closed': false,
        'enrollmentKey': '',
        'teacherId': $rootScope.user.id
    };

    vm.createCourse = function () {
        return courseService.createCourse(vm.course).then(function () {
            $state.go('courseList')
        }).catch(function (err) {
            console.log(err);
        });
    };

    vm.deleteCourse = function (course) {
        return courseService.deleteCourse(course).then(function () {
            $state.reload();
        }).catch(function (err) {
            console.log(err);
        });
    };
});

