// Remove parent block. Used to delete image
function RemoveBlock(block) {
    $(block).parent().remove();
    ValidateImage();
}

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
        console.log($('#imageBlock').get()[0].children.length);
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
                    $('#imageBlock').append("<div style='margin: 5px;'><img class='img-thumbnail small-image img-small-loaded' src='#'/><label style='text-align: center;'></label><input type='button' class='btn-outline-secondary btn-close' onclick='RemoveBlock(this)' value='x' /></div>");
                    $('#imageBlock').children().last().children().first().attr('src', e.target.result);
                    ValidateImage();
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

// Load partial view, apply style to menu item and display in div with id content
// url - url to send request
function LoadViewFromMenu(element, url) {
    $('.selected-item').removeClass("selected-item");
    $(element).parent().parent().addClass("selected-item");
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

// Load partial view and display in div with id content
// url - url to send request
function LoadView(url) {
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

// Send request to controller to perform delete and remove item from menu
// url - url to send request
// id - id of the recor to delete
function DeleteItem(url, id) {

    result = confirm("Вы действительно хотите удалить данную страницу?");
    if (result) {
        $("#content").load(url);
        $('#elId' + id).remove();
    }
}


// Send request to controller to perform edit
// id - id of edited record
// textId - id of text to display on menu after edditing
// url - url to send request to
// idOfElementsToReorder - if enable elements reordering, else null
function Edit(id, textId, url, idOfElementsToReorder) {
    var $form = $('form');
    $form.validate();
    if (!$form.valid() | !ValidateImage()) { alert("Заполните все поля!"); return false; }
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
    $('.selected-item').removeClass("selected-item");
}

// Send request to controller to perform edit
// id - id of edited record
// textId - id of text to display on menu after edditing
// url - url to send request to
function EditGallery(id, textId, url) {
    var $form = $('form');
    $form.validate();
    if (!$form.valid() | !ValidateInputUnits()) { alert("Заполните все поля!"); return false; }
    $.ajax({
        url: url,
        type: 'POST',
        data: new FormData($('#form').get()[0]),
        processData: false,
        contentType: false
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
        $('#' + parentToId + ' > div[id=elId' + id + ']').insertAfter($('#' + parentToId + ' > a'));
    }
}

// Save data from form to db and add new hall to menu
// textId - id of the field to display in menu
function CreateHall(textId) {
    var $form = $('form');
    $form.validate();
    if (!$form.valid()) { alert("Заполните все поля!"); return false; }
    $.ajax({
        url: '/Halls/Create',
        type: 'POST',
        data: new FormData($('#form').get()[0]),
        processData: false,
        contentType: false,
        success: function (id) {
            $('#halls').append("<div id='elId" + id + "'><div><div style='margin-top: 10px;' class='flex-row'><button class='btn btn-link collapsed' data-toggle='collapse' data-target='#id" + id + "' aria-expanded='false' aria-controls='id" + id + "' style='margin-bottom:3px;padding:2px;'><i class='fa' aria-hidden='false'></i></button><a class='a-header' id='elName" + id + "' onclick=\"LoadViewFromMenu(this, '/Halls/Index/" + id + "')\">" + $('#' + textId).val() + "</a></div></div><div class='collapse' id='id" + id + "'><div><div style='margin-left: 50px'><div onclick=\"LoadViewFromMenu(this, '/Stands/Create?hallId=" + id + "')\"><img src='/images/add.png' class='img-btn' /><a class='a-btn' >Добавить стенд</a></div></div></div></div></div>");
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
    if (!$form.valid()) { alert("Заполните все поля!"); return false; }
    $.ajax({
        url: '/Stands/Create?hallId=' + hallId,
        type: 'POST',
        data: new FormData($('#form').get()[0]),
        processData: false,
        contentType: false,
        success: function (id) {
            $('#id' + hallId).append("<div id='elId" + id + "'><div><div style='margin-top: 10px;margin-left: 50px' class='flex-row'><button class='btn btn-link collapsed' data-toggle='collapse' data-target='#id" + id + "' aria-expanded='false' aria-controls='id" + id + "' style='margin-bottom:3px;padding:2px;'><i class='fa' aria-hidden='false'></i></button><a class='a-header' id='elName" + id + "' onclick=\"LoadViewFromMenu(this, '/Stands/Index/" + id + "?hallId=" + hallId + "')\">" + $('#' + textId).val() + "</a></div></div><div class='collapse' id='id" + id + "'><div><div style='margin-left: 100px'><div onclick=\"LoadViewFromMenu(this, '/Exhibits/Create?standId=" + id + "&hallId=" + hallId + "')\"><img src='/images/add.png' class='img-btn' /><a class='a-btn' >Добавить экспонат</a></div></div></div></div></div>");
            $('#content').empty();
            $('.')
        }
    });
    $('.selected-item').removeClass("selected-item");
}

// Save data from form to db and add new exhibit to menu
// textId - id of the field to display in menu
function CreateExhibit(hallId, standId, textId) {
    var $form = $('form');
    $form.validate();
    if (!$form.valid()) { alert("Заполните все поля!"); return false; }
    $.ajax({
        url: '/Exhibits/Create?hallId=' + hallId + '&standId=' + standId,
        type: 'POST',
        data: new FormData($('#form').get()[0]),
        processData: false,
        contentType: false,
        success: function (id) {
            $('#id' + standId).append("<div><div style='margin-top: 10px;margin-left: 100px;padding:2px;' class='flex-row' id='elId" + id + "'><a class='a-header' id='elName" + id + "' onclick=\"LoadViewFromMenu(this, '/Exhibits/Index?id=" + id + "&standId=" + standId + "&hallId=" + hallId + "')\">" + $('#' + textId).val() +"</a ></div></div>");
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
    if (!$form.valid() | !ValidateInputUnits()) { alert("Заполните все поля!"); return false; }
    $.ajax({
        url: '/Exhibits/Create?hallId=' + hallId + '&standId=' + standId,
        type: 'POST',
        data: new FormData($('#form').get()[0]),
        processData: false,
        contentType: false,
        success: function (id) {
            $('#id' + standId).append("<div><div style='margin-top: 10px;margin-left: 100px;padding:2px;' class='flex-row' id='elId" + id + "'><a class='a-header' id='elName" + id + "' onclick=\"LoadViewFromMenu(this, '/Exhibits/Index?id=" + id + "&standId=" + standId + "&hallId=" + hallId + "')\">" + $('#' + textId).val() +"</a ></div></div>");
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
    $('#photos').append("<div><div><h5>Описание фотографии</h5><label>Фото</label><span class='field-validation-error'></span><div><div class='d-flex form-group'><button type='button' onclick='LoadPhoto(this)' class='btn btn-outline-secondary btn-form'><img src='/images/upload.png' class='img-btn' />Загрузить</button><input class='photoLoader' style='visibility: hidden' type='file' name='files' /></div></div></div><div class='form-group'><label for='descriptionBe'>Описание фото (BY)</label><span class='field-validation-error'></span><input type='text' placeholder='Краткое описание фото на белорусском языке' name='photoDescriptionBe' class='form-control validation-unit' /></div><div class='form-group'><label for='descriptionRu'>Описание фото (RU)</label><span class='field-validation-error'></span><input type='text' placeholder='Краткое описание фото на русском языке' name='photoDescriptionRu' class='form-control validation-unit' /></div><div class='form-group'><label for='descriptionEn'>Описание фото (EN)</label><span class='field-validation-error'></span><input type='text' placeholder='Краткое описание фото на английском языке' name='photoDescriptionEn' class='form-control validation-unit' /></div><button type='button' onclick='DeletePhotoInfo(this)' class='btn btn-outline-secondary btn-form'><img src='/images/delete.png' class='img-btn' />Удалить запись</button></div></div>");
}

// Add new PhotoInfo part when editing exhibits galllery
function AddPhotoInfoEdit() {
    $('#photos').append("<div><div><h5>Описание фотографии</h5><label>Фото</label><span class='field-validation-error'></span><div><div class='d-flex form-group'><button type='button' onclick='LoadPhoto(this)' class='btn btn-outline-secondary btn-form'><img src='/images/upload.png' class='img-btn' />Загрузить</button><input class='photoLoaderEdit' style='visibility: hidden' type='file' name='files' /></div></div></div><div class='form-group'><label for='descriptionBe'>Описание фото (BY)</label><span class='field-validation-error'></span><input type='text' placeholder='Краткое описание фото на белорусском языке' name='photoDescriptionBe' class='form-control validation-unit' /></div><div class='form-group'><label for='descriptionRu'>Описание фото (RU)</label><span class='field-validation-error'></span><input type='text' placeholder='Краткое описание фото на русском языке' name='photoDescriptionRu' class='form-control validation-unit' /></div><div class='form-group'><label for='descriptionEn'>Описание фото (EN)</label><span class='field-validation-error'></span><input type='text' placeholder='Краткое описание фото на английском языке' name='photoDescriptionEn' class='form-control validation-unit' /></div><button type='button' onclick='DeletePhotoInfo(this)' class='btn btn-outline-secondary btn-form'><img src='/images/delete.png' class='img-btn' />Удалить запись</button></div></div>");
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
                    $(input).parent().parent().prepend("<div style='margin:0px 0px 10px 0px;'><img class='img-thumbnail small-image img-small-loaded' src='#'/><input type='button' class='btn-outline-secondary btn-close' onclick='RemovePhoto(this)' value='x' /></div>");
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
                    $(input).parent().parent().prepend("<div style='margin:0px 0px 10px 0px;'><img class='img-thumbnail small-image img-small-loaded' src='#'/><input type='button' class='btn-outline-secondary btn-close' onclick='RemovePhoto(this)' value='x' /><input type='hidden' value='0' name='ids' /></div>");
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
        $(block).parent().remove();
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
