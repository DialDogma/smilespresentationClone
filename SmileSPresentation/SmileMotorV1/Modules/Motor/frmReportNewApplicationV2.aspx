<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmReportNewApplicationV2.aspx.cs" Theme="default" Inherits="SmileMotorV1.Modules.Motor.frmReportNewApplicationV2" EnableEventValidation="false" %>

<%@ Register TagPrefix="uc1" TagName="ddlfirstdayofmonth" Src="~/Modules/Motor/UserControls/DropdownUserControls/ddlFirstDayOfMonth.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ddlbranch" Src="~/Modules/Motor/UserControls/DropdownUserControls/ddlBranch.ascx" %>
<%@ Register TagPrefix="uc2" TagName="ucemployeesearch" Src="~/Modules/Motor/UserControls/ucEmployeeSearch.ascx" %>
<%@ Register TagPrefix="uc3" TagName="ucDatepicker_1" Src="~/CommonUserControls/ucDatepicker.ascx" %>

<%@ Register Src="UserControls/DropdownUserControls/ddlMotorApplicationStatus.ascx" TagName="ddlMotorApplicationStatus" TagPrefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h4>รายงาน NewApplication</h4>
            </div>
            <div class="panel-body">
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 12.5%; text-align: right;">เงื่อนไข : </td>
                        <td style="width: 12.5%;">
                            <asp:DropDownList runat="server" ID="ddlCondition">
                                <asp:ListItem Value="1">วันที่สร้างข้อมูล</asp:ListItem>
                                <asp:ListItem Value="2">วันที่คุ้มครอง</asp:ListItem>
                                <asp:ListItem Value="3">วันที่อนุมัติ</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 12.5%;"></td>
                        <td style="width: 12.5%;"></td>
                        <td style="width: 12.5%;"></td>
                        <td style="width: 12.5%;"></td>
                    </tr>
                    <tr>
                        <td style="width: 12.5%; text-align: right;">วันที่ - ถึงวันที่ :</td>
                        <td style="width: 12.5%;">
                            <uc3:ucDatepicker_1 ID="ucDatepickerFrom" runat="server" />
                        </td>
                        <td style="width: 12.5%; text-align: right;">
                            <uc3:ucDatepicker_1 ID="ucDatepickerTo" runat="server" />
                        </td>
                        <td style="width: 12.5%;"></td>
                        <td style="width: 12.5%;"></td>
                        <td style="width: 12.5%;"></td>
                    </tr>
                    <tr>
                        <td style="width: 12.5%; text-align: right;">สถานะ : </td>
                        <td style="width: 12.5%;">
                            <uc4:ddlMotorApplicationStatus ID="ddlMotorApplicationStatus1" runat="server" />
                        </td>
                        <td style="width: 12.5%;">&nbsp;</td>
                        <td style="width: 12.5%;">&nbsp;</td>
                        <td style="width: 12.5%;"></td>
                        <td style="width: 12.5%;"></td>
                    </tr>
                    <tr>
                        <td style="width: 12.5%; text-align: right;">สาขา :</td>
                        <td style="width: 12.5%;">
                            <uc1:ddlbranch runat="server" ID="ddlBranch" />
                        </td>
                        <td style="width: 12.5%;"></td>
                        <td style="width: 12.5%;"></td>
                        <td style="width: 12.5%;"></td>
                        <td style="width: 12.5%;"></td>
                    </tr>
                    <tr>

                        <td style="width: 75%;" colspan="4">
                            <uc2:ucemployeesearch ID="ucEmployeeSearch1" runat="server" />
                        </td>
                        <td style="width: 12.5%;"></td>
                        <td style="width: 12.5%;"></td>
                    </tr>
                    <tr>
                        <td style="width: 12.5%; text-align: right;"></td>
                        <td style="width: 12.5%;">
                            <asp:Button ID="btnSearch" runat="server" Text="แสดงรายงาน" SkinID="primary" OnClick="btnSearch_OnClick" />
                        </td>
                        <td style="width: 12.5%;">
                            <asp:Button ID="btnExportExcel" runat="server" Text="Export Excel" SkinID="success" OnClick="btnExportExcel_OnClick" />
                        </td>
                        <td style="width: 12.5%; text-align: right;"></td>
                        <td style="width: 12.5%; text-align: right;"></td>
                        <td style="width: 12.5%; text-align: right;"></td>
                    </tr>
                    <tr>
                        <td style="width: 100%;" colspan="6">
                            <asp:GridView ID="dgvMain" runat="server"
                                AutoGenerateColumns="false"
                                EmptyDataText="ไม่พบข้อมูล"
                                AllowPaging="True"
                                OnPageIndexChanging="dgvMain_OnPageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-Width="0">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>.
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="MotorApplication_Code" HeaderText="Application ID" />
                                    <asp:BoundField DataField="ProductDetail" HeaderText="ผลิตภัณฑ์" />
                                    <asp:BoundField DataField="PremiumPerMonth" HeaderText="เบี้ยรายเดือน" />
                                    <asp:BoundField DataField="PremiumPerYear" HeaderText="เบี้ยรายปี" />
                                    <asp:BoundField DataField="Sale1_Employee_Code" HeaderText="ตัวแทน" />
                                    <asp:BoundField DataField="StartCover" HeaderText="วันที่เริ่มคุ้มครอง" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="MotorApplicationStatusDetail" HeaderText="สถานะ" />
                                    <asp:BoundField DataField="ApproveBy_Employee_Code" HeaderText="อนุมัติโดย" />
                                    <asp:BoundField DataField="ApproveDate" HeaderText="วันที่อนุมัติ" DataFormatString="{0:dd/MM/yyyy}" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>