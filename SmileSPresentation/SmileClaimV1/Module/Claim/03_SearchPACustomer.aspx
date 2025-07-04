<%@ Page Title="ค้นหาข้อมูลผู้เอาประกัน PA" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="03_SearchPACustomer.aspx.cs" Inherits="SmileClaimV1.Module.Claim._03_SearchPACustomer" Theme="default" %>

<%@ Register Src="~/Module/Claim/UserControl/ucPACustomerDetailSearch.ascx" TagPrefix="uc1" TagName="ucPACustomerDetailSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:ucPACustomerDetailSearch runat="server" ID="ucPACustomerDetailSearch" />
</asp:Content>
