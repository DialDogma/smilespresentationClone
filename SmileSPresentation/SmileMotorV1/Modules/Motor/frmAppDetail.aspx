<%@ Page Title="รายละเอียด Application" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmAppDetail.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmAppDetail" Theme="default" EnableEventValidation="false" %>

<%@ Register Src="~/Modules/Motor/UserControls/ucAppDetail.ascx" TagPrefix="uc1" TagName="ucAppDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="panel-body">
            <div class="row" style="margin-bottom: 0px">
                <uc1:ucAppDetail runat="server" ID="ucAppDetail" />
            </div>
            <div class="row" style="margin-top: 0px; text-align: right">
                <asp:Button ID="btnRenew" runat="server" Text="ต่ออายุ" Visible="False" OnClick="btnRenew_Click" />
                <ajaxToolkit:ConfirmButtonExtender runat="server" TargetControlID="btnRenew" ConfirmText="ต้องการทำการต่ออายุ Application นี้ใช่หรือไม่?" />
                <asp:Button ID="btnEdit" runat="server" Text="แก้ไข" OnClick="btnEdit_Click" />
                <asp:HiddenField ID="hdfMotorApplicationID" runat="server" />
            </div>
        </div>
    </div>

    <script>
        $(function () {
            $(window).scroll(function () {
                if ($(this).scrollTop() >= 100) {        // If page is scrolled more than 50px
                    $('#divList').fadeIn(200);    // Fade in the arrow
                } else {
                    $('#divList').fadeOut(200);    //Else fade out the arrow
                }
            });
        });
    </script>
</asp:Content>