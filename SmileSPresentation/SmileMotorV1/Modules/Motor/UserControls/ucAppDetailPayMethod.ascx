<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAppDetailPayMethod.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucAppDetailPayMethod" %>
<link href="../../../Content/meStyleSheet.css" rel="stylesheet" />
<ajaxToolkit:Accordion ID="Accordion1"
    HeaderCssClass="accordionHeader"
    ContentCssClass="accordionContent"
    runat="server"
    FadeTransitions="true"
    TransitionDuration="500"
    AutoSize="None"
    SelectedIndex="0"
    RequireOpenedPane="false"
    SuppressHeaderPostbacks="false" Height="100%" Width="100%">
    <Panes>
        <ajaxToolkit:AccordionPane ID="Pane1" runat="server">
            <Header>
                <h4>วิธีการชำระเงินต่ออายุ</h4>
            </Header>
            <Content>
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 12.5%; text-align: right;">งวดชำระ :</td>
                        <td style="width: 12.5%;">
                             <asp:Label ID="lblPeriod" runat="server" SkinID="result"></asp:Label>
                         
                        </td>
                        <td style="width: 12.5%; text-align: right;">วิธีการชำระเงิน :</td>
                        <td style="width: 12.5%;">  <asp:Label ID="lblPaymentPeriodType" runat="server" SkinID="result"></asp:Label></td>
                        <td style="width: 12.5%; text-align: right;"></td>
                        <td style="width: 12.5%;"></td>
                    </tr>
                    <tr>
                        <td style="width: 12.5%; text-align: right;">ธนาคาร :</td>
                        <td style="width: 12.5%;">
                            <asp:Label ID="lblBank" runat="server" SkinID="result"></asp:Label>
                        </td>
                        <td style="width: 12.5%; text-align: right;">ชื่อบัญชี :</td>
                        <td style="width: 12.5%;">
                            <asp:Label ID="lblAccountName" runat="server" SkinID="result"></asp:Label>
                        </td>
                        <td style="width: 12.5%; text-align: right;">เลขที่บัญชี :</td>
                        <td style="width: 12.5%">
                            <asp:Label ID="lblAccountNumber" runat="server" SkinID="result"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 12.5%; text-align: right;">เบี้ยประกันสุทธิ :</td>
                        <td style="width: 12.5%">
                            <asp:Label ID="lblPremiumAmount" runat="server" SkinID="result"></asp:Label>
                        </td>
                        <td style="width: 12.5%; text-align: right;">เบี้ยประกันรวมภาษีอากร :</td>
                        <td style="width: 12.5%">
                            <asp:Label ID="lblPremiumTaxAmount" runat="server" SkinID="result"></asp:Label>
                        </td>
                        <td style="width: 12.5%; text-align: right;">เบี้ยประกันนำส่ง :</td>
                        <td style="width: 12.5%">
                            <asp:Label ID="lblPremiumDeliverAmount" runat="server" SkinID="result"></asp:Label>
                        </td>
                    </tr>
                </table>
            </Content>
        </ajaxToolkit:AccordionPane>
    </Panes>
</ajaxToolkit:Accordion>

