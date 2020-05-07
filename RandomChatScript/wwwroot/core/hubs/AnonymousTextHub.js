/// <reference path="../controllers/anonymoustextuicontroller.js" />

let connectedStateFlag = false;


const connection = new signalR.HubConnectionBuilder()
    .withUrl("/anonymousHub")
    .withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Trace)
    .build();

connection.start().then(function () {
    console.log("connected");

});



document.querySelector('#sendBtn').addEventListener('click', e => {
    let msg = AnonymousTextUIController.GetSendTextBoxValue();
    if (msg == null || msg.trim() === "" || connectedStateFlag == false) {
        return;
    }
    AnonymousTextUIController.AddToChatArea("myTextMessage", msg);
    AnonymousTextUIController.EndOfChatArea();
    connection.invoke("Routing", msg).catch(err => console.error(err.toString()));
    AnonymousTextUIController.EmptySendTextBoxValue();
});

messageTextbox.addEventListener('keypress', e => {
    if (e.which == 13 || e.keyCode == 13) {
        let msg = AnonymousTextUIController.GetSendTextBoxValue();
    
        if (msg == null || msg.trim() === "" || connectedStateFlag == false) {
            return;
        }
        AnonymousTextUIController.AddToChatArea("myTextMessage", msg);
        AnonymousTextUIController.EndOfChatArea();
        connection.invoke("Routing", msg).catch(err => console.error(err.toString()));
        AnonymousTextUIController.EmptySendTextBoxValue();
    }
});

connection.on("strangerMessage", (model) => {
    if (model.type == 'T')
        window.AnonymousTextUIController.AddToChatArea("friendTextMessage", model.message, model.Date);
    if (model.type == 'I')
        window.AnonymousTextUIController.AddToChatArea("friendImageMessage", model.message, model.Date);

    AnonymousTextUIController.EndOfChatArea();
});
connection.on("serverMessage", (messageKey, date) => {
    window.AnonymousTextUIController.AddToChatArea("serverTextMessage", Lang[messageKey]);
});
connection.on("settingUpSetting", () => {
    connectedStateFlag = true;
});
connection.on("settingDownSetting", () => {
    connectedStateFlag = false;
});

$('#my-file-selector').change(function () {
    if (connectedStateFlag == false)
        return;
    var formData = new FormData();
    var totalFiles = document.getElementById("my-file-selector").files.length;
    var file = document.getElementById("my-file-selector").files[0];

    formData.append("file", file);
    $.ajax({
        type: 'post',
        url: '/Upload',
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (response) {

            window.AnonymousTextUIController.AddToChatArea("myImageMessage", response);
            connection.invoke("RoutingImages", response).catch(err => console.error(err.toString()));
        },
        error: function (error) {
            console.log(error);
        }
    });
});
