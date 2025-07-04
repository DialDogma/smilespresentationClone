<%@ Page Title="จัดการข้อมูลสิทธิประโยชน์" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmManageProductBenefit.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmManageProductBenefit" EnableEventValidation = "false" Theme="default" %>
<%@ Register Src="UserControls/ucBenefit.ascx" TagName="ucBenefit" TagPrefix="uc1" %>
<%@ Register Src="~/Modules/Motor/UserControls/DropdownUserControls/ddlMotorProduct.ascx" TagPrefix="uc1" TagName="ddlMotorProduct" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function OpenPopup(id) {
            var newWindow = PopupCenter("frmManageProductBenefitAddEdit.aspx?id=" + id , 'popup', '900', '500');
            return false;
        }

        function OpenPopupAdd() {
            //  ใส่ Script window.open เพื่อให้ Popup.asp เปิดขึ้นมา
            var newWindow = PopupCenter("frmManageProductBenefitAddEdit.aspx?", 'popup', '900', '500');
            return false;
        }
   function PostBack() {
            var btn = document.getElementById('<%=btnShow.ClientID%>');
            if (btn) btn.click();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="panel-group">
            <div class="panel  panel-default">
                <div class="panel-heading input-md">
                    <h2>จัดการข้อมูลสิทธิประโยชน์</h2>
                </div>
                <div class="panel-body">
                    <table style="width: 100%">
                        <tr>
                            <td style="text-align: right">ผลิตภันฑ์ :</td>
                            <td style="width: 12.5%" colspan="2">
                                <uc1:ddlMotorProduct runat="server" ID="ddlMotorProduct" />
                            </td>
                            <td style="width: 12.5%; text-align:center">
                                <asp:Button ID="btnShow" runat="server" Text="แสดง" SkinID="info" Width="80%" OnClick="btnShow_Click" />
                            </td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>

                        </tr>
                        <tr>
                            <td colspan="8">
                                <uc1:ucBenefit ID="ucBenefit1" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 12.5%">
                                <asp:Button ID="btnAdd" runat="server" SkinID="success" Text="เพิ่ม" Width="80%" />
                            </td>
                            <td style="width: 12.5%">
                                <asp:Button ID="btnEdit" runat="server" SkinID="warning" Text="แก้ไข" Width="80%" />
                            </td>
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
