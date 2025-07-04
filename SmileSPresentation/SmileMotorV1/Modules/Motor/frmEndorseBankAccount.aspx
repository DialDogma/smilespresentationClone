<%@ Page Title="บันทึกแก้ไขเลขที่บัญชี" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmEndorseBankAccount.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmEndorseBankAccount" Theme="default" EnableEventValidation="false" %>

<%@ Register Src="../../CommonUserControls/ucProgressbar.ascx" TagName="ucProgressbar" TagPrefix="uc2" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAppDetailApplication.ascx" TagPrefix="uc2" TagName="ucAppDetailApplication" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAppDetail.ascx" TagPrefix="uc2" TagName="ucAppDetail" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAppDetailCustomer.ascx" TagPrefix="uc2" TagName="ucAppDetailCustomer" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAppDetailPayer.ascx" TagPrefix="uc2" TagName="ucAppDetailPayer" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAppDetailVehicle.ascx" TagPrefix="uc2" TagName="ucAppDetailVehicle" %>
<%@ Register Src="~/CommonUserControls/ucDatepicker.ascx" TagPrefix="uc2" TagName="ucDatepicker" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAddBankAccount.ascx" TagPrefix="uc2" TagName="ucAddBankAccount" %>
<%@ Register TagPrefix="uc2" TagName="ddlBankCompany" Src="~/Modules/Motor/UserControls/DropdownUserControls/ddlBankCompany.ascx" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAppDetailPayMethod.ascx" TagPrefix="uc2" TagName="ucAppDetailPayMethod" %>
<%@ Register Src="~/CommonUserControls/ucAlert.ascx" TagPrefix="uc2" TagName="ucAlert" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<asp:ScriptManager runat="server"></asp:ScriptManager>--%>
    <uc2:ucAlert runat="server" ID="ucAlert" />
    <div class="container">
        <div class="panel panel-default">
            <div class="panel-heading input-md">
                <h4>บันทึกแก้ไขเลขที่บัญชี</h4>
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
                            <td colspan="6">
                                <uc2:ucAppDetailApplication runat="server" ID="ucAppDetailApplication" />
                                <uc2:ucAppDetailVehicle runat="server" ID="ucAppDetailVehicle" />
                                <uc2:ucAppDetailCustomer runat="server" ID="ucAppDetailCustomer" />
                                <uc2:ucAppDetailPayer runat="server" ID="ucAppDetailPayer" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <uc2:ucAppDetailPayMethod runat="server" ID="ucAppDetailPayMethod" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:GridView ID="dgvBankAccount" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="5" OnPageIndexChanging="dgvBankAccount_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="1px">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex+1 %>.
                                            </ItemTemplate>
                                            <HeaderStyle Width="1px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="BankDetail" HeaderText="ธนาคาร" />
                                        <asp:BoundField DataField="PersonBankAccountName" HeaderText="ชื่อบัญชี" />
                                        <asp:BoundField DataField="PersonBankAccountNo" HeaderText="เลขที่บัญชี" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 12.5%; text-align: right;">
                                <asp:Label runat="server" ID="lblBank" Visible="False" Text="ธนาคาร :"></asp:Label></td>
                            <td style="width: 12.5%; text-align: right;">
                                <uc2:ddlBankCompany ID="ddlBankCompany1" runat="server" Visible="False" />
                            </td>
                            <td style="width: 12.5%; text-align: right;">
                                <asp:Label runat="server" ID="lblAccount" Visible="False" Text="เลขที่บัญชี :"></asp:Label></td>

                            <td style="width: 12.5%; text-align: right;">
                                <asp:TextBox ID="txtBankAccountNo" runat="server" Visible="False"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender runat="server" FilterType="Numbers" TargetControlID="txtBankAccountNo" />
                            </td>
                            <td style="width: 7%; text-align: left;">
                                <asp:Button ID="btnEdit" runat="server" Text="แก้ไข" SkinID="warning" OnClick="btnEdit_OnClick" />
                                <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" SkinID="danger" OnClick="btnCancel_OnClick" />
                            </td>
                            <td style="width: 5.5%; text-align: left;">&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 12.5%; text-align: right;">
                                <asp:HiddenField runat="server" ID="hdfAppID" />
                                <asp:HiddenField runat="server" ID="hdfBankID" />
                                <asp:HiddenField runat="server" ID="hdfBankAccountNo" />
                            </td>
                            <td style="width: 12.5%; text-align: right;"></td>
                            <td style="width: 12.5%; text-align: right;"></td>
                            <td style="width: 12.5%; text-align: right;"></td>
                            <td style="width: 12.5%; text-align: right;"></td>
                            <td style="width: 12.5%; text-align: right;"></td>
                        </tr>
                        <tr>
                            <td style="width: 12.5%;">
                                <asp:Button ID="btnDetail" runat="server" Text="รายละเอียดทั้งหมด" SkinID="info" />
                            </td>
                            <td style="width: 12.5%; text-align: right;"></td>
                            <td style="width: 12.5%; text-align: right;"></td>
                            <td style="width: 12.5%; text-align: right;" colspan="2"></td>
                            <td style="width: 12.5%; text-align: right;">
                                <asp:Button ID="btnSave" runat="server" Text="บันทึกแก้ไขเลขที่บัญชี" SkinID="success" OnClick="btnSave_Click" />
                                <ajaxToolkit:ConfirmButtonExtender ID="confBtn3" runat="server" TargetControlID="btnSave" ConfirmText="ยืนยันการทำรายการหรือไม่ ?" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
</asp:Content>