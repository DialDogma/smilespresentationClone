<%@ Page Title="" Language="C#" MasterPageFile="~/Master1.Master" AutoEventWireup="true" CodeBehind="frmUpdatePassword.aspx.cs" Inherits="SmileSDataCenterV1.Module.Identity.frmUpdatePassword" Theme="default" %>

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
        <h3>Change Password</h3>
    </div>
    <div class="container" style="margin-top: 30px">
        <div class="row">
            <div class="col-sm-6 col-md-4 col-md-offset-4">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <strong>เปลี่ยนพาสเวริ์ด</strong>
                    </div>
                    <div class="panel-body">
                        <fieldset>
                            <div class="row">
                                <div class="center-block">
                                    <img class="profile-img"
                                        src="https://cdn4.iconfinder.com/data/icons/small-n-flat/24/lock-128.png" alt="" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-md-10  col-md-offset-1 ">
                                    <div class="form-group">
                                        <div class="input-group">
                                            <span class="input-group-addon">
                                                <i class="glyphicon glyphicon-user"></i>
                                            </span>
                                            <input class="form-control" placeholder="Username" name="SearchUser" type="text" id="SearchUser" runat="server" required="required" autofocus="autofocus" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Button runat="server" ID="btnSearchUser" Text="ค้นหาผู้ใช้" SkinID="info" OnClick="btnSearchUser_Click" />
                                    </div>
                                </div>
                                <div class="col-sm-12 col-md-10  col-md-offset-1 ">
                                    <div class="form-group">
                                        <asp:GridView runat="server" ID="dgvSearchUser" AutoGenerateColumns="false" OnSelectedIndexChanged="dgvSearchUser_SelectedIndexChanged">
                                            <Columns>
                                                <asp:ButtonField CommandName="Select" HeaderText="Select" Text="Select" />
                                                <asp:BoundField DataField="UserName" HeaderText="UserName" />
                                                <asp:BoundField DataField="Id" HeaderText="User ID" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <asp:PlaceHolder runat="server" ID="plhChangePasswordform" Visible="false">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon">
                                                    <i class="glyphicon glyphicon-lock"></i>
                                                </span>
                                                <asp:TextBox runat="server" ID="txtOldPassword" placeholder="Old Password" TextMode="Password" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon">
                                                    <i class="glyphicon glyphicon-lock"></i>
                                                </span>
                                                <asp:TextBox runat="server" ID="txtPassword" placeholder="New Password" TextMode="Password" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon">
                                                    <i class="glyphicon glyphicon-lock"></i>
                                                </span>
                                                <asp:TextBox runat="server" ID="txtConfirmPassword" placeholder="Confirm New Password" TextMode="Password" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <p class="text-danger text-center">
                                            <asp:Label runat="server" ID="lblPasswordCheck"></asp:Label>
                                        </p>
                                        <div class="form-group">
                                            <asp:Button runat="server" ID="btnChangePassword" Text="ยืนยัน" SkinID="success" OnClick="btnChangePassword_click" />
                                        </div>
                                    </asp:PlaceHolder>
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
