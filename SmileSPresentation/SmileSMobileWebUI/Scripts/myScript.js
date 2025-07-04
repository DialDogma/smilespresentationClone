$(function () {
    moment.locale('th');
});

const loadingSpinner = (fadeType, delay) => {
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
        default:
    }
}

function getRandomColor() {
    const letters = '0123456789ABCDEF';
    var color = '#';
    for (let i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}

function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    const regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)");
    const results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}
//--------------------------------------ALERT MESSAGE--------------------------------------------------//
const swal_confirm = (title, text, success, cancel) => {
    window.swal({
        title: title,
        text: text,
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#277020',
        confirmButtonText: 'ตกลง',
        cancelButtonText: 'ยกเลิก',
        closeOnConfirm: false,
        closeOnEsc: false,
        closeOnCancel: true
    }, (IsConfirm) => {
        if (IsConfirm) {
            if (typeof success === "function") {
                //callback
                success();
            }
        } else {
            if (typeof cancel === "function") {
                //callback
                cancel();
            }
        }
    });
}

const swal_success = (success, cancel) => {
    window.swal({
        title: 'ดำเนินการเรียบร้อย!',
        type: 'success',
        showCancelButton: false,
        confirmButtonColor: '#26A65B',
        confirmButtonText: 'ตกลง',
        closeOnEsc: false
    }, (IsConfirm) => {
        if (IsConfirm) {
            if (typeof success === "function") {
                //callback
                success();
            }
        } else {
            if (typeof cancel === "function") {
                //callback
                cancel();
            }
        }
    });
}

const swal_fail = (text, callback) => {
    window.swal({
        title: 'เกิดข้อผิดพลาด!',
        type: 'error',
        text: text,
        showCancelButton: false,
        confirmButtonColor: '#ed2b09',
        confirmButtonText: 'ตกลง',
        closeOnEsc: false
    }, () => {
        if (typeof callback === "function") {
            //callback
            callback();
        }
    });
}