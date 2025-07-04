$(window).on('load', function () {
    $('#loading').fadeOut('slow');
});

$(function () {
    /*--------------------------------------------------------------*/
    $("form").validate({
        errorPlacement: function (error, element) {
            if (element.hasClass('select2')) {
                error.insertAfter(element.next('span'));
                element.parent('div').children('span').children('span').children('span').css('border-color', '#dd4b39');
            }
            else if (element.parent('div').hasClass('input-group')) {
                //debugger;
                //error.appendTo('#error-div');
                error.appendTo(element.parent('div').parent('div'));
            }
            else {
                error.insertAfter(element);
            }
        }//, success: function (element) {
        //    if (element.hasClass('select2')) {
        //        element.parent('div').children('span').children('span').children('span').css('border-color', '');
        //    }
        //}
    });
    /*--------------------------------------------------------------*/
    $(".datepicker").datepicker({
        autoclose: true,
        todayBtn: "linked"
    });
    /*--------------------------------------------------------------*/
    $(".timepicker").timepicker({
        showInputs: false,
        minuteStep: 1,
        secondStep: 1,
        modalBackdrop: true,
        defaulttime: 'current',
        showSeconds: true,
        showMeridian: false, //false = 24,true=12
        explicitMode: true
    });
    /*--------------------------------------------------------------*/
    $('input[type="checkbox"], input[type="radio"]').iCheck({
        checkboxClass: "icheckbox_minimal-blue",
        radioClass: "iradio_minimal-blue"
        /*--------------------------------------------------------------*/
    });

    $("#datemask").inputmask("dd/mm/yyyy", { "placeholder": "dd/mm/yyyy" });
    $("#datemask2").inputmask("mm/dd/yyyy", { "placeholder": "mm/dd/yyyy" });
    $("[data-mask]").inputmask();
    /*--------------------------------------------------------------*/
    jQuery.extend(jQuery.validator.messages, {
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
        maxlength: jQuery.validator.format("Please enter no more than {0} characters."),
        minlength: jQuery.validator.format("Please enter at least {0} characters."),
        rangelength: jQuery.validator.format("Please enter a value between {0} and {1} characters long."),
        range: jQuery.validator.format("Please enter a value between {0} and {1}."),
        max: jQuery.validator.format("Please enter a value less than or equal to {0}."),
        min: jQuery.validator.format("Please enter a value greater than or equal to {0}.")
    });
    /*--------------------------------------------------------------*/
    //add method validator check N/A
    jQuery.validator.addMethod("checkNA", function (value, element) {
        if (value == "-1") {
            return false;
        } else {
            return true;
        };
    }, "กรุณาเลือก");
    /*--------------------------------------------------------------*/
    //jquery validation check identity ID Card
    jQuery.validator.addMethod("checkID", function (value, element) {
        $(element).parent().removeClass('has-error');
        $(element).parent().removeClass('has-success');
        if (value == '') {
            return true;
        }
        if (value.length != 13) {
            $(element).parent().addClass('has-error');
            return false;
        }
        var sum = 0;
        for (var i = 0; i < 12; i++) {
            sum += parseFloat(value.charAt(i)) * (13 - i);
        }
        if ((11 - (sum % 11)) % 10 != parseFloat(value.charAt(12))) {
            $(element).parent().addClass('has-error');
            return false;
        }
        $(element).parent().addClass('has-success');
        return true;
    }, "เลขบัตรประจำตัวประชาชนไม่ถูกต้อง!");
});
/*--------------------------------------------------------------*/
//Display date from json (Buddhist era)
function DisplayJsonDateBE(data) {
    var result = "";
    if (data !== null) {
        var d = moment(data).toDate();
        var month = ("0" + (d.getMonth() + 1)).slice(-2);
        var day = ("0" + d.getDate()).slice(-2);
        var year = d.getFullYear() + 543;
        result = day + '/' + month + '/' + year;
    }
    return result;
};

//----------------------------------------Check Zero-------------------------------------------------//
function check_zero(value, callback) {
    if (value == "0") {
        callback();
    }
}
//--------------------------------------ALERT MESSAGE--------------------------------------------------//
function swal_confirm(title, text, success, cancel) {
    swal({
        title: title,
        text: text,
        type: 'info',
        showCancelButton: true,
        confirmButtonColor: '#277020',
        confirmButtonText: 'Confirm',
        cancelButtonText: 'Cancel',
        closeOnConfirm: false,
        closeOnEsc: false,
        closeOnCancel: true
    }, function (IsConfirm) {
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

function swal_success(success, cancel) {
    window.swal({
        title: 'ดำเนินการเรียบร้อย!',
        type: 'success',
        showCancelButton: false,
        confirmButtonColor: '#26A65B',
        confirmButtonText: 'ตกลง',
        closeOnEsc: false
    }, function (IsConfirm) {
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

function swal_fail(text, callback) {
    window.swal({
        title: 'เกิดข้อผิดพลาด!',
        type: 'error',
        text: text,
        showCancelButton: false,
        confirmButtonColor: '#ed2b09',
        confirmButtonText: 'ตกลง',
        closeOnEsc: false
    }, function () {
        if (typeof callback === "function") {
            //callback
            callback();
        }
    });
}