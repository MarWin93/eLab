(function(){
    'use strict';

    angular.module('topics')
        .service('topicService', ['$q', '$http', 'API_PATH', TopicService]);

    function TopicService($q, $http, API_PATH) {
        var topics = [];

        return {
            loadAllTopics : function() {
                var def = $q.defer();

                $http.get(API_PATH + 'topics')
                    .success(function(data) {
                        topics = data;
                        def.resolve(data);
                    })
                    .error(function() {
                        def.reject("Failed to get topics");
                    });
                return def.promise;
            },
            loadTopic: function (topic_id) {
                var def = $q.defer();

                $http.get(API_PATH + 'topics/' + topic_id)
                    .success(function (data) {
                        topics = data;
                        def.resolve(data);
                    })
                    .error(function () {
                        def.reject("Failed to get topic");
                    });
                return def.promise;
            },
            loadAllGroups: function () {
                var def = $q.defer();

                $http.get(API_PATH + 'groups')
                    .success(function (data) {
                        topics = data;
                        def.resolve(data);
                    })
                    .error(function () {
                        def.reject("Failed to get groups");
                    });
                return def.promise;
            },
            navigation: {
                editTopic: function (topic_id) {
                    window.location = "#/topics/" + topic_id + "/edit";
                },
                startTopic: function (topic_id) {
                    // TODO change hardcoded class ID
                    window.location = "#/topics/" + topic_id;
                },
                addTopic: function () {
                    window.location = '#/topics/add';
                }
            }
        };
    }

})();
