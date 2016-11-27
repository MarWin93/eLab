angular.module('eLabApp').service('topicService', function ($http, API_PATH) {
    return {
        getTopics: function() {
            return $http.get(API_PATH + 'topics');
        },

        getTopic: function (topicId) {
            return $http.get(API_PATH + 'topics/' + topicId);
        },

        createTopic: function (topic) {
            return $http.post(API_PATH + 'topics', topic);
        },

        deleteTopic: function (topic) {
            return $http.delete(API_PATH + 'topics/' + topic.id);
        },

        updateTopic: function (topic) {
            return $http.put(API_PATH + 'topics/' + topic.id, topic);
        }
    };
});
