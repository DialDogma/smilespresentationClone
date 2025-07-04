$(function () {
    $('input[type="checkbox"], input[type="radio"]').iCheck({
        checkboxClass: "icheckbox_minimal-blue",
        radioClass: "iradio_minimal-blue"
    });

    $("#datemask").inputmask("dd/mm/yyyy", { "placeholder": "dd/mm/yyyy" });
    $("#datemask2").inputmask("mm/dd/yyyy", { "placeholder": "mm/dd/yyyy" });
    $("[data-mask]").inputmask();

    // Fix sidebar white space at bottom of page on resize
    $(window).on("load", function () {
        $("#loading").fadeOut("slow");

        setTimeout(function () {
            $("body").layout("fix");
            $("body").layout("fixSidebar");
        }, 250);
    });

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

    $('form').validate();

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
        //min: window.jQuery.validator.format("Please enter a value greater than or equal to {0}.")
        min: window.jQuery.validator.format("เบอร์โทรศัพท์ไม่ถูกต้อง")
    });
});

//----------------------------------------------------//
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

const swal_confirm2 = (title, text, success, cancel) => {
    window.swal({
        title: title,
        text: text,
        type: 'success',
        showCancelButton: true,
        confirmButtonText: 'ตกลง',
        cancelButtonText: 'ยกเลิก',
        showCancelButton: false,
        closeOnConfirm: false,
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

const swal_confirm = (title, text, success, cancel) => {
    window.swal({
        title: title,
        text: text,
        type: 'info',
        showCancelButton: true,
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

const swal_confirm_input = (title, text, callback) => {
    window.swal({
        title: title,
        text: text,
        type: 'input',
        confirmButtonText: 'ยืนยัน',
        cancelButtonText: 'ยกเลิก',
        showCancelButton: true,
        closeOnConfirm: false,
        animation: "slide-from-top"
    }, function (inputValue) {
        callback(inputValue);
    });
};

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