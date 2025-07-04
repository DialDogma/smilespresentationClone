'use strict';
/*---colors---*/
window.chartColors = {
    red: 'rgb(255, 99, 132)',
    red2: 'rgb(249, 0, 0)',
    orange: 'rgb(255, 159, 64)',
    yellow: 'rgb(244, 229, 66)',
    green: 'rgb(66, 244, 140)',
    blue: 'rgb(66, 188, 244)',
    purple: 'rgb(159, 132, 249)',
    grey: 'rgb(201, 203, 207)',
    brown: 'rgb(150, 75, 0)'
};
$(window).on('load', function () {
    $("#loading").fadeOut("slow");
});
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
/*********/
function negative(n) {
    return n < 0;
}
/********/
function formatNumber(num) {
    return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
}
/*---random---*/
const randomScalingFactor = () => {
    return Math.round(Math.random() * 100);
};

/*add method validator check null*/
window.jQuery.validator.addMethod("checknull", function (value) {
    if (value === null) {
        return false;
    } else {
        return true;
    }
}, "**กรุณาเลือก");

/*add method validator check NA select*/
window.jQuery.validator.addMethod("checkNA", function (value) {
    if (value === "-1") {
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

window.jQuery.validator.addMethod("checkNagative", function (value) {
    if (value < 0) {
        return false;
    } else {
        return true;
    }
}, "จำนวนเงินต้องไม่เป็นค่าติดลบ");

window.jQuery.validator.addMethod("checkOnlyNum", function (value) {
    var regex = new RegExp("^[0-9]+$");

    if (!regex.test(value)) {
        return false;
    } else {
        return true;
    }
}, "กรอกเฉพาะตัวเลขเท่านั้น(0-9)");

//add method validator Date format dd/mm/yyyy or dd-mm-yyyy or dd.mm.yyyy By BOOM
window.jQuery.validator.addMethod("dateTH", function (value) {
    var reg = /^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/;
    return value.match(reg);
}, "กรุณาตรวจสอบรูปแบบวันที่ - วัน/เดือน/ปี");

//--------------------------------------ALERT MESSAGE--------------------------------------------------//
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
        closeOnCancel: true,
        disableButtonsOnConfirm: true
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

const swal_confirm2 = (title, text, success, cancel) => {
    window.swal({
        title: title,
        text: text,
        type: 'warning',
        showCancelButton: true,
        confirmButtonText: 'ยืนยัน',
        cancelButtonText: 'ยกเลิก',
        closeOnConfirm: false,
        closeOnEsc: false,
        closeOnCancel: true,
        disableButtonsOnConfirm: true
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

const swal_success = (success, cancel) => {
    window.swal({
        title: 'ดำเนินการเรียบร้อย!',
        type: 'success',
        showCancelButton: false,
        showConfirmButton: false,
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

const swal_fail = (text, callback) => {
    window.swal({
        title: "เกิดข้อผิดพลาด",
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

const swal_info = (text) => {
    window.swal({
        title: 'แจ้งเตือน!',
        type: 'warning',
        text: text,
        showCancelButton: false,
        confirmButtonColor: '#7cd1f9',
        confirmButtonText: 'ตกลง',
        closeOnEsc: false
    });
};

/*--------------------------------------------------------------*/
window.jQuery.extend(window.jQuery.validator.messages, {
    required: "กรุณากรอกข้อมูลช่องนี้",
    remote: "Please fix this field.",
    email: "กรุณากรอก Email ให้ถูกต้อง",
    url: "Please enter a valid URL.",
    date: "Please enter a valid date.",
    dateISO: "Please enter a valid date (ISO).",
    number: "กรุณากรอกข้อมูลตัวเลขเท่านั้น",
    digits: "Please enter only digits.",
    creditcard: "Please enter a valid credit card number.",
    equalTo: "Please enter the same value again.",
    accept: "Please enter a value with a valid extension.",
    maxlength: window.jQuery.validator.format("Please enter no more than {0} characters."),
    minlength: window.jQuery.validator.format("Please enter at least {0} characters."),
    rangelength: window.jQuery.validator.format("Please enter a value between {0} and {1} characters long."),
    range: window.jQuery.validator.format("Please enter a value between {0} and {1}."),
    max: window.jQuery.validator.format("Please enter a value less than or equal to {0}."),
    min: window.jQuery.validator.format("Please enter a value greater than or equal to {0}.")
});
/*--------------------------------------------------------------*/