angular.module('eLabApp').controller('TopicController', function ($scope, $state, topicService, $stateParams, courses) {
    var vm = this;
    vm.courses = courses;
    vm.topic = {
        name: '',
        description: '',
        isArchived: false,
        courseId: $stateParams.courseId
    };
    vm.createTopic = function () {
        return topicService.createTopic(vm.topic).then(function () {
            console.log(vm.topic);
            $state.go('courseDetails', {courseId: $stateParams.courseId})
        }).catch(function (err) {
            console.log(err);
        });
    };

    vm.deleteTopic = function (topic) {
        return topicService.deleteTopic(topic).then(function () {
            $state.reload();
        }).catch(function (err) {
            console.log(err);
        });
    };
});
