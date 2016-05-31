(function(){
    'use strict';

    angular.module('elab')
        .service('elabService', ['$q', ElabService]);
    
    function ElabService($q){
        var courses = [
            {
                id: 3,
                name: 'Nierelacyjne bazy danych'
            },
            {
                id: 1,
                name: 'Eksploracja danych'
            },
            {
                id: 2,
                name: 'Hurtownie danych'
            }

        ];

        // Promise-based API
        return {
            loadAllCourses : function() {
                // Simulate async nature of real remote calls
                return $q.when(courses);
            }
        };
    }

})();
