$(window).on('load', function () {
    $("#loading").fadeOut("slow");
});

$(function () {
    $(".datepicker").datepicker({
        autoclose: true
    });

    $('input[type="checkbox"], input[type="radio"]').iCheck({
        checkboxClass: "icheckbox_minimal-blue",
        radioClass: "iradio_minimal-blue"
    });

    $("#datemask").inputmask("dd/mm/yyyy", { "placeholder": "dd/mm/yyyy" });
    $("#datemask2").inputmask("mm/dd/yyyy", { "placeholder": "mm/dd/yyyy" });
    $("[data-mask]").inputmask();

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
    if (data !== null) {
        var d = moment(data).toDate();
        var month = ("0" + (d.getMonth() + 1)).slice(-2);
        var day = ("0" + d.getDate()).slice(-2);
        var year = d.getFullYear() + 543;
        result = day + '/' + month + '/' + year;
    }
    return result;
};

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
};

function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

function CalcDays(dateFuture, dateNow) {

    let diffInMilliSeconds = Math.abs(dateFuture - dateNow) / 1000;

    let hours; //ชั่วโมงรวม
    let days; //จำนวนวัน
    let hourLeft; //ชั่วโมงเศษ
    const hourToCountDay = 5; //ชั่วโมงที่จะปัดขึ้นเป็น 1 วัน

    // calculate hours
    hours = Math.floor(diffInMilliSeconds / 3600);
    days = hours / 24;
    hourLeft = hours % 24;

    if (hourLeft > hourToCountDay) {
        days += 1
    }

    return days;
}