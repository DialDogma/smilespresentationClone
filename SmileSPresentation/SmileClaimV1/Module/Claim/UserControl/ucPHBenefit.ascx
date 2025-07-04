<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPHBenefit.ascx.cs" Inherits="SmileClaimV1.Module.Claim.UserControl.ucPHBenefit" %>


<div class="container">
    <div class="panel panel-default">
        <div class="panel-heading input-md">
            <h4>
                <asp:Label ID="lblTextHeader" runat="server" Text="สิทธิประโยชน์[PH]"></asp:Label>
            </h4>
        </div>
        <div class="panel-body">
            <table style="width: 100%">
                <tr>
                    <td style="width: 12.5%; text-align: right;">OPD อุบัติเหตุ ครั้งละไม่เกิน :
                    </td>
                    <td style="width: 12.5%; text-align: right;">
                        <asp:Label ID="lblAccidentOverOPD" runat="server" SkinID="result"></asp:Label>
                    </td>
                    <td style="width: 12.5%; text-align: left;">บาท</td>
                    <td style="width: 12.5%;"></td>
                    <td style="width: 12.5%; text-align: right;"></td>
                    <td style="width: 12.5%;"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%; text-align: right;">OPD เจ็บป่วย ครั้งละไม่เกิน :
                    </td>
                    <td style="width: 12.5%; text-align: right;">
                        <asp:Label ID="lblSickOverOPD" runat="server" SkinID="result"></asp:Label>
                    </td>
                    <td style="width: 12.5%; text-align: left;">บาท</td>
                    <td style="width: 12.5%; text-align: right;">คงเหลือ :</td>
                    <td style="width: 12.5%; text-align: right;">
                        <asp:Label ID="lblSickOverCount" runat="server" SkinID="result"></asp:Label>
                    </td>
                    <td style="width: 12.5%; text-align: left;">ครั้ง</td>
                </tr>
            </table>
        </div>
    </div>
</div>
