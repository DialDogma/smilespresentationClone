$(function () {
    $(".datepicker").datepicker({
        autoclose: true
    });

    //Cancer
    $('#formCancerQuestion').validate({
        rules:
        {
            QueueAuditStatus: { required: true },
            CallStatus: { required: true },
            AuditPaymentCancerStatus: { required: true },
            AuditUnderwriteStatus: { required: true },
            AuditPoliciesGivenStatus: { required: true },
            AuditCancerStatus: { required: true },
            //AuditUnderwriteResultStatusRemark: { required: true },
            AuditCancerSpecifyStatus: { required: true },
            //AuditPaymentCancerRemark: { required: true },
            AuditCancerSpecifyRemark: { required: true },
        },
        messages:
        {
            QueueAuditStatus: { required: "<span>กรุณาเลือก..<span />" },
            CallStatus: { required: "<span>กรุณาเลือก..<span />" },
            AuditPaymentCancerStatus: { required: "<span>กรุณาเลือก..<span />" },
            AuditUnderwriteStatus: { required: "<span>กรุณาเลือก..<span />" },
            AuditPoliciesGivenStatus: { required: "<span>กรุณาเลือก..<span />" },
            AuditUnderwriteResultStatusRemark: { required: "<span>กรุณาระบุ..<span />" },
            AuditCancerStatus: { required: "<span>กรุณาเลือก..<span />" },
            AuditCancerRemark: { required: "<span>กรุณาระบุ..<span />" },
            AuditPaymentCancerRemark: { required: "<span>กรุณาระบุ..<span />" },
            AuditCancerSpecifyStatus: { required: "<span>กรุณาเลือก..<span />" },
            AuditCancerSpecifyRemark: { required: "<span>กรุณาระบุ..<span />" },
        },
        errorPlacement: function (error, element) {
            if (element.is(":radio")) {
                if ((element.is(":radio")) && element.is(".answerList-Child")) {
                    error.prependTo(element.parents('.answerList-Child'));
                } else {
                    error.prependTo(element.parents('.answerList'));
                }
            } else if (element.is(":checkbox")) {
                error.prependTo(element.parents('.answerList'));
            }
            else { // This is the default behavior
                error.insertAfter(element);
            }
        }
    });

    //Motor
    $('#formQuestionBPT').validate({
        rules:
        {
            QueueAuditStatus: { required: true },
            CallStatus: { required: true },
            AuditPaymentMotorStatus: { required: true },
            AuditUnderwriteStatus: { required: true },
            AuditPoliciesGivenStatus: { required: true },
            AuditMotorStatus: { required: true },
            //AuditUnderwriteResultStatusRemark: { required: true },
            AuditMotorSpecifyStatus: { required: true },
            //AuditPaymentMotorRemark: { required: true },
            AuditMotorSpecifyRemark: { required: true },
        },
        messages:
        {
            QueueAuditStatus: { required: "<span>กรุณาเลือก..<span />" },
            CallStatus: { required: "<span>กรุณาเลือก..<span />" },
            AuditPaymentMotorStatus: { required: "<span>กรุณาเลือก..<span />" },
            AuditUnderwriteStatus: { required: "<span>กรุณาเลือก..<span />" },
            AuditPoliciesGivenStatus: { required: "<span>กรุณาเลือก..<span />" },
            AuditUnderwriteResultStatusRemark: { required: "<span>กรุณาระบุ..<span />" },
            AuditMotorStatus: { required: "<span>กรุณาเลือก..<span />" },
            AuditMotorRemark: { required: "<span>กรุณาระบุ..<span />" },
            AuditPaymentMotorRemark: { required: "<span>กรุณาระบุ..<span />" },
            AuditMotorSpecifyStatus: { required: "<span>กรุณาเลือก..<span />" },
            AuditMotorSpecifyRemark: { required: "<span>กรุณาระบุ..<span />" },
        },
        errorPlacement: function (error, element) {
            if (element.is(":radio")) {

                if ((element.is(":radio")) && element.is(".answerList-Child")) {
                    error.prependTo(element.parents('.answerList-Child'));
                } else {
                    error.prependTo(element.parents('.answerList'));
                }
            } else if (element.is(":checkbox")) {
                error.prependTo(element.parents('.answerList'));
            }
            else { // This is the default behavior
                error.insertAfter(element);
            }
        }
    });

    $('#formAuditInsure').validate({
        rules:
        {
            AuditInsureStatus: { required: true },
            AuditInsureStatusRemark: { required: true },
            AuditInsureConsiderStatus: { required: true },
            AuditInsureConsiderStatusRemark: { required: true },
        },
        messages:
        {
            AuditInsureStatus: { required: "<span>กรุณาเลือก..<span />" },
            AuditInsureStatusRemark: { required: "<span>กรุณาระบุ..<span />" },
            AuditInsureConsiderStatus: { required: "<span>กรุณาเลือก..<span />" },
            AuditInsureConsiderStatusRemark: { required: "<span>กรุณาระบุ..<span />" },
        },
        errorPlacement: function (error, element) {
            if (element.is(":radio")) {
                if ((element.is(":radio")) && element.is(".answerList-Child")) {
                    error.prependTo(element.parents('.answerList-Child'));
                } else {
                    error.prependTo(element.parents('.answerList'));
                }
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

    // Fix sidebar white space at bottom of page on resize
    $(window).on("load", function () {
        setTimeout(function () {
            $("body").layout("fix");
            $("body").layout("fixSidebar");
        }, 250);
    });
});