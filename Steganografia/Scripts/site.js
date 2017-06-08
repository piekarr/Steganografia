var initializeConversations = function () {
	var $buttons = $('.conversationsBody .conversations button');
	initializeConversationButtons($buttons);
	if ($buttons.length) {
		$buttons.first().trigger('click');
	}
};
var activeInterval = null;
var initializeConversationButtons = function ($buttons) {
	$buttons.on('click', function () {
		clearInterval(activeInterval);
		$buttons.removeClass('active');
		var $this = $(this);
		$.get("home/messages", { id: $this.data('conversationId') }, function (data) {
			$(".conversationsBody .messages").html(data);
			activeInterval  = setInterval(refreshMessages($this.data('conversationId')), 500);
			initializeConversationBody();
			$this.addClass('active');
		});
	});
};

var refreshMessages = function (conversationId) {
	return function () {
		var $lastchildren = $(".conversationsBody .messages .message").last();
		if ($lastchildren.length) {
			$.get("home/newMessages", { id: conversationId, lastMessageId: $lastchildren.data('messageId') }, function (data) {
				var $messagesList = $('.messagesList');
				$messagesList.append(data);
				$messagesList.scrollTop($messagesList[0].scrollHeight);
			});
		}
	};
};

var initializeConversationBody = function () {
	var $messagesList = $('.messagesList');
	$messagesList.scrollTop($messagesList[0].scrollHeight);
	$(".conversationsBody .messages form").submit(apendMessage);
};

var apendMessage = function (e) {
	$.ajax({
		type: 'POST',
		url: $(this).attr('action'),
		data: $(this).serialize(),
		success: function (json) {
			var $messagesList = $('.messagesList');
			$('.newMessage textarea').val('');
			if ($messagesList.children('.alert').length) {
				$messagesList.empty();
			}
			$messagesList.append(json);
			$messagesList.scrollTop($messagesList[0].scrollHeight);
		}
	})
	return false;
};
