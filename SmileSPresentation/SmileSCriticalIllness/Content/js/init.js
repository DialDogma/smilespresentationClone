$(window).on('load', function () {
    $("#loading").fadeOut("slow");})

$(function () {
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

    $('input[type="checkbox"], input[type="radio"]').iCheck({
        checkboxClass: "icheckbox_minimal-blue",
        radioClass: "iradio_minimal-blue"
    });

    $("#datemask").inputmask("dd/mm/yyyy", { "placeholder": "dd/mm/yyyy" });
    $("#datemask2").inputmask("mm/dd/yyyy", { "placeholder": "mm/dd/yyyy" });
    $("[data-mask]").inputmask();

    /*--------------DATETIME------------------------------------------------*/
    window.$(".datepicker").datepicker({
        todayHighlight: true,
        autoclose: true
    });
});