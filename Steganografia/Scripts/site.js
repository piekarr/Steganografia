var initializeConversations = function () {
    var $buttons = $('.conversationsBody .conversations button');
    initializeConversationButtons($buttons);
    if ($buttons.length) {
        $buttons.first().trigger('click');
    }
};

var initializeConversationButtons = function ($buttons) {
    $buttons.on('click', function () {
        $buttons.removeClass('active');
        var $this = $(this);
        $.get("home/messages", { id: $this.data('conversationId') }, function (data) {
            $(".conversationsBody .messages").html(data);
            $this.addClass('active');
        });
    });
};