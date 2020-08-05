// Remove parent block. Used to delete image
function RemoveBlock(block) {
    $(block).parent().remove();
}

// Activate input type file by button click
function LoadFile(id) {
    $(id).click();
}

// Drow selected images to block with id imageBlock
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

// Used to invoke method that display selected images
$(document).on("change", "#inputFile", function () {
    readURL($('#inputFile').get()[0]);
});

// Move block up. Used to reorder blocks
function MoveUpRow(button) {
    var row = $(button).parent().parent();
    row.insertBefore(row.prev());
}

// Move block down. Used to reorder blocks
function MoveDownRow(button) {
    var row = $(button).parent().parent();
    row.insertAfter(row.next());
}

// Load partial view and display in div with id content
// url - url to send request
function loadView(url) {
    $("#content").load(url);
}

// Send request to controller to perform delete and remove item from menu
// url - url to send request
// id - id of the recor to delete
function deleteItem(url, id) {
    $("#content").load(url);
    $('#elId' + id).remove();
}


// Send request to controller to perform edit
// id - id of edited record
// textId - id of text to display on menu after edditing
// url - url to send request to
// idOfElementsToReorder - if enable elements reordering, else null
function edit(id, textId, url, idOfElementsToReorder) {
    $.ajax({
        url: url,
        type: 'POST',
        data: new FormData($('#form').get()[0]),
        processData: false,
        contentType: false
    });

    if (idOfElementsToReorder != null) {
        var parentFrom = $('#' + idOfElementsToReorder).get()[0];
        if (parentFrom.children.length > 0) {
            Reorder(parentFrom, 'id' + id)
        }
    }

    $('#elName' + id).text($('#' + textId).val());
    $('#content').empty();
}

// Reorder elements in navigation menu
// parentFrom - parent of elements with new ordering
// parentToId - id of parent of elements in menu
function Reorder(parendFrom, parentToId) {
    for (var i = parendFrom.children.length - 1; i >= 0; i--) {
        id = parendFrom.children[i].children[0].value;
        $('#' + parentToId + ' > div[id=elId' + id + ']').insertAfter($('#' + parentToId + ' > a'));
    }
}

function CreateHall(textId) {
    $.ajax({
        url: '/Halls/Create',
        type: 'POST',
        data: new FormData($('#form').get()[0]),
        processData: false,
        contentType: false,
        success: function (id) {
            $('#halls').append("<div id='elId" + id + "'><div style='margin-top: 10px;' class='flex-row'><button class='btn btn-link collapsed' data-toggle='collapse' data-target='#id" + id + "' aria-expanded='false' aria-controls='id" + id + "' style='margin-bottom:3px;padding:2px;'> <i class='fa' aria-hidden='false'></i></button><a class='a-header' id='elName" + id + "' onclick=\"loadView('/Halls/Index/" + id + "')\">" + $('#' + textId).val() + "</a></div><div class='collapse' id='id" + id + "' style='margin-left: 50px'><a class='a-btn' onclick=\"loadView('/Stands/Create?hallId=" + id + "')\">Add stand</a></div></div>");
            $('#content').empty();
        }
    });
}

function CreateStand(hallId, textId) {
    $.ajax({
        url: '/Stands/Create?hallId=' + hallId,
        type: 'POST',
        data: new FormData($('#form').get()[0]),
        processData: false,
        contentType: false,
        success: function (id) {
            $('#id' + hallId).append("<div id='elId" + id + "'><div style='margin-top: 10px;' class='flex-row'><button class='btn btn-link collapsed' data-toggle='collapse' data-target='#id" + id + "' aria-expanded='false' aria-controls='id" + id + "' style='margin-bottom:3px;padding:2px;'> <i class='fa' aria-hidden='false'></i></button><a class='a-header' id='elName" + id + "' onclick=\"loadView('/Stands/Index/" + id + "?hallId=" + hallId + "')\">" + $('#' + textId).val() + "</a></div><div class='collapse' id='id" + id + "' style='margin-left: 50px'><a class='a-btn' onclick=\"loadView('/Exhibits/Create?standId=" + id + "&hallId=" + hallId + "')\">Add exhibit</a></div></div>");
            $('#content').empty();
        }
    });
}

function CreateExhibit(hallId, standId, textId) {
    $.ajax({
        url: '/Exhibits/Create?hallId=' + hallId + '&standId=' + standId,
        type: 'POST',
        data: new FormData($('#form').get()[0]),
        processData: false,
        contentType: false,
        success: function (id) {
            $('#id' + standId).append("<div style='margin-top: 10px;' class='flex-row' id='elId" + id + "'><a class='a-header' id='elName" + id + "' onclick=\"loadView('/Exhibits/Index?id=" + id + "&standId=" + standId + "&hallId=" + hallId + "')\">" + $('#' + textId).val() + "</a></div>");
            $('#content').empty();
        }
    });
}