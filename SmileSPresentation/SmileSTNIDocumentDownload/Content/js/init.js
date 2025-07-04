$(window).on('load', function () {
    $("#loading").fadeOut("slow");
});

$(function () {
    
    $('#formModal').validate({
        rules:
        {
            healthChildPassCM: { required: true },
            IsSpecify_CM: { required: true },
            healthChildPassOtherRemarkCM: { required: true }
        },
        messages:
        {
            IsSpecify_CM: { required: "<br>กรุณาเลือก..<br/>" },
            healthChildPassCM: { required: "<br>กรุณาเลือก..<br/>" },
            healthChildPassOtherRemarkCM: { required: "<br>กรุณาระบุข้อมูลข้อยกเว้น..<br/>" }
        },
        errorPlacement: function (error, element) {
            if (element.is(":radio")) {
                error.prependTo(element.parents('.answerList'));
            } else if (element.is(":checkbox")) {
                error.prependTo(element.parents('.answerList'));
            }
            else { // This is the default behavior
                error.insertAfter(element);
            }
        }
    });

    $('#formModalUnapproved').validate({
        rules:
        {
            healthCM: { required: true },
            healthChildNotPassCM: { required: true }
        },
        messages:
        {
            healthCM: { required: "<br>กรุณาเลือก..<br/>" },
            healthChildNotPassCM: { required: "<br>กรุณาเลือก..<br/>" }
        },
        errorPlacement: function (error, element) {
            if (element.is(":radio")) {
                error.prependTo(element.parents('.answerList'));
            } else if (element.is(":checkbox")) {
                error.prependTo(element.parents('.answerList'));
            }
            else { // This is the default behavior
                error.insertAfter(element);
            }
        }
    });

    $('form').validate({
        rules:
        {
            underwriteStatus: { required: true },
            typeSelect: { required: true },
            typeSelectChild: { required: true },
            uwPaymentType: { required: true },
            health: { required: true },
            healthChildPass: { required: true },
            healthChildNotPass: { required: true },
            callStatus: { required: true },
            foundCustomer: { required: true },
            IsSpecify: { required: true },
            healthChildPassOtherRemark: { required: true }
        },
        messages:
        {
            underwriteStatus: { required: "<br>กรุณาเลือก..<br/>" },
            typeSelect: { required: "<br>กรุณาเลือก..<br/>" },
            typeSelectChild: { required: "<br>กรุณาเลือก..<br/>" },
            uwPaymentType: { required: "<br>กรุณาเลือก..<br/>" },
            health: { required: "<br>กรุณาเลือก..<br/>" },
            healthChildPass: { required: "<br>กรุณาเลือก..<br/>" },
            healthChildNotPass: { required: "<br>กรุณาเลือก..<br/>" },
            callStatus: { required: "<br>กรุณาเลือก..<br/>" },
            foundCustomer: { required: "<br>กรุณาเลือก..<br/>" },
            IsSpecify: { required: "<br>กรุณาเลือก..<br/>" },
            healthChildPassOtherRemark: { required: "<br>กรุณาระบุข้อมูลข้อยกเว้น..<br/>" }
        },
        errorPlacement: function (error, element) {
            if (element.is(":radio")) {
                error.prependTo(element.parents('.answerList'));
            } else if (element.is(":checkbox")) {
                error.prependTo(element.parents('.answerList'));
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

    /*--------------DATETIME------------------------------------------------*/
    window.$(".datepicker").datepicker({
        todayHighlight: true,
        autoclose: true
    });
    /*********************************valid checkbox & radio**************************************/
    //jQuery.validator.addClassRules({
    //    r1: {
    //        required: true
    //    },
    //    c1: {
    //        required: true
    //    }
    //});
});