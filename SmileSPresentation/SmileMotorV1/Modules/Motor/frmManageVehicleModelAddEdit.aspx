<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmManageVehicleModelAddEdit.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmManageVehicleModelAddEdit" %>

<%@ Register src="UserControls/DropdownUserControls/ddlMotorVehicleSegment.ascx" tagname="ddlMotorVehicleSegment" tagprefix="uc1" %>
<%@ Register src="UserControls/DropdownUserControls/ddlMotorVehicleBrand.ascx" tagname="ddlMotorVehicleBrand" tagprefix="uc2" %>
<%@ Register Src="~/Modules/Motor/UserControls/DropdownUserControls/ddlActiveStatus.ascx" TagPrefix="uc3" TagName="ddlActiveStatus" %>

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
                    <h2>Add/Edit Vehicle Model</h2>
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
                            <td style="width: 12.5%; text-align: right">ขนาดยานพาหนะ :</td>
                            <td style="width: 12.5%">
                                <uc1:ddlMotorVehicleSegment ID="ddlMotorVehicleSegment1" runat="server" />
                            </td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                        </tr>

                        <tr>
                            <td style="width: 12.5%; text-align: right">ยี่ห้อรถ :</td>
                            <td style="width: 12.5%">
                                <uc2:ddlMotorVehicleBrand ID="ddlMotorVehicleBrand1" runat="server" />
                            </td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                        </tr>

                        <tr>
                            <td style="width: 12.5%; text-align: right">รุ่นยานพาหนะ :</td>
                            <td style="width: 12.5%">
                                <asp:TextBox ID="txtVehicleModel" runat="server" CssClass="form-control input-sm" Width="100%" placeholder="รุ่น"></asp:TextBox>
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
                                <uc3:ddlActiveStatus ID="ddlActiveStatus1" runat="server" />
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
