<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPolicyNumbersAdd.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucPolicyNumbersAdd" %>
<div class="panel panel-default">
    <div class="panel-heading">
        <h4 class="panel-title">เพิ่มเลขที่กรมธรรม์</h4>
    </div>
    <div class="panel-body">
        <div class="table-responsive">

            <table style="width: 100%">
                <tr>
                    <td style="width: 12.5%; text-align: right">&nbsp;</td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%; text-align: right">เลขที่กรมธรรม์</td>
                    <td colspan="5">
                        <asp:TextBox ID="txtPolicyNumber" runat="server" Width="100%"></asp:TextBox>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td colspan="3" style="text-align: center">
                        <asp:Label ID="lblWarning" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%">
                        <asp:HiddenField ID="hdfMotorApplication_ID" runat="server" />
                    </td>
                    <td style="width: 12.5%"></td>
                    <td colspan="3" style="text-align: center"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
            </table>
        </div>
    </div>
</div>
