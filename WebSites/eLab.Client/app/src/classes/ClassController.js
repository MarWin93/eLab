(function(){

    angular
        .module('classes')
        .controller('ClassController', [
            'classService', '$routeParams', '$filter', '$scope',
            ClassController
        ]);

    function ClassController(classService, $routeParams, $filter, $scope) {
        var self = this;
        self.groups = [];
        self.selected = null;
        self.group_count = null;

        classService.loadAllClasses()
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
                // TODO obsługa zwiększania ilości grup tutaj
                // połączenie wartości self.group_count z ilością elementów w self.selected.lists
            }
        );
    }
})();
