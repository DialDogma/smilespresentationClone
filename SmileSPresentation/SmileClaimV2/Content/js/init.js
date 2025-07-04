$(window).on('load', function () {
    if (!msieversion()) {
        $('#loading').fadeOut('slow');
    }
});

$(function () {
    /*--------------------------------------------------------------*/
    window.$("form").validate({
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
    /*--------------DATETIME------------------------------------------------*/
    window.$('.datepicker').datepicker({
        autoclose: true,
        todayBtn: "linked",
    });
    /*--------------------------------------------------------------*/
    window.$('input[type="checkbox"], input[type="radio"]').iCheck({
        checkboxClass: "icheckbox_minimal-blue",
        radioClass: "iradio_minimal-blue"
        /*--------------------------------------------------------------*/
    });

    window.$("#datemask").inputmask("dd/mm/yyyy", { "placeholder": "dd/mm/yyyy" });
    window.$("#datemask2").inputmask("mm/dd/yyyy", { "placeholder": "mm/dd/yyyy" });
    window.$("[data-mask]").inputmask();
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
        maxlength: window.jQuery.validator.format("โปรดป้อนข้อมูลความยาวไม่เกิน  {0} ตัวอักษร."),
        minlength: window.jQuery.validator.format("โปรดป้อนข้อมูลความยาวไม่น้อยกว่า {0} ตัวอักษร."),
        rangelength: window.jQuery.validator.format("Please enter a value between {0} and {1} characters long."),
        range: window.jQuery.validator.format("Please enter a value between {0} and {1}."),
        max: window.jQuery.validator.format("Please enter a value less than or equal to {0}."),
        min: window.jQuery.validator.format("Please enter a value greater than or equal to {0}.")
    });
    /*--------------------------------------------------------------*/
    //add method validator Date format dd/mm/yyyy or dd-mm-yyyy or dd.mm.yyyy By BOOM
    window.jQuery.validator.addMethod("checkCharThEnOnly", function (value) {
        var regex = new RegExp("^[ ก-๙A-Za-z]+$");
        var str = value;

        if (str == "") return false;

        if (!regex.test(str)) {
            return false;
        } else {
            return true;
        }
    }, "กรุณากรอกข้อมูลตัวอักษรไทยหรืออังกฤษเท่านั้น");

    //add method validator check N/A
    window.jQuery.validator.addMethod("checkNA", function (value) {
        if (value == "-1") {
            return false;
        } else {
            return true;
        }
    }, "กรุณาเลือก");

    //add method validator check N/A
    window.jQuery.validator.addMethod("checkStrEmpty", function (value) {
        if (value.trim() === "") {
            return false;
        } else {
            return true;
        }
    }, "กรุณากรอกข้อมูลช่องนี้");
    /*--------------------------------------------------------------*/
    //add method validator Date format dd/mm/yyyy or dd-mm-yyyy or dd.mm.yyyy By BOOM
    window.jQuery.validator.addMethod("dateTH", function (value) {
        var reg = /^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/;
        return value.match(reg);
    }, "กรุณาตรวจสอบรูปแบบวันที่ - วัน/เดือน/ปี");
    /*--------------------------------------------------------------*/
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
});
/*--------------------------------------------------------------*/
//Display date from json (Buddhist era)
function DisplayJsonDateBE(data) {
    var result = "";
    if (data !== null) {
        var d = window.moment(data).toDate();
        var month = ("0" + (d.getMonth() + 1)).slice(-2);
        var day = ("0" + d.getDate()).slice(-2);
        var year = d.getFullYear() + 543;
        result = day + '/' + month + '/' + year;
    }
    return result;
};

function swal_info(title, text, buttonText) {
    window.swal({
        title: title,
        type: 'warning',
        text: text,
        showCancelButton: false,
        confirmButtonColor: '#23a3f1',
        confirmButtonText: buttonText,
        closeOnEsc: false
    });
};

function msieversion() {
    var ua = window.navigator.userAgent;
    var msie = ua.indexOf("MSIE ");

    if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))  // If Internet Explorer, return version number
    {
        alert("เว็บไซต์ไม่รองรับเบราว์เซอร์ - Internet Explorer " + parseInt(ua.substring(msie + 5, ua.indexOf(".", msie))) + "เพื่อการใช้งานที่เต็มที่ของเว็บไซต์นี้ แนะนำใช้งานเบราว์เซอร์ Google Chrome,Mozilla Firefox,Microsoft Edge");
        return true;
    }
    //else  // If another browser, return 0
    //{
    //    alert('otherbrowser');
    //}

    return false;
}