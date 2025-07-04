<%@ Page Title="อนุมัติ-ไม่อนุมัติ Application" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmApproveNewApp.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmApproveNewApp" Theme="default" EnableEventValidation="false" %>

<%@ Register Src="~/Modules/Motor/UserControls/ucAppDetail.ascx" TagPrefix="uc1" TagName="ucAppDetail" %>
<%@ Register Src="~/CommonUserControls/ucAlert.ascx" TagPrefix="uc1" TagName="ucAlert" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAppDetailDept.ascx" TagPrefix="uc1" TagName="ucAppDetailDept" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:ucAlert runat="server" ID="ucAlert" />
    <div class="container">
        <div class="panel-body">
            <div class="row" style="margin-bottom: 0px">
                <uc1:ucAppDetail runat="server" ID="ucAppDetail" />
            </div>
            <div class="row" style="margin-top: 0px; text-align: right">
                <div class="col-sm-6">
                </div>
                <div class="col-sm-2">
                    <asp:Button ID="btnApprove" runat="server" SkinID="success" Text="อนุมัติ" OnClick="btnApprove_Click" />
                    <ajaxToolkit:ConfirmButtonExtender ID="confirmApprove" runat="server" TargetControlID="btnApprove" ConfirmText="ยืนยันการทำรายการหรือไม่ ?" />
                </div>
                <div class="col-sm-2">
                    <asp:Button ID="btnPending" runat="server" SkinID="warning" Text="ส่งแก้ไข" OnClick="btnPending_Click" />
                    <ajaxToolkit:ConfirmButtonExtender ID="confirmPending" runat="server" TargetControlID="btnPending" ConfirmText="ยืนยันการทำรายการหรือไม่ ?" />
                </div>
                <div class="col-sm-2">
                    <asp:Button ID="btnUnApprove" runat="server" SkinID="danger" Text="ไม่อนุมัติ" OnClick="btnUnApprove_Click" />
                    <ajaxToolkit:ConfirmButtonExtender ID="confirmUnApprove" runat="server" TargetControlID="btnUnApprove" ConfirmText="ยืนยันการทำรายการหรือไม่ ?" />
                </div>
                <div class="col-sm-2">
                    <asp:Button ID="btnCancel" runat="server" Text="บันทึกยกเลิก" SkinID="danger" OnClick="btnCancel_OnClick" />
                    <ajaxToolkit:ConfirmButtonExtender ID="confirmCancel" runat="server" TargetControlID="btnCancel" ConfirmText="ยืนยันการทำรายการหรือไม่ ?" />
                </div>
            </div>
            <%--<table style="width: 100%">
                <tr>
                    <td colspan="6">
                        <uc1:ucAppDetail runat="server" ID="ucAppDetail" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%">
                        <asp:Button ID="btnApprove" runat="server" SkinID="success" Text="อนุมัติ" OnClick="btnApprove_Click" />
                    </td>
                    <td style="width: 12.5%">
                        <asp:Button ID="btnPending" runat="server" SkinID="warning" Text="ส่งแก้ไข" OnClick="btnPending_Click" />
                    </td>
                    <td style="width: 12.5%">
                        <asp:Button ID="btnUnApprove" runat="server" SkinID="danger" Text="ไม่อนุมัติ" OnClick="btnUnApprove_Click" />
                    </td>
                </tr>
            </table>--%>
        </div>
    </div>
</asp:Content>