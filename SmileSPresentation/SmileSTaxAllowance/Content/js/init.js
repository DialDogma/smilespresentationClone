﻿$(function () {
    //$(".datepicker").datepicker({
    //    autoclose: true
    //});

    $('input[type="checkbox"], input[type="radio"]').iCheck({
        checkboxClass: "icheckbox_minimal-blue",
        radioClass: "iradio_minimal-blue"
    });

    //$("#datemask").inputmask("dd/mm/yyyy", { "placeholder": "dd/mm/yyyy" });
    //$("#datemask2").inputmask("mm/dd/yyyy", { "placeholder": "mm/dd/yyyy" });
    //$("[data-mask]").inputmask();

    //add method validator check N/A
    jQuery.validator.addMethod("checkNA", function (value, element) {
        if (value == "-1") {
            return false;
        } else {
            return true;
        };
    }, "กรุณาเลือก");

    //jquery validation check identity ID Card
    jQuery.validator.addMethod("checkID", function (value, element) {
        $(element).parent().removeClass('form-group has-error');
        $(element).parent().removeClass('form-group has-success');
        if (value == '') {
            return true;
        }
        if (value.length != 13) {
            $(element).parent().addClass('form-group has-error');
            return false;
        }
        var sum = 0;
        for (var i = 0; i < 12; i++) {
            sum += parseFloat(value.charAt(i)) * (13 - i);
        }
        if ((11 - (sum % 11)) % 10 != parseFloat(value.charAt(12))) {
            $(element).parent().addClass('form-group has-error');
            return false;
        }
        $(element).parent().addClass('form-group has-success');
        return true;
    }, "เลขบัตรประจำตัวประชาชนไม่ถูกต้อง!");
});


//Display date from json (Buddhist era)
function DisplayJsonDateBE(data) {
    var result = "";
    if (data != null) {
        var d = moment(data).toDate();
        var month = ("0" + (d.getMonth() + 1)).slice(-2);
        var day = ("0" + d.getDate()).slice(-2);
        var year = d.getFullYear() + 543;
        result = day + '/' + month + '/' + year;
    }
    return result;
};