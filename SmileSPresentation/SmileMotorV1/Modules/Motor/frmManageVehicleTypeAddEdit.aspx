<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmManageVehicleTypeAddEdit.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmManageVehicleTypeAddEdit" %>

<%@ Register Src="~/Modules/Motor/UserControls/DropdownUserControls/ddlActiveStatus.ascx" TagPrefix="uc1" TagName="ddlActiveStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        window.onload = UpdateParent();

        window.onbeforeunload = UpdateParent();

        function UpdateParent() {
            window.opener.PostBack();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="panel-group">
            <div class="panel panel-default input-md">
                <div class="panel-heading">
                    <h2>Add/Edit Vehicle Type</h2>
                </div>
                <div class="panel-body">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 12.5%; text-align: right">รหัส :</td>
                            <td style="width: 12.5%">
                                <asp:TextBox ID="txtCode" runat="server" CssClass="form-control input-sm" Width="100%" placeholder="รหัส"></asp:TextBox>
                            </td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                        </tr>

                        <tr>
                            <td style="width: 12.5%; text-align: right">ประเภทยานพาหนะ :</td>
                            <td style="width: 12.5%">
                                <asp:TextBox ID="txtVehicleType" runat="server" CssClass="form-control input-sm" Width="100%" placeholder="ประเภท"></asp:TextBox>
                            </td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                        </tr>

                        <tr>
                            <td style="width: 12.5%; text-align: right">สถานะการใช้งาน :</td>
                            <td style="width: 12.5%">
                                <uc1:ddlActiveStatus ID="ddlActiveStatus1" runat="server" />
                            </td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                        </tr>

                        <tr>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%" class="parent">
                                <table>
                                    <tr>
                                        <td style="width: 6%; text-align: left" class="child">
                                            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success form-control input-sm" Text="บันทึก" Width="100%" />
                                        </td>
                                        <td style="width: 6%; text-align: right" class="child">
                                            <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-danger  input-sm" Text="ยกเลิก" Width="90%" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                        </tr>

                        <tr>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>


    </div>
</asp:Content>
