angular.module('eLabApp').service('chatHelper', function () {
    // Self Object
    var chat = this;
    $scope.channel = 'messages-channel';

    chat.current_user = "Lolek";
    // Sent Indicator
    chat.status = "";
    chat.messages = [
          {
              "date": "2017-04-23T22:00:00.000Z",
              "content": "lalalalallalalal",
              "sender": "Marian"
          },
          {
              "date": "2016-04-23T00:00:00.000Z",
              "content": "hej",
              "sender": "Lolek"
          },
          {
              "date": "2016-04-23T00:00:00.000Z",
              "content": "fdsffdfsdd",
              "sender": "Jurek"
          },
          {
              "date": "2016-04-23T00:00:00.000Z",
              "content": "hej",
              "sender": "Lolek"
          },
          {
              "date": "2016-04-23T00:00:00.000Z",
              "content": "hej",
              "sender": "Lolek"
          },
          {
              "date": "2016-04-23T00:00:00.000Z",
              "content": "fdsffdfsdd",
              "sender": "Jurek"
          },
          {
              "date": "2016-04-23T00:00:00.000Z",
              "content": "hej",
              "sender": "Lolek"
          }
    ];

    chat.send = function () {
        console.log(chat.textbox);
        chat.status = "sending";
        chat.textbox = "";
        setTimeout(function () { chat.status = "" }, 1200);
    };

});



