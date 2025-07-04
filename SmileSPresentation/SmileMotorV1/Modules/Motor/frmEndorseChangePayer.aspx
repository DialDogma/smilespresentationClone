<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmEndorseChangePayer.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmEndorseChangePayer" Theme="default" %>

<%@ Register Src="~/Modules/Motor/UserControls/ucAppDetailApplication.ascx" TagPrefix="uc1" TagName="ucAppDetailApplication" %>
<%@ Register TagPrefix="uc1" TagName="ucAppDetailVehicle" Src="~/Modules/Motor/UserControls/ucAppDetailVehicle.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ucAppDetailCustomer" Src="~/Modules/Motor/UserControls/ucAppDetailCustomer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ucAppDetailAddress_1" Src="~/Modules/Motor/UserControls/ucAppDetailAddress.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ucAppDetailPayer" Src="~/Modules/Motor/UserControls/ucAppDetailPayer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ucAppDetailPayMethod" Src="~/Modules/Motor/UserControls/ucAppDetailPayMethod.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ucPersonSearchByIdentityCardNumber" Src="~/Modules/Motor/UserControls/ucPersonSearchByIdentityCardNumber.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ddlPersonType" Src="~/Modules/Motor/UserControls/DropdownUserControls/ddlPersonType.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ucAppDetailCustomerChange" Src="~/Modules/Motor/UserControls/ucAppDetailCustomer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ddlRelation" Src="~/Modules/Motor/UserControls/DropdownUserControls/ddlRelation.ascx" %>
<%@ Register Src="~/CommonUserControls/ucAlert.ascx" TagName="ucAlert" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc2:ucAlert ID="ucAlert1" runat="server" />
    <div id="tab1" class="container">
        <div class="panel-body">
            <table style="width: 100%;">
                <tr>
                    <td style="width: 80%; text-align: left;">
                        <asp:Label ID="lblAppCode" runat="server" Font-Bold="true" Font-Size="X-Large">บันทึกสลักหลัง : เปลี่ยนแปลงผู้ชำระเบี้ย</asp:Label>
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
                <br />
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="panel panel-danger">
                            <div class="panel-heading input-md">
                                <h4>
                                    <asp:Label ID="lblHeader" runat="server" Text="เปลี่ยนแปลงผู้ชำระเบี้ย"></asp:Label>
                                </h4>
                            </div>
                            <table style="width: 100%;">
                                <tr>
                                    <td colspan="3" style="width: 50%;">
                                        <div class="panel panel-default">
                                            <div class="panel-heading input-md">
                                                <h4>
                                                    <asp:Label ID="lblTextHeader" runat="server" Text="ประเภทผู้ชำระเบี้ย"></asp:Label>
                                                </h4>
                                            </div>
                                            <div class="panel-body">
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td style="width: 12.5%; text-align: right;">ประเภท :</td>
                                                        <td style="width: 12.5%">
                                                            <uc1:ddlPersonType runat="server" ID="ddlPersonType1" OneSelectChanged="ddlPersonType1_eSelectChanged" AutoPostback="true" />
                                                        </td>
                                                        <td style="width: 12.5%"></td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </td>
                                    <td colspan="3" style="width: 50%;">
                                        <uc1:ucPersonSearchByIdentityCardNumber runat="server"
                                            ID="ucPersonSearchByIdentityCardNumber"
                                            OneSelectedPersonIDChanged="ucPersonSearchByIdentityCardNumber_eSelectedPersonIDChanged"
                                            OneClickPersonIDChangednull="ucPersonSearchByIdentityCardNumber_eClickPersonIDChangednull1" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <div class="panel panel-default">
                                            <uc1:ucAppDetailCustomer runat="server" ID="ucAppDetailCustomerChange" />
                                        </div>
                                        <%--<div class="panel panel-default">
                                <uc1:ucAppDetailCorporate runat="server" ID="ucAppDetailCorporate" />
                            </div>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <div class="panel panel-default">
                                            <div class="panel-heading input-md">
                                                <h4 class="panel-title">บันทึกข้อมูลผู้ชำระเบี้ย</h4>
                                            </div>
                                            <div class="panel-body">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 50%; text-align: right;" colspan="2">ความสัมพันธ์กับผู้เอาประกัน :</td>
                                                        <td style="width: 50%">
                                                            <uc1:ddlRelation runat="server" ID="ddlRelation" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <%--<tr>
                        <td colspan="3"></td>
                        <td>
                            <asp:Button ID="Button1" runat="server" Text="ยกเลิก" SkinID="danger" />
                        </td>
                        <td>
                            <asp:Button ID="Button2" runat="server" Text="บันทึก" SkinID="success" />
                        </td>
                    </tr>--%>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="row" style="margin-top: 0px; text-align: right">
                <div class="col-sm-8">
                </div>
                <div class="col-sm-2">
                    <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" SkinID="danger" OnClick="btnCancel_Click" />
                    <ajaxToolkit:ConfirmButtonExtender ID="confirmCancel" runat="server" TargetControlID="btnCancel" ConfirmText="ต้องการยกเลิกการทำรายการหรือไม่ ?" />
                </div>
                <div class="col-sm-2">
                    <asp:Button ID="btnSave" runat="server" SkinID="success" Text="บันทึก" OnClick="btnSave_Click" />
                    <ajaxToolkit:ConfirmButtonExtender ID="confirmSave" runat="server" TargetControlID="btnSave" ConfirmText="ยืนยันการทำรายการหรือไม่ ?" />
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdfOldPerson_ID" runat="server" />
    <asp:HiddenField ID="hdfCustomer_ID" runat="server"/>
    <asp:HiddenField ID="hdfPerson_ID" runat="server" />
    <asp:HiddenField ID="hdfApp_ID" runat="server" />
</asp:Content>