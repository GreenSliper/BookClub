var selected = [];

function ToggleBookAndPlaceFirst(id) {
    if (selected.indexOf(id) > -1)
        selected.splice(selected.indexOf(id), 1);
    else {
        $('#' + id).insertAfter($('#tableHeader'));
        selected.push(id);
    }
    ToggleBook(id);
}

function ToggleBookAndHideIt(id)
{
    var elem = document.getElementById(id);
    elem.style.display = 'none';
    ToggleBook(id);
}

function ToggleBook(id) {
    var req = {
        id: id,
    };
    $.ajax({
        type: 'POST',
        url: '/BookPicker/ToggleBook',
        data: req,
        success: function () { }
    });
}