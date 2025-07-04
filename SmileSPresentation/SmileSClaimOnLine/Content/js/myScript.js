window.jQuery.validator.addMethod("checkOnlyNum", function (value) {
    var regex = new RegExp("^[0-9]+$");

    if (!regex.test(value)) {
        return false;
    } else {
        return true;
    }
}, "กรอกเฉพาะตัวเลขเท่านั้น(0-9)");

window.jQuery.validator.addMethod("checkOnlyCharacter", function (value) {
    var regex = new RegExp("^[^0-9-*/+]+$");

    if (!regex.test(value)) {
        return false;
    } else {
        return true;
    }
}, "กรอกเฉพาะตัวอักษรเท่านั้น");

window.jQuery.validator.addMethod("checkFormatPhoneNumbers", function (value) {
    var regex = new RegExp("^[0]{1}[1-9]{1}[0-9]{8}$");

    if (!regex.test(value)) {
        return false;
    } else {
        return true;
    }
}, "กรุณาตรวจสอบรูปแบบหมายเลขโทรศัพท์");

/*add method validator check null*/
window.jQuery.validator.addMethod("checknull", function (value) {
    if (value === null) {
        return false;
    } else {
        return true;
    }
}, "**กรุณาเลือก");

/*add method validator check NA select*/
window.jQuery.validator.addMethod("checkNA", function (value) {
    if (value === "-1") {
        return false;
    } else {
        return true;
    }
}, "**กรุณาเลือก");

/*add method validator check 0 */
window.jQuery.validator.addMethod("checkZero", function (value) {
    if (value == 0) {
        return false;
    } else {
        return true;
    }
}, "จำนวนเงินต้องไม่ใช่ 0");

window.jQuery.validator.addMethod("checkNagative", function (value) {
    if (value < 0) {
        return false;
    } else {
        return true;
    }
}, "จำนวนเงินต้องไม่เป็นค่าติดลบ");

//--------------------------------------ALERT MESSAGE--------------------------------------------------//
const swal_confirm = (title, text, success, cancel) => {
    window.swal({
        title: title,
        text: text,
        type: 'info',
        showCancelButton: true,
        confirmButtonColor: '#277020',
        confirmButtonText: 'ยืนยัน',
        cancelButtonText: 'ยกเลิก',
        closeOnConfirm: false,
        closeOnEsc: false,
        closeOnCancel: true
    }, (IsConfirm) => {
        if (IsConfirm) {
            if (typeof success === "function") {
                //callback
                success();
            }
        } else {
            if (typeof cancel === "function") {
                //callback
                cancel();
            }
        }
    });
};

const swal_success = (success, cancel) => {
    window.swal({
        title: 'ดำเนินการเรียบร้อย!',
        type: 'success',
        showCancelButton: false,
        showConfirmButton: true,
        closeOnEsc: false
    }, (IsConfirm) => {
        if (IsConfirm) {
            if (typeof success === "function") {
                //callback
                success();
            }
        } else {
            if (typeof cancel === "function") {
                //callback
                cancel();
            }
        }
    });
};

const swal_fail = (text, callback) => {
    window.swal({
        title: "เกิดข้อผิดพลาด",
        type: 'error',
        text: text,
        showCancelButton: false,
        confirmButtonColor: '#ed2b09',
        confirmButtonText: 'ตกลง',
        closeOnEsc: false
    }, (IsConfirm) => {
        if (IsConfirm) {
            if (typeof callback === "function") {
                //callback
                callback();
            }
        }
    });
};

const swal_warning = (text, callback) => {
    window.swal({
        title: "แจ้งเตือน",
        type: 'warning',
        text: text,
        showCancelButton: false,
        confirmButtonColor: '#ed2b09',
        confirmButtonText: 'ตกลง',
        closeOnEsc: false
    }, (IsConfirm) => {
        if (IsConfirm) {
            if (typeof callback === "function") {
                //callback
                callback();
            }
        }
    });
};

const swal_info = (text) => {
    window.swal({
        title: 'แจ้งเตือน!',
        type: 'warning',
        text: text,
        showCancelButton: false,
        confirmButtonColor: '#7cd1f9',
        confirmButtonText: 'ตกลง',
        closeOnEsc: false
    });
};