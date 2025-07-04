<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="09_DetailConfirmClaim.aspx.cs" Inherits="SmileClaimV1.Module.Claim._09_DetailConfirmClaim" Theme="default" %>

<%@ Register Src="~/Module/Claim/UserControl/ucClaimPrivilegeDetail.ascx" TagPrefix="uc1" TagName="ucClaimPrivilegeDetail" %>
<%@ Register Src="~/Module/Claim/UserControl/ucPHCustomerDetail.ascx" TagPrefix="uc1" TagName="ucPHCustomerDetail" %>
<%@ Register Src="~/Module/Claim/UserControl/ucPACustomerDetail.ascx" TagPrefix="uc1" TagName="ucPACustomerDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table style="width: 100%;">
        <tr>
            <td colspan="8">
                <uc1:ucClaimPrivilegeDetail runat="server" ID="ucClaimPrivilegeDetail" />
                <uc1:ucPHCustomerDetail runat="server" ID="ucPHCustomerDetail" />
                <uc1:ucPACustomerDetail runat="server" ID="ucPACustomerDetail" />
            </td>
        </tr>
    </table>
    <div class="container">


        <table style="width: 100%">
            <tr>
                <td style="width: 12.5%">
                    <asp:Button ID="btnPrivilegeConfirmPrint" runat="server" Text="พิมพ์เอกสารยืนยันการใช้สิทธิ์" SkinID="success"/>
                </td>
                <td style="width: 12.5%">
                    <asp:Button ID="btnPrint" runat="server" Text="พิมพ์หนังสือรับรองการจ่าย" SkinID="info" />
                </td>
                <td style="width: 12.5%"></td>
                <td style="width: 12.5%"></td>
                <td style="width: 12.5%"></td>
                <td style="width: 12.5%"></td>
                <td style="width: 12.5%"></td>
                <td style="width: 12.5%"></td>
            </tr>
        </table>
    </div>
</asp:Content>
