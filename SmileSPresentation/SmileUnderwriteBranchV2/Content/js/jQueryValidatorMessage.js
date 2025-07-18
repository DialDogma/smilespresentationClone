﻿$(function () {
    jQuery.extend(jQuery.validator.messages, {
       // required: "กรุณากรอกข้อมูลช่องนี้",
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
        maxlength: jQuery.validator.format("กรุณากรอกข้อมูลไม่เกิน {0} ตัวอักษร."),
        minlength: jQuery.validator.format("กรุณากรอกข้อมูลอย่างน้อย {0} ตัวอักษร."),
        rangelength: jQuery.validator.format("Please enter a value between {0} and {1} characters long."),
        range: jQuery.validator.format("Please enter a value between {0} and {1}."),
        max: jQuery.validator.format("Please enter a value less than or equal to {0}."),
        min: jQuery.validator.format("Please enter a value greater than or equal to {0}.")
    });

    $("form").validate();
});