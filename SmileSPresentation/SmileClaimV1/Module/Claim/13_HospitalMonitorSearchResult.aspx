<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="13_HospitalMonitorSearchResult.aspx.cs" Inherits="SmileClaimV1.Module.Claim._13_HospitalMonitorSearchResult" %>

<%@ Register Src="~/Module/Claim/UserControl/ucHospitalMonitorSearchResult.ascx" TagPrefix="uc1" TagName="ucHospitalMonitorSearchResult" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <uc1:ucHospitalMonitorSearchResult runat="server" ID="ucHospitalMonitorSearchResult" />

</asp:Content>
