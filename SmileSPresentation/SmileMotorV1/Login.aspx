<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SmileMotorV1.Login" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script src="scripts/jquery-3.1.1.min.js"></script>
    <script src="scripts/bootstrap.js"></script>
</head>
<body>
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
    <form id="frmLogin" runat="server">
        <nav class="navbar navbar-inverse">
            <div class="container-fluid">
                <div class="navbar-header">
                    <a class="navbar-brand">Motor System</a>
                </div>
            </div>
        </nav>       
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
                                                <input class="form-control" placeholder="ชื่อผู้ใช้งาน" name="loginname" type="text" id="Username" runat="server" required="required" autofocus="autofocus" />
                                                <%--<asp:TextBox class="form-control" runat="server" ID="txtUsername" ></asp:TextBox>--%>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon">
                                                    <i class="glyphicon glyphicon-lock"></i>
                                                </span>
                                                <%--<asp:TextBox class="form-control" runat="server" ID="txtPassword" TextMode="Password"  ></asp:TextBox>	--%>
                                                <input class="form-control" placeholder="รหัสผ่าน" name="password" type="password" id="Password" runat="server" required="required" value="" />
                                            </div>
                                        </div>
                                        <p class="text-danger text-center">
                                            <asp:Label runat="server" ID="FailureText" ></asp:Label>
                                        </p>
                                        <div class="form-group" style="text-align: right">
                                            <%--<input type="submit" class="btn btn-lg btn-primary btn-block" value="Sign in">--%>
                                            <asp:Button runat="server" OnClick="LogIn" Text="เข้าสู่ระบบ" CssClass="btn btn-info btn-block" />
                                        </div>
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                        <div class="panel-footer text-center" >
                          ©Copyrights 2017 | Motor System 
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
