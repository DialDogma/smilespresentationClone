<%@ Page Title="จัดการข้อมูลผลิตภัณฑ์" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmManageProduct.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmManageProduct" EnableEventValidation="false" Theme="default"%>
<%@ Register Src="~/Modules/Motor/UserControls/DropdownUserControls/ddlMotorProductType.ascx" TagPrefix="uc1" TagName="ddlMotorProductType" %>
<%@ Register Src="~/Modules/Motor/UserControls/DropdownUserControls/ddlMotorCompanyInsurance.ascx" TagPrefix="uc2" TagName="ddlMotorCompanyInsurance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function OpenPopup(id) {
            //  ใส่ Script window.open เพื่อให้ Popup.asp เปิดขึ้นมา
            var newWindow = PopupCenter("frmManageProductAddEdit.aspx?id=" + id, 'popup', '900', '500');
            return false;
        }

        function OpenPopupAdd() {
            //  ใส่ Script window.open เพื่อให้ Popup.asp เปิดขึ้นมา
            var newWindow = PopupCenter("frmManageProductAddEdit.aspx?", 'popup', '900', '500');
            return false;
        }
        function PostBack() {
            var btn = document.getElementById('<%=btnShow.ClientID%>');
            if (btn) btn.click();
        }  </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="panel-group">
            <div class="panel  panel-default">
                <div class="panel-heading input-md">
                    <h2>จัดการข้อมูลผลิตภัณฑ์</h2>
                </div>
                <div class="panel-body">
                    <table style="width: 100%">
                        <tr>
                            <td style="text-align: right">กลุ่มผลิตภัณฑ์ :</td>
                            <td class="auto-style1">
                                <uc1:ddlMotorProductType ID="ddlMotorProductType" runat="server" />
                            </td>
                            <td style="text-align: right"></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td style="text-align: right">บริษัท :</td>
                            <td style="width: 12.5%">
                                <uc2:ddlMotorCompanyInsurance ID="ddlMotorCompanyInsurance" runat="server" />
                            </td>
                            <td style="width: 12.5%; text-align:center">
                                <asp:Button ID="btnShow" runat="server" Text="แสดง" SkinID="info" Width="80%" OnClick="btnShow_Click" />
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
            <div class="panel  panel-default">
                <div class="panel-heading">
                </div>
                <div class="panel-body">
                    <table style="width: 100%">
                        <tr>
                            <td colspan="8">
                                <asp:GridView ID="dgvProduct" runat="server" Width="100%" AutoGenerateColumns="False" OnRowDataBound="OnRowDataBound" OnSelectedIndexChanged="OnSelectedIndexChanged" >
                                    <Columns>
                                        <asp:BoundField DataField="Product_ID" HeaderText="รหัสผลิตภัณฑ์" />
                                        <asp:BoundField DataField="ProductDetail" HeaderText="ชื่อผลิตภัณฑ์" />
                                        <asp:BoundField DataField="PremiumPerYear" HeaderText="เบี้ยรายปี" />
                                        <asp:BoundField DataField="PremiumPerMonth" HeaderText="เบี้ยรายเดือน" />
                                        <asp:BoundField DataField="IsActive" HeaderText="สถานะการใช้งาน" />
                                    </Columns>
                                  
                                </asp:GridView>
                            </td>
                        </tr>

                        <tr>
                            <td style="width: 12.5%; text-align: center">
                                <asp:Button ID="btnAdd" runat="server" Text="เพิ่ม" SkinID="success" Width="80%" />
                            </td>
                            <td style="width: 12.5%; text-align: center">
                                <asp:Button ID="btnEdit" runat="server" Text="แก้ไข" SkinID="warning" Width="80%" />
                            </td>
                            <td style="width: 12.5%; text-align: center">&nbsp;</td>
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

                        <tr>
                            <td colspan="8">
                                <div class="panel-footer"></div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
