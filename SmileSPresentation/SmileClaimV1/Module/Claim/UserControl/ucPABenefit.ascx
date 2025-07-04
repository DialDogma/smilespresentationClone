<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPABenefit.ascx.cs" Inherits="SmileClaimV1.Module.Claim.UserControl.ucPABenefit" %>


<div class="container">
    <div class="panel panel-default">
        <div class="panel-heading input-md">
            <h4>
                <asp:Label ID="lblTextHeader" runat="server" Text="สิทธิประโยชน์[PA]"></asp:Label>
            </h4>
        </div>
        <div class="panel-body">
            <table style="width: 100%">
                <tr>
                    <td style="width: 25%; text-align: right;" colspan="2">
                        ค่ารักษาพยาบาลอุบัติเหตุต่อครั้ง :
                    </td>
                    <td style="width: 12.5%;text-align: right;">
                        <asp:Label ID="lblAccidentPerTime" runat="server" SkinID="result"></asp:Label>
                    </td>
                    <td style="width: 12.5%; text-align: left;">บาท</td>
                    <td style="width: 12.5%;"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%; text-align: right;" colspan="2">
                        ค่ารักษาพยาบาลกรณีเจ็บป่วยสุขภาพ/โรค/ครั้ง :
                    </td>
                    <td style="width: 12.5%;text-align: right;">
                        <asp:Label ID="lblSickPerTime" runat="server" SkinID="result"></asp:Label>
                    </td>
                    <td style="width: 12.5%; text-align: left;">บาท</td>
                    <td style="width: 12.5%;"></td>
                </tr>
            </table>
        </div>
    </div>
</div>
