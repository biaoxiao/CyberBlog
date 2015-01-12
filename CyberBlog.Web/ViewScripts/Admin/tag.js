var tagId;

var onActionEvent = function (e) {
	switch (e.data.action) {
		case 'update':
			updateTag(); break;
		case 'new':
			newTag(); break;
		case 'delete':
			delTag(); break
	}

}

$('#btnUpdate').click({ action: 'update' }, onActionEvent);
$('#btnDelete').click({ action: 'delete' }, onActionEvent);
$('#btnNew').click({ action: 'new' }, onActionEvent);

$('#admin-tabs li').removeClass('active');
$('#tag').addClass('active');

$('#tab-tag ul').on('click', 'li', function () {
	$('#tab-tag ul li').removeClass('list-group-item-success');
	$(this).addClass('list-group-item-success');
	tagId = $(this).attr('id');
	$('#input-tag').val($(this).text());
});


function newTag() {
	if ($('#input-tag').val().length == 0) {
		return false;
	}
	else {
		var isValid = true;
		var newTag = $('#input-tag').val().toLocaleLowerCase();
		$('#tab-tag li').each(function () {
			if ($(this).text().toLocaleLowerCase() == newTag) {
				isValid = false;
				alert('There is a duplicate tag.');
				return;
			}
		});

		if (isValid) {
			var url = urlNewTag;
			var post = { Tag: $('#input-tag').val() };
			$.ajax({
				beforeSend: showLoading(),
				url: url,
				contentType: "application/json",
				type: 'POST',
				dataType: 'text',
				data: JSON.stringify(post),
				success: function (id) {
					closeLoading();
					if (parseInt(id) > 0) {
						$('#tab-tag ul').append("<li class='list-group-item' id='" + id + "'>" + $('#input-tag').val() + "</li>");
						$('#input-tag').val('');
						alert('A new tag has been submitted successfully.');
					}
					else {
						alert('Sorry, the new tag cannot be submitted.');
					}
				},
				error: function () {
					closeLoading();
					alert("There was an error creating the new tag.");
				}
			});
		}
	}
	return false;
}

function delTag() {
	if (tagId == null) {
		alert('Please select a tag.');
		return false;
	}
	else {
		var url = urlDelTag;
		var post = { TagId: tagId };
		$.ajax({
			beforeSend: showLoading(),
			url: url,
			contentType: "application/json",
			type: 'DELETE',
			dataType: 'json',
			data: JSON.stringify(post),
			success: function (resp) {
				closeLoading();
				if (resp) {
					$("#tab-tag li[id='" + tagId + "']").remove();
					$('#input-tag').val('');
					tagId = null;
					alert('The tag has been deleted successfully.');
				}
				else {
					alert('Sorry, the tag cannot be deleted.');
				}
			},
			error: function () {
				closeLoading();
				alert("There was an error deleting the tag.");
			}
		});
	}
	return false;
}


function updateTag() {
	if ($('#input-tag').val().length == 0 || tagId == null) {
		return false;
	}
	else {
		var url = urlUpdateTag;
		var post = { Tag: $('#input-tag').val(), TagId: tagId };
		$.ajax({
			beforeSend: showLoading(),
			url: url,
			contentType: "application/json",
			type: 'PUT',
			dataType: 'json',
			data: JSON.stringify(post),
			success: function (resp) {
				closeLoading();
				if (resp) {
					$("#tab-tag li[id='" + tagId + "']").text($('#input-tag').val());
					$('#input-tag').val('');
					tagId = null;
					alert('The tag has been updated successfully.');
				}
				else {
					alert('Sorry, the tag cannot be updated.');
				}
			},
			error: function () {
				closeLoading();
				alert("There was an error updating the tag.");
			}
		});
	}
	return false;
}