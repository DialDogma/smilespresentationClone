<%@ Page Title="บันทึกสลักหลัง : เปลี่ยนแปลงแหล่งจัดเก็บ" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmEndorseChangeSource.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmEndorseChangeSource" Theme="default" %>

<%@ Register Src="~/Modules/Motor/UserControls/ucAppDetailApplication.ascx" TagPrefix="uc1" TagName="ucAppDetailApplication" %>
<%@ Register TagPrefix="uc1" TagName="ucAppDetailVehicle" Src="~/Modules/Motor/UserControls/ucAppDetailVehicle.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ucAppDetailCustomer" Src="~/Modules/Motor/UserControls/ucAppDetailCustomer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ucAppDetailAddress_1" Src="~/Modules/Motor/UserControls/ucAppDetailAddress.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ucAppDetailPayer" Src="~/Modules/Motor/UserControls/ucAppDetailPayer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ucAppDetailPayMethod" Src="~/Modules/Motor/UserControls/ucAppDetailPayMethod.ascx" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAddEditPayMethod.ascx" TagPrefix="uc1" TagName="ucAddEditPayMethod" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="tab1" class="container">
        <div class="panel-body">
            <table style="width: 100%;">
                <tr>
                    <td style="width: 80%; text-align: left;">
                        <asp:Label ID="lblAppCode" runat="server" Font-Bold="true" Font-Size="X-Large">บันทึกสลักหลัง : เปลี่ยนแปลงแหล่งจัดเก็บ</asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="panel-group">
            <div class="panel-body">
                <div class="panel panel-default">
                    <uc1:ucAppDetailApplication runat="server" ID="ucAppDetailApplication" />
                </div>
                <div class="panel panel-default">
                    <uc1:ucAppDetailVehicle runat="server" ID="ucAppDetailVehicle" />
                </div>
                <div class="panel panel-default">
                    <uc1:ucAppDetailCustomer runat="server" ID="ucAppDetailCustomer" />
                </div>
                <div class="panel panel-default">
                    <uc1:ucAppDetailAddress_1 runat="server" ID="ucAppDetailAddressCustomer" />
                </div>

                <div class="panel panel-default">
                    <uc1:ucAppDetailPayer runat="server" ID="ucAppDetailPayer" />
                </div>
                <div class="panel panel-default">
                    <uc1:ucAppDetailAddress_1 runat="server" ID="ucAppDetailAddressPayer" />
                </div>
                <div class="panel panel-default">
                    <uc1:ucAppDetailPayMethod runat="server" ID="ucAppDetailPayMethod" />
                </div>
            </div>
            <div class="panel-body">
                <div class="panel panel-danger">
                    <div class="panel-heading input-md">
                        <h4>
                            <asp:Label ID="lblHeader" runat="server" Text="เปลี่ยนแปลงแหล่งจัดเก็บ"></asp:Label>
                        </h4>
                    </div>
                </div>
                <uc1:ucAddEditPayMethod runat="server" ID="ucAddEditPayMethod" />
                <br />
                <div class="row" style="margin-top: 0px; text-align: right">
                    <div class="col-sm-8">
                    </div>
                    <div class="col-sm-2">
                        <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" SkinID="danger" OnClick="btnCancel_OnClick" />
                        <ajaxToolkit:ConfirmButtonExtender ID="confirmCancel" runat="server" TargetControlID="btnCancel" ConfirmText="ต้องการยกเลิกการทำรายการหรือไม่ ?" />
                    </div>
                    <div class="col-sm-2">
                        <asp:Button ID="btnSave" runat="server" SkinID="success" Text="บันทึก" OnClick="btnSave_OnClick" />
                        <ajaxToolkit:ConfirmButtonExtender ID="confirmSave" runat="server" TargetControlID="btnSave" ConfirmText="ยืนยันการทำรายการหรือไม่ ?" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdfMotorID" />
</asp:Content>