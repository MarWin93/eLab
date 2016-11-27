angular.module('eLabApp').service('chatHelper', function ($rootScope) {

    myself = null;
    activeParticipants = {
            //3:{
            //    id: '3',
            //    base64Image: '',
            //    loop: 1
            //},
            //4:{
            //    id: '4',
            //    base64Image: '',
            //    loop: 1
            //}
    };

    self.activeParticipantsGet = function() {
        return activeParticipants;
    }

    self.registerClientMethods = function (chatHub, me) {

        myself = me;
        // Calls when user successfully logged in
        chatHub.client.onConnected = function (id, userName, allUsers, messages, userId) {

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
                AddUser(chatHub, allUsers[i].ConnectionId, allUsers[i].UserName, userId);
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
        chatHub.client.onNewUserConnected = function (id, name, userId) {
            console.log("Adding new user");
            AddUser(chatHub, id, name, userId);
        }

        // On User Disconnected
        chatHub.client.onUserDisconnected = function (id, userName) {
            console.log("Usuniecie Usera:" + id);
            $('#' + id).remove();
            delete activeParticipants[id];
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

    self.registerEvents = function(chatHub) {
        $("#txtMessage").keypress(function (e) {
            if (e.which == 13) {
                $('#btnSendMsg').click();
            }
        });

        $('#btnSendMsg').click(function () {
            console.log("clicked");
            var msg = $("#txtMessage").val();
            if (msg.length > 0) {
                console.log(myself.user.name + ":" + myself.user.id + ":" + msg + ":" + myself.topic.id)
                chatHub.server.sendMessageToAll(myself.user.name, msg, myself.topic.id, myself.user.id);
                $("#txtMessage").val('');
            }
        });
    }

    // Add User
    function AddUser(chatHub, id, name, userIdFromHub) {
        //activeParticipants.push({ id: id, login: name, base64Image: '' });
        activeParticipants[userIdFromHub] = { id: userIdFromHub, login: name, base64Image: '' };
        var userId = $('#hdId').val();
        var userName = myself.user.name;
        console.log("User name: " + userName + ", new name:" + name);
        var code = "";

        if (userName == name && $('.loginUser').length == 0) {
            code = $(
            '<div class="cloud-user loginUser"><div class="avatar"><img src="/app/assets/img/avatar_2.png"></div>' +
            '<h3><b>' + name + '</b></h3></div>');
        }
        else {
            $('#' + id).remove();
            code = $(
                 '<div class="cloud-user" id="' + id + '">' +
                 '<div class="avatar"><img src="/app/assets/img/avatar_4.png"></div>' +
                 '<h3><b>' + name + '</b></h3></div>');
            $(code).click(function () {
                var id = $(this).attr('id');
                if (userName != myself.user.name) {
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
        console.log(userName === myself.user.name);
        if (userName === myself.user.name) {
            avatarClass += (" main");
            cloudClass += (" main");
        }
        $('#conversation').append(
            '<div class="' + avatarClass + '"><img src="/app/assets/img/avatar_4.png"></div>' +
            '<div class="' + cloudClass + '">' +
            ' <h3><b>' + userName + '</b><i class="prefix mdi-action-alarm"></i><span class="message-date"> ' + datetime + ' </span>   <br> </h3>' +
            ' <span>' + message + ' </span>' +
            ' </div><div class="clear"></div>');
        var height = $('#conversation')[0].scrollHeight;
        $('#conversation').scrollTop(height);
    }

    return self;
});


