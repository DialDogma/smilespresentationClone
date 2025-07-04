$(function () {
    $(".datepicker").datepicker({
        autoclose: true
    });

    //callStatus
    $('#cancelFormCallStatus').validate({
        rules:
        {
            cancelCallStatus: { required: true },
        },
        messages:
        {
            cancelCallStatus: { required: "<span>กรุณาเลือก..<span />" },
        },
        errorPlacement: function (error, element) {
            if (element.is(":radio")) {
                error.prependTo(element.parents('.msg-error'));
            } else if (element.is(":checkbox")) {
                error.prependTo(element.parents('.msg-error'));
            }
            else { // This is the default behavior
                error.insertAfter(element);
            }
        }
    });

    //มีประเด็นหรือไม่
    $('#cancelFormIssuedQueue').validate({
        rules:
        {
            cancelisIssue: { required: true },
        },
        messages:
        {
            cancelisIssue: { required: "<span>กรุณาเลือก..<span />" },
        },
        errorPlacement: function (error, element) {
            if (element.is(":radio")) {
                error.prependTo(element.parents('.msg-error'));
            } else if (element.is(":checkbox")) {
                error.prependTo(element.parents('.msg-error'));
            }
            else { // This is the default behavior
                error.insertAfter(element);
            }
        }
    });

    //สาเหตุการยกเลิก
    $('#cancelFormOther').validate({
        rules:
        {
            isCancelCause: { required: true },
            cancelisIssue: { required: true },
        },
        messages:
        {
            isCancelCause: { required: "<span>กรุณาเลือก..<span />" },
            cancelisIssue: { required: "<span>กรุณาเลือก..<span />" },
        },
        errorPlacement: function (error, element) {
            if (element.is(":radio")) {
                error.prependTo(element.parents('.msg-error'));
            } else if (element.is(":checkbox")) {
                error.prependTo(element.parents('.msg-error'));
            }
            else { // This is the default behavior
                error.insertAfter(element);
            }
        }
    });

    //บันทึกข้อความ
    $('#cancelFormResultRemark').validate({
        rules:
        {
            txtResultRemark: { required: true },
        },
        messages:
        {
            txtResultRemark: { required: "<span>กรุณากรอกข้อมูล..<span />" },
        },
        errorPlacement: function (error, element) {
            if (element.is(":radio")) {
                error.prependTo(element.parents('.msg-error'));
            } else if (element.is(":checkbox")) {
                error.prependTo(element.parents('.msg-error'));
            }
            else { // This is the default behavior
                error.insertAfter(element);
            }
        }
    });

    $('input[type="checkbox"], input[type="radio"]').iCheck({
        checkboxClass: "icheckbox_minimal-blue",
        radioClass: "iradio_minimal-blue"
    });

    $("#datemask").inputmask("dd/mm/yyyy", { "placeholder": "dd/mm/yyyy" });
    $("#datemask2").inputmask("mm/dd/yyyy", { "placeholder": "mm/dd/yyyy" });
    $("[data-mask]").inputmask();

    // Fix sidebar white space at bottom of page on resize
    $(window).on("load", function () {
        setTimeout(function () {
            $("body").layout("fix");
            $("body").layout("fixSidebar");
        }, 250);
    });
});