$(function () {
    $(".datepicker").datepicker({
        autoclose: true,
        todayBtn: true,
        todayHighlight: true
    });

    $('input[type="checkbox"], input[type="radio"]').iCheck({
        checkboxClass: "icheckbox_minimal-blue",
        radioClass: "iradio_minimal-blue"
    });

    $("#datemask").inputmask("dd/mm/yyyy", { "placeholder": "dd/mm/yyyy" });
    $("#datemask2").inputmask("mm/dd/yyyy", { "placeholder": "mm/dd/yyyy" });
    $("[data-mask]").inputmask();
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
//sweet alert success
const swal_success = (success, title = 'ดำเนินการเรียบร้อย!') => {
    window.swal({
        title: title,
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