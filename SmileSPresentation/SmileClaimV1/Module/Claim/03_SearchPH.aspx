<%@ Page Title="ค้นหา ประกันส่วนบุคคล" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="03_SearchPH.aspx.cs" Inherits="SmileClaimV1.Module.Claim._03_Search" Theme="default" %>

<%@ Register Src="~/Module/Claim/UserControl/ucPHCustomerDetailSearch.ascx" TagPrefix="uc1" TagName="ucPHCustomerDetailSearch" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:ucPHCustomerDetailSearch runat="server" ID="ucPHCustomerDetailSearch" />
</asp:Content>
