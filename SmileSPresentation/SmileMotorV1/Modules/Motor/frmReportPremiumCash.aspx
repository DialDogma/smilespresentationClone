<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmReportPremiumCash.aspx.cs" Theme="default" Inherits="SmileMotorV1.Modules.Motor.frmReportPremiumCash" %>

<%@ Register Src="~/Modules/Motor/UserControls/DropdownUserControls/ddlFirstDayOfMonth.ascx" TagPrefix="uc1" TagName="ddlFirstDayOfMonth" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h4>รายงานค้างชำระ</h4>
            </div>
            <div class="panel-body">
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 12.5%; text-align: right;">งวดความคุ้มครอง :</td>
                        <td style="width: 12.5%;">
                            <uc1:ddlFirstDayOfMonth runat="server" ID="ddlFirstDayOfMonth" />
                        </td>
                        <td style="width: 12.5%;"></td>
                        <td style="width: 12.5%;"></td>
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
                                    <asp:BoundField DataField="Sticker" HeaderText="Application ID" />
                                    <asp:BoundField DataField="ชื่อผู้เอาประกัน" HeaderText="ชื่อผู้เอาประกัน" />
                                    <asp:BoundField DataField="แผน" HeaderText="แผน" />
                                    <asp:BoundField DataField="เบี้ยประกัน" HeaderText="เบี้ยประกัน" />
                                    <asp:BoundField DataField="การชำระเบี้ย" HeaderText="การชำระเบี้ย" />
                                    <asp:BoundField DataField="ตัวแทน" HeaderText="ตัวแทน" />
                                    <asp:BoundField DataField="StartCover" HeaderText="วันที่เริ่มคุ้มครอง" DataFormatString="{0:dd/MM/yyyy}" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>