<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAppDetailFirstPeriodPayment.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucAppDetailFirstPeriodPayment" %>
<div class="panel panel-default">
    <div class="panel-heading">
        <h4 class="panel-title">การชำระเบี้ยงวดแรก</h4>
    </div>
    <div class="panel-body">
        <table style="width: 100%;">
            <tr>
                <td style="width: 12.5%; text-align: right;">วิธีการชำระเงิน :</td>
                <td style="width: 12.5%;">
                    <asp:Label ID="lblPaymentPeriodType" runat="server" SkinID="result"></asp:Label>
                </td>
                <td style="width: 12.5%; text-align: right;">&nbsp;</td>
                <td style="width: 12.5%; text-align: right;">เลขที่ใบเสร็จ :</td>
                <td style="width: 12.5%;">
                    <asp:Label ID="lblReceiptNo" runat="server" SkinID="result"></asp:Label>
                </td>
                <td style="width: 12.5%; text-align: right;">&nbsp;</td>
                <td style="width: 12.5%; text-align: right;">&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 12.5%; text-align: right;">จำนวนงวด :</td>
                <td style="width: 12.5%">
                    <asp:Label ID="lblAmountPeriod" runat="server" SkinID="result"></asp:Label>
                </td>
                <td style="width: 12.5%; text-align: right;">&nbsp;</td>
                <td style="width: 12.5%; text-align: right;">จำนวนเงิน :</td>
                <td style="width: 12.5%">
                    <asp:Label ID="lblAmountMoney" runat="server" SkinID="result"></asp:Label>
                </td>
                <td style="width: 12.5%; text-align: right;">&nbsp;</td>
                <td style="width: 12.5%; text-align: right;">&nbsp;</td>
            </tr>
        </table>
    </div>
</div>
