'use strict';
/*---colors---*/
window.chartColors = {
    red: 'rgb(255, 99, 132)',
    orange: 'rgb(255, 159, 64)',
    yellow: 'rgb(255, 205, 86)',
    green: 'rgb(75, 192, 192)',
    blue: 'rgb(54, 162, 235)',
    purple: 'rgb(153, 102, 255)',
    grey: 'rgb(201, 203, 207)'
};

//jquery validation check identity ID Card
window.jQuery.validator.addMethod("checkID", function (value, element) {
    window.$(element).parent().removeClass('has-error');
    window.$(element).parent().removeClass('has-success');
    if (value == '') {
        return true;
    }
    if (value.length != 13) {
        window.$(element).parent().addClass('has-error');
        return false;
    }
    var sum = 0;
    for (var i = 0; i < 12; i++) {
        sum += parseFloat(value.charAt(i)) * (13 - i);
    }
    if ((11 - (sum % 11)) % 10 != parseFloat(value.charAt(12))) {
        window.$(element).parent().addClass('has-error');
        return false;
    }
    window.$(element).parent().addClass('has-success');
    return true;
}, "เลขบัตรประจำตัวประชาชนไม่ถูกต้อง!");

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
}, "กรุณาเลือก");
/*add method validator check -1*/
window.jQuery.validator.addMethod("checkNA", function (value) {
    if (value === "-1") {
        return false;
    } else {
        return true;
    }
}, "กรุณาเลือก");
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

const swal_success = (title, text, success, cancel) => {
    window.swal({
        title: title,
        type: 'success',
        text: text,
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
    }, (IsConfirm) => {
        if (IsConfirm) {
            if (typeof callback === "function") {
                //callback
                callback();
            }
        }
    });
}

const swal_info = (text) => {
    window.swal({
        title: 'info!',
        type: 'warning',
        text: text,
        showCancelButton: false,
        confirmButtonColor: '#7cd1f9',
        confirmButtonText: 'ตกลง',
        closeOnEsc: false
    });
}

/*--------------------------------------------------------------*/
window.jQuery.extend(window.jQuery.validator.messages, {
    required: "กรุณากรอกข้อมูลช่องนี้",
    remote: "Please fix this field.",
    email: "กรุณากรอก Email ให้ถูกต้อง",
    url: "Please enter a valid URL.",
    date: "Please enter a valid date.",
    dateISO: "Please enter a valid date (ISO).",
    number: "Please enter a valid number.",
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