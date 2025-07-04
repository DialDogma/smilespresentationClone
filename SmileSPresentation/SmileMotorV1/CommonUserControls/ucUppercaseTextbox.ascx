<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucUppercaseTextbox.ascx.cs" Inherits="SmileMotorV1.CommonUserControls.ucUppercaseTextbox" %>
<script src="<%= ResolveClientUrl("~/Scripts/jquery-3.1.1.min.js")%>"></script>
<script src="<%= ResolveClientUrl("~/JQueryUI/scripts/jquery-ui.js")%>"></script>

<table style="width: 100%;padding: 0; margin: 0; border: 0;">
    <tr>
        <td style="width: 50%;">
            <asp:TextBox ID="txtUppercase" runat="server" CssClass="form-control input-sm" Width="100%"></asp:TextBox>

        </td>
        <td style="width: 50%">
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                ErrorMessage="*"
                ValidationExpression="^[a-zA-Z0-9]+$"
                ControlToValidate="txtUppercase"
                ForeColor="Red">
            </asp:RegularExpressionValidator>
        </td>
    </tr>
</table>

