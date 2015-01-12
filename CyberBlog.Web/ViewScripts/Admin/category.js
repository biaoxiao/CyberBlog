$('#admin-tabs li').removeClass('active');
$('#category').addClass('active');

var onActionEvent = function (e) {
	switch (e.data.action) {
		case 'update':
			updateCategory(); break;
		case 'new':
			newCategory(); break;
		case 'delete':
			delCategory(); break
	}

}

$('#btnUpdate').click({ action: 'update' }, onActionEvent);
$('#btnDelete').click({ action: 'delete' }, onActionEvent);
$('#btnNew').click({ action: 'new' }, onActionEvent);

var categoryId;

$('#tab-category ul').on('click', 'li', function () {
	$('#tab-category ul li').removeClass('list-group-item-success');
	$(this).addClass('list-group-item-success');
	categoryId = $(this).attr('id');
	$('#input-category').val($(this).text());
});

function delCategory() {
	if (categoryId == null) {
		alert('Please select a category.');
		return false;
	}
	else {
		var url = urlDelCategory;
		var post = { CategoryId: categoryId };
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
					$("#tab-category li[id='" + categoryId + "']").remove();
					$('#input-category').val('');
					categoryId = null;
					alert('The category has been deleted successfully.');
				}
				else {
					alert('Sorry, the category cannot be deleted.');
				}
			},
			error: function () {
				closeLoading();
				alert("There was an error deleting the category.");
			}
		});
	}
	return false;
}

function updateCategory() {
	if ($('#input-category').val().length == 0 || categoryId == null) {
		return false;
	}
	else {
		var url = urlUpdateCategory;
		var post = { Category: $('#input-category').val(), CategoryId: categoryId };
		$.ajax({
			beforeSend: showLoading(),
			url: url,
			contentType: "application/json",
			type: 'POST',
			dataType: 'json',
			data: JSON.stringify(post),
			success: function (resp) {
				closeLoading();
				if (resp) {
					$("#tab-category li[id='" + categoryId + "']").text($('#input-category').val());
					$('#input-category').val('');
					categoryId = null;
					alert('The category has been updated successfully.');
				}
				else {
					alert('Sorry, the category cannot be updated.');
				}
			},
			error: function () {
				closeLoading();
				alert("There was an error updating the category.");
			}
		});
	}
	return false;
}

function newCategory() {
	if ($('#input-category').val().length == 0) {
		return false;
	}
	else {
		var isValid = true;
		var newCategory = $('#input-category').val().toLocaleLowerCase();
		$('#tab-category li').each(function () {
			if ($(this).text().toLocaleLowerCase() == newCategory) {
				isValid = false;
				alert('There is a duplicate category.');
				return;
			}
		});

		if (isValid) {
			var url = urlNewCategory;
			var post = { Category: $('#input-category').val() };
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
						$('#tab-category ul').append("<li class='list-group-item' id='" + id + "'>" + $('#input-category').val() + "</li>");
						$('#input-category').val('');
						alert('The new category has been submitted successfully.');
					}
					else {
						alert('Sorry, the category cannot be submitted.');
					}
				},
				error: function () {
					closeLoading();
					alert("There was an error creating the new category.");
				}
			});
		}
	}
	return false;
}