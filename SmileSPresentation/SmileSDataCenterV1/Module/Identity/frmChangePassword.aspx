<%@ Page Title="" Language="C#" MasterPageFile="~/Master1.Master" AutoEventWireup="true" CodeBehind="frmChangePassword.aspx.cs" Inherits="SmileSDataCenterV1.Module.frmChangePassword" %>

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
    <%-- Form --%>
    <div class="container" style="margin-top: 40px">
        <div class="row">
            <div class="col-sm-6 col-md-4 col-md-offset-4">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <strong>เปลี่ยนรหัสผ่าน คุณ<%= Page.User.Identity.Name %></strong>
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
                                                <i class="glyphicon glyphicon-lock"></i>
                                            </span>
                                            <input class="form-control" placeholder=" Old Password" name="password" type="password" id="OldPassword" runat="server" required="required" value="" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="input-group">
                                            <span class="input-group-addon">
                                                <i class="glyphicon glyphicon-lock"></i>
                                            </span>
                                            <input class="form-control" placeholder="New Password" name="password" type="password" id="NewPassword" runat="server" required="required" value="" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="input-group">
                                            <span class="input-group-addon">
                                                <i class="glyphicon glyphicon-lock"></i>
                                            </span>
                                            <input class="form-control" placeholder="Confirm New Password" name="password" type="password" id="ConfirmNewPassword" runat="server" required="required" value="" />
                                        </div>
                                    </div>
                                    <p class="text-danger text-center">
                                        <asp:Label runat="server" ID="FailureText"></asp:Label>
                                    </p>
                                    <div class="form-group" style="text-align: right">
                                        <asp:Button runat="server" Text="เปลี่ยนรหัสผ่าน" CssClass="btn btn-info btn-block" OnClick="ChangePassword" />
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                    <div class="panel-footer text-center">
                        ©Copyrights 2017 | SmileS Dev.
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%-- end Form --%>
</asp:Content>
