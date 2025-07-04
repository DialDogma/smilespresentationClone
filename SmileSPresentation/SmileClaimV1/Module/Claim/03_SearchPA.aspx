<%@ Page Title="ค้นหา โครงการโรงเรียนห่วงใย" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="03_SearchPA.aspx.cs" Inherits="SmileClaimV1.Module.Claim._03_SearchPA" Theme="default" %>

<%@ Register Src="~/Module/Claim/UserControl/ucPASchoolDetailSearch.ascx" TagPrefix="uc1" TagName="ucPASchoolDetailSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:ucPASchoolDetailSearch runat="server" ID="ucPASchoolDetailSearch" />
</asp:Content>
