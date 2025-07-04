<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmManageVehicleBrand.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmManageVehicleBrand" EnableEventValidation="false" Theme="default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <script type="text/javascript">

        function OpenPopup(brandid) {
            //  ใส่ Script window.open เพื่อให้ Popup.asp เปิดขึ้นมา
            var newWindow = PopupCenter("frmManageVehicleBrandAddEdit.aspx?brandid=" + brandid , 'popup', '900', '500');
            return false;
        }

        function OpenPopupAdd() {
            //  ใส่ Script window.open เพื่อให้ Popup.asp เปิดขึ้นมา
            var newWindow = PopupCenter("frmManageVehicleBrandAddEdit.aspx?", 'popup', '900', '500');
            return false;
        }
        function PostBack() {
            var btn = document.getElementById('<%=btnRefresh.ClientID%>');
       if (btn) btn.click();
   }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="panel-group">
            <%--<div class="panel  panel-default">
                <div class="panel-heading input-md">
                    <h2>จัดการข้อมูลยี่ห้อยานพาหนะ</h2>
                    </div>
                    <div class="panel-body">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 12.5%; text-align: right">ประเภทยานพาหนะ :</td>
                                <td style="width: 12.5%">
                                    <uc1:ucVehicleTypeDropdownList ID="ucVehicleTypeDropdownList1" runat="server" />
                                </td>
                                <td style="width: 12.5%; text-align: right">ยี่ห้อ :</td>
                                <td style="width: 12.5%">
                                    <uc2:ucVehicleBrandDropdownList ID="ucVehicleBrandDropdownList1" runat="server" />
                                </td>
                                <td style="width: 12.5%"></td>
                                <td style="width: 12.5%"></td>
                                <td style="width: 12.5%"></td>
                                <td style="width: 12.5%"></td>
                            </tr>
                        </table>

                    </div>
                </div>
        </div>--%>

            <div class="panel panel-default">
                <div class="panel-heading input-md">
                    <h2>จัดการข้อมูลยี่ห้อยานพาหนะ</h2>
                </div>
                <div class="panel-body">
                    <table style="width: 100%">
                        <tr>
                            <td colspan="8">
                                <asp:GridView ID="dgvMain" runat="server"  AutoGenerateColumns="False" OnRowDataBound="OnRowDataBound" OnSelectedIndexChanged="OnSelectIndexChanged">
                                    <Columns>
                                        <asp:BoundField DataField="VehicleBrand_ID" HeaderText="รหัส" />
                                        <asp:BoundField DataField="VehicleBrandDetail" HeaderText="ยี่ห้อ" />
                                        <asp:BoundField DataField="IsActive" HeaderText="สถานะการใช้งาน" />
                                        <asp:BoundField DataField="CreateBy_ID" HeaderText="สร้างโดย" />
                                        <asp:BoundField DataField="CreateDate" HeaderText="วันที่สร้าง" />
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
                            <td style="width: 12.5%; text-align: center">
                                <asp:Button ID="btnRefresh" runat="server" style="visibility:hidden" OnClick="btnRefresh_Click" />
                            </td>
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
