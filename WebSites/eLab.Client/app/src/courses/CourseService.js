(function(){
    'use strict';

    angular.module('courses')
        .service('courseService', ['$q', CourseService]);

    function CourseService($q){
        var courses = [
            {
                id: 3,
                name: 'Nierelacyjne bazy danych',
                description: 'Więcej informacji o NO-SQL.',
                topics: [
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
                ],
                files: [
                    {
                        name: "instrukcja_do_lab1.pdf",
                        size: 871
                    },
                    {
                        name: "instrukcja_do_lab2.pdf",
                        size: 192
                    }
                ]
            },
            {
                id: 1,
                name: 'Eksploracja danych',
                description: 'Więcej informacji o eksploracji danych.',
                topics: [],
                files: []
            },
            {
                id: 2,
                name: 'Hurtownie danych',
                description: 'ETL rządzi!',
                topics: [],
                files: []
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
