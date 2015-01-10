
function initDataTable() {
	table=$('#posts-table').dataTable({
		"stateSave":true,
		"iDisplayLength": 10,
		"bLengthChange": false,
		"bSort": true,
		"bServerSide": true,
		"bProcessing": true,
		"sAjaxSource": urlPosts,
		"aoColumns": [
			{ "sName": "Id", "sTitle": "Id", "bVisible": false },
			{ "sName": "Title", "sTitle": "Title", "bSortable": true },
			{ "sName": "Name", "sTitle": "Category", "bSortable": true },
			{ "sName": "Tags", "sTitle": "Tag(s)", "bSortable": false },
			{ "sName": "Author", "sTitle": "Author", "bSortable": false },
			{ "sName": "PostedDate", "sTitle": "Posted Date", "bSortable": true},
			{ "sName": "Published", "sTitle": "Status","bSortable":false }
		]
	});
}

function deleteRow() {
	var anSelected = fnGetSelected(oTable);
	if (anSelected.length !== 0) {
		oTable.fnDeleteRow(anSelected[0]);
	}
}

/* Get the rows which are currently selected */
function fnGetSelected(oTableLocal) {
	return oTableLocal.$('tr.row_selected');
}

$('.dataTables_filter').each(function () {
	var datatable = $(this);
	var search_input = datatable.closest('.dataTables_wrapper').find('div[id$=_filter] input');
	search_input.attr('placeholder', 'Search');
	search_input.addClass('form-control input-sm');
	var length_sel = datatable.closest('.dataTables_wrapper').find('div[id$=_length] select');
	length_sel.addClass('form-control input-sm');
});

/* Add a click handler to the rows - this could be used as a callback */
$("#posts-table").on("click", "tbody tr", function (e) {

	//alert(oSelectedRow);
	if ($(this).hasClass('row_selected')) {
		$(this).removeClass('row_selected');
		oSelectedRow = null;
	}
	else {
		oTable.$('tr.row_selected').removeClass('row_selected');
		$(this).addClass('row_selected');
		oSelectedRow = oTable.fnGetData(this);
	}
});

function Cancel() {
	$('#posts-container').show();
	$('#btn-container').show()
	$('#post-edit-container').hide();
}

function delPost() {
	if (oSelectedRow == null) {
		alert("Please select a post to delete.");
	}
	else if (confirm("Are you sure you want to delete the selected post?")) {
		var _url = urlDelPost + "/" + oSelectedRow[0];
		$.ajax({
			type: "DELETE",
			url: _url,
			success: function (data) {
				if (data == true) {
					oTable.fnClearTable(0);
					oTable.fnDraw();
					//deleteRow();
					oSelectedRow = null;
				}
				else {
					alert("Sorry, the post cannot be deleted.");
				}
			},
			error: function () {
				alert("There was an error deleting the post.");
			}
		});
	}
}

function saveChanges() {
	var tags = [];
	if ($.trim($('#postTag').val()).length > 0) {
		var _tags = $.trim($('#postTag').val()).split(',');
		for (i = 0; i <= _tags.length - 1; i++) {
			var item = { "TagId": 0, "Tag": _tags[i] };
			tags.push(item);
		}
	}
	var post = { Title: $('#postTitle').val(),Published:$('#postStatus').prop("checked") ,Tags: tags, UrlSlug: $('#postSlug').val(), Author: $('#postAuthor').val(),ShortDesc:$('#postShortDesc').code(), FullDesc: $('#postContent').code(), PostId: $('#postId').val(),CategoryId:$('#postCategory').val() };
	$.ajax({
		beforeSend:showLoading(),
		url: urlSave,
		contentType: "application/json;charset=utf-8",
		type: 'POST',
		dataType: 'json',
		data: JSON.stringify(post),
		success: function (msg) {
			closeLoading();
			if (msg == true) {
				alert('Your post has been saved successfully.');
				$('#postTitle').val('');
				$('#postTag').val('');
				$('#postAuthor').val('');
				$('#postContent').code('');
				//oTable.fnClearTable(0);
				//oTable.fnDraw();
				oTable.api().ajax.reload(null,false);
				oSelectedRow = null;
				$('#posts-container').show();
				$('#btn-container').show()
				$('#post-edit-container').hide();
			}
			else {
				alert('Sorry, your post cannot be saved.');
			}
		},
		error: function () {
			closeLoading();
			alert("There was an error saving the post.");
		}
	});
	return false;
}

function editPost() {
	if (oSelectedRow == null) {
		alert("Please select a post to edit.");
	}
	else {
		var url = urlEdit + "?postId=" + oSelectedRow[0];
		$.getJSON(url, null, function (data) {
			if (data == 'fail') {
				alert('Sorry');
			}
			else {
				var tags = '';
				var i = 1;
				$.each(data.Tags, function (idx) {
					if (i == data.Tags.length) {
						tags += data.Tags[idx].Tag
					}
					else {
						tags += data.Tags[idx].Tag + ","
					}
					i++;
				});
				$('#postTitle').val(data.Title);
				$('#postStatus').prop('checked',data.Published);
				$('#postShortDesc').code(data.ShortDesc);
				$('#postTag').tagsinput('removeAll');					
				$('#postTag').tagsinput('add', tags);
				$('#postCategory').val(data.CategoryId);
				$('#postAuthor').val(data.Author);
				$('#postSlug').val(data.UrlSlug);
				$('#postId').val(data.PostId);
				$('#postContent').code(data.FullDesc);
				$('#posts-container').hide();
				$('#btn-container').hide()
				$('#post-edit-container').show();
			}
		});

	}
}

function urlSlug(title) {
	$('#postSlug').val(slug(title));
}

function slug(input) {
	return input
		.replace(/#+/g, 'sharp') //replace # with sharp
		.replace(/^\s\s*/, '') // Trim start
		.replace(/\s\s*$/, '') // Trim end
		.toLowerCase() // Camel case is bad
		.replace(/[^a-z0-9_\-~!\+\s]+/g, '') // Exchange invalid chars
		.replace(/[\s]+/g, '-'); // Swap whitespace for single hyphen
}