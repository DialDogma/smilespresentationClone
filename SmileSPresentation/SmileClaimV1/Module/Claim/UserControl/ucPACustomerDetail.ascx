<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPACustomerDetail.ascx.cs" Inherits="SmileClaimV1.Module.Claim.UserControl.ucPACustomerDetail" %>

<div class="container">
    <div class="panel panel-default">
        <div class="panel-heading input-md">
            <h4>ข้อมูลผู้เอาประกัน[PA]</h4>
        </div>
        <div class="panel-body">
            <table style="width: 100%">
                <tr>
                    <td style="width: 12.5%" class="right">ปีการศึกษา :</td>
                    <td style="width: 12.5%">
                        <asp:Label ID="lblSchoolYear" runat="server"  SkinID="result" ></asp:Label>
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%" class="right">ชื่อ-สกุล ผู้เอาประกัน :</td>
                    <td style="width: 12.5%">
                        <asp:Label ID="lblName" runat="server" SkinID="result"></asp:Label>
                    </td>
                    <td style="width: 12.5%" class="right">เลขที่อ้างอิง :</td>
                    <td style="width: 12.5%">
                        <asp:Label ID="lblAppID" runat="server" SkinID="result"></asp:Label>
                    </td>
                    <td style="width: 12.5%" class="right">ประเภทผู้เอาประกัน :</td>
                    <td style="width: 12.5%">
                        <asp:Label ID="lblCustomerType" runat="server" SkinID="result"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 12.5%" class="right">สถานศึกษา :</td>
                    <td style="width: 12.5%">
                        <asp:Label ID="lblSchoolName" runat="server" SkinID="result" ></asp:Label>
                    </td>
                    <td style="width: 12.5%" class="right">ระดับชั้น :</td>
                    <td style="width: 12.5%">
                        <asp:Label ID="lblLevelRoom" runat="server" SkinID="result"></asp:Label>
                    </td>
                    <td style="width: 12.5%" class="right">ตำแหน่ง :</td>
                    <td style="width: 12.5%">
                     <asp:Label ID="lblPosition" runat="server" SkinID="result"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 12.5%" class="right">วันเริ่มความคุ้มครอง :</td>
                    <td style="width: 12.5%">
                        <asp:Label ID="lblStartCoverDate" runat="server" SkinID="result"></asp:Label>
                    </td>
                    <td style="width: 12.5%" class="right">วันสิ้นสุดความคุ้มครอง :</td>
                    <td style="width: 12.5%">
                        <asp:Label ID="lblEndCoverDate" runat="server" SkinID="result"></asp:Label>
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
            </table>
        </div>
    </div>
</div>
<asp:HiddenField ID="hdfCustomerID" runat="server" />
<asp:HiddenField ID="hdfApplicationID" runat="server" />