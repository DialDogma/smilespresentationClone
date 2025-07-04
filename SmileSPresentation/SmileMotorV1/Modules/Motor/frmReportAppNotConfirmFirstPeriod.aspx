<%@ Page Title="รายงาน App ที่ยังไม่ยืนยันการชำระงวดแรก" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmReportAppNotConfirmFirstPeriod.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmReportAppNotConfirmFirstPeriod" Theme="default" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="panel panel-default">
            <div class="panel-heading input-md">
                <h4>รายงาน App ที่ยังไม่ยืนยันการชำระงวดแรก</h4>
            </div>
            <div class="panel-body">
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 16.66%;"></td>
                        <td style="width: 16.66%;"></td>
                        <td style="width: 16.66%;"></td>
                        <td style="width: 16.66%;"></td>
                        <td style="width: 16.66%;">
                            <asp:Button ID="btnShow" runat="server" Text="แสดงรายการ" SkinID="primary" OnClick="btnShow_Click"/>
                        </td>
                        <td style="width: 16.66%;">
                            <asp:Button ID="btnExport" runat="server" Text="Export" SkinID="success" OnClick="btnExport_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%;" colspan="6">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="dgvMain" runat="server"
                                        AutoGenerateColumns="False"
                                        EmptyDataText="ไม่พบข้อมูล"
                                        AllowPaging="True" OnPageIndexChanging="dgvMain_PageIndexChanging">
                                        <Columns>
                                            <asp:BoundField DataField="ApproveDate" HeaderText="วันที่อนุมัติ" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="MotorApplication_Code" HeaderText="Application" />
                                            <asp:BoundField DataField="CustName" HeaderText="ผู้เอาประกัน" />
                                            <asp:BoundField DataField="PayerName" HeaderText="ผู้ชำระเบี้ย" />
                                            <asp:BoundField DataField="ProductTypeDetail" HeaderText="ประเภทผลิตภันฑ์" />
                                            <asp:BoundField DataField="ProductDetail" HeaderText="ผลิตภันฑ์" />
                                            <asp:BoundField DataField="PremiumPerMonth" HeaderText="เบี้ยรายเดือน" />
                                            <asp:BoundField DataField="PremiumPerYear" HeaderText="เบี้ยรายปี" />
                                            <asp:BoundField DataField="StartCover" HeaderText="วันที่เริ่มคุ้มครอง" DataFormatString="{0:dd/MM/yyyy}" />
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
