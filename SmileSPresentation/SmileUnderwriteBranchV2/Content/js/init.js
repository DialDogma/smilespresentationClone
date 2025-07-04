$(function () {



    $("#sectionGiveCardFrom").validate({
        rules:
        {
            bewareRemark: { required: true }
        },
        messages:
        {
            bewareRemark: {
                required: "กรุณาเลือก.."
            },
        },
        errorPlacement: function (error, element) {
            debugger;
            if (element.is(":radio")) {
                error.prependTo(element.parents('.answerList'));
            } else if (element.is(":checkbox")) {
                error.prependTo(element.parents('.answerList'));
            } else if (element.is(".healthChildPassOtherRemark")) {
                error.prependTo(element.parents('.answerList'));
            }
            else { // This is the default behavior
                error.insertAfter(element);
            }
        }
    });

    //cancer
    $('#formQuestionCancer').validate({
        rules:
        {
            underwriteStatus: { required: true },
            typeSelect: { required: true },
            typeSelectChild: { required: true },
            uwPaymentType: { required: true },
            cancer: { required: true },
            cancerChildPass: { required: true },
            cancerChildNotPass: { required: true },
            IsSpecify: { required: true },
            cancerChildNotPassOtherRemark: { required: true },
            cancerChildPassOtherRemark: { required: true },
            txtCause: { required: true },
            callStatus: { required: true },
            foundCustomer: { required: true },
        },
        messages:
        {
            underwriteStatus: { required: "<span>กรุณาเลือก..<span />" },
            typeSelect: { required: "<span>กรุณาเลือก..<span />" },
            typeSelectChild: { required: "<span>กรุณาเลือก..<span />" },
            uwPaymentType: { required: "<span>กรุณาเลือก..<span />" },
            cancer: { required: "<span>กรุณาเลือก..<span />" },
            cancerChildPass: { required: "<span>กรุณาเลือก..<span />" },
            cancerChildNotPass: { required: "<span>กรุณาเลือก..<span />" },
            IsSpecify: { required: "<span>กรุณาเลือก..<span />" },
            cancerChildPassOtherRemark: { required: "<span>กรุณาระบุข้อมูลติดเงื่อนไข..<span />" },
            cancerChildNotPassOtherRemark: { required: "<span>อื่นๆ โปรดระบุ..<span />" },
            txtCause: { required: "<span>โปรดระบุ..<span />" },
            callStatus: { required: "<span>กรุณาเลือก..<span/>" },
            foundCustomer: { required: "<span>กรุณาเลือก..<span/>" },
        },
        errorPlacement: function (error, element) {
            debugger
            if (element.is(":radio")) {
                if ((element.is(":checkbox")) && element.is(".answerList-Child")) {
                    error.prependTo(element.parents('.answerList-Child'));
                } else if (element.is(":radio") && element.is(".answerList-Child")) {
                    error.prependTo(element.parents('.answerList-Child'));
                } else if (element.is(":radio") && element.is(".answerList-Child-Child")) {
                    error.prependTo(element.parents('.answerList-Child-Child'));
                } else {
                    error.prependTo(element.parents('.answerList'));
                }
            } else if ((element.is(":checkbox") || element.is(":radio")) && element.is("#cancerChildPassOtherRemark")) {
                error.prependTo(element.parents('.answerList-Child-Child'));
            } else if (element.is(".answerList-Child2")) {
                error.prependTo(element.parents('.answerList-Child2'));
            } else if (element.is(".answerList-Child1")) {
                error.prependTo(element.parents('.answerList-Child1'));
            } else if (element.is(".answerList-Child")) {
                error.prependTo(element.parents('.answerList-Child'));
            } else if (element.is(":checkbox") && element.is(".answerList")) {
                error.prependTo(element.parents('.answerList'));
            }
            else {
                error.insertAfter(element);// This is the default behavior
            }
        }
    });

    $('#formModalApprovedConditionCancer').validate({
        rules:
        {
            cancerChildPassCM: { required: true },
            IsSpecify_CM: { required: true },
            cancerChildPassOtherRemarkCM: { required: true }
        },
        messages:
        {
            IsSpecify_CM: { required: "<span>กรุณาเลือก..<span/>" },
            cancerChildPassCM: { required: "<span>กรุณาเลือก..<span/>" },
            cancerChildPassOtherRemarkCM: { required: "<span>กรุณาระบุข้อมูลข้อยกเว้น..<span/>" }
        },
        errorPlacement: function (error, element) {
            if (element.is(":radio")) {
                error.prependTo(element.parents('.answerList'));
            } else if (element.is(":checkbox")) {
                error.prependTo(element.parents('.answerList'));
            } else if (element.is("#cancerChildNotPassOtherRemarkCM")) {
                error.prependTo(element.parents('.box-cancer-notpass'));
            } else { // This is the default behavior
                error.insertAfter(element);
            }
        }
    });

    $('#formModalUnapprovedCancer').validate({
        rules:
        {
            cancerCM: { required: true },
            cancerChildNotPassCM: { required: true },
            cancerChildNotPassOtherRemarkCM: { required: true }
        },
        messages:
        {
            cancerCM: { required: "<span>กรุณาเลือก..<span/>" },
            cancerChildNotPassCM: { required: "<span>กรุณาเลือก..<span/>" },
            cancerChildNotPassOtherRemarkCM: { required: "<span>อื่นๆ โปรดระบุ..<span/>" },
        },
        errorPlacement: function (error, element) {
            if (element.is(":radio")) {
                error.prependTo(element.parents('.answerList'));
            } else if (element.is(":checkbox")) {
                error.prependTo(element.parents('.answerList-child'));
            } else if (element.is("#cancerChildNotPassOtherRemarkCM")) {
                error.insertAfter(element);
            } else { // This is the default behavior
                error.insertAfter(element);
            }
        }
    });
    //end cancer

    //motor
    $('#motorFormQuestionUpdate').validate({

        rules:
        {
            motorUnderwriteStatus: { required: true },
            typeSelect: { required: true },
            typeSelectChild: { required: true },
            typeSelectChildCall: { required: true },
            paymentTypeMotor: { required: true },
            motor: { required: true },
            motorChildPass: { required: true },
            motorChildNotPass: { required: true },
            IsSpecify: { required: true },
            motorChildNotPassOtherRemark: { required: true },
            motorChildPassOtherRemark: { required: true },
            txtCause: { required: true },
            callStatus: { required: true },
            foundCustomer: { required: true },
        },
        messages:
        {
            motorUnderwriteStatus: { required: "<span>กรุณาเลือก..</span>" },
            typeSelect: { required: "<span>กรุณาเลือก..</span>" },
            typeSelectChild: { required: "<span>กรุณาเลือก..</span>" },
            typeSelectChildCall: { required: "<span>กรุณาเลือก..</span>" },
            paymentTypeMotor: { required: "<span>กรุณาเลือก..</span>" },
            motor: { required: "<span>กรุณาเลือก..</span>" },
            motorChildPass: { required: "<span>กรุณาเลือก..</span>" },
            motorChildNotPass: { required: "<span>กรุณาเลือก..</span>" },
            IsSpecify: { required: "<span>กรุณาเลือก..</span>" },
            motorChildNotPassOtherRemark: { required: "<span>อื่นๆ โปรดระบุ..</span>" },
            motorChildPassOtherRemark: { required: "<span>กรุณาระบุข้อมูลติดเงื่อนไข..</span>" },
            txtCause: { required: "<span>กรุณาเลือก..</span>" },
            callStatus: { required: "<span>กรุณาเลือก..</span>" },
            foundCustomer: { required: "<span>กรุณาเลือก..</span>" },
        },
        errorPlacement: function (error, element) {
            if (element.is(":radio")) {
                if ((element.is(":checkbox")) && element.is(".answerList-Child")) {
                    debugger
                    error.prependTo(element.parents('.answerList-Child'));
                } else if (element.is(":radio") && element.is(".answerList-Child")) {
                    error.prependTo(element.parents('.answerList-Child'));
                } else if (element.is(":radio") && element.is(".answerList-Child-Child")) {
                    error.prependTo(element.parents('.answerList-Child-Child'));
                } else {
                    error.prependTo(element.parents('.answerList'));
                }
            } else if ((element.is(":checkbox") || element.is(":radio")) && element.is("#motorChildPassOtherRemark")) {
                error.prependTo(element.parents('.answerList-Child-Child'));
            } else if (element.is(".answerList-Child2")) {
                error.prependTo(element.parents('.answerList-Child2'));
            } else if (element.is(".answerList-Child1")) {
                error.prependTo(element.parents('.answerList-Child1'));
            } else if (element.is(".answerList-Child")) {
                error.prependTo(element.parents('.answerList-Child'));
            } else if (element.is(":checkbox") && element.is(".answerList")) {
                error.prependTo(element.parents('.answerList'));
            }
            else {
                error.insertAfter(element);// This is the default behavior
            }
        }
    });

    $('#formModalUnapprovedMotor').validate({
        rules:
        {
            motorCM: { required: true },
            motorChildNotPassCM: { required: true },
            motorChildNotPassOtherRemarkCM: { required: true }
        },
        messages:
        {
            motorCM: { required: "<span>กรุณาเลือก..<span/>" },
            motorChildNotPassCM: { required: "<span>กรุณาเลือก..<span/>" },
            motorChildNotPassOtherRemarkCM: { required: "<span>อื่นๆ โปรดระบุ..<span/>" },
        },
        errorPlacement: function (error, element) {
            if (element.is(":radio")) {
                error.prependTo(element.parents('.answerList'));
            } else if (element.is(":checkbox")) {
                error.prependTo(element.parents('.box-motor-notpass'));
            } else if (element.is("#motorChildNotPassOtherRemarkCM")) {
                error.prependTo(element.parents('.box-motor-notpass'));
            } else { // This is the default behavior
                error.insertAfter(element);
            }
        }
    });

    $('#formModalApprovedConditionMotor').validate({
        rules:
        {
            motorChildPassCM: { required: true },
            IsSpecify_CM: { required: true },
            motorChildPassOtherRemarkCM: { required: true }
        },
        messages:
        {
            IsSpecify_CM: { required: "<span>กรุณาเลือก..<span/>" },
            motorChildPassCM: { required: "<span>กรุณาเลือก..<span/>" },
            motorChildPassOtherRemarkCM: { required: "<span>กรุณาระบุข้อมูลข้อยกเว้น..<span/>" }
        },
        errorPlacement: function (error, element) {
            if (element.is(":radio")) {
                error.prependTo(element.parents('.answerList'));
            } else if (element.is(":checkbox")) {
                error.prependTo(element.parents('.answerList'));
            } else if (element.is("#motorChildNotPassOtherRemarkCM")) {
                //error.prependTo(element.parents('.box-motor-notpass'));
                error.insertAfter(element);
            } else { // This is the default behavior
                error.insertAfter(element);
            }
        }
    });
    //end motro

    $(".datepicker").datepicker({
        autoclose: true
    });

    $('#formModalApprovedCondition').validate({
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

    $('#formQuestion').validate({
        rules:
        {
            underwriteStatus: { required: true },
            typeSelect: { required: true },
            typeSelectChild: { required: true },
            uwPaymentType: { required: true },
            health: { required: true },
            healthChildPass: { required: true },
            healthChildNotPass: { required: true },
            IsSpecify: { required: true },
            healthChildPassOtherRemark: { required: true },
            healthChildNotPassOtherRemark: { required: true },
            txtCause: { required: true },
            callStatus: { required: true },
            foundCustomer: { required: true },
        },
        messages:
        {
            underwriteStatus: { required: "<br>กรุณาเลือก..<br />" },
            typeSelect: { required: "<br>กรุณาเลือก..<br />" },
            typeSelectChild: { required: "<br>กรุณาเลือก..<br />" },
            uwPaymentType: { required: "<br>กรุณาเลือก..<br />" },
            health: { required: "<br>กรุณาเลือก..<br />" },
            healthChildPass: { required: "<br>กรุณาเลือก..<br />" },
            healthChildNotPass: { required: "<br>กรุณาเลือก..<br />" },
            IsSpecify: { required: "<br>กรุณาเลือก..<br />" },
            healthChildPassOtherRemark: { required: "<br>กรุณาระบุข้อมูลข้อยกเว้น..<br />" },
            healthChildNotPassOtherRemark: { required: "<br>อื่นๆ โปรดระบุ..<br />" },
            txtCause: { required: "<br>โปรดระบุ..<br />" },
            callStatus: { required: "<br>กรุณาเลือก..<br/>" },
            foundCustomer: { required: "<br>กรุณาเลือก..<br/>" },
        },
        errorPlacement: function (error, element) {
            if (element.is(":radio")) {
                error.prependTo(element.parents('.answerList'));
            } else if (element.is(":checkbox")) {
                error.prependTo(element.parents('.answerList'));
            } else if (element.is(".healthChildPassOtherRemark")) {
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

    // Fix sidebar white space at bottom of page on resize
    $(window).on("load", function () {
        setTimeout(function () {
            $("body").layout("fix");
            $("body").layout("fixSidebar");
        }, 250);
    });
});

