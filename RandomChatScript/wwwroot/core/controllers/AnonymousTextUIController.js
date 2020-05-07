/// <reference path="templatescontroller.js" />

var AnonymousTextUIController = (function (templateController) {
    let chatArea = document.querySelector('#conversation');
    let sendButton = document.querySelector('#sendBtn');
    let messageTextbox = document.querySelector('#messageTextbox');



    // Chat Area Logic
    function clearChatAreaFromPreviousChat() {
        while (chatArea.firstChild && chatArea.removeChild(chatArea.firstChild));
    }
    function addToChatArea(type, content, date) {
        chatArea.insertAdjacentHTML('beforeend', templateController.MessagesTemplates[type](content, date));
    }
    function animateToEndChatArea() {
        chatArea.animate({ scrollTop: 999999999999999999999999 });
    }
    function getSendTextValue() {
        return messageTextbox.value;
    }
    function emptySendTextValue() {
        messageTextbox.value = "";
    }


   
   


    return {
        ClearChatArea: clearChatAreaFromPreviousChat,
        AddToChatArea: addToChatArea,
        EndOfChatArea: animateToEndChatArea,
        GetSendTextBoxValue: getSendTextValue,
        EmptySendTextBoxValue: emptySendTextValue
    }
})(TemplateController);
