<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmLogDetail.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmLogDetail" Theme="default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container">
        <div class="panel-group">
            <div class="panel  panel-default">
                <div class="panel-heading input-md">
                    <h2>Log detail</h2>
                </div>
                <div class="panel-body">
                    <asp:GridView runat="server" ID="dgvLogDetail"
                         AutoGenerateColumns="true"
                         GridLines="Vertical"
                         HeaderStyle-CssClass="header" >
                    </asp:GridView>
                </div>
            </div>
        </div>

    </div>
</asp:Content>
