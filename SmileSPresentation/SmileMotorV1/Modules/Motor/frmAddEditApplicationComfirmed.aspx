<%@ Page Title="รายละเอียดยืนยันการทำรายการ" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmAddEditApplicationComfirmed.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmAddEditApplicationComfirmed" Theme="default" %>

<%@ Register Src="~/Modules/Motor/UserControls/ucAppDetail.ascx" TagPrefix="uc1" TagName="ucAppDetail" %>
<%@ Register Src="~/CommonUserControls/ucAlert.ascx" TagPrefix="uc1" TagName="ucAlert" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:ucAlert runat="server" ID="ucAlert" />
    <uc1:ucAppDetail runat="server" ID="ucAppDetail" />
    <table style="width:100%;">
        <tr>
            <td style="width:12.5%;"></td>
            <td style="width:12.5%;"></td>
            <td style="width:12.5%;"></td>
            <td style="width:12.5%;"></td>
            <td style="width:12.5%;">
                <asp:Button ID="btnContinueNewApp" runat="server" SkinID="info" Text="บันทึก New Application" OnClick="btnContinueNewApp_Click"/>
            </td>
            <td style="width:12.5%; text-align:right;">
                <asp:Button ID="btnOpenCashReceive" runat="server" SkinID="warning" Text="บันทึกการชำระเงิน Cash Receive"/>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hdfApp_ID" runat="server" />
</asp:Content>