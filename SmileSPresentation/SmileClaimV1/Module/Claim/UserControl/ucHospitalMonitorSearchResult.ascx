<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHospitalMonitorSearchResult.ascx.cs" Inherits="SmileClaimV1.Module.Claim.UserControl.ucHospitalMonitorSearchResult" %>

<div class="container">
    <div class="panel panel-default">
        <div class="panel-heading input-md">
            <h4>
                <asp:Label ID="lblTextHeader" runat="server" Text="ผลการค้นหา"></asp:Label>
            </h4>
        </div>
        <div class="panel-body">
            <table style="width: 100%">
                <tr>
                    <td colspan="6">
                        <asp:GridView ID="dgvMain" runat="server" AutoGenerateColumns="false">
                            <Columns>
                                <asp:BoundField HeaderText="เลือก" />
                                <asp:BoundField HeaderText="เลขที่การแจ้ง" />
                                <asp:BoundField HeaderText="เลขที่กรมธรรม์" />
                                <asp:BoundField HeaderText="ชื่อ - สกุล" />
                                <asp:BoundField HeaderText="วันที่เข้ารับการรักษา" />
                                <asp:BoundField HeaderText="วันที่แจ้งใช้สิทธิ์" />
                                <asp:BoundField HeaderText="สถานะ" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
