<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEmployeeSearch.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucEmployeeSearch" %>
<%--<script src="../../../Scripts/sss.js"></script>
<link href="../../../Content/meStyleSheet.css" rel="stylesheet" />--%>

<table style="width:100%">
    <tr>
        <td style="width: 25%; text-align: right;">รหัสพนักงาน :</td>
        <td style="width: 25%">
            <asp:TextBox ID="txtEmployeeCode" runat="server" MaxLength="5" AutoPostBack="true" OnTextChanged="txtEmployeeCode_TextChanged" ToolTip="รหัสพนักงาน 5 หลัก" placeholder="รหัสพนักงาน5หลัก"></asp:TextBox>
        </td>
        <td style="width: 12.5%">
            <asp:Button ID="btnPopup" runat="server" Text="ค้นหา" SkinID="info" />
            <ajaxToolkit:ModalPopupExtender ID="mpe" runat="server"
                TargetControlID="btnPopup" PopupControlID="ModalPanel" BackgroundCssClass="modalBackground" />
        </td>
        <td style="width: 37.5%">
            <asp:Label ID="lblFullName" runat="server"></asp:Label>
        </td>
    </tr>
</table>

<asp:Panel ID="ModalPanel" runat="server" Width="500px" CssClass="modalPopup" align="center" Style="display: none">

    <table style="width: 100%">
        <tr>
            <td style="width: 25%; text-align: right;">
                <asp:Label ID="lblTitle" runat="server" Text="คำค้นหา :" CssClass="h4"></asp:Label>
            </td>
            <td style="width: 30%;">
                <asp:TextBox ID="txtSearch" runat="server" ToolTip="ค้นหาโดยชื่อ หรือ รหัสพนักงาน" placeholder="ค้นหาโดยชื่อ / รหัส"></asp:TextBox>
            </td>
            <td style="width: 25%;">
                <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="ค้นหา" />
            </td>
            <td style="width: 20%;">&nbsp;</td>
        </tr>
    </table>

    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <asp:GridView ID="dgvMain" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="dgvMain_SelectedIndexChanged" OnRowDataBound="OnRowDataBound">
                <Columns>
                    <asp:BoundField DataField="EmployeeCode" HeaderText="รหัสพนักงาน" />
                    <asp:BoundField DataField="FirstName" HeaderText="ชื่อ" />
                    <asp:BoundField DataField="LastName" HeaderText="สกุล" />
                    <asp:BoundField DataField="NickName" HeaderText="ชื่อเล่น" />
                    <asp:BoundField DataField="BranchDetail" HeaderText="สาขา" />
                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
            <asp:PostBackTrigger ControlID="dgvMain" />
        </Triggers>
    </asp:UpdatePanel>
    <table style="width: 100%">
        <tr>
            <td style="text-align: center">
                <asp:Button ID="btnClose" runat="server" Text="ปิด" />
            </td>
        </tr>
    </table>
</asp:Panel>
