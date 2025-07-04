<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="02_Main.aspx.cs" Inherits="SmileClaimV1.Module.Claim._02_Main" %>

<%@ Register Src="~/Module/Claim/UserControl/ucMain.ascx" TagPrefix="uc1" TagName="ucMain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:ucMain runat="server" id="ucMain" />
</asp:Content>
