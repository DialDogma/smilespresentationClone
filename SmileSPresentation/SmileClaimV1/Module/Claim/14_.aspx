<%@ Page Title="ค้นหาเช็คสิทธิ์โรงพยาบาล" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="14_.aspx.cs" Inherits="SmileClaimV1.Module.Claim._14_" Theme="default" %>

<%@ Register Src="~/Module/Claim/UserControl/ucClaimHCIDetail.ascx" TagPrefix="uc1" TagName="ucClaimHCIDetail" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:ucClaimHCIDetail runat="server" ID="ucClaimHCIDetail" />
</asp:Content>
