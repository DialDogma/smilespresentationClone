<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPHCustomerDetail.ascx.cs" Inherits="SmileClaimV1.Module.Claim.UserControl.ucPHCustomerDetail" %>

<div class="container">
    <div class="panel panel-default">
        <div class="panel-heading input-md">
            <h4>ข้อมูลผู้เอาประกัน[PH]</h4>
        </div>
        <div class="panel-body">
            <table style="width: 100%">
                <tr>
                    <td style="width: 12.5%; text-align: right">เลขที่อ้างอิง :</td>
                    <td style="width: 12.5%">
                        <asp:Label ID="lblAppID" runat="server" SkinID="result"></asp:Label></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>

                <tr>
                    <td style="width: 12.5%; text-align: right">ชื่อ-สกุล :</td>
                    <td style="width: 12.5%">
                        <asp:Label ID="lblName" runat="server" SkinID="result"></asp:Label>
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>

                <tr>
                    <td style="width: 12.5%; text-align: right">เลขบัตรประชาชน:</td>
                    <td style="width: 12.5%">
                        <asp:Label ID="lblIdentityNumbers" runat="server" SkinID="result"></asp:Label>
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>

                <tr>
                    <td style="width: 12.5%; text-align: right">วันเกิด :</td>
                    <td style="width: 12.5%">
                        <asp:Label ID="lblBirthDate" runat="server" SkinID="result"></asp:Label>
                    </td>
                    <td style="width: 12.5%; text-align: right">อายุ :</td>
                    <td style="width: 12.5%">
                        <asp:Label ID="lblAge" runat="server" SkinID="result"></asp:Label>
                    </td>
                    <td style="width: 12.5%; text-align: right">วันที่เริ่มคุ้มครอง : </td>
                    <td style="width: 12.5%">
                        <asp:Label ID="lblStrartCoverDate" runat="server" SkinID="result"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td style="width: 12.5%; text-align: right">สถานะกรมธรรม์ :</td>
                    <td style="width: 12.5%">
                        <asp:Label ID="lblStatus" runat="server" SkinID="result"></asp:Label>
                    </td>
                    <td style="width: 12.5%; text-align: right">โรคยกเว้น :</td>
                    <td style="width: 12.5%">
                        <asp:Label ID="DiseaseExcept" runat="server" SkinID="result"></asp:Label>
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>

                <tr>
                    <td style="width: 12.5%; text-align: right">
                        <asp:Label ID="lblRemark" runat="server" SkinID="result">หมายเหตุ :</asp:Label>
                    </td>
                    <td style="width: 12.5%">
                        <asp:Label ID="lblRemarkDetail" runat="server" SkinID="result"></asp:Label>
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>

                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%">
                        <asp:Label ID="lblRemarkDetail1" runat="server" SkinID="result"></asp:Label>
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
            </table>
        </div>
    </div>
</div>
<asp:HiddenField ID="hdfApplicationID" runat="server" />