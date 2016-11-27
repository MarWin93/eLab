angular.module('eLabApp').directive('drawing', ['drawerHelper', function(drawerHelper) {
    return {
        restrict: "A",
        link: function ($scope, element) {
            var vm = this;
            
            vm.signalRSetup = function (topic, user) {
                $scope.topicsHub = null; // holds the reference to hub
                $.connection.hub.url = 'http://localhost:8089/signalr';
                $.connection.hub.jsonp = true;
                $.connection.hub.logging = true; //debuging purpose

                $scope.topicsHub = $.connection.topicsHub; // initializes hub

                registerClientMethods($scope.topicsHub);

                //client side function declarations
                $scope.topicsHub.client.broadcastMessage = function (message) {
                    drawerHelper.fromSignalR(angular.fromJson(message));
                };

                // starts hub
                $.connection.hub.start({ jsonp: true }).done(function () {
                    //server (hub) side function declarations
                    $scope.topicsHub.server.joinGroup(topic.id, user.id, user.name);
                    registerEvents($scope.topicsHub);
                    console.log("Topic id:" + topic.id);
                    $scope.newMessage = function (parameters) { //- funkcja podpieta pod przycisk dodaj plik w widoku warsztatu
                        // sends a new message to the server
                        parameters = angular.toJson(parameters);
                        $scope.topicsHub.server.lock(parameters, topic.id);
                    };
                });
            };

            //PASS topic here, god knows how?
            vm.topic = {
              id: 1
            };
            vm.user = {
                id: $scope.user.id,
                name: $scope.user.name
            };

            console.log("Root from drawer:" + $scope.user.id);

            vm.signalRSetup(vm.topic, vm.user);
            
            var ctx = element[0].getContext('2d');

            // variable that decides if something should be drawn on mousemove
            var drawing = false;
            // the last coordinates before the current move
            var currentX;
            var currentY;

            $scope.$watch(
                function () {
                    return [element[0].offsetWidth, element[0].offsetHeight].join('x');
                },
                function (value) {
                    element[0].height = element[0].offsetHeight;
                    element[0].width = element[0].offsetWidth;
                }
            );

            element.bind('mousedown', function (event) {
                currentX = event.offsetX;
                currentY = event.offsetY;

                $scope.newMessage({
                    operation: 'beginPath',
                    x: event.offsetX / element[0].width,
                    y: event.offsetY / element[0].height
                });

                // begins new line
                drawing = true;
            });

            element.bind('mousemove', function (event) {
                //console.log(event.offsetX);
                if (drawing && drawerHelper.getTool() != 'highlighter') {
                    //drawerHelper.drawLine(currentX, currentY, event.offsetX, event.offsetY);
                    draw();
                }
            });

            element.bind('mouseup', function (event) {
                if (drawing && drawerHelper.getTool() == 'highlighter')
                    highlight();

                stopDrawing();
            });

            element.bind('mouseout', function (event) {
                stopDrawing();
            });

            function draw() {
                // get current mouse position
                currentX = event.offsetX;
                currentY = event.offsetY;

                $scope.newMessage({
                    operation: 'draw',
                    tool: drawerHelper.getTool(),
                    lineWidth: drawerHelper.getLineWidth(),
                    color: drawerHelper.getColor(),
                    x: currentX / element[0].width,
                    y: currentY / element[0].height
                });
            }

            function highlight() {
                $scope.newMessage({
                    operation: 'highlight',
                    tool: drawerHelper.getTool(),
                    lineWidth: drawerHelper.getLineWidth(),
                    color: drawerHelper.getColor(),
                    x: currentX / element[0].width,
                    y: currentY / element[0].height,
                    x2: event.offsetX / element[0].width,
                    y2: event.offsetY / element[0].height
                });

                // get current mouse position
                currentX = event.offsetX;
                currentY = event.offsetY;
            }

            function stopDrawing() {
                if (drawerHelper.getTool() != 'highlighter')
                    $scope.newMessage({
                        operation: 'endPath'
                    });
                drawing = false;
            }

            function registerClientMethods(chatHub) {
                // Calls when user successfully logged in
                chatHub.client.onConnected = function (id, userName, allUsers, messages) {

                    console.log("On Connected");

                    $('#hdId').val(id);
                    $('#hdUserName').val(userName);
                    $('#spanUser').html(userName);

                    //clear Users
                    var users = document.getElementById("users-list");
                    while (users.firstChild) {
                        users.removeChild(users.firstChild);
                    }

                    // Add All Users
                    for (i = 0; i < allUsers.length; i++) {
                        console.log("Adding all users");
                        AddUser(chatHub, allUsers[i].ConnectionId, allUsers[i].UserName);
                    }

                    //clear Existing Messages
                    var messagesExisting = document.getElementById("conversation");
                    while (messagesExisting.firstChild) {
                        messagesExisting.removeChild(messagesExisting.firstChild);
                    }

                    // Add Existing Messages
                    for (i = 0; i < messages.length; i++) {
                        var datetime = new Date(Date.parse(messages[i].SendDateTime));
                        var offset = new Date().getTimezoneOffset();
                        datetime.setMinutes(datetime.getMinutes() + offset);
                        AddMessage(messages[i].UserName, messages[i].Message, datetime.toLocaleTimeString());
                    }
                }

                // On New User Connected
                chatHub.client.onNewUserConnected = function (id, name) {
                    console.log("Adding new user");
                    AddUser(chatHub, id, name);
                }

                // On User Disconnected
                chatHub.client.onUserDisconnected = function (id, userName) {
                    console.log("Usuniecie Usera:" + id);
                    $('#' + id).remove();

                  //  var ctrId = 'private_' + id;
                  //  $('#' + ctrId).remove();

                    var disc = $('<div class="disconnect">"' + userName + '" logged off.</div>');

                    $(disc).hide();
                    $('#users-list').prepend(disc);
                    $(disc).fadeIn(200).delay(2000).fadeOut(200);
                }

                // On User Disconnected Existing
                chatHub.client.onUserDisconnectedExisting = function (id, userName) {
                    console.log("Usuniecie Usera Existing:" + id);
                    $('#' + id).remove();
                 //   var ctrId = 'private_' + id;
                  //  $('#' + ctrId).remove();
                }

                chatHub.client.messageReceived = function (userName, message, datetime) {
                    AddMessage(userName, message, datetime);
                }

                chatHub.client.sendPrivateMessage = function (windowId, fromUserName, message, userEmail, email, status, fromUserId) {
                    var ctrId = 'private_' + windowId;
                    if (status == 'Click') {
                        if ($('#' + ctrId).length == 0) {
                            createPrivateChatWindow(chatHub, windowId, ctrId, fromUserName, userEmail, email);
                            chatHub.server.getPrivateMessage(userEmail, email, loadMesgCount).done(function (msg) {
                                for (i = 0; i < msg.length; i++) {
                                    $('#' + ctrId).find('#divMessage').append('<div class="message"><span class="userName">' + msg[i].userName + '</span>: ' + msg[i].message + '</div>');
                                    // set scrollbar
                                    scrollTop(ctrId);
                                }
                            });
                        }
                        else {
                            $('#' + ctrId).find('#divMessage').append('<div class="message"><span class="userName">' + fromUserName + '</span>: ' + message + '</div>');
                            // set scrollbar
                            scrollTop(ctrId);
                        }
                    }

                    if (status == 'Type') {
                        if (fromUserId == windowId)
                            $('#' + ctrId).find('#msgTypeingName').text('typing...');
                    }
                    else { $('#' + ctrId).find('#msgTypeingName').text(''); }
                }
            }

            function registerEvents(chatHub) {
                $("#txtMessage").keypress(function (e) {
                    if (e.which == 13) {
                        $('#btnSendMsg').click();
                    }
                });

                $('#btnSendMsg').click(function () {
                    console.log("clicked");
                    var msg = $("#txtMessage").val();
                    if (msg.length > 0) {
                        console.log(vm.user.name + ":" + vm.user.id + ":" + msg + ":" + vm.topic.id)
                        chatHub.server.sendMessageToAll(vm.user.name, msg, vm.topic.id, vm.user.id);
                        $("#txtMessage").val('');
                    }
                });
            }

            // Add User
            function AddUser(chatHub, id, name) {
                var userId = $('#hdId').val();
                var userName = vm.user.name;
                console.log("User name: " + userName + ", new name:" +name);
                var code = "";

                if (userName == name && $('.loginUser').length == 0) {
                    code = $(
                    '<div class="cloud-user loginUser"><div class="avatar"><img src="/app/assets/img/avatar_2.png"></div>' +
                    '<h3><b>' + name + '</b></h3></div>');
                }
                else {
                    $('#' + id).remove();
                    code = $(
                         '<div class="cloud-user" id="' + id + '">'+ 
                         '<div class="avatar"><img src="/app/assets/img/avatar_4.png"></div>' +
                         '<h3><b>' + name + '</b></h3></div>');
                    $(code).click(function () {
                        var id = $(this).attr('id');
                        if (userName != vm.user.name) {
                            OpenPrivateChatWindow(chatHub, id, name)
                        }
                    });
                }
                $("#users-list").append(code);
            }

            // Add Message
            function AddMessage(userName, message, datetime) {
                var avatarClass = "avatar";
                var cloudClass = "cloud"
                console.log(userName === vm.user.name);
                if (userName === vm.user.name) {
                    avatarClass += (" main");
                    cloudClass +=(" main");
                }
                $('#conversation').append(
                    '<div class="' + avatarClass + '"><img src="/app/assets/img/avatar_4.png"></div>' +
                    '<div class="' + cloudClass + '">' +
                    ' <h3><b>' + userName + '</b><i class="prefix mdi-action-alarm"></i><span class="message-date"> ' +  datetime + ' </span>   <br> </h3>' +
                    ' <span>' + message +' </span>' +
                    ' </div><div class="clear"></div>');
                var height = $('#conversation')[0].scrollHeight;
                $('#conversation').scrollTop(height);
            }
        }
    };
}]);
    

