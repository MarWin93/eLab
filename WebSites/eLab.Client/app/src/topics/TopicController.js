(function(){

    angular
        .module('topics')
        .controller('TopicController', [
            '$scope', 'topicService', '$routeParams', '$filter', '$timeout', '$q', '$http', 'API_PATH',
            TopicController
        ]);

    function TopicController($scope, topicService, $routeParams, $filter, $timeout, $q, $http, API_PATH) {
        var self = this;
        //self.groups = [];
        self.topics = [];
        self.selected = null;
        self.group_count = null;
        self.navigation = topicService.navigation;
        var selected_id = $routeParams.topicId;
        
        if (selected_id)
            topicService.loadTopic(selected_id)
                .then(function (topic) {
                    self.selected = topic;
                });
        else
            topicService.loadAllTopics()
                .then(function (topics) {
                    self.topics = topics;
                });

        this.updateTopic = function () {
            var def = $q.defer();

            if (selected_id)
                $http.put(API_PATH + 'topics/' + selected_id, self.selected)
                    .success(function (data) {
                        window.location = "#/topics";
                    })
                    .error(function () {
                        def.reject("Failed to update course");
                    });
            else
                $http.post(API_PATH + 'topics', self.selected)
                    .success(function (data) {
                        window.location = "#/topics";
                    })
                    .error(function () {
                        def.reject("Failed to update course");
                    });

            return def.promise;
        }

        this.deleteTopic = function (selected_id) {
            var def = $q.defer();

            $http.delete(API_PATH + 'topics/' + selected_id, self.selected)
                .success(function (data) {
                    location.reload();
                })
                .error(function () {
                    def.reject("Failed to update course");
                });
        }

        $scope.$watch(
            function watchGroupCount(scope){
                return self.group_count;
            },
            function handleGroupCountChange(newValue, oldValue){
                //console.log(self.group_count);
                // TODO obsługa zwiększania ilości grup tutaj
                // połączenie wartości self.group_count z ilością elementów w self.selected.lists
            }
        );
    }
})();
