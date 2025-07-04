<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHospitalMonitorSearch.ascx.cs" Inherits="SmileClaimV1.Module.Claim.UserControl.ucHospitalMonitorSearch" %>
<%@ Register Src="~/CommonUserControl/ucDatepicker.ascx" TagPrefix="uc1" TagName="ucDatepicker" %>


<div class="container">
    <div class="panel panel-default">
        <div class="panel-heading input-md">
            <h4>
                <asp:Label ID="lblTextHeader" runat="server" Text="ค้นหาการแจ้งยืนยันใช้สิทธิ์"></asp:Label>
            </h4>
        </div>
        <div class="panel-body">
            <table style="width: 100%">
                <tr>
                    <td style="width: 12.5%; text-align: right;">
                        วันที่ - ถึงวันที่ :
                    </td>
                    <td style="width: 12.5%">
                        <uc1:ucDatepicker runat="server" ID="ucDateFrom" />
                    </td>
                    <td style="width: 12.5%">
                        <uc1:ucDatepicker runat="server" ID="ucDateTo" />
                    </td>
                    <td style="width: 12.5%; text-align: right;"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%; text-align: right;">
                        ประเภทการเข้ารักษา :
                    </td>
                    <td style="width: 12.5%" colspan="2">
                        <asp:DropDownList ID="ddlCureType" runat="server"></asp:DropDownList>
                    </td>
                    <td style="width: 12.5%">
                        <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" OnClick="btnSearch_Click" SkinID="primary" />
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%" colspan="6">
                        <asp:GridView ID="dgvMain" runat="server" AutoGenerateColumns="false">
                            <Columns>
                                <asp:BoundField HeaderText="วันที่" />
                                <asp:BoundField HeaderText="ยืนยันใช้สิทธิ์" />
                                <asp:BoundField HeaderText="บันทึกหนังสือรับรองการจ่ายเสร็จสิ้น" />
                                <asp:BoundField HeaderText="ยกเลิกการใช้สิทธิ์" />
                                <asp:BoundField HeaderText="คงค้าง" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
