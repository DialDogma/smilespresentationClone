<%@ Page Title="" Language="C#" MasterPageFile="~/Master1.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SmileSDataCenterV1.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- Style --%>
    <style>
        .panel-heading {
            padding: 5px 15px;
        }

        .panel-footer {
            padding: 1px 15px;
            color: #A0A0A0;
        }

        .profile-img {
            width: 96px;
            height: 96px;
            margin: 0 auto 10px;
            display: block;
            -moz-border-radius: 50%;
            -webkit-border-radius: 50%;
            border-radius: 50%;
        }
    </style>
    <%--end Style --%>
    <%-- Form ก็อบมาจาก SmileClaim ของเจมส์กะบูม --%>
    <div class="container" style ="margin-top:30px">
    <table style="width: 100%">
        <tr>
            <td colspan="8" class="center">
                <asp:Label ID="lblWelcome" runat="server" Text="Smile Data Center" Font-Size="X-Large"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="8" class="center">
                <img src="../../../Image/logoSiamSmile.jpg" />
            </td>
        </tr>
        <tr>
            <td colspan="8" class="center">
                <asp:Label ID="lblContact" runat="server" Text="หากท่านมีปัญหาการใช้งาน กรุณาติดต่อ 1150 หรือ Download คู่มือ" Font-Size="Medium"></asp:Label>
                <asp:LinkButton ID="lnkDownload" runat="server" Text="ที่นี่" Font-Size="Large" Font-Bold="true"></asp:LinkButton>
            </td>
        </tr>
    </table>
</div>
    <%-- end Form --%>
</asp:Content>
