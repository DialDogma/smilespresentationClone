<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSearchCustomer.ascx.cs" Inherits="SmileClaimV1.Module.Claim.UserControl.ucSearchCustomer" %>

<style>
    .center {
        text-align: center;
    }

    .left {
        text-align: left;
    }

    .right {
        text-align: right;
    }
</style>

<div class="container">
    <div class="panel panel-default">
        <div class="panel-heading input-md">
            <h4>
                <asp:Label ID="lblTextHeader" runat="server" Text="ค้นหาผู้เอาประกัน"></asp:Label>
            </h4>
        </div>
        <div class="panel-body">
            <table style="width: 100%">
                <tr>
                    <td style="width: 12.5%" class="right">ประเภท :</td>
                    <td style="width: 12.5%">
                        <asp:DropDownList ID="ddlProductType" runat="server" OnSelectedIndexChanged="ddlProductType_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%" class="right">คำค้นหา :</td>
                    <td style="width: 12.5%">
                        <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
                    </td>
                    <td style="width: 12.5%">
                        <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" SkinID="info" OnClick="btnSearch_Click" />
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
            </table>
        </div>
    </div>
</div>
