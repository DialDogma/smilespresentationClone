<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucMain.ascx.cs" Inherits="SmileClaimV1.Module.Claim.UserControl.ucMain" %>

<style>
    .center {
        text-align: center;
    }

    .left {
        text-align: left;
    }

    .right {
        text-align: right;
    }

</style>

<div class="container" style ="margin-top:30px">
    <table style="width: 100%">
        <tr>
            <td colspan="8" class="center">
                <asp:Label ID="lblWelcome" runat="server" Text="ระบบ Smile Claim ยินดีต้อนรับ" Font-Size="X-Large"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="8" class="center">
                <img src="../../../Image/logoSiamSmile.jpg" />
            </td>
        </tr>
        <tr>
            <td colspan="8" class="center">
                <asp:Label ID="lblContact" runat="server" Text="หากท่านพบปัญหาการใช้งาน กรุณาติดต่อ 000000 หรือ Download คู่มือ" Font-Size="Medium"></asp:Label>
                <asp:LinkButton ID="lnkDownload" runat="server" Text="ที่นี่" Font-Size="Large" Font-Bold="true"></asp:LinkButton>
            </td>
        </tr>
    </table>
</div>
