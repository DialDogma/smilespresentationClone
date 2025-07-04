<%@ Page Title="บันทึกยกเลิก / คืนสถานะ" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmEndorseApplicationCancel.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmEndorseApplicationCancel" Theme="default" EnableEventValidation="false" %>

<%@ Register Src="../../CommonUserControls/ucProgressbar.ascx" TagName="ucProgressbar" TagPrefix="uc2" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAppDetailApplication.ascx" TagPrefix="uc2" TagName="ucAppDetailApplication" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAppDetail.ascx" TagPrefix="uc2" TagName="ucAppDetail" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAppDetailCustomer.ascx" TagPrefix="uc2" TagName="ucAppDetailCustomer" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAppDetailPayer.ascx" TagPrefix="uc2" TagName="ucAppDetailPayer" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAppDetailVehicle.ascx" TagPrefix="uc2" TagName="ucAppDetailVehicle" %>
<%@ Register Src="~/CommonUserControls/ucDatepicker.ascx" TagPrefix="uc2" TagName="ucDatepicker" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<asp:ScriptManager runat="server"></asp:ScriptManager>--%>
    <div class="container">
        <div class="panel panel-default">
            <div class="panel-heading input-md">
                <h4>บันทึกยกเลิก / คืนสถานะ</h4>
            </div>
            <div class="panel-body">
                <div class="table-responsive">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 12.5%; text-align: right;">คำค้นหา :</td>
                            <td style="width: 12.5%;" colspan="2">
                                <asp:TextBox ID="txtSearch" runat="server" placeholder="พิมพ์คำค้นหาที่นี่"></asp:TextBox>
                            </td>
                            <td style="width: 12.5%;">
                                <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" SkinID="info" Width="90%" OnClick="btnSearch_Click" /></td>
                            <td style="width: 12.5%; text-align: right;"></td>
                            <td style="width: 12.5%; text-align: right;"></td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:GridView ID="dgvMain" runat="server"
                                    AutoGenerateColumns="false"
                                    AllowPaging="True"
                                    OnPageIndexChanging="dgvMain_PageIndexChanging"
                                    OnRowDataBound="dgvMain_RowDataBound"
                                    OnSelectedIndexChanged="dgvMain_SelectedIndexChanged"
                                    PageSize="5">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="0">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex +1 %>.
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="insurancecompanydetail" HeaderText="บริษัทผู้รับประกัน" />
                                        <asp:BoundField DataField="productdetail" HeaderText="ผลิตภัณฑ์" />
                                        <asp:BoundField DataField="motorapplication_code" HeaderText="เลขแอพพลิเคชั่น" />
                                        <asp:BoundField DataField="customer" HeaderText="ชื่อ-สกุล" />
                                        <asp:BoundField DataField="startcover" HeaderText="วันที่เริ่มคุ้มครอง" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="vehiclebranddetail" HeaderText="ยี่ห้อ" />
                                        <asp:BoundField DataField="vehiclemodeldetail" HeaderText="รุ่น" />
                                        <asp:BoundField DataField="vehicleregistrationnumber" HeaderText="เลขทะเบียน" />
                                        <asp:BoundField DataField="motorapplicationstatusdetail" HeaderText="สถานะกรมธรรม์" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <uc2:ucAppDetailApplication runat="server" ID="ucAppDetailApplication" />
                                <uc2:ucAppDetailVehicle runat="server" ID="ucAppDetailVehicle" />
                                <uc2:ucAppDetailCustomer runat="server" ID="ucAppDetailCustomer" />
                                <uc2:ucAppDetailPayer runat="server" ID="ucAppDetailPayer" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 12.5%; text-align: right;">
                                <asp:HiddenField runat="server" ID="hdfAppID" />
                            </td>
                            <td style="width: 12.5%; text-align: right;">
                                <asp:Label ID="lblDateCancel" runat="server" Text="วันที่ยกเลิก :"></asp:Label>
                            </td>
                            <td style="width: 12.5%; text-align: right;">
                                <uc2:ucDatepicker runat="server" ID="ucDatepicker" />
                            </td>
                            <td style="width: 12.5%; text-align: right;">
                                <asp:Label ID="lblDateCancel0" runat="server" Text="สาเหตุยกเลิก :"></asp:Label>
                            </td>
                            <td style="width: 12.5%; text-align: right;">
                                <asp:DropDownList runat="server" ID="ddlCancelCause" />
                            </td>
                            <td style="width: 12.5%; text-align: right;">
                                <asp:TextBox runat="server" ID="txtRemark" placeholder="หมายเหตุ"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 12.5%;">
                                <asp:Button ID="btnDetail" runat="server" Text="รายละเอียดทั้งหมด" SkinID="info" />
                            </td>
                            <td style="width: 12.5%; text-align: right;"></td>
                            <td style="width: 12.5%; text-align: right;"></td>
                            <td style="width: 12.5%; text-align: right;" colspan="2">
                                <asp:Button ID="btnWaitCancel" runat="server" Text="บันทึกรอยกเลิก" SkinID="warning" OnClick="btnWaitCancel_OnClick" />
                                <ajaxToolkit:ConfirmButtonExtender ID="confBtn1" runat="server" TargetControlID="btnWaitCancel" ConfirmText="ยืนยันการทำรายการหรือไม่ ?" />

                                <asp:Button ID="btnReverse" runat="server" Text="บันทึกคืนสถานะปกติ" SkinID="primary" OnClick="btnReverse_Click" />
                                <ajaxToolkit:ConfirmButtonExtender ID="confBtn2" runat="server" TargetControlID="btnReverse" ConfirmText="ยืนยันการทำรายการหรือไม่ ?" />
                            </td>
                            <td style="width: 12.5%; text-align: right;">
                                <asp:Button ID="btnCancel" runat="server" Text="บันทึกยกเลิก" SkinID="danger" OnClick="btnCancel_Click" />
                                <ajaxToolkit:ConfirmButtonExtender ID="confBtn3" runat="server" TargetControlID="btnCancel" ConfirmText="ยืนยันการทำรายการหรือไม่ ?" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
</asp:Content>