$(window).on('load', function () {
    $("#loading").fadeOut("slow");
});


$(function () {
    $(".datepicker").datepicker({
        autoclose: true,
        todayHighlight: false
    });

    $('input[type="checkbox"], input[type="radio"]').iCheck({
        checkboxClass: "icheckbox_minimal-blue",
        radioClass: "iradio_minimal-blue"
    });

    $("#datemask").inputmask("dd/mm/yyyy", { "placeholder": "dd/mm/yyyy" });
    $("#datemask2").inputmask("mm/dd/yyyy", { "placeholder": "mm/dd/yyyy" });
    $("[data-mask]").inputmask();
});


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

function CheckStartCoverDate(date) {
    var d = new Date();

    var day = date.getDate();

    //var day = 31;

    var rs;
    if (day >= 1 && day <= 10) {
        rs = new Date(d.getFullYear(), d.getMonth(), 16);
    } else if (day >= 11 && day <= 25) {
        rs = new Date(d.getFullYear(), d.getMonth() + 1, 1);
    } else {
        rs = rs = new Date(d.getFullYear(), d.getMonth() + 1, 16);
    }

    return rs;
}

     function CheckStartCoverDateByNumber(number) {
         var d = new Date();

         //var day = date.getDate();

         var day = number;

         var rs;
         debugger;
         if (day >= 1 && day <= 10) {
             rs = new Date(d.getFullYear(), d.getMonth(), 16);
         } else if (day >= 11 && day <= 25) {
             rs = new Date(d.getFullYear(), d.getMonth() + 1, 1);
         } else {
             rs = rs = new Date(d.getFullYear(), d.getMonth() + 1, 16);
         }

         return rs;
}


function commaSeparateNumber(val) {
    val = val.toString().replace(/,/g, ''); //remove existing commas first
    var valRZ = val.replace(/^0+/, ''); //remove leading zeros, optional
    var valSplit = valRZ.split('.'); //then separate decimals

    while (/(\d+)(\d{3})/.test(valSplit[0].toString())) {
        valSplit[0] = valSplit[0].toString().replace(/(\d+)(\d{3})/, '$1' + ',' + '$2');
    }

    if (valSplit.length == 2) { //if there were decimals
        val = valSplit[0] + "." + valSplit[1]; //add decimals back
    } else {
        val = valSplit[0];
    }

    return val;
}

