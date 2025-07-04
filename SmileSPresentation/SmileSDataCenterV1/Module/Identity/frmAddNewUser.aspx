<%@ Page Title="" Language="C#" MasterPageFile="~/Master1.Master" AutoEventWireup="true" CodeBehind="frmAddNewUser.aspx.cs" Inherits="SmileSDataCenterV1.Module.Identity.frmUserManager" %>

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
    <div class="container" style="margin-top: 30px">
        <div class="row">
            <div class="col-sm-6 col-md-4 col-md-offset-4">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <strong>New User</strong>
                    </div>
                    <div class="panel-body">
                        <fieldset>
                            <div class="row">
                                <div class="center-block">
                                    <img class="profile-img"
                                        src="https://cdn1.iconfinder.com/data/icons/unique-round-blue/93/user-256.png" alt="" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-md-10  col-md-offset-1 ">
                                    <div class="form-group">
                                        <div class="input-group">
                                            <span class="input-group-addon">
                                                <i class="glyphicon glyphicon-user"></i>
                                            </span>
                                            <input class="form-control" placeholder="Username" name="NewUserName" type="text" id="Username" runat="server" required="required" autofocus="autofocus" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Button runat="server" ID="btnValidate" Text="ตรวจสอบชื่อผู้ใช้" CssClass="btn btn-info btn-block" OnClick="btnValidate_Click" ViewStateMode="Enabled" />
                                    </div>
                                    <p class="text-danger text-center">
                                        <asp:Label runat="server" ID="lblUserCheck"></asp:Label>
                                    </p>
                                    <div class="form-group">
                                        <div class="input-group">
                                            <span class="input-group-addon">
                                                <i class="glyphicon glyphicon-lock"></i>
                                            </span>
                                            <asp:TextBox runat="server" ID="txtPassword" placeholder="Password" TextMode="Password" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="input-group">
                                            <span class="input-group-addon">
                                                <i class="glyphicon glyphicon-lock"></i>
                                            </span>
                                            <asp:TextBox runat="server" ID="txtConfirmPassword" placeholder="Confirm Password" TextMode="Password" CssClass="form-control"></asp:TextBox>
                                            <%--<input class="form-control" placeholder="ConfirmPassword" name="password" type="password" id="ConfirmPassword" runat="server" required="required" value="" />--%>
                                        </div>
                                    </div>
                                    <p class="text-danger text-center">
                                        <asp:Label runat="server" ID="lblPasswordCheck"></asp:Label>
                                    </p>

                                </div>
                            </div>
                        </fieldset>
                        <fieldset>
                            <div class="row">
                                <div class="col-sm-12 col-md-10  col-md-offset-1 ">
                                    <div class="form-group">
                                        <asp:PlaceHolder runat="server" ID="plhRoleSelect" Visible="false">
                                            <asp:CheckBoxList ID="cblRolesSelect" runat="server"></asp:CheckBoxList>
                                            <asp:Button runat="server" ID="btnAddUser" Text="เพิ่มผู้ใช้" CssClass="btn btn-success btn-block" OnClick="btnAddUser_Click" />
                                        </asp:PlaceHolder>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%-- end Form --%>
</asp:Content>
