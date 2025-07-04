<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmReportSendForCover.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmReportSendForCover" Theme="default" %>

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
                <h4>รายงานนำส่งขอความคุ้มครอง</h4>
            </div>
            <div class="panel-body">
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 12.5%; text-align: right;">
                            <asp:RadioButton ID="rdoDateCase1" runat="server" Text="งวดคุ้มครอง" GroupName="DateCase" Checked="True" OnCheckedChanged="rdoDateCase1_CheckedChanged" />
                        </td>
                        <td style="width: 12.5%;">&nbsp;</td>
                        <td style="width: 12.5%;">&nbsp;</td>
                        <td style="width: 12.5%;">&nbsp;</td>
                        <td style="width: 12.5%;">&nbsp;</td>
                        <td style="width: 12.5%;">&nbsp;</td>
                    </tr>
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
                    <tr runat="server" id="trPeriod1">
                        <td style="width: 12.5%; text-align: right;">งวดคุ้มครอง : </td>
                        <td style="width: 12.5%;">

                            <uc1:ddlfirstdayofmonth runat="server" ID="ddlFirstDayOfMonthFrom" />
                        </td>
                        <td style="width: 12.5%;">

                            <span style="white-space: nowrap;">
                                <uc1:ddlfirstdayofmonth runat="server" ID="ddlFirstDayOfMonthTo" />
                            </span>
                        </td>
                        <td style="width: 12.5%;"></td>
                        <td style="width: 12.5%;"></td>
                        <td style="width: 12.5%;"></td>
                    </tr>
                    <tr runat="server" id="trPeriod2" visible="false">
                        <td style="width: 12.5%; text-align: right;">วันเริ่มคุ้มครอง</td>
                        <td style="width: 12.5%;">

                            <uc3:ucDatepicker_1 ID="ucDatepickerStartCoverDate" runat="server" />
                        </td>
                        <td style="width: 12.5%;">
                            <uc3:ucDatepicker_1 ID="ucDatepickerEndCoverDate" runat="server" />
                        </td>
                        <td style="width: 12.5%;">&nbsp;</td>
                        <td style="width: 12.5%;">&nbsp;</td>
                        <td style="width: 12.5%;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 12.5%; text-align: right;"></td>
                        <td style="width: 12.5%;">&nbsp;</td>
                        <td style="width: 12.5%;">&nbsp;</td>
                        <td style="width: 12.5%;">&nbsp;</td>
                        <td style="width: 12.5%;">&nbsp;</td>
                        <td style="width: 12.5%;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 12.5%; text-align: right;">
                            <asp:RadioButton ID="rdoDateCase2" runat="server" Text="วันที่นำส่ง" GroupName="DateCase" OnCheckedChanged="rdoDateCase2_CheckedChanged" /></td>
                        <td style="width: 12.5%;">
                            <uc3:ucDatepicker_1 ID="ucDatepickerCreatedDateFrom" runat="server" />
                        </td>
                        <td style="width: 12.5%;">
                            <uc3:ucDatepicker_1 ID="ucDatepickerCreatedDateTo" runat="server" />
                        </td>
                        <td style="width: 12.5%;">&nbsp;</td>
                        <td style="width: 12.5%;">&nbsp;</td>
                        <td style="width: 12.5%;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 12.5%; text-align: right;">&nbsp;</td>
                        <td style="width: 12.5%;">&nbsp;</td>
                        <td style="width: 12.5%;">&nbsp;</td>
                        <td style="width: 12.5%;">&nbsp;</td>
                        <td style="width: 12.5%;">&nbsp;</td>
                        <td style="width: 12.5%;">&nbsp;</td>
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
                                    <asp:BoundField DataField="SendForCoverHeader_Code" HeaderText="เลขอ้างอิงการนำส่ง" />
                                    <asp:BoundField DataField="MotorApplicationContractTypeDetail" HeaderText="ประเภทสัญญา" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="Period" HeaderText="งวดความคุ้มครอง" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="ApplicationCount" HeaderText="จำนวนแอพที่นำส่ง" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="PeriodTypeDetail" HeaderText="ประเภทความคุ้มครอง" />
                                    <asp:BoundField DataField="CreatedDate" HeaderText="วันที่นำส่ง" DataFormatString="{0:dd/MM/yyyy hh:mm}" />
                                    <asp:BoundField DataField="CreatedUserFullName" HeaderText="ดำเนินการโดย" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>