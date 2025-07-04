'use strict';

$(window).on('load', function () {
    $("#loading").fadeOut("slow");
});

/***format number*****/
function formatNumberCurrency(num, decimalplace) {
    let n = Number.parseFloat(num).toFixed(decimalplace);
    return n.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
}

//add method validator check N/A
jQuery.validator.addMethod("checkNA", function (value, element) {
    if (value == "-1") {
        return false;
    } else {
        return true;
    };
}, "กรุณาเลือก");

window.jQuery.validator.addMethod("checkOnlyNum", function (value) {
    var regex = new RegExp("^[0-9]+$");

    if (!regex.test(value)) {
        return false;
    } else {
        return true;
    }
}, "กรอกเฉพาะตัวเลขเท่านั้น(0-9)");

/*add method validator check null*/
window.jQuery.validator.addMethod("checknull", function (value) {
    if (value === null) {
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

/*add method validator check number only*/
window.jQuery.validator.addMethod("checkNagative", function (value) {
    if (value < 0) {
        return false;
    } else {
        return true;
    }
}, "จำนวนเงินต้องไม่เป็นค่าติดลบ");

/*add method validator check text characters only*/
window.jQuery.validator.addMethod("checkText", function (value) {
    var regex = new RegExp("^[ก-๙A-Za-z]+$");

    var str = value;

    if (str == "") return false;

    if (!regex.test(str)) {
        return false;
    } else {
        return true;
    }
}, "กรอกเฉพาะตัวอักษร");

//sweet alert success
const swal_success = (success, title = 'ดำเนินการเรียบร้อย!') => {
    window.swal({
        title: title,
        type: 'success',
        showCancelButton: false,
        showConfirmButton: true,
        closeOnEsc: false,
        html: true
    }, function (IsConfirm) {
        if (IsConfirm) {
            if (typeof success === "function") {
                //callback
                success();
            }
        }
    });
};

//sweet alert success2
const swal_success2 = (title, success) => {
    window.swal({
        title: title,
        type: 'success',
        showCancelButton: false,
        showConfirmButton: true,
        closeOnEsc: false
    }, function (IsConfirm) {
        if (IsConfirm) {
            if (typeof success === "function") {
                //callback
                success();
            }
        }
    });
};

//sweet alert fail
const swal_fail = (text, callback) => {
    window.swal({
        title: 'เกิดข้อผิดพลาด!',
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
//sweet alert info
const swal_info = (text, callback) => {
    window.swal({
        title: 'เกิดข้อผิดพลาด!',
        type: 'warning',
        text: text,
        showCancelButton: false,
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
//sweet alert confirm
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
    }, function (IsConfirm) {
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

///
const swal_confirm2 = (title, text, success, cancel) => {
    window.swal({
        title: title,
        text: text,
        type: 'success',
        showCancelButton: true,
        confirmButtonText: 'ตกลง',
        cancelButtonText: 'ยกเลิก',
        showCancelButton: false,
        closeOnConfirm: false,
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

//spinner
const loadingSpinner = (fadeType, delay = 999999) => {
    switch (fadeType) {
        case "fadeIn":
            $("#loading").fadeIn(500, (e) => {
                setTimeout(() => {
                    $("#loading").fadeOut();
                }, delay);
            });
            break;
        case "fadeOut":
            $("#loading").fadeOut(2000);
            break;
        default:
    }
}

//fucntion enter
function handleEnter(field, event) {
    var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
    if (keyCode == 13) {
        var i;
        for (i = 0; i < field.form.elements.length; i++)
            if (field == field.form.elements[i])
                break;
        i = (i + 1) % field.form.elements.length;
        field.form.elements[i].focus();
        return false;
    }
    else

        return true;
}
//check xhr status
function checkXHRStatus(xhrStatus, exception) {
    var msg = '';
    if (xhrStatus.status === 0) {
        msg = 'Not connect.\n Verify Network.';
    } else if (xhrStatus.status == 404) {
        msg = 'Requested page not found. [404]';
    } else if (xhrStatus.status == 500) {
        msg = 'Internal Server Error [500].';
    } else if (exception === 'parsererror') {
        msg = 'Requested JSON parse failed.';
    } else if (exception === 'timeout') {
        msg = 'Time out error.';
    } else if (exception === 'abort') {
        msg = 'Ajax request aborted.';
    } else {
        msg = 'Uncaught Error.\n' + xhrStatus.responseText;
    }

    alert(msg)
}

//check Mobile Tablet
const isMobileTablet = () => {
    var check = false;
    (function (a) {
        if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino|android|ipad|playbook|silk/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4)))
            check = true;
    })(navigator.userAgent || navigator.vendor || window.opera);
    return check;
}