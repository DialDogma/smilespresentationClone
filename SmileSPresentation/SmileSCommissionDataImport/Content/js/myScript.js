'use strict';
/*---colors---*/
window.chartColors = {
    red: 'rgb(255, 99, 132)',
    red2: 'rgb(249, 0, 0)',
    orange: 'rgb(255, 159, 64)',
    yellow: 'rgb(244, 229, 66)',
    green: 'rgb(66, 244, 140)',
    blue: 'rgb(66, 188, 244)',
    purple: 'rgb(159, 132, 249)',
    grey: 'rgb(201, 203, 207)',
    brown: 'rgb(150, 75, 0)'
};

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
        showConfirmButton: false,
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