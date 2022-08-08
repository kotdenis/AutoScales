export function initialDisabled() {
    $("#buttonbegin").attr("disabled", "true")
    $("#buttonSave").attr("disabled", "true")
}

export function afterAccept() {
    $("#buttonbegin").removeAttr("disabled");
    $("#buttonAccept").attr("disabled", "true");
}

export function endWeighing() {
    $('#inputWeight').css('background-color', '#98FB98');
    $("#buttonbegin").attr("disabled", "true");
    $("#buttonSave").removeAttr("disabled");
}

export function saveWeighing() {
    $("#buttonbegin").attr("disabled", "true");
    $("#buttonSave").attr("disabled", "true");
    $("#buttonAccept").removeAttr("disabled");
    $('#inputWeight').css('background-color', '#FFFFFF');
}

export function showSuccess() {
    $('#modalSuccess').show();
}

export function closeSuccess() {
    $('#modalSuccess').hide();
}

export function showError() {
    $('#modalError').show();
}

export function closeError() {
    $('#modalError').hide();
}

export function showDelUpdate() {
    $('#modalDelUpdate').show();
}

export function closeDelUpdate() {
    $('#modalDelUpdate').hide();
}

export function makeTest(test) {
    alert(test);
}