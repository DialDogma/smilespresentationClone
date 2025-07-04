<%@ Page Title="" Language="C#" MasterPageFile="~/Master1.Master" AutoEventWireup="true" CodeBehind="frmAddRoleToUsers.aspx.cs" Inherits="SmileSDataCenterV1.Module.Identity.frmAddRoleToUsers" %>
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

        div {
            text-align: center;
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
    <%-- Form --%>
    <asp:PlaceHolder runat="server" ID="successMessage" Visible="false" ViewStateMode="Disabled">
        <p class="text-success"><%: SuccessMessage %></p>
    </asp:PlaceHolder>
    <asp:PlaceHolder runat="server" ID="errorMessage" Visible="false" ViewStateMode="Disabled">
        <p class="text-danger"><%:ErrorMessage %></p>
    </asp:PlaceHolder>
    <div class="page-header">
        <h3>เพิ่มผู้ใช้</h3>
    </div>

    <%-- end Form --%>
</asp:Content>
