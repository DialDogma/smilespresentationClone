window.jQuery.validator.addMethod("checkDate", function (value) {
    var regex = new RegExp("^(0[1-9]|[12][0-9]|3[01])[-/.](0[1-9]|1[012])[-/.][0-9]{4}$");

    if (!regex.test(value)) {
        return false;
    } else {
        return true;
    }
}, "กรุณาตรวจสอบวันที่");