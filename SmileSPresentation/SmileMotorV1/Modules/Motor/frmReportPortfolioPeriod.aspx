<%@ Page Title="รายงานตามงวดที่คิดผลงาน" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmReportPortfolioPeriod.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmReportPortfolioPeriod" Theme="default" EnableEventValidation="false" %>

<%@ Register Src="~/CommonUserControls/ucDatepicker.ascx" TagPrefix="uc1" TagName="ucDatepicker" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h4>รายงานตามงวดที่คิดผลงาน</h4>
            </div>
            <div class="panel-body">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 12.5%; text-align: right">เลือกวันที่คิดผลงาน :</td>
                        <td style="width: 12.5%">
                            <uc1:ucDatepicker runat="server" ID="ucDateFrom" />
                        </td>
                        <td style="width: 12.5%">
                            <uc1:ucDatepicker runat="server" ID="UcDateTo" />
                        </td>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%"></td>                  
                    </tr>
                    <tr>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%">
                            <asp:Button ID="btnShow" SkinID="primary" runat="server" Text="แสดงรายการ" OnClick="btnShow_Click" />
                        </td>
                        <td style="width: 12.5%">
                            <asp:Button ID="btnExport" SkinID="success" runat="server" Text="Export" OnClick="btnExport_Click" />
                        </td>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%"></td>
                    </tr>
                    <tr>
                        <td style="width: 100%" colspan="6">
                            <asp:GridView ID="dgvMain" runat="server"
                                AutoGenerateColumns="false"
                                AllowPaging="true"
                                EmptyDataText="ไม่พบข้อมูล" OnPageIndexChanging="dgvMain_PageIndexChanging" >
                                <Columns>
                                    <asp:BoundField DataField="PortfolioPeriod" HeaderText="วันที่คิดผลงาน" DataFormatString="{0:dd/MM/yyyy}" />
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
                    <tr>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%"></td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
