angular.module('eLabApp').service('classService', function($q){
    return {
        loadAllClasses : function() {
            // Simulate async nature of real remote calls
            return $q.when(classes);
        }
    };
});
