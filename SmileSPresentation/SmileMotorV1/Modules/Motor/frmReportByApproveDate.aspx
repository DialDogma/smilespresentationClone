<%@ Page Title="รายงานตามวันที่อนุมัติ" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmReportByApproveDate.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmReportByApproveDate" Theme="default" EnableEventValidation="false" %>

<%@ Register Src="~/Modules/Motor/UserControls/DropdownUserControls/ddlBranch.ascx" TagPrefix="uc1" TagName="ddlBranch" %>
<%@ Register Src="UserControls/ucEmployeeSearch.ascx" TagName="ucEmployeeSearch" TagPrefix="uc2" %>

<%@ Register Src="../../CommonUserControls/ucDatepicker.ascx" TagName="ucDatepicker" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h4>รายงานตามวันที่อนุมัติ</h4>
            </div>
            <div class="panel-body">
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 12.5%; text-align: right;">อนุมัติวันที่ - ถึงวันที่ :</td>
                        <td style="width: 12.5%;">
                            <uc3:ucDatepicker ID="ucDatepickerFrom" runat="server" />
                        </td>
                        <td style="width: 12.5%; text-align: right;">
                            <uc3:ucDatepicker ID="ucDatepickerTo" runat="server" />
                        </td>
                        <td style="width: 12.5%;"></td>
                        <td style="width: 12.5%;"></td>
                        <td style="width: 12.5%;"></td>
                    </tr>
                    <tr>
                        <td style="width: 12.5%; text-align: right;">สาขา :</td>
                        <td style="width: 12.5%;">
                            <uc1:ddlBranch runat="server" ID="ddlBranch" />
                        </td>
                        <td style="width: 12.5%;"></td>
                        <td style="width: 12.5%;"></td>
                        <td style="width: 12.5%;"></td>
                        <td style="width: 12.5%;"></td>
                    </tr>
                    <tr>
                        <td style="width: 75%;" colspan="4">
                            <uc2:ucEmployeeSearch ID="ucEmployeeSearch1" runat="server" />
                        </td>
                        <td style="width: 12.5%;"></td>
                        <td style="width: 12.5%;"></td>
                    </tr>
                    <tr>
                        <td style="width: 12.5%; text-align: right;"></td>
                        <td style="width: 12.5%;">
                            <asp:Button ID="btnSearch" runat="server" Text="แสดงรายงาน" SkinID="primary" OnClick="btnSearch_Click" />
                        </td>
                        <td style="width: 12.5%;">
                            <asp:Button ID="btnExportExcel" runat="server" Text="Export Excel" SkinID="success" OnClick="btnExportExcel_Click" />
                        </td>
                        <td style="width: 12.5%; text-align: right;"></td>
                        <td style="width: 12.5%; text-align: right;"></td>
                        <td style="width: 12.5%; text-align: right;"></td>
                    </tr>
                    <tr>
                        <td style="width: 100%;" colspan="6">
                            <asp:GridView ID="dgvMain" runat="server"
                                AutoGenerateColumns="False"
                                EmptyDataText="ไม่พบข้อมูล"
                                AllowPaging="True"
                                OnPageIndexChanging="dgvMain_SelectedIndexChanging">
                                <Columns>
                                    <asp:BoundField DataField="Sticker" HeaderText="Sticker" />
                                    <asp:TemplateField HeaderText="ชื่อ-สกุล">
                                        <ItemTemplate>
                                            <div><%#Eval("คำนำหน้า")%><%#Eval("ชื่อ")%> <%#Eval("สกุล")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ยี่ห้อ" HeaderText="ยี่ห้อ" />
                                    <asp:BoundField DataField="ป้ายทะเบียน" HeaderText="ป้ายทะเบียน" />
                                    <asp:BoundField DataField="วันที่เริ่มคุ้มครอง" HeaderText="วันที่เริ่มคุ้มครอง" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="เบี้ย" HeaderText="เบี้ย" />
                                    <asp:BoundField DataField="แผน" HeaderText="แผน" />
                                    <%--                                    <asp:BoundField DataField="ApproveDate" HeaderText="วันที่อนุมัติ" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="MotorApplication_Code" HeaderText="Application ID" />
                                    <asp:BoundField DataField="ProductDetail" HeaderText="ผลิตภัณฑ์" />
                                    <asp:BoundField DataField="PremiumPerMonth" HeaderText="เบี้ยรายเดือน" />
                                    <asp:BoundField DataField="PremiumPerYear" HeaderText="เบี้ยรายปี" />
                                    <asp:BoundField DataField="Sale1_Employee_Code" HeaderText="ตัวแทน" />
                                    <asp:BoundField DataField="StartCover" HeaderText="วันที่เริ่มคุ้มครอง" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="MotorApplicationStatusDetail" HeaderText="สถานะ" />
                                    <asp:BoundField DataField="ApproveBy_Employee_Code" HeaderText="อนุมัติโดย" />--%>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>