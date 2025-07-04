<%@ Page Title="" Language="C#" MasterPageFile="~/Master1.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SmileSDataCenterV1.Login" Theme="default" %>

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
    <%-- Form ก๊อบล๊อกอินฟอร์มมาจาก SmileMotorV1 ของไอ่บูมมา --%>
    <div class="container" style="margin-top: 40px">
        <div class="row">
            <div class="col-sm-6 col-md-4 col-md-offset-4">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <strong>Login เข้าสู่ระบบ</strong>
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
                                            <input class="form-control" placeholder="Username" name="loginname" type="text" id="Username" runat="server" required="required" autofocus="autofocus" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="input-group">
                                            <span class="input-group-addon">
                                                <i class="glyphicon glyphicon-lock"></i>
                                            </span>
                                            <input class="form-control" placeholder="Password" name="password" type="password" id="Password" runat="server" required="required" value="" />
                                        </div>
                                    </div>
                                    <%--<div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon">
                                                    <i class="glyphicon glyphicon-paperclip"></i>
                                                </span>
                                                <input class="form-control" placeholder="ชื่อเจ้าหน้าที่" name="officername" type="text" id="OfficerName" runat="server" required="required" value="" />
                                            </div>
                                        </div>--%>
                                    <p class="text-danger text-center">
                                        <asp:Label runat="server" ID="FailureText"></asp:Label>
                                    </p>
                                    <div class="form-group" style="text-align: right">
                                        <asp:Button runat="server" Text="เข้าสู่ระบบ" CssClass="btn btn-info btn-block" OnClick="LogIn" />
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
