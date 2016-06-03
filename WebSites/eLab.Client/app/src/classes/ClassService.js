(function(){
    'use strict';

    angular.module('classes')
        .service('classService', ['$q', ClassService]);

    function ClassService($q){
        var classes = [
            {
                id: 1,
                name: 'Nierelacyjne bazy danych',
                group_count: 2,
                selected: null,
                lists: {
                    name: 'unassigned',
                    students: [
                        'Maria Klima',
                        'Marcin Winkler',
                        'Bartosz Klukaczewski',
                        'Magda Kwidzyńska',
                        'Piotr Grabuszyński'
                    ],
                    'Grupa 1': [],
                    'Grupa 2': []
                }
            }
        ];

        return {
            loadAllClasses : function() {
                // Simulate async nature of real remote calls
                return $q.when(classes);
            }
        };
    }

})();
