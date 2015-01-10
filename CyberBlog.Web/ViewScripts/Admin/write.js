
function urlSlug(title) {
	$('#postSlug').val(slug(title));
}

function slug(input) {
	return input
		.replace(/#+/g,'sharp') //replace # with sharp
		.replace(/^\s\s*/, '') // Trim start
		.replace(/\s\s*$/, '') // Trim end
		.toLowerCase() // Camel case is bad
		.replace(/[^a-z0-9_\-~!\+\s]+/g, '') // Exchange invalid chars
		.replace(/[\s]+/g, '-'); // Swap whitespace for single hyphen
}

function addPost() {
	var tags = [];
	if ($.trim($('#postTag').val()).length > 0) {
		var _tags = $.trim($('#postTag').val()).split(',');
		for (i = 0; i <= _tags.length - 1; i++) {
			var item = { "TagId": 0, "Tag": _tags[i] };
			tags.push(item);
		}
	}
	var post = { Title: $('#postTitle').val(), Published: $('#postStatus').prop("checked"), Tags: tags, UrlSlug: $('#postSlug').val(), Author: $('#postAuthor').val(), ShortDesc: $('#postShortDesc').code(), FullDesc: $('#postContent').code(), CategoryId: $('#postCategory').val() };
	$.ajax({
		beforeSend: showLoading(),
		url: urlAddPost,
		contentType: "application/json;charset=utf-8",
		type: 'POST',
		dataType: 'json',
		data: JSON.stringify(post),
		success: function (msg) {
			closeLoading();
			if (msg == true) {
				alert('Your post has been submitted.');
				$('#postTag').tagsinput('removeAll');
				$('#postTag').val('');
				$('#postShortDesc').code('');
				$('#postContent').code('');
				$('#postTitle').val('');
				$('#postSlug').val('');
			}
			else {
				alert('Sorry, your post cannot be submitted.');
			}
		},
		error: function () {
			closeLoading();
			alert("There was an error creating the new post.");
		}
	});
	return false;
}

