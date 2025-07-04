$(window).on('load', function () {
    $("#loading").fadeOut("slow");
});

$(function () {
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
        }, success: function (element) {
            if (element.hasClass('error')) {
                element.parent('div').children('span').children('span').children('span').css('border-color', '');

                //element.css('display', 'none');

                element.remove();
            }
        }
    });
    /*--------------------------------------------------------------------------------*/
    $(".datepicker").datepicker({
        autoclose: true
    });
    /*--------------------------------------------------------------------------------*/
    $('input[type="checkbox"], input[type="radio"]').iCheck({
        checkboxClass: "icheckbox_minimal-blue",
        radioClass: "iradio_minimal-blue"
    });
    /*--------------------------------------------------------------------------------*/
    $("#datemask").inputmask("dd/mm/yyyy", { "placeholder": "dd/mm/yyyy" });
    $("#datemask2").inputmask("mm/dd/yyyy", { "placeholder": "mm/dd/yyyy" });
    $("[data-mask]").inputmask();
    /*--------------------------------------------------------------------------------*/
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
    /*--------------------------------------------------------------------------------*/
    //add method validator check N/A
    jQuery.validator.addMethod("checkNA", function (value, element) {
        if (value == "-1") {
            return false;
        } else {
            return true;
        };
    }, "กรุณาเลือก");
    /*--------------------------------------------------------------------------------*/
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

    jQuery.validator.addMethod("checkPP", function (value, element) {
        $(element).parent().removeClass('has-error');
        $(element).parent().removeClass('has-success');
        if (value == '') {
            return true;
        }
        if (value.length != 9) {
            $(element).parent().addClass('has-error');
            return false;
        }

        $(element).parent().addClass('has-success');
        return true;
    }, "เลขบัตร Passport ไม่ถูกต้อง!");

    $('.select2').select2();
});

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
};

function validateEmail(email) {
    const re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
}