<%@ Page Title="เพิ่ม/แก้ไข" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmManageProductAddEdit.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmManageProductAddEdit" Theme="default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="../../CommonUserControls/ucTextboxDecimalNumber.ascx" TagName="ucTextboxDecimalNumber" TagPrefix="uc3" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucBenefit.ascx" TagPrefix="uc1" TagName="ucBenefit" %>


<%@ Register Src="UserControls/DropdownUserControls/ddlMotorProductType.ascx" TagName="ddlMotorProductType" TagPrefix="uc2" %>
<%@ Register Src="UserControls/DropdownUserControls/ddlCompany.ascx" TagName="ddlCompany" TagPrefix="uc4" %>

<%@ Register Src="UserControls/DropdownUserControls/ddlMotorCompanyInsurance.ascx" TagName="ddlMotorCompanyInsurance" TagPrefix="uc6" %>


<%@ Register Src="UserControls/DropdownUserControls/ddlActiveStatus.ascx" TagName="ddlActiveStatus" TagPrefix="uc7" %>


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
                    <h2>Add/Edit Product </h2>
                </div>
                <div class="panel-body">
                    <table style="width: 100%">
                        <tr>
                            <td colspan="2" style="width: 12.5%; text-align: right">รหัส :</td>
                            <td style="width: 12.5%">
                                <asp:TextBox ID="txtProductID" runat="server" CssClass="form-control input-sm" disabled="disabled" placeholder="Product id" Width="100%"></asp:TextBox>
                            </td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                        </tr>
                        <tr>
                            <td colspan="2" style="width: 12.5%; text-align: right" >ประเภทผลิตภัณฑ์ :</td>
                            <td style="width: 12.5%">
                                <uc2:ddlMotorProductType ID="ddlMotorProductType1" runat="server" />
                            </td>
                           <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                        </tr>
                        <tr>
                            <td colspan="2" style="width: 12.5%; text-align: right">บริษัท :</td>
                            <td style="width: 12.5%">
                                <uc6:ddlMotorCompanyInsurance ID="ddlMotorCompanyInsurance1" runat="server" />
                            </td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                        </tr>
                        <tr>
                            <td colspan="2" style="width: 12.5%; text-align: right">ผลิตภัณฑ์ :</td>
                            <td style="width: 12.5%">
                                <asp:TextBox ID="txtProduct" runat="server" CssClass="form-control input-sm" Width="100%"></asp:TextBox>
                            </td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                        </tr>
                        <tr>
                            <td colspan="2" style="width: 12.5%; text-align: right">เบี้ย :</td>
                            <td style="width: 12.5%">
                                <uc3:ucTextboxDecimalNumber ID="ucTextboxDecimalNumber1" runat="server" />
                            </td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                        </tr>
                        <tr>
                            <td colspan="2" style="width: 12.5%; text-align: right">สถานะ :</td>
                            <td style="width: 12.5%">
                                <uc7:ddlActiveStatus ID="ddlActiveStatus1" runat="server" />
                            </td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                        </tr>
                        <tr>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%" >
                                
                            </td>
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
                        </tr>
                    </table>
                </div>
            </div>
        </div>

    </div>

</asp:Content>
