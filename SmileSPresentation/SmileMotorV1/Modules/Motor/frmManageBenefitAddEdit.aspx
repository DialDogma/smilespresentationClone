<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmManageBenefitAddEdit.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmManageBenefitAddEdit" Theme="default" %>

<%@ Register Src="UserControls/DropdownUserControls/ddlMotorProductType.ascx" TagName="ddlMotorProductType" TagPrefix="uc1" %>
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
            <div class="panel  panel-default">
                <div class="panel-heading input-md">
                    <h2>Add/Edit  Benefit</h2>
                </div>
                <div class="panel-body">
                    <table style="width: 100%">
                        <tr>
                            <td colspan="2" style="width: 12.5%; text-align: right">ประเภทผลิตภัณฑ์ :</td>
                            <td style="width: 12.5%">
                                <uc1:ddlMotorProductType ID="ddlMotorProductType1" runat="server" />
                            </td>

                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                        </tr>

                        <tr>

                            <td colspan="2" style="width: 12.5%; text-align: right">สิทธิประโยชน์ :</td>
                            <td colspan="2" style="width: 12.5%">
                                <asp:TextBox ID="txtBenefitDetail" runat="server" ToolTip=""></asp:TextBox>
                            </td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%">&nbsp;</td>
                            <td style="width: 12.5%">&nbsp;</td>

                        </tr>
                        <tr>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%">
                                <table>
                                    <tr>
                                        <td style="width: 6%; text-align: left" class="child">
                                            <asp:Button ID="Button1" runat="server" CssClass="btn btn-success form-control input-sm" Text="บันทึก" Width="100%" />
                                        </td>
                                        <td style="width: 6%; text-align: right" class="child">
                                            <asp:Button ID="Button2" runat="server" CssClass="btn btn-danger  input-sm" Text="ยกเลิก" Width="90%" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
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
