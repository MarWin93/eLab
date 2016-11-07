angular.module('eLabApp').controller('ElabController', function ($scope, $rootScope, $state, elabService,
                                                                 $mdSidenav, loadingOverlayService) {
    var deregisterStateChangeStartHandler = $rootScope.$on('$stateChangeStart', function () {
        loadingOverlayService.start();
    });

    var deregisterStateChangeEndHandler = $rootScope.$on('$stateChangeSuccess', function () {
        loadingOverlayService.stop();
    });

    $rootScope.$on('$destroy', function () {
        deregisterStateChangeStartHandler();
        deregisterStateChangeEndHandler();
    });
    
    var vm = this;
    vm.chat = true;
    vm.selected = null;
    vm.chat = sessionStorage.getItem('chatWindow') != 'false';

    $rootScope.user = {
        name: sessionStorage.getItem('userName'),
        id: sessionStorage.getItem('userId'),
        group: sessionStorage.getItem('userGroup')
    };

    vm.login_username = null;
    vm.login_password = null;
    vm.error = null;
    
    vm.toggleSidenav = function () {
        $mdSidenav('left').toggle();
    };

    vm.toggleChatWindow = function() {
        vm.chat = !vm.chat;
        sessionStorage.getItem('chatWindow') == 'false'
            ? sessionStorage.setItem('chatWindow', 'true')
            : sessionStorage.setItem('chatWindow', 'false');
    };

    vm.isTeacher = function () {
        return $rootScope.user.group == 'Prowadzacy';
    };

    vm.login = function (){
        if (!vm.login_password){
            vm.error = 'Hasło jest wymagane.';
            return
        }
        var userLogin = {
            grant_type: 'password',
            username: vm.login_username,
            password: vm.login_password
        };
        elabService.login(userLogin).then(function (response) {
            if (!response.data.username){
                vm.error = 'Niepoprawny login lub hasło.';
                return
            }
            console.log(response.data);
            sessionStorage.setItem('userName', response.data.username);
            sessionStorage.setItem('userId', response.data.id);
            sessionStorage.setItem('userGroup', response.data.roleName);
            sessionStorage.setItem('accessToken', response.data.access_token);
            sessionStorage.setItem('refreshToken', response.data.refresh_token);

            $rootScope.user = {
                name: sessionStorage.getItem('userName'),
                id: sessionStorage.getItem('userId'),
                group: sessionStorage.getItem('userGroup')
            };
            if ($rootScope.user.group == 'Prowadzacy') {
                $state.go('teacher');
            } else {
                $state.go('user');
            }
        }).catch(function (err) {
            console.log(err);
        });
    };
    vm.logout = function (){
        $rootScope.user = null;
        sessionStorage.removeItem('userName');
        sessionStorage.removeItem('accessToken');
        window.location = "#";
    }
});
