(function(){
    'use strict';

    angular.module('topics')
        .service('topicService', ['$q', TopicService]);

    function TopicService($q){
        var topics = [
            {
                'id': 1,
                'name': 'Wprowadzenie do nierelacyjnych baz danych',
                'description': 'krótki opis danej lekcji'
            },
            {
                'id': 2,
                'name': 'Zapoznanie z narzędziami oraz środowiskiem',
                'description': 'krótki opis danej lekcji'
            }
        ];

        return {
            loadAllTopics : function() {
                // Simulate async nature of real remote calls
                return $q.when(topics);
            }
        };
    }

})();
