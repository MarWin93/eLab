(function(){

    angular
        .module('topics')
        .controller('TopicController', [
            'topicService', '$routeParams', '$filter', '$scope',
            TopicController
        ]);

    function TopicController(topicService, $routeParams, $filter, $scope) {
        var self = this;
        self.groups = [];
        self.selected = null;
        self.group_count = null;
        
        topicService.loadAllTopics()
            .then( function(groups) {
                self.groups = [].concat(groups);
                var selected_id = $routeParams.groupId;
                self.selected = $filter('filter')(self.groups, {id: selected_id})[0];
                self.group_count = self.selected.group_count;
            });

        $scope.$watch(
            function watchGroupCount(scope){
                return self.group_count;
            },
            function handleGroupCountChange(newValue, oldValue){
                console.log(self.group_count);
                // TODO obsługa zwiększania ilości grup tutaj
                // połączenie wartości self.group_count z ilością elementów w self.selected.lists
            }
        );
    }
})();
