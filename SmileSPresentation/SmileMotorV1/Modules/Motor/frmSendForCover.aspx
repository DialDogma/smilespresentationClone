<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmSendForCover.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmSendForCover" Theme="default" %>

<%@ Register TagPrefix="uc1" TagName="ddlfirstdayofmonth" Src="~/Modules/Motor/UserControls/DropdownUserControls/ddlFirstDayOfMonth.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ddlbranch" Src="~/Modules/Motor/UserControls/DropdownUserControls/ddlBranch.ascx" %>
<%@ Register TagPrefix="uc2" TagName="ucemployeesearch" Src="~/Modules/Motor/UserControls/ucEmployeeSearch.ascx" %>
<%@ Register TagPrefix="uc3" TagName="ucDatepicker_1" Src="~/CommonUserControls/ucDatepicker.ascx" %>

<%@ Register Src="UserControls/DropdownUserControls/ddlPeriodType.ascx" TagName="ddlPeriodType" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h4>นำส่งขอความคุ้มครอง</h4>
            </div>
            <div class="panel-body">
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 12.5%; text-align: right;">งวดชำระ : </td>
                        <td style="width: 12.5%;">
                            <uc2:ddlPeriodType ID="ddlPeriodType1" runat="server" Autopostback="true" OnSelectChanged="ddlPeriodType1_SelectChanged" />
                        </td>
                        <td style="width: 12.5%;">&nbsp;</td>
                        <td style="width: 12.5%;">&nbsp;</td>
                        <td style="width: 12.5%;"></td>
                        <td style="width: 12.5%;"></td>
                    </tr>
                    <tr runat="server" id="trPeriod">
                        <td style="width: 12.5%; text-align: right;">งวดคุ้มครอง : </td>
                        <td style="width: 12.5%;">
                            <uc1:ddlfirstdayofmonth runat="server" ID="ddlFirstDayOfMonth" />
                            <%--<uc3:ucDatepicker_1 ID="ucDatepickerPeriod" runat="server" />--%>
                        </td>
                        <td style="width: 12.5%;">&nbsp;</td>
                        <td style="width: 12.5%;">&nbsp;</td>
                        <td style="width: 12.5%;"></td>
                        <td style="width: 12.5%;"></td>
                    </tr>
                    <tr>
                        <td style="width: 12.5%; text-align: right;">ประเภทสัญญา : </td>
                        <td style="width: 12.5%;">
                            <asp:DropDownList ID="ddlCoverType" runat="server"></asp:DropDownList>
                        </td>
                        <td style="width: 12.5%;">&nbsp;</td>
                        <td style="width: 12.5%;">&nbsp;</td>
                        <td style="width: 12.5%;"></td>
                        <td style="width: 12.5%;"></td>
                    </tr>
                    <tr>
                        <td style="width: 12.5%; text-align: right;"></td>
                        <td style="width: 12.5%;">
                            <asp:Button ID="btnSearch" runat="server" Text="แสดงรายงาน" SkinID="primary" OnClick="btnSearch_OnClick" />
                        </td>
                        <td style="width: 12.5%;">
                            <asp:Button ID="btnExportExcel" runat="server" Text="นำส่งขอความคุ้มครอง Export Excel" SkinID="success" OnClick="btnExportExcel_OnClick" />
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
                                    <asp:BoundField DataField="MotorApplication_Code" HeaderText="เลขแอพพลิเคชั่น" />
                                    <asp:BoundField DataField="ProductDetail" HeaderText="ผลิตภัณฑ์" />
                                    <asp:BoundField DataField="PremiumPerMonth" DataFormatString="{0:#,0}" HeaderText="เบี้ยรายเดือน" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="PremiumPerYear" DataFormatString="{0:#,0}" HeaderText="เบี้ยรายปี" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="CustomerFullName" HeaderText="ชื่อ - สกุล" />
                                    <asp:BoundField DataField="VehicleBrandDetail" HeaderText="ยี่ห้อ" />
                                    <asp:BoundField DataField="VehicleModelDetail" HeaderText="รุ่น" />
                                    <asp:BoundField DataField="LicencePlateNo" HeaderText="เลขทะเบียน" />
                                    <asp:BoundField DataField="StartCover" HeaderText="วันที่เริ่มคุ้มครอง" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="EndCover" HeaderText="วันสิ้นสุดคุ้มครอง" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="MotorApplicationStatusDetail" HeaderText="สถานะ" />
                                    <%--   <asp:BoundField DataField="ApproveBy_Employee_Code" HeaderText="อนุมัติโดย" />
                                    <asp:BoundField DataField="ApproveDate" HeaderText="วันที่อนุมัติ" DataFormatString="{0:dd/MM/yyyy}" />--%>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>