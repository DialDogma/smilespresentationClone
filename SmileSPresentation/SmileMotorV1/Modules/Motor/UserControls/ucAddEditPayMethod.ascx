<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAddEditPayMethod.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucAddEditPayMethod" %>
<%@ Register Src="DropdownUserControls/ddlBankCompany.ascx" TagName="ddlBankCompany" TagPrefix="uc3" %>
<%@ Register Src="~/Modules/Motor/UserControls/DropdownUserControls/ddlPeriodType.ascx" TagPrefix="uc3" TagName="ddlPeriodType" %>
<%@ Register Src="~/Modules/Motor/UserControls/DropdownUserControls/ddlPayMethodType.ascx" TagPrefix="uc3" TagName="ddlPayMethodType" %>
<%@ Register Src="DropdownUserControls/ddlBankAccountNo.ascx" TagName="ddlBankAccountNo" TagPrefix="uc1" %>
<%@ Register Src="ucAddBankAccount.ascx" TagName="ucAddBankAccount" TagPrefix="uc2" %>
<%@ Register Src="~/Modules/Motor/UserControls/DropdownUserControls/ddlBankAccountNo.ascx" TagPrefix="uc2" TagName="ddlBankAccountNo" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAddBankAccount.ascx" TagPrefix="uc1" TagName="ucAddBankAccount" %>

<div class="panel panel-default">
    <div class="panel-heading">
        <h4 class="panel-title">
            <asp:Label runat="server" ID="lblHeader" Text="วิธีการชำระเงินต่ออายุ"></asp:Label></h4>
    </div>
    <div class="panel-body">
        <div class="table-responsive">

            <asp:UpdatePanel ID="up1" runat="server">
                <ContentTemplate>

                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 12.5%; text-align: right;">
                                <asp:Label runat="server" ID="lblPeriod" Text="งวดชำระ :"></asp:Label>
                            </td>
                            <td style="width: 12.5%;">
                                <uc3:ddlPeriodType ID="ddlPeriodType" runat="server" OnSelectChanged="ddlPeriodType_SelectChanged" />
                            </td>
                            <td style="width: 25%;" colspan="2">
                                <asp:Label ID="lblWarning" runat="server" SkinID="info" Text="การชำระเงินงวดแรก บันทึกในโปรแกรม Cash Receive"></asp:Label>
                            </td>
                            <td style="width: 12.5%;"></td>
                            <td style="width: 12.5%;">
                                <uc1:ucAddBankAccount runat="server" ID="ucAddBankAccount" OneClickBankAccountSave="ucAddBankAccount_eClickBankAccountSave" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 12.5%; text-align: right;">วิธีการชำระเงิน :</td>
                            <td style="width: 12.5%;">
                                <uc3:ddlPayMethodType ID="ddlPayMethodType" runat="server" OnSelectChanged="ddlPayMethodType_SelectChanged" />
                            </td>
                            <td style="width: 12.5%; text-align: right;">ธนาคาร :</td>
                            <td style="width: 12.5%;">
                                <uc3:ddlBankCompany ID="ddlBankCompany" runat="server" OneBankCompanySelect="ddlBankCompany_eBankCompanySelect" />
                            </td>
                            <td style="width: 12.5%; text-align: right;">เลขที่บัญชี :</td>
                            <td style="width: 12.5%;">
                                <uc2:ddlBankAccountNo runat="server" ID="ddlBankAccountNo" />
                            </td>
                        </tr>
                        <tr id="trPremium" runat="server">
                            <td style="width: 12.5%; text-align: right;">เบี้ยประกันสุทธิ:
                            </td>
                            <td style="width: 12.5%;">
                                <asp:TextBox ID="txtPremiumAmount" runat="server" Text="0.00"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="txtPremiumAmount_FilteredTextBoxExtender"
                                    runat="server"
                                    BehaviorID="txtPremiumAmount_FilteredTextBoxExtender"
                                    TargetControlID="txtPremiumAmount"
                                    FilterType="Custom, Numbers"
                                    ValidChars=".," />
                            </td>
                            <td style="width: 12.5%; text-align: right;">เบี้ยประกันรวมภาษีอากร:
                            </td>
                            <td style="width: 12.5%;">
                                <asp:TextBox ID="txtPremiumTaxAmount" runat="server" Text="0.00"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="txtPremiumTaxAmount_FilteredTextBoxExtender"
                                    runat="server"
                                    BehaviorID="txtPremiumTaxAmount_FilteredTextBoxExtender"
                                    TargetControlID="txtPremiumTaxAmount"
                                    FilterType="Custom, Numbers"
                                    ValidChars=".," />
                            </td>
                            <td style="width: 12.5%; text-align: right;">เบี้ยประกันนำส่ง:
                            </td>
                            <td style="width: 12.5%;">
                                <asp:TextBox ID="txtPremiumDeliverAmount" runat="server" Text="0.00"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="txtPremiumDeliverAmount_FilteredTextBoxExtender"
                                    runat="server"
                                    BehaviorID="txtPremiumDeliverAmount_FilteredTextBoxExtender"
                                    TargetControlID="txtPremiumDeliverAmount"
                                    FilterType="Custom, Numbers"
                                    ValidChars=".," />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
<asp:HiddenField ID="hdfPayerID" runat="server" />