<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTextboxDecimalNumber.ascx.cs" Inherits="SmileMotorV1.CommonUserControls.ucTextboxDecimalNumber" %>
<asp:TextBox ID="txtPremium" runat="server" CssClass="form-control input-sm" Width="100%"></asp:TextBox>
<ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxNumber" runat="server" FilterType="Numbers , Custom" ValidChars="." TargetControlID="txtPremium" />
