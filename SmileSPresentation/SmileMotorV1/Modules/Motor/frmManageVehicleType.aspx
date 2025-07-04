<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmManageVehicleType.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmManageVehicleType" EnableEventValidation="false" Theme="default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        function OpenPopup(VehicleTypeid) {
            //  ใส่ Script window.open เพื่อให้ Popup.asp เปิดขึ้นมา
            var newWindow = PopupCenter("frmManageVehicleTypeAddEdit.aspx?vehicletypeid=" + VehicleTypeid, 'popup', '900', '500');
            return false;
        }

        function OpenPopupAdd() {
            //  ใส่ Script window.open เพื่อให้ Popup.asp เปิดขึ้นมา
            var newWindow = PopupCenter("frmManageVehicleTypeAddEdit.aspx?", 'popup', '900', '500');
            return false;
        }
        function PostBack() {
            var btn = document.getElementById('<%=btnRefresh.ClientID%>');
            if (btn) btn.click();
        }

        <%--           function PostBack() {
            var page = document.getElementById('<%=Page.ClientID%>');
            if (page) page.onload();
        }--%>

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="panel-group">
            <div class="panel panel-default input-md">
                <div class="panel-heading">
                    <h2>จัดการข้อมูลประเภทยานพาหนะ</h2>
                </div>
                <div class="panel-body">
                    <table style="width: 100%">

                        <tr>
                            <td colspan="8">
                                <asp:GridView ID="dgvMain" runat="server" AutoGenerateColumns="False" OnRowDataBound="OnRowDataBound" OnSelectedIndexChanged="OnSelectedIndexChanged">
                                    <Columns>
                                        <asp:BoundField DataField="VehicleType_ID" HeaderText="รหัส" />
                                        <asp:BoundField DataField="VehicleTypeDetail" HeaderText="ประเภทรถ" />
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
                                <asp:Button ID="btnRefresh" runat="server" style="visibility:hidden"  OnClick="btnRefresh_Click" />
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
