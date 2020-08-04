function RemoveBlock(block) {
    $(block).parent().remove();
}

function LoadFile(id) {
    $(id).click();
}

function readURL(input) {
    if (input.files) {
        $('#imageBlock').children().remove();
        for (var i = 0; i < input.files.length; i++) {
            if (input.files[i]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#imageBlock').append("<div style='margin: 5px;'><img class='img-thumbnail small-image img-small-loaded' src='#'/><label style='text-align: center;'></label><input type='button' class='btn-outline-secondary btn-close' onclick='RemoveBlock(this)' value='x' /></div>");
                    $('#imageBlock').children().last().children().first().attr('src', e.target.result);
                }
                reader.readAsDataURL(input.files[i]);
            }
        }
    }
}

$(document).on("change", "#inputFile", function () {
    readURL($('#inputFile').get()[0]);
});

function MoveUpRow(button) {
    var row = $(button).parent().parent();
    row.insertBefore(row.prev());
}

function MoveDownRow(button) {
    var row = $(button).parent().parent();
    row.insertAfter(row.next());
}

function createPage(url) {
    $("#content").load(url);
}

function loadIndexPage(url) {
    $("#content").load(url);
}

function loadEditPage(url) {
    $("#content").load(url);
}

function deleteItem(url, id) {
    $("#content").load(url);
    $('#id' + id).parent().remove();
}

function edit(id, textId, url) {
    $.ajax({
        url: url,
        type: 'POST',
        data: new FormData($('#form').get()[0]),
        processData: false,
        contentType: false
    });

    $('#el' + id).text($('#' + textId).val());
    $('#content').empty();
}
