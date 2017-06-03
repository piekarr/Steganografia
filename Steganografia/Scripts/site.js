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
			initializeConversationBody();
			$this.addClass('active');
		});
	});
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
			if ($messagesList.children('.alert').length) {
				$messagesList.empty();
			}
			$messagesList.append(json);
			$messagesList.scrollTop($messagesList[0].scrollHeight);
		}
	})
	return false;
};
