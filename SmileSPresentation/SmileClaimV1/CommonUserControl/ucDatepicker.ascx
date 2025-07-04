<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDatepicker.ascx.cs" Inherits="SmileClaimV1.CommonUserControl.ucDatepicker" %>

<script type="text/javascript" src="<%= ResolveClientUrl("~/JQueryUI/scripts/jquery-1.6.min.js")%>"></script>
<script type="text/javascript" src="<%= ResolveClientUrl("~/JQueryUI/scripts/jquery-ui.min.js")%>"></script>
<script type="text/javascript" src="<%= ResolveClientUrl("~/JQueryUI/scripts/jquery.ui.datepicker-th.js")%>"></script>
<script type="text/javascript" src="<%= ResolveClientUrl("~/JQueryUI/scripts/jquery.ui.datepicker.ext.be.js")%>"></script>

<%--<link href='<%= Page.ResolveClientUrl("~/JQueryUI/css/cupertino/jquery-ui.css")%>' rel="stylesheet" />--%>
<link href='<%= Page.ResolveClientUrl("~/JQueryUI/css/cupertino/jquery-ui.min.css")%>' rel="stylesheet" />
<link href='<%= Page.ResolveClientUrl("~/JQueryUI/css/cupertino/theme.css")%>' rel="stylesheet" />

<script type="text/javascript">

    $(document).ready(function () {
        $("#<%= txtDate.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true,
            dateFormat: 'dd/mm/yy',
            isBE: true,
            autoConversionField: false,
            yearRange: "1918:2097"
        });
    });

    var prm = Sys.WebForms.PageRequestManager.getInstance();

    prm.add_endRequest(function () {
        // re-bind your jQuery events here
        $("#<%= txtDate.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true,
            dateFormat: 'dd/mm/yy',
            isBE: true,
            autoConversionField: false,
            yearRange: "1918:2097"
        });
    });

</script>

<asp:TextBox 
    ID="txtDate" 
    runat="server" 
    OnTextChanged="txtDate_TextChanged" 
    Class="datepicker" 
    CssClass="form-control calendartextbox input-sm" 
    AutoPostBack="true" ></asp:TextBox>

<asp:Label ID="lblWarning" runat="server" Text="" Font-Size="Small" ForeColor="Red"></asp:Label>