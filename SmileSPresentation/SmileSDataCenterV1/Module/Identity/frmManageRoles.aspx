<%@ Page Title="" Language="C#" MasterPageFile="~/Master1.Master" AutoEventWireup="true" CodeBehind="frmManageRoles.aspx.cs" Inherits="SmileSDataCenterV1.Module.Identity.frmManageRoles" Theme="default" %>

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
    <div class="container" style="margin-top: 30px">
        <div class="row">
            <div class="colspan8">
                <h3>จัดการ Roles</h3>
            </div>
            <div class="span8" style="text-align: left">
                <h4>เพิ่ม Role</h4>
            </div>
            <div class="row">
                <div class="col-md-3">
                    <asp:TextBox runat="server" ID="txtAddRole" placeholder="Role Name"></asp:TextBox>
                </div>
                <div class="col-md-3">
                    <asp:Button runat="server" ID="btnAddRole" Text="เพิ่ม Role" SkinID="success" OnClick="btnAddRole_Click" />
                </div>
                <div class="col-md-2">
                    <asp:Label runat="server" ID="lblResult"></asp:Label>
                </div>
            </div>
        </div>
        <table style="width: 100%">
            <tr>
                <td colspan="8" class="center">
                    <div class="row">

                        <div class="span4"></div>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="8" class="center">
                    <asp:GridView runat="server" ID="dgvAllRoles" AutoGenerateColumns="false" EmptyDataText="ไม่พบข้อมูล" DataKeyNames="Name" OnSelectedIndexChanged="dgvAllRoles_SelectedIndexChanged">
                        <Columns>
                            <asp:BoundField DataField="Name" HeaderText="Role" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Id" HeaderText="RoleID" ItemStyle-HorizontalAlign="Center" />
                            <asp:ButtonField CommandName="Select" HeaderText="เลือก" Text="เลือก" ItemStyle-HorizontalAlign="Center" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
        <div class="row">
            <div class="col-md-3" style="float:right">
                <asp:PlaceHolder runat="server" ID="plhButton" Visible="false">
                    <asp:Button runat="server" ID="btnDeleteRole" Text="Delete Role" SkinID="danger" OnClick="deleteRole" />
                </asp:PlaceHolder>
            </div>
        </div>
    </div>
    <%-- end Form --%>
</asp:Content>
