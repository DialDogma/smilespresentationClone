<%@ Page Title="รายละเอียดข้อมูลผู้เอาประกัน" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="04_Detail.aspx.cs" Inherits="SmileClaimV1.Module.Claim._04_Detail" Theme="default" %>

<%@ Register Src="~/Module/Claim/UserControl/ucPHCustomerDetail.ascx" TagPrefix="uc1" TagName="ucPHCustomerDetail" %>
<%@ Register Src="~/Module/Claim/UserControl/ucPACustomerDetail.ascx" TagPrefix="uc1" TagName="ucPACustomerDetail" %>
<%@ Register Src="~/CommonUserControl/ucDatepicker.ascx" TagPrefix="uc1" TagName="ucDatepicker" %>
<%@ Register Src="~/Module/Claim/UserControl/ucPHBenefit.ascx" TagPrefix="uc1" TagName="ucPHBenefit" %>
<%@ Register Src="~/Module/Claim/UserControl/ucOpenClaim.ascx" TagPrefix="uc1" TagName="ucOpenClaim" %>
<%@ Register Src="~/Module/Claim/UserControl/ucPABenefit.ascx" TagPrefix="uc1" TagName="ucPABenefit" %>
<%@ Register Src="~/CommonUserControl/ucAlert.ascx" TagPrefix="uc1" TagName="ucAlert" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:ucAlert runat="server" ID="ucAlert" />
    <uc1:ucPHCustomerDetail runat="server" ID="ucPHCustomerDetail" />
    <uc1:ucPACustomerDetail runat="server" ID="ucPACustomerDetail" />
    <div class="container">
        <div class="panel panel-default">
            <div class="panel-heading input-md">
                <h4>
                    <asp:Label ID="lblTextHeader" runat="server" Text="วันที่เกิดเหตุ"></asp:Label>
                </h4>
            </div>
            <div class="panel-body">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 12.5%" class="right">
                            วันเกิดเหตุ :
                        </td>
                        <td style="width: 12.5%">
                            <uc1:ucDatepicker runat="server" ID="ucDatepicker" AutoPostback="true" />
                        </td>
                        <td style="width: 12.5%">&nbsp;</td>
                        <td style="width: 12.5%">&nbsp;</td>
                        <td style="width: 12.5%">&nbsp;</td>
                        <td style="width: 12.5%"></td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <uc1:ucPHBenefit runat="server" ID="ucPHBenefit" />
    <uc1:ucPABenefit runat="server" ID="ucPABenefit" />
    <uc1:ucOpenClaim runat="server" ID="ucOpenClaim" />
    <div class="container">
        <table style="width: 100%">
            <tr>
                <td style="width: 12.5%">
                    <asp:Button ID="btnHome" runat="server" SkinID="danger" Text="กลับหน้าหลัก" PostBackUrl="~/Module/Claim/02_Main.aspx" />
                </td>
                <td style="width: 12.5%">
                    <asp:HiddenField ID="hdfProductGroupID" runat="server" />
                </td>
                <td style="width: 12.5%"></td>
                <td style="width: 12.5%"></td>
                <td style="width: 12.5%"></td>
                <td style="width: 12.5%">
                    <asp:Button ID="btnConfirm" runat="server" SkinID="primary" Text="ยืนยันการใช้สิทธิ์" OnClick="btnConfirm_Click" /></td>
            </tr>
        </table>
    </div>

</asp:Content>
