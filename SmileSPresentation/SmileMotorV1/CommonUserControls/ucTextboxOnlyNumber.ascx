<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTextboxOnlyNumber.ascx.cs" Inherits="SmileMotorV1.CommonUserControls.ucTextboxOnlyNumber" %>
<asp:TextBox ID="txtNumber" runat="server" CssClass="form-control input-sm" Width="100%"></asp:TextBox>
<ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxNumber" runat="server" FilterType="Numbers"  TargetControlID="txtNumber" />
