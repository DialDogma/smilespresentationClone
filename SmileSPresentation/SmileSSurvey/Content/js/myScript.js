'use strict';

$(window).on('load', function () {
    $("#loading").fadeOut("slow");
});

//spinner
const loadingSpinner = (fadeType, delay = 999999) => {
    switch (fadeType) {
        case "fadeIn":
            $("#loading").fadeIn(500, (e) => {
                setTimeout(() => {
                    $("#loading").fadeOut();
                }, delay);
            });
            break;
        case "fadeOut":
            $("#loading").fadeOut(2000);
            break;
        default:
    }
}

//check xhr status
function checkXHRStatus(xhrStatus, exception) {
    var msg = '';
    if (xhrStatus.status === 0) {
        msg = 'Not connect.\n Verify Network.';
    } else if (xhrStatus.status == 404) {
        msg = 'Requested page not found. [404]';
    } else if (xhrStatus.status == 500) {
        msg = 'Internal Server Error [500].';
    } else if (exception === 'parsererror') {
        msg = 'Requested JSON parse failed.';
    } else if (exception === 'timeout') {
        msg = 'Time out error.';
    } else if (exception === 'abort') {
        msg = 'Ajax request aborted.';
    } else {
        msg = 'Uncaught Error.\n' + xhrStatus.responseText;
    }

    alert(msg)
}