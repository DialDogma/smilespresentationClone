<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmReportEndCoverNotice.aspx.cs" Theme="default" Inherits="SmileMotorV1.Modules.Motor.frmReportEndCoverNotice" %>

<%@ Register Src="UserControls/DropdownUserControls/ddlBranch.ascx" TagName="ddlBranch" TagPrefix="uc1" %>

<%@ Register Src="UserControls/DropdownUserControls/ddlPeriodType.ascx" TagName="ddlPeriodType" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 12.5%;
            height: 27px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h4>รายงาน Application ใกล้หมดอายุ</h4>
            </div>
            <div class="panel-body">
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 12.5%; text-align: right">สาขา :</td>
                        <td style="width: 12.5%;">
                            <uc1:ddlBranch ID="ddlBranch1" runat="server" />
                        </td>
                        <td style="width: 12.5%;"></td>
                        <td style="width: 12.5%;"></td>
                        <td style="width: 12.5%;"></td>
                        <td style="width: 12.5%;"></td>
                    </tr>
                    <tr>
                        <td style="width: 12.5%; text-align: right;">งวดชำระ :</td>
                        <td style="width: 12.5%;">
                            <uc2:ddlPeriodType ID="ddlPeriodType1" runat="server" />
                        </td>
                        <td style="width: 12.5%;"></td>
                        <td style="width: 12.5%;"></td>
                        <td style="width: 12.5%;"></td>
                        <td style="width: 12.5%;"></td>
                    </tr>
                    <tr>
                        <td style="width: 12.5%; text-align: right">แจ้งเตือนล่วงหน้า :</td>
                        <td style="width: 12.5%;">
                            <asp:DropDownList runat="server" ID="ddlEndCoverNotice">
                                <asp:ListItem runat="server" Value="30">30 วัน</asp:ListItem>
                                <asp:ListItem runat="server" Value="60">60 วัน</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 12.5%;"></td>
                        <td style="width: 12.5%;"></td>
                        <td style="width: 12.5%;"></td>
                        <td style="width: 12.5%;"></td>
                    </tr>
                    <tr>
                        <td class="auto-style1"></td>
                        <td class="auto-style1">
                            <asp:Button ID="btnSearch" runat="server" Text="แสดงรายงาน" SkinID="primary" OnClick="btnSearch_OnClick" />
                        </td>
                        <td class="auto-style1">
                            <asp:Button ID="btnExportExcel" runat="server" Text="Export Excel" SkinID="success" OnClick="btnExportExcel_OnClick" />
                        </td>
                        <td class="auto-style1"></td>
                        <td class="auto-style1"></td>
                        <td class="auto-style1"></td>
                    </tr>
                    <tr>
                        <td style="width: 100%;" colspan="6">
                            <asp:GridView ID="dgvMain" runat="server"
                                AutoGenerateColumns="False"
                                EmptyDataText="ไม่พบข้อมูล"
                                AllowPaging="True"
                                OnPageIndexChanging="dgvMain_OnPageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-Width="0">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>.
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ApplicationID" HeaderText="Application ID" />
                                    <asp:BoundField DataField="Customer" HeaderText="ชื่อผู้เอาประกัน" />
                                    <asp:BoundField DataField="ProductDetail" HeaderText="แผน" />
                                    <asp:BoundField DataField="PremiumPerYear" HeaderText="เบี้ยประกัน" />
                                    <asp:BoundField DataField="PeriodTypeDetail" HeaderText="การชำระเบี้ย" />
                                    <asp:BoundField DataField="Sale1_Employee_Code" HeaderText="ตัวแทน" />
                                    <asp:BoundField DataField="StartCover" HeaderText="วันที่เริ่มคุ้มครอง" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="EndCover" HeaderText="วันที่สิ้นสุดความคุ้มครอง" DataFormatString="{0:dd/MM/yyyy}" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>