<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmManageProductBenefitAddEdit.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmManageProductBenefitAddEdit" Theme="default" %>

<%@ Register Src="UserControls/DropdownUserControls/ddlMotorProduct.ascx" TagName="ddlMotorProduct" TagPrefix="uc1" %>
<%@ Register Src="UserControls/DropdownUserControls/ddlMotorProductType.ascx" TagName="ddlMotorProductType" TagPrefix="uc2" %>
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
                    <h2>Add/Edit Product Benefit</h2>
                </div>
                <div class="panel-body">
                    <table style="width: 100%">
                        <tr>
                            <td colspan="2" style="width: 12.5%; text-align: right">ผลิตภัณฑ์ :</td>

                            <td colspan="2" style="width: 12.5%">
                                <uc1:ddlMotorProduct ID="ddlMotorProduct1" runat="server" />
                            </td>

                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                        </tr>
                        <tr>
                            <td colspan="2" style="width: 12.5%; text-align: right">ประเภทผลิตภัณฑ์ :</td>
                            <td colspan="2" style="width: 12.5%">
                                <uc2:ddlMotorProductType ID="ddlMotorProductType1" runat="server" />
                            </td>

                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <asp:GridView ID="dgvMain" runat="server" Width="100%" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkRow" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="BenefitDetail" HeaderText="สิทธิประโยชน์" ItemStyle-HorizontalAlign="Left">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="ความคุ้มครอง">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtCoverage" runat="server"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%">
                                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success form-control input-sm" Text="บันทึก" Width="100%" />
                            </td>
                            <td style="width: 12.5%">
                                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-danger  input-sm" Text="ยกเลิก" Width="90%" />
                            </td>

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
