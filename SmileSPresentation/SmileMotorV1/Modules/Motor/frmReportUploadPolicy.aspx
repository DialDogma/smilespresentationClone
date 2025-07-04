<%@ Page Title="รายงานการ Upload กรมธรรม์" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmReportUploadPolicy.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmReportUploadPolicy" Theme="default"  EnableEventValidation="false"%>

<%@ Register Src="~/CommonUserControls/ucDatepicker.ascx" TagPrefix="uc1" TagName="ucDatepicker" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h4>รายงานการ Upload กรมธรรม์</h4>
            </div>
            <div class="panel-body">
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 12.5%; text-align: right;">วันที่อัพโหลด - ถึงวันที่ :</td>
                        <td style="width: 12.5%;">
                            <uc1:ucDatepicker runat="server" ID="ucDatepickerFrom" />
                        </td>
                        <td style="width: 12.5%; text-align: right;">
                            <uc1:ucDatepicker runat="server" ID="ucDatepickerTo" />
                        </td>
                        <td style="width: 12.5%;">           
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
                            <asp:Button ID="btnExportExcel" runat="server" Text="Export Excel" SkinID="success" OnClick="btnExportExcel_Click"/>
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
                                AllowPaging="True" OnPageIndexChanging="dgvMain_PageIndexChanging" >
                                <Columns>
                                    <asp:BoundField DataField="ScanUpdateDate" HeaderText="วันที่อัพโหลด" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="MotorApplication_Code" HeaderText="Application ID" />
                                    <asp:BoundField DataField="ProductDetail" HeaderText="ผลิตภัณฑ์" />
                                    <asp:BoundField DataField="PremiumPerMonth" HeaderText="เบี้ยรายเดือน" />
                                    <asp:BoundField DataField="PremiumPerYear" HeaderText="เบี้ยรายปี" />
                                    <asp:BoundField DataField="Sale1_Employee_Code" HeaderText="ตัวแทน" />
                                    <asp:BoundField DataField="StartCover" HeaderText="วันที่เริ่มคุ้มครอง" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="MotorApplicationStatusDetail" HeaderText="สถานะ" />
                                </Columns>

                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

</asp:Content>
