<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDatepicker-DisablePastDates.ascx.cs" Inherits="SmileMotorV1.CommonUserControls.ucDatepicker_DisablePastDates" %>
<script type="text/javascript" src="<%= ResolveClientUrl("~/JQueryUI/scripts/jquery-1.6.min.js")%>"></script>
<script type="text/javascript" src="<%= ResolveClientUrl("~/JQueryUI/scripts/jquery-ui.min.js")%>"></script>
<script type="text/javascript" src="<%= ResolveClientUrl("~/JQueryUI/scripts/jquery.ui.datepicker-th.js")%>"></script>
<script type="text/javascript" src="<%= ResolveClientUrl("~/JQueryUI/scripts/jquery.ui.datepicker.ext.be.js")%>"></script>

<link href='<%= Page.ResolveClientUrl("~/JQueryUI/css/cupertino/jquery-ui.css")%>' rel="stylesheet" />
<link href='<%= Page.ResolveClientUrl("~/JQueryUI/css/cupertino/jquery-ui.min.css")%>' rel="stylesheet" />
<link href='<%= Page.ResolveClientUrl("~/JQueryUI/css/cupertino/theme.css")%>' rel="stylesheet" />

<script type="text/javascript">

    $(document).ready(function () {
        const startDate = new Date();
        const endDate = new Date(startDate.getFullYear(), startDate.getMonth() + 3, 0); //0 = last day
        $("#<%= txtDate.ClientID %>").datepicker({
            changeMonth: false,
            changeYear: false,
            showButtonPanel: false,
            dateFormat: 'dd/mm/yy',
            isBE: true,
            autoConversionField: false,
            minDate: startDate,
            maxDate: endDate
        });

        /*fix auto postback*/
        const prm = window.Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            // re-bind your jQuery events here
            $("#<%= txtDate.ClientID %>").datepicker({
                changeMonth: false,
                changeYear: false,
                showButtonPanel: false,
                dateFormat: 'dd/mm/yy',
                isBE: true,
                autoConversionField: false,
                minDate: startDate,
                maxDate: endDate
            });
        });
    });
</script>

<asp:TextBox
    ID="txtDate"
    runat="server"
    OnTextChanged="txtDate_TextChanged"
    Class="datepicker"
    CssClass="form-control calendartextbox input-sm"
    AutoPostBack="True"></asp:TextBox>

<asp:Label ID="lblWarning" runat="server" Text="" Font-Size="Small" ForeColor="Red"></asp:Label>