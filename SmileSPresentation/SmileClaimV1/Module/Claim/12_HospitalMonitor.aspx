<%@ Page Title="Monitor เช็คสิทธิ์โรงพยาบาล" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="12_HospitalMonitor.aspx.cs" Inherits="SmileClaimV1.Module.Claim._12_HospitalMonitor" Theme="default" %>

<%@ Register Src="~/Module/Claim/UserControl/ucHospitalMonitorSearch.ascx" TagPrefix="uc1" TagName="ucHospitalMonitorSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <uc1:ucHospitalMonitorSearch runat="server" ID="ucHospitalMonitorSearch" />

</asp:Content>
