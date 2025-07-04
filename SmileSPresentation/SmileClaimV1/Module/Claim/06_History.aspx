<%@ Page Title="ประวัติการใช้สิทธิ์" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="06_History.aspx.cs" Inherits="SmileClaimV1.Module.Claim._06_History" Theme="default" %>

<%@ Register Src="~/Module/Claim/UserControl/ucSearchConfirmClaimPrivilege.ascx" TagPrefix="uc1" TagName="ucSearchConfirmClaimPrivilege" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:ucSearchConfirmClaimPrivilege runat="server" ID="ucSearchConfirmClaimPrivilege" />
</asp:Content>
