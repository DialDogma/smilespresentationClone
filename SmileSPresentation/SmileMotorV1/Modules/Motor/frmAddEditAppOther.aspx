<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmAddEditAppOther.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmAddEditPayment" EnableEventValidation="false" Theme="default" %>

<%@ Register Src="~/Modules/Motor/UserControls/ucDocument.ascx" TagPrefix="uc1" TagName="ucDocument" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucApplicationOwner.ascx" TagPrefix="uc1" TagName="ucApplicationOwner" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucApplicationMemo.ascx" TagPrefix="uc1" TagName="ucApplicationMemo" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucPersonDetail_mini.ascx" TagPrefix="uc1" TagName="ucPersonDetail_mini" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAppDetailApplication.ascx" TagPrefix="uc1" TagName="ucAppDetailApplication" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <table style="width: 100%">
            <tr>
                <td style="width: 50%">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4 class="panel-title">ผู้เอาประกัน</h4>
                        </div>
                        <div class="panel-body">
                            <uc1:ucPersonDetail_mini runat="server" ID="ucPersonDetail_customer" />
                        </div>
                    </div>
                </td>
                <td style="width: 50%">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4 class="panel-title">ผู้ชำระเบี้ย</h4>
                        </div>
                        <div class="panel-body">
                            <uc1:ucPersonDetail_mini runat="server" ID="ucPersonDetail_payer" />
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <div class="panel panel-default">
            <uc1:ucAppDetailApplication runat="server" ID="ucAppDetailApplication" />
        </div>
        <div class="panel panel-default">
            <uc1:ucDocument runat="server" ID="ucDocument" />
        </div>
        <div class="panel panel-default">
            <uc1:ucApplicationMemo runat="server" ID="ucApplicationMemo" />
        </div>
        <table style="width: 100%;">
            <tr>
                <td style="width: 12.5%; text-align: center"></td>
                <td style="width: 12.5%; text-align: center"></td>
                <td style="width: 12.5%; text-align: center"></td>
                <td style="width: 12.5%; text-align: center"></td>
                <td style="width: 12.5%; text-align: center"></td>
                <td style="width: 12.5%; text-align: center"></td>
                <td style="width: 12.5%; text-align: center">
                    <asp:Button ID="btnBackward" runat="server" Text="ย้อนกลับ" SkinID="warning" Width="80%" OnClick="btnBackward_Click" />
                </td>
                <td style="width: 12.5%; text-align: center">
                    <asp:Button ID="btnSuccess" runat="server" Text="ส่งข้อมูล" SkinID="success" Width="100%" OnClick="btnSuccess_Click" />
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hdfCustomer_ID" runat="server" />
    <asp:HiddenField ID="hdfPayer_ID" runat="server" />
    <asp:HiddenField ID="hdfApp_ID" runat="server" />
</asp:Content>