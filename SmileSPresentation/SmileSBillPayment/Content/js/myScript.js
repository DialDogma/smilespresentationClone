'use strict';

$(window).on('load', function () {
    $("#loading").fadeOut("slow");
});

/***format number*****/
function formatNumberCurrency(num, decimalplace) {
    let n = Number.parseFloat(num).toFixed(decimalplace);
    return n.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
}

//add method validator check N/A
jQuery.validator.addMethod("checkNA", function (value, element) {
    if (value == "-1") {
        return false;
    } else {
        return true;
    };
}, "กรุณาเลือก");

window.jQuery.validator.addMethod("checkOnlyNum", function (value) {
    var regex = new RegExp("^[0-9]+$");

    if (!regex.test(value)) {
        return false;
    } else {
        return true;
    }
}, "กรอกเฉพาะตัวเลขเท่านั้น(0-9)");

/*add method validator check null*/
window.jQuery.validator.addMethod("checknull", function (value) {
    if (value === null) {
        return false;
    } else {
        return true;
    }
}, "**กรุณาเลือก");

/*add method validator check 0 */
window.jQuery.validator.addMethod("checkZero", function (value) {
    if (value == 0) {
        return false;
    } else {
        return true;
    }
}, "จำนวนเงินต้องไม่ใช่ 0");

/*add method validator check number only*/
window.jQuery.validator.addMethod("checkNagative", function (value) {
    if (value < 0) {
        return false;
    } else {
        return true;
    }
}, "จำนวนเงินต้องไม่เป็นค่าติดลบ");

/*add method validator check text characters only*/
window.jQuery.validator.addMethod("checkText", function (value) {
    var regex = new RegExp("^[ก-๙A-Za-z]+$");

    var str = value;

    if (str == "") return false;

    if (!regex.test(str)) {
        return false;
    } else {
        return true;
    }
}, "กรอกเฉพาะตัวอักษร");

//sweet alert success
const swal_success = (success, cancel) => {
    window.swal({
        title: 'ดำเนินการเรียบร้อย!',
        type: 'success',
        showCancelButton: false,
        showConfirmButton: true,
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
};
//sweet alert fail
const swal_fail = (text, callback) => {
    window.swal({
        title: 'เกิดข้อผิดพลาด!',
        type: 'error',
        text: text,
        showCancelButton: false,
        confirmButtonColor: '#ed2b09',
        confirmButtonText: 'ตกลง',
        closeOnEsc: false
    }, (IsConfirm) => {
        if (IsConfirm) {
            if (typeof callback === "function") {
                //callback
                callback();
            }
        }
    });
};
//sweet alert confirm
const swal_confirm = (title, text, success, cancel) => {
    window.swal({
        title: title,
        text: text,
        type: 'info',
        showCancelButton: true,
        confirmButtonColor: '#277020',
        confirmButtonText: 'ยืนยัน',
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
};

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

//fucntion enter
function handleEnter(field, event) {
    var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
    if (keyCode == 13) {
        var i;
        for (i = 0; i < field.form.elements.length; i++)
            if (field == field.form.elements[i])
                break;
        i = (i + 1) % field.form.elements.length;
        field.form.elements[i].focus();
        return false;
    }
    else

        return true;
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