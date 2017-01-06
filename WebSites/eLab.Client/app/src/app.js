var eLabApp = angular.module('eLabApp', ['ngMaterial', 'ngResource', 'ui.router', 'dndLists', 'ngFileUpload', 'SignalR', 'ngPDFViewer'])
    .config(function($mdThemingProvider, $mdIconProvider, $stateProvider, $sceProvider){
        $mdIconProvider.defaultIconSet('./assets/svg/avatars.svg', 128);
        $mdThemingProvider.theme('default').primaryPalette('teal').accentPalette('red');
        $sceProvider.enabled(false);
        $stateProvider
            .state('settings', {
                url: '/settings',
                templateUrl: 'templates/settings.html'
            })
            .state('help', {
                url: '/help',
                templateUrl: 'templates/help.html'
            })

            .state('teacher', {
                url: '/teacher',
                templateUrl: 'templates/home.html',
                controller: 'CourseController as ctrl',
                resolve: {
                    courses: function (courseService, $rootScope) {
                        return courseService.getCourses().then(function (response) {
                            return response.data.filter(function(elem) {
                                return elem.teacherId == $rootScope.user.id;
                            });
                        });
                    },
                    topics: function (topicService, $rootScope) {
                        return topicService.getTopics().then(function (response) {
                            return response.data.filter(function(elem) {
                                return !elem.courseId;
                            });
                        });
                    }
                }
            })
            .state('courseList', {
                url: '/teacher/courses',
                templateUrl: 'templates/courses/course-list.html',
                controller: 'CourseController as ctrl',
                resolve: {
                    courses: function (courseService, $rootScope) {
                        return courseService.getCourses().then(function (response) {
                            return response.data.filter(function(elem) {
                                return elem.teacherId == $rootScope.user.id;
                            });
                        });
                    },
                    topics: function (topicService, $rootScope) {
                        return topicService.getTopics().then(function (response) {
                            return response.data.filter(function(elem) {
                                return !elem.courseId;
                            });
                        });
                    }
                }
            })
            .state('courseCreate', {
                url: '/teacher/courses/create',
                templateUrl: 'templates/courses/course-add.html',
                controller: 'CourseController as ctrl',
                resolve: {
                    courses: function (courseService, $rootScope) {
                        return courseService.getCourses().then(function (response) {
                            return response.data.filter(function(elem) {
                                return elem.teacherId == $rootScope.user.id;
                            });
                        });
                    },
                    topics: function (topicService, $rootScope) {
                        return topicService.getTopics().then(function (response) {
                            return response.data.filter(function(elem) {
                                return !elem.courseId;
                            });
                        });
                    }
                }
            })
            .state('courseDetails', {
                url: '/teacher/courses/:courseId',
                templateUrl: 'templates/courses/course-detail.html',
                controller: 'CourseDetailsController as ctrl',
                resolve: {
                    course: function (courseService, $stateParams) {
                        return courseService.getCourse($stateParams.courseId).then(function (response) {
                            return response.data;
                        });
                    }
                }
            })
            .state('courseUpdate', {
                url: '/teacher/courses/:courseId/update',
                templateUrl: 'templates/courses/course-edit.html',
                controller: 'CourseDetailsController as ctrl',
                resolve: {
                    course: function (courseService, $stateParams) {
                        return courseService.getCourse($stateParams.courseId).then(function (response) {
                            return response.data;
                        });
                    }
                }
            })
            .state('topicList', {
                url: '/topics',
                templateUrl: 'templates/topics/topic-list.html',
                controller: 'TopicController as ctrl',
                resolve: {

                }
            })
            .state('topicCreate', {
                url: '/teacher/courses/:courseId/topics/create',
                templateUrl: 'templates/topics/topic-add.html',
                controller: 'TopicController as ctrl',
                resolve: {
                    courses: function (courseService, $rootScope) {
                        return courseService.getCourses().then(function (response) {
                            return response.data.filter(function(elem) {
                                return elem.teacherId == $rootScope.user.id;
                            });
                        });
                    }
                }
            })
            .state('topicDetails', {
                url: '/teacher/courses/:courseId/topics/:topicId',
                templateUrl: 'templates/topics/topic-detail.html',
                controller: 'TopicDetailsController as ctrl',
                resolve: {
                    topic: function (topicService, $stateParams) {
                        return topicService.getTopic($stateParams.topicId).then(function (response) {
                            return response.data;
                        });
                    },
                    course: function (courseService, $stateParams) {
                        return courseService.getCourse($stateParams.courseId).then(function (response) {
                            return response.data;
                        });
                    },
                    courses: function (courseService) {
                        return courseService.getCourses().then(function (response) {
                            return response.data;
                        });
                    }
                }
            })
            .state('topicUpdate', {
                url: '/teacher/courses/:courseId/topics/:topicId/update',
                templateUrl: 'templates/topics/topic-edit.html',
                controller: 'TopicDetailsController as ctrl',
                resolve: {
                    topic: function (topicService, $stateParams) {
                        return topicService.getTopic($stateParams.topicId).then(function (response) {
                            return response.data;
                        });
                    },
                    course: function (courseService, $stateParams) {
                        return courseService.getCourse($stateParams.courseId).then(function (response) {
                            return response.data;
                        });
                    },
                    courses: function (courseService) {
                        return courseService.getCourses().then(function (response) {
                            return response.data;
                        });
                    }
                }
            })
            .state('topicStart', {
                //TODO
                url: '/teacher/courses/:courseId/topics/:topicId/start',
                templateUrl: 'templates/topics/topic-detail.html',
                controller: 'TopicDetailsController as ctrl',
                resolve: {
                    topic: function (topicService, $stateParams) {
                        return topicService.getTopic($stateParams.topicId).then(function (response) {
                            return response.data;
                        });
                    },
                    course: function (courseService, $stateParams) {
                        return courseService.getCourse($stateParams.courseId).then(function (response) {
                            return response.data;
                        });
                    },
                    courses: function (courseService) {
                        return courseService.getCourses().then(function (response) {
                            return response.data;
                        });
                    }
                }
            })
            .state('topicScreens', {
                url: '/screens',
                templateUrl: 'templates/screens/screen-list.html',
                controller: 'ScreensController',
                controllerAs: 'ctrl',
                parent: 'topicDetails'
            })
            .state('screenComputer', {
                url: '/screenId',
                templateUrl: 'templates/screens/screen-detail.html',
                controller: 'ScreensController',
                controllerAs: 'ctrl',
                parent: 'topicScreens'
            })

            .state('user', {
                url: '/user',
                templateUrl: 'templates/home-user.html',
                controller: 'CourseController as ctrl',
                resolve: {
                    courses: function (courseService, $rootScope) {
                        return courseService.getCourses().then(function (response) {
                            return response.data;
                        });
                    },
                    topics: function (topicService, $rootScope) {
                        return topicService.getTopics().then(function (response) {
                            return response.data.filter(function(elem) {
                                return !elem.courseId;
                            });
                        });
                    }
                }
            })
            .state('courseListUser', {
                url: '/user/courses',
                templateUrl: 'templates/courses/course-list-user.html',
                controller: 'CourseController as ctrl',
                resolve: {
                    courses: function (courseService, $rootScope) {
                        return courseService.getUserCourses($rootScope.user.id).then(function (response) {
                          
                            return response.data
                        });
                    },
                    topics: function (topicService, $rootScope) {
                        return topicService.getTopics().then(function (response) {
                            return response.data.filter(function(elem) {
                                return !elem.courseId;
                            });
                        });
                    }
                }
            })
            .state('courseDetailsUser', {
                url: '/user/courses/:courseId',
                templateUrl: 'templates/courses/course-enroll-user.html',
                controller: 'CourseDetailsController as ctrl',
                resolve: {
                    course: function (courseService, $stateParams) {
                        return courseService.getCourse($stateParams.courseId).then(function (response) {
                            return response.data;
                        });
                    }
                }
            })
          .state('topicUserView', {
              url: '/user/courses/:courseId/topics/:topicId/view',
              templateUrl: 'templates/topics/topic-detail.html',
              controller: 'TopicDetailsController as ctrl',
             resolve: {
                 topic: function (topicService, $stateParams) {
                     return topicService.getTopic($stateParams.topicId).then(function (response) {
                         return response.data;
                     });
                 },
                 course: function (courseService, $stateParams) {
                     return courseService.getCourse($stateParams.courseId).then(function(response) {
                         return response.data;
                     });
                 },
                 courses: function (courseService) {
                     return courseService.getCourses().then(function (response) {
                         return response.data;
                     });
                 }
                 //courses: function (courseService, $rootScope) {
                 //    return courseService.getCourses().then(function (response) {
                 //        return response.data.filter(function (elem) {
                 //            return elem.teacherId == $rootScope.user.id;
                 //        });
                 //    });
                 //}
             }
         });
    });

eLabApp.value('API_PATH', 'http://elab-pg.azurewebsites.net/api/');
