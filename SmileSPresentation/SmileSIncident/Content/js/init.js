$(window).on('load', function () {
    $("#loading").fadeOut("slow");
});

$(function () {
    $('form').validate();

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
        startDate: 'today',
        endDate: '+1m +1d',
        autoclose: true
    });
});