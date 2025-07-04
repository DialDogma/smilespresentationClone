<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAddEditFirstPeriodPayment.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucAddEditFirstPeriodPayment" %>

<div class="panel panel-default">
    <div class="panel-heading">
        <h4 class="panel-title">การชำระเบี้ยงวดแรก</h4>
    </div>
    <div class="panel-body">
        <table style="width: 100%;">
            <tr>
                <td style="width: 12.5%; text-align: right;">วิธีการชำระเงิน :</td>
                <td style="width: 12.5%; text-align: right;">
                    <asp:DropDownList ID="ddlPaymentType" runat="server" >
                        <asp:ListItem>เงินสด</asp:ListItem>
                        <asp:ListItem>โอนเงิน</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="width: 12.5%; text-align: right;">&nbsp;</td>
                <td style="width: 12.5%; text-align: right;">เลขที่ใบเสร็จ :</td>
                <td style="width: 12.5%; text-align: right;">
                    <asp:TextBox ID="txtPaymentNumber" runat="server" placeholder="เลขที่ใบเสร็จ"></asp:TextBox>
                </td>
                <td style="width: 12.5%; text-align: right;">&nbsp;</td>
                <td style="width: 12.5%; text-align: right;">&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 12.5%; text-align: right;">จำนวนงวด :</td>
                <td style="width: 12.5%; text-align: right;">
                    <asp:DropDownList ID="ddlPeriodType" runat="server">
                        <asp:ListItem>1</asp:ListItem>
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>3</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>
                        <asp:ListItem>5</asp:ListItem>
                        <asp:ListItem>6</asp:ListItem>
                        <asp:ListItem>7</asp:ListItem>
                        <asp:ListItem>8</asp:ListItem>
                        <asp:ListItem>9</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>11</asp:ListItem>
                        <asp:ListItem>12</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="width: 12.5%; text-align: right;">&nbsp;</td>
                <td style="width: 12.5%; text-align: right;">จำนวนเงิน :</td>
                <td style="width: 12.5%; text-align: right;">
                    <asp:TextBox ID="txtPaymentTotal" runat="server" CssClass="form-control" Width="100%" placeholder="จำนวนเงินในใบเสร็จ"></asp:TextBox>
                </td>
                <td style="width: 12.5%; text-align: right;">&nbsp;</td>
                <td style="width: 12.5%; text-align: right;">&nbsp;</td>
            </tr>
        </table>
    </div>
</div>
