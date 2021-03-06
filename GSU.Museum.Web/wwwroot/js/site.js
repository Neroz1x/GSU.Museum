﻿// Remove parent block. Used to delete image
function RemoveBlock(block) {
    $(block).parent().remove();
    ValidateImage();
}

// Sidebar resize calculation
document.addEventListener('DOMContentLoaded', function () {

    // Query the element
    const resizer = document.getElementById('resizer');
    const sidebar = document.getElementsByClassName('sidebar')[0];
    const content = document.getElementById('content');

    if (resizer != null) {
        // The current position of mouse
        let x = 0;
        let sidebarWidth = 0;

        // Handle the mousedown event that's triggered when user drags the resizer
        const mouseDownHandler = function (e) {

            // Get the current mouse position
            x = e.clientX;
            sidebarWidth = sidebar.getBoundingClientRect().width;

            // Attach the listeners to `document`
            document.addEventListener('mousemove', mouseMoveHandler);
            document.addEventListener('mouseup', mouseUpHandler);
        };

        const mouseMoveHandler = function (e) {

            // Set maximum and minimum width of sidebar in percent
            const minWidth = 15;
            const maxWidth = 50;

            // Mouse movement distance
            const dx = e.clientX - x;

            newSidebarWidth = (sidebarWidth + dx) * 100 / document.getElementsByClassName('container-page')[0].getBoundingClientRect().width;

            if (newSidebarWidth > maxWidth) {
                newSidebarWidth = maxWidth;
            }

            if (newSidebarWidth < minWidth) {
                newSidebarWidth = minWidth;
            }

            sidebar.style.width = `${newSidebarWidth}%`;
            content.style.marginLeft = `${newSidebarWidth}%`;

            html = document.getElementsByTagName('html')[0];

            // Set resize cursor and disable selection
            html.style.cursor = 'ew-resize';
            html.style.userSelect = 'none';
            html.style.pointerEvents = 'none';
        };

        const mouseUpHandler = function () {

            // Set cookie to save current sidebar width
            var date = new Date();
            date.setTime(date.getTime() + (365 * 24 * 60 * 60 * 1000));
            expires = "; expires=" + date.toGMTString();
            document.cookie = encodeURIComponent("MenuWidth") + "=" + encodeURIComponent(newSidebarWidth + "%") + expires + "; path=/";

            // Set normal cursor and enable selection
            html = document.getElementsByTagName('html')[0];
            html.style.removeProperty('cursor');
            html.style.removeProperty('user-select');
            html.style.removeProperty('pointer-events');

            // Remove the handlers of `mousemove` and `mouseup`
            document.removeEventListener('mousemove', mouseMoveHandler);
            document.removeEventListener('mouseup', mouseUpHandler);
        };

        // Attach the handler
        resizer.addEventListener('mousedown', mouseDownHandler);
    }
});

// Validate image input
function ValidateImage() {
    if ($('#imageBlock2').get()[0]) {
        if ($('#imageBlock').get()[0].children.length == 0 && $('#imageBlock2').get()[0].children.length == 0) {
            $('#photoSpan').removeClass("field-validation-valid");
            $('#photoSpan').addClass("field-validation-error");
            $('#photoSpan').text("Выберите фото");
            return false;
        }
        else {
            $('#photoSpan').addClass("field-validation-valid");
            $('#photoSpan').removeClass("field-validation-error");
            $('#photoSpan').text("");
        }
    }
    else {
        if ($('#imageBlock').get()[0].children.length == 0) {
            $('#photoSpan').removeClass("field-validation-valid");
            $('#photoSpan').addClass("field-validation-error");
            $('#photoSpan').text("Выберите фото");
            $("#inputFile").val("");
            return false;
        }
        else {
            $('#photoSpan').addClass("field-validation-valid");
            $('#photoSpan').removeClass("field-validation-error");
            $('#photoSpan').text("");
        }
    }
    return true;
}

// Activate input type file by button click
function LoadFile(id) {
    $(id).click();
}

// Draw selected images to block with id imageBlock
function readURL(input) {
    if (input.files) {
        $('#imageBlock').children().remove();
        for (var i = 0; i < input.files.length; i++) {
            if (input.files[i]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#imageBlock').append("<div style='margin: 5px;' class='ml-3'><img class='img-thumbnail small-image img-small-loaded' src='#'/><label style='text-align: center;'></label><input type='button' class='btn-outline-primary btn-close' onclick='RemoveBlock(this)' value='x' /></div>");
                    $('#imageBlock').children().last().children().first().attr('src', e.target.result);
                    ValidateImage();
                }
                reader.readAsDataURL(input.files[i]);
            }
        }
    }
}

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

// Add plus button to menu itme on hover

$(document).on("mouseenter mouseleave", ".menu-item-hall", function (e) {
    if (e.type == "mouseenter") {
        $(this).children().first().append("<div onclick=\"LoadViewFromMenu(null, '/Stands/Create?hallId=" + $(this).attr("data-hall-id") + "', false, true, event)\" type='button' class='btn btn-add'><svg xmlns = 'http://www.w3.org/2000/svg' viewBox = '0 0 24 24' class= 'plus-svg'><path fill='currentColor' d='M19 13h-6v6h-2v-6H5v-2h6V5h2v6h6v2z'/></svg></div >");
    } else {
        $(this).children().first().find('.btn-add').remove();
    }
});

$(document).on("mouseenter mouseleave", ".menu-item-stand", function (e) {
    if (e.type == "mouseenter") {
        $(this).children().first().append("<button onclick=\"LoadViewFromMenu(null, '/Exhibits/Create?standId=" + $(this).attr("data-stand-id") + "&hallId=" + $(this).attr("data-hall-id") + "', false, true, event)\" type='button' class='btn btn-add'><svg xmlns='http://www.w3.org/2000/svg' viewBox ='0 0 24 24' class='plus-svg'><path d='M0 0h24v24H0z' fill='none'/><path fill='currentColor' d='M19 13h-6v6h-2v-6H5v-2h6V5h2v6h6v2z'/></svg></button>");
    } else {
        $(this).children().first().find('.btn-add').remove();
    }
});

// Collapse menu itmes
// element - sender element
// id of the collapsing area
// e - event
function Collapse(element, id, e) {
    if ($(element).attr('aria-expanded') == 'true') {
        $('#id' + id).collapse('hide');
    }
    else {
        $('#id' + id).collapse('show');
    }

    e.stopPropagation();
}

// Load partial view, apply style to menu item and display in div with id content
// element - element invoked action
// url - url to send request
function LoadViewFromMenu(element, url, shouldHighlightMenuItem, shouldChangeHighlightedItem, e) {
    e.stopPropagation();
    if (shouldHighlightMenuItem) {
        $('.selected-item').removeClass("selected-item");
        $(element).addClass("selected-item");
    }
    else if (shouldChangeHighlightedItem){
        $('.selected-item').removeClass("selected-item");
    }
    $.ajax({
        url: url,
        type: 'GET',
        processData: false,
        contentType: false,
        beforeSend: function () {
            $('#content').empty();
            $('#loader').css('visibility', 'visible');
        },
        success: function (view) {
            $('#loader').css('visibility', 'collapse');
            $('#content').html(view);
            $("#form").removeData("validator");
            $("#form").removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse("#form");
        }
    });
}

// Athenticate user
function Authenticate() {
    var $form = $('form');
    $form.validate();
    if (!$form.valid()) { Alert("Заполните все поля!"); return false; }

    $.ajax({
        url: '/Authentication/Authentication',
        type: 'POST',
        data: new FormData($('#form').get()[0]),
        processData: false,
        contentType: false,
        beforeSend: function () {
            $('#loader').css('visibility', 'visible');
        },
        statusCode: {
            200: function (view) {
                $('#loader').css('visibility', 'collapse');
                $('main').html(view);
                $("#form").removeData("validator");
                $("#form").removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse("#form");
            },
            204: function () {
                $('#loader').css('visibility', 'collapse');
                window.location.href = '/Home';
            }
        }
    });
}

// Revoke Tokens
function Revoke(login) {
    $.ajax({
        url: '/Authentication/Revoke',
        type: 'POST',
        data: { "login": login },
        beforeSend: function () {
            $('#loader').css('visibility', 'visible');
        },
        statusCode: {
            204: function (view) {
                $('#loader').css('visibility', 'collapse');
                window.location.href = '/Home';
            }
        }
    });
}

// Send request to controller to perform delete and remove item from menu
// url - url to send request
// id - id of the recor to delete
// checkForEmtpy - check parent container to be emtpy after delete
function DeleteItem(url, id, checkForEmtpy) {
    $('#deleteConfirm').attr('data-id', id);
    $('#deleteConfirm').attr('data-url', url);
    $('#deleteConfirm').attr('data-checkForEmtpy', checkForEmtpy);
    $('#deleteConfirm').modal("show");
}

// Show custom alert dialog
function Alert(text) {
    $('#alert .modal-body').children().first().text(text);
    $('#alert').modal("show");
}

// Send request to controller to perform edit
// id - id of edited record
// textId - id of text to display on menu after edditing
// url - url to send request to
// idOfElementsToReorder - if enable elements reordering, else null
function Edit(id, textId, url, idOfElementsToReorder) {
    var $form = $('form');
    $form.validate();
    if (!$form.valid() | !ValidateImage()) { Alert("Заполните все поля!"); return false; }
    $.ajax({
        url: url,
        type: 'POST',
        data: new FormData($('#form').get()[0]),
        processData: false,
        contentType: false,
    });

    if (idOfElementsToReorder != null) {
        if ($('#' + idOfElementsToReorder).get()[0]) {
            var parentFrom = $('#' + idOfElementsToReorder).get()[0];
            if (parentFrom.children.length > 1) {
                Reorder(parentFrom, 'id' + id)
            }
        }
    }

    $('#elName' + id).text($('#' + textId).val());
    $('#content').empty();
    $('.selected-item').removeClass("selected-item");
}

// Send request to controller to perform edit
// id - id of edited record
// textId - id of text to display on menu after edditing
// url - url to send request to
function EditArticle(id, textId, url) {
    var $form = $('form');
    $form.validate();
    if (!$form.valid()) { Alert("Заполните все поля!"); return false; }
    $.ajax({
        url: url,
        type: 'POST',
        data: new FormData($('#form').get()[0]),
        processData: false,
        contentType: false,
    });

    $('#elName' + id).text($('#' + textId).val());
    $('#content').empty();
    $('.selected-item').removeClass("selected-item");
}

// Send request to controller to perform edit
// id - id of edited record
// textId - id of text to display on menu after edditing
// url - url to send request to
function EditGallery(id, textId, url) {
    var $form = $('form');
    $form.validate();
    if (!$form.valid() | !ValidateInputUnits()) { Alert("Заполните все поля!"); return false; }
    $.ajax({
        url: url,
        type: 'POST',
        data: new FormData($('#form').get()[0]),
        processData: false,
        contentType: false,
    });

    $('#elName' + id).text($('#' + textId).val());
    $('#content').empty();
    $('.selected-item').removeClass("selected-item");
}

// Reorder elements in navigation menu
// parentFrom - parent of elements with new ordering
// parentToId - id of parent of elements in menu
function Reorder(parendFrom, parentToId) {
    for (var i = parendFrom.children.length - 1; i >= 0; i--) {
        id = parendFrom.children[i].children[0].value;
        $('#' + parentToId).prepend($('#' + parentToId + ' > div[id=elId' + id + ']'));
    }
}

// Save data from form to db and add new hall to menu
// textId - id of the field to display in menu
function CreateHall(textId) {
    var $form = $('form');
    $form.validate();
    if (!$form.valid()) { Alert("Заполните все поля!"); return false; }
    $.ajax({
        url: '/Halls/Create',
        type: 'POST',
        data: new FormData($('#form').get()[0]),
        processData: false,
        contentType: false,
        success: function (id) {
            $("<div id='elId" + id + "'><div class='menuItem menu-item-hall' data-hall-id='" + id + "' onclick=\"LoadViewFromMenu(this, '/Halls/Index/" + id + "', true, false, event)\"><div class='d-flex flex-row align-items-center'><div class='btn btn-arrow collapsed' data-toggle='collapse' onclick=\"Collapse(this, '" + id + "', event)\" data-target='#id" + id + "' aria-expanded='false' aria-controls='id" + id + "' style='visibility: hidden;'><i class='fa' aria-hidden='false'></i></div ><a class='a-header overflow' id='elName" + id + "'>" + $('#' + textId).val() + "</a></div></div><div class='collapse' id='id" + id + "'></div></div>").insertBefore($('#halls').children().last());
            $('#content').empty();
        }
    });
    $('.selected-item').removeClass("selected-item");
}

// Save data from form to db and add new stand to menu
// textId - id of the field to display in menu
function CreateStand(hallId, textId) {
    var $form = $('form');
    $form.validate();
    if (!$form.valid()) { Alert("Заполните все поля!"); return false; }
    $.ajax({
        url: '/Stands/Create?hallId=' + hallId,
        type: 'POST',
        data: new FormData($('#form').get()[0]),
        processData: false,
        contentType: false,
        success: function (id) {
            $('#elId' + hallId).children().first().children().first().children().first().css("visibility", "visible");
            $('#id' + hallId).append("<div id='elId" + id + "'><div class='menuItem menu-item-stand' data-stand-id='" + id + "' data-hall-id='" + hallId + "' onclick=\"LoadViewFromMenu(this, '/Stands/Index/" + id + "?hallId=" + hallId + "', true, false, event)\"><div class='d-flex flex-row align-items-center'><div class='btn btn-arrow collapsed' data-toggle='collapse' onclick=\"Collapse(this, '" + id + "', event)\" data-target='#id" + id + "' aria-expanded='false' aria-controls='id" + id + "' style='visibility: hidden;'><i class='fa' aria-hidden='false'></i></div ><a class='a-header overflow' id='elName" + id + "'>" + $('#' + textId).val() + "</a></div></div><div class='collapse' id='id" + id + "'></div></div>");
            $('#content').empty();
        }
    });
    $('.selected-item').removeClass("selected-item");
}

// Save data from form to db and add new exhibit to menu
// textId - id of the field to display in menu
function CreateExhibit(hallId, standId, textId) {
    var $form = $('form');
    $form.validate();
    if (!$form.valid()) { Alert("Заполните все поля!"); return false; }
    $.ajax({
        url: '/Exhibits/Create?hallId=' + hallId + '&standId=' + standId,
        type: 'POST',
        data: new FormData($('#form').get()[0]),
        processData: false,
        contentType: false,
        success: function (id) {
            $('#elId' + standId).children().first().children().first().children().first().css("visibility", "visible");
            $('#id' + standId).append("<div style='margin-top: 10px;' id='elId" + id + "'><div class='menuItem' onclick=\"LoadViewFromMenu(this, '/Exhibits/Index?id=" + id + "&standId=" + standId + "&hallId=" + hallId + "', true, false, event)\"><div style='margin-left: 74px; padding:2px;' class='d-flex flex-row' ><a class='a-header overflow' id='elName" + id + "'>" + $('#' + textId).val() + "</a></div></div></div>");
            $('#content').empty();
        }
    });
    $('.selected-item').removeClass("selected-item");
}

// Save data from form to db and add new exhibit to menu
// textId - id of the field to display in menu
function CreateExhibitGallery(hallId, standId, textId) {
    var $form = $('form');
    $form.validate();
    if (!$form.valid() | !ValidateInputUnits()) { Alert("Заполните все поля!"); return false; }
    $.ajax({
        url: '/Exhibits/Create?hallId=' + hallId + '&standId=' + standId,
        type: 'POST',
        data: new FormData($('#form').get()[0]),
        processData: false,
        contentType: false,
        success: function (id) {
            $('#elId' + standId).children().first().children().first().children().first().css("visibility", "visible");
            $('#id' + standId).append("<div style='margin-top: 10px;' class='flex-row' id='elId" + id + "'><div class='menuItem' onclick=\"LoadViewFromMenu(this, '/Exhibits/Index?id=" + id + "&standId=" + standId + "&hallId=" + hallId + "', true, false, event)\"><div style='margin-left: 4em; padding:2px;'><a class='a-header' id='elName" + id + "'>" + $('#' + textId).val() + "</a></div></div></div>");
            $('#content').empty();
        }
    });
    $('.selected-item').removeClass("selected-item");
}

// Combobox onchange method to change form content
function ExhibitTypeChange(combobox, hallId, standId) {
    if ($(combobox).val() == 'Статья') {
        $.ajax({
            url: '/Exhibits/LoadCreateArticle?hallId=' + hallId + '&standId=' + standId,
            type: 'GET',
            processData: false,
            contentType: false,
            beforeSend: function () {
                $('#formContent').empty();
                $('#loader').css('visibility', 'visible');
            },
            success: function (view) {
                $('#loader').css('visibility', 'collapse');
                $('#formContent').html(view);
                $("#form").removeData("validator");
                $("#form").removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse("#form");
            }
        });
    }
    else if ($(combobox).val() == 'Галерея'){
        $.ajax({
            url: '/Exhibits/LoadCreateGallery?hallId=' + hallId + '&standId=' + standId,
            type: 'GET',
            processData: false,
            contentType: false,
            beforeSend: function () {
                $('#formContent').empty();
                $('#loader').css('visibility', 'visible');
            },
            success: function (view) {
                $('#loader').css('visibility', 'collapse');
                $('#formContent').html(view);
                $("#form").removeData("validator");
                $("#form").removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse("#form");
            }
        });
    }
}

// Add new PhotoInfo part when creating exhibits galllery
function AddPhotoInfo() {
    $('#photos').append("<div class='mt-3'><div><h5>Описание фотографии</h5><label class='ml-3'>Фото</label><span class='field-validation-error'></span><div class='ml-3'><div class='d-flex form-group ml-3'><button type='button' onclick='LoadPhoto(this)' class='btn btn-primary btn-form'><svg xmlns='http://www.w3.org/2000/svg' height='1.3em' viewBox='0 0 24 24' width='1.3em'><path fill='currentColor' d='M19 7v2.99s-1.99.01-2 0V7h-3s.01-1.99 0-2h3V2h2v3h3v2h-3zm-3 4V8h-3V5H5c-1.1 0-2 .9-2 2v12c0 1.1.9 2 2 2h12c1.1 0 2-.9 2-2v-8h-3zM5 19l3-4 2 3 3-4 4 5H5z' /></svg>Загрузить</button><input class='photoLoader' style='visibility: hidden' type='file' name='files' /></div></div></div><div class='form-group ml-3'><label for='descriptionBe'>Описание фото (BY)</label><span class='field-validation-error'></span><input type='text' placeholder='Краткое описание фото на белорусском языке' name='photoDescriptionBe' class='form-control validation-unit ml-3' /></div><div class='form-group ml-3'><label for='descriptionRu'>Описание фото (RU)</label><span class='field-validation-error'></span><input type='text' placeholder='Краткое описание фото на русском языке' name='photoDescriptionRu' class='form-control validation-unit ml-3' /></div><div class='form-group ml-3'><label for='descriptionEn'>Описание фото (EN)</label><span class='field-validation-error'></span><input type='text' placeholder='Краткое описание фото на английском языке' name='photoDescriptionEn' class='form-control validation-unit ml-3' /></div><div class='form-group'><button type='button' onclick='DeletePhotoInfo(this)' class='btn btn-danger btn-form'><svg width='1.2em' height='1.2em' viewBox='0 0 16 16' class='bi bi-trash-fill' fill='currentColor' xmlns='http://www.w3.org/2000/svg'><path fill-rule='evenodd' d='M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1H2.5zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5zM8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5zm3 .5a.5.5 0 0 0-1 0v7a.5.5 0 0 0 1 0v-7z'/></svg>Удалить запись</button></div></div></div>");
}

// Add new PhotoInfo part when editing exhibits galllery
function AddPhotoInfoEdit() {
    $('#photos').append("<div class='mt-3'><div><h5>Описание фотографии</h5><label class='ml-3'>Фото</label><span class='field-validation-error'></span><div class='ml-3'><div class='d-flex form-group ml-3'><button type='button' onclick='LoadPhoto(this)' class='btn btn-primary btn-form'><svg xmlns='http://www.w3.org/2000/svg' height='1.3em' viewBox='0 0 24 24' width='1.3em'><path fill='currentColor' d='M19 7v2.99s-1.99.01-2 0V7h-3s.01-1.99 0-2h3V2h2v3h3v2h-3zm-3 4V8h-3V5H5c-1.1 0-2 .9-2 2v12c0 1.1.9 2 2 2h12c1.1 0 2-.9 2-2v-8h-3zM5 19l3-4 2 3 3-4 4 5H5z' /></svg>Загрузить</button><input class='photoLoaderEdit' style='visibility: hidden' type='file' name='files' /></div></div></div><div class='form-group ml-3'><label for='descriptionBe'>Описание фото (BY)</label><span class='field-validation-error'></span><input type='text' placeholder='Краткое описание фото на белорусском языке' name='photoDescriptionBe' class='form-control validation-unit ml-3' /></div><div class='form-group ml-3'><label for='descriptionRu'>Описание фото (RU)</label><span class='field-validation-error'></span><input type='text' placeholder='Краткое описание фото на русском языке' name='photoDescriptionRu' class='form-control validation-unit ml-3' /></div><div class='form-group ml-3'><label for='descriptionEn'>Описание фото (EN)</label><span class='field-validation-error'></span><input type='text' placeholder='Краткое описание фото на английском языке' name='photoDescriptionEn' class='form-control validation-unit ml-3' /></div><div class='form-group'><button type='button' onclick='DeletePhotoInfo(this)' class='btn btn-danger btn-form'><svg width='1.2em' height='1.2em' viewBox='0 0 16 16' class='bi bi-trash-fill' fill='currentColor' xmlns='http://www.w3.org/2000/svg'><path fill-rule='evenodd' d='M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1H2.5zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5zM8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5zm3 .5a.5.5 0 0 0-1 0v7a.5.5 0 0 0 1 0v-7z'/></svg>Удалить запись</button></div></div></div>");
}

// Used to invoke method that display selected images
function LoadPhoto(button) {
    $(button).parent().children().last().get()[0].click();
    ValidateInputFile($(button).parent().children().last());
}

// Event listeners to validate exhibits gallery
$(document).on("change", ".photoLoader", function () {
    ReadPhoto(this);
});

$(document).on("change", ".photoLoaderEdit", function () {
    ReadPhotoEdit(this);
});

$(document).on("focusout", ".validation-unit", function () {
    ValidateRequired(this);
});

$(document).on("input", ".validation-unit", function () {
    ValidateRequired(this);
});

// Used to invoke method that display selected images
$(document).on("change", "#inputFile", function () {
    readURL($('#inputFile').get()[0]);
});

// Is activated when confirm delete button pressed
$(document).on("click", ".confirmOk", function () {
    if ($('#deleteConfirm').attr('data-checkforemtpy') == "true") {
        // Check parent container to have more children. If it is empty - make collapse button invisible
        if ($('#elId' + $('#deleteConfirm').attr('data-id')).parent().children().length == 1) {
            $('#elId' + $('#deleteConfirm').attr('data-id')).parent().parent().children().first().children().first().children().first().css("visibility", "hidden");
        }
    }

    $("#content").load($('#deleteConfirm').attr('data-url'));
    $('#elId' + $('#deleteConfirm').attr('data-id')).remove();
    $('#deleteConfirm').modal("hide");
});
// -----------------


// Validate input in photo description
function ValidateRequired(input) {
    if ($(input).val() == "") {
        $(input).prev().text("Введите описание");
        $(input).addClass("input-validation-error");
        return false;
    }
    else {
        $(input).prev().text("");
        $(input).removeClass("input-validation-error");
        return true;
    }
}

// Draw selected images to block in exhibit gallery
function ReadPhoto(input) {
    if (input.files) {
        if ($(input).parent().parent().children().length == 2) {
            $(input).parent().parent().children().first().remove();
        }
        for (var i = 0; i < input.files.length; i++) {
            if (input.files[i]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $(input).parent().parent().prepend("<div style='margin:0px 0px 10px 0px;' class='ml-3'><img class='img-thumbnail small-image img-small-loaded' src='#'/><input type='button' class='btn-outline-primary btn-close' onclick='RemovePhoto(this)' value='x' /></div>");
                    $(input).parent().parent().children().first().children().first().attr('src', e.target.result);
                }
                reader.readAsDataURL(input.files[i]);
            }
        }
    }
    ValidateInputFile(input);
}

// Draw selected images to block in exhibit gallery
function ReadPhotoEdit(input) {
    if (input.files) {
        if ($(input).parent().parent().children().length == 2) {
            $(input).parent().parent().children().first().remove();
        }
        for (var i = 0; i < input.files.length; i++) {
            if (input.files[i]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $(input).parent().parent().prepend("<div style='margin:0px 0px 10px 0px;' class='ml-3'><img class='img-thumbnail small-image img-small-loaded' src='#'/><input type='button' class='btn-outline-primary btn-close' onclick='RemovePhoto(this)' value='x' /><input type='hidden' value='0' name='ids' /></div>");
                    $(input).parent().parent().children().first().children().first().attr('src', e.target.result);
                }
                reader.readAsDataURL(input.files[i]);
            }
        }
    }
    ValidateInputFile(input);
}

// Remove photo in Exhibits galery
function RemovePhoto(photo) {
    var div = $(photo).parent().parent().get()[0];
    $(photo).parent().remove();
    var input = $(div).children().first().children().last().get()[0];
    $(input).val("");
    ValidateInputFile(input);
}

// Remove block with photo in Exhibits galery
function DeletePhotoInfo(block) {
    if ($('#photos').children().length > 1) {
        $(block).parent().parent().remove();
    }
}

// Validate selected input for photo
function ValidateInputFile(input) {
    if ($(input).val() == "") {
        $(input).parent().parent().parent().find('span').text("Выберите фото");
        return false;
    }
    $(input).parent().parent().parent().find('span').text("");
    return true;
}

// Validate selected input for photo
function ValidateInputFileEdit(input) {
    if ($(input).parent().parent().children().length != 2) {
        $(input).parent().parent().parent().find('span').text("Выберите фото");
        return false;
    }
    $(input).parent().parent().parent().find('span').text("");
    return true;
}

// Validate all inputs including photos
function ValidateInputUnits() {
    var isOk = true;
    $(".validation-unit").each(function (i, obj)
    {
        if (!ValidateRequired(obj)) {
            isOk = false;
        }
    })

    $(".photoLoader").each(function (i, obj) {
        if (!ValidateInputFile(obj)) {
            isOk = false;
        }
    })

    $(".photoLoaderEdit").each(function (i, obj) {
        if (!ValidateInputFileEdit(obj)) {
            isOk = false;
        }
    })

    if (isOk) {
        return true;
    }
    return false;
}

function CreateCache() {
    if ($('#ruText').prop("checked") == false && $('#enText').prop("checked") == false
        && $('#beText').prop("checked") == false && $('#photos').prop("checked") == false) {
        Alert("Выберите хотя бы одну опцию!");
        return;
    }
    $.ajax({
        url: '/Home/CreateCache?isRussianChecked=' + $('#ruText').prop("checked") + '&isEnglishChecked=' + $('#enText').prop("checked")
            + '&isBelarussianChecked=' + $('#beText').prop("checked") + '&isPhotosChecked=' + $('#photos').prop("checked") ,
        type: 'POST',
        processData: false,
        contentType: false,
        beforeSend: function () {
            $('#loader').css('visibility', 'visible');
        },
        success: function () {
            $('#loader').css('visibility', 'collapse');
            Alert("Кеш успешно сохранен!");
            // wait for user to close alert pop up
            WaitingToRedirect();
        },
        error: function () {
            $('#loader').css('visibility', 'collapse');
            Alert("Что-то пошло не так...");
        }
    });
}

function WaitingToRedirect() {
    var timer = setTimeout(function () {
        if (!($('#alert').hasClass("show"))) {
            if (timer) {
                clearTimeout(timer);
            }
            var getUrl = window.location;
            var baseUrl = getUrl.protocol + "//" + getUrl.host + "/" + getUrl.pathname.split('/')[1];
            window.location.replace(baseUrl);
        }
        else {
            SetLoop();
        }
    }, 500);
}
