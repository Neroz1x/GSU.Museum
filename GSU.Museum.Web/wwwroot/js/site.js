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

$("#inputFile").change(function () {
    readURL(this);
});

function MoveUpRow(button) {
    var row = $(button).parent().parent();
    row.insertBefore(row.prev());
}

function MoveDownRow(button) {
    var row = $(button).parent().parent();
    row.insertAfter(row.next());
}

function MoveLeftRow(button) {
    var row = $(button).parent();
    row.insertBefore(row.prev());
}

function MoveRightRow(button) {
    var row = $(button).parent();
    row.insertAfter(row.next());
}