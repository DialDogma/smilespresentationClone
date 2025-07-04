<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmManageVehicleModel.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmManageVehicleModel" EnableEventValidation = "false" Theme="default" %>

<%@ Register src="UserControls/DropdownUserControls/ddlMotorVehicleType.ascx" tagname="ddlMotorVehicleType" tagprefix="uc1" %>
<%@ Register src="UserControls/DropdownUserControls/ddlMotorVehicleSegment.ascx" tagname="ddlMotorVehicleSegment" tagprefix="uc2" %>
<%@ Register src="UserControls/DropdownUserControls/ddlMotorVehicleBrand.ascx" tagname="ddlMotorVehicleBrand" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        function OpenPopup(modelid) {
            //  ใส่ Script window.open เพื่อให้ Popup.asp เปิดขึ้นมา
            var newWindow = PopupCenter("frmManageVehicleModelAddEdit.aspx?modelid=" + modelid , 'popup', '900', '500');
            return false;
        }

        function OpenPopupAdd() {
            //  ใส่ Script window.open เพื่อให้ Popup.asp เปิดขึ้นมา
            var newWindow = PopupCenter("frmManageVehicleModelAddEdit.aspx?", 'popup', '900', '500');
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
            <div class="panel panel-default input-md">
                <div class="panel-heading">
                    <h2>จัดการข้อมูลรุ่นยานพาหนะ</h2>
                </div>
                <div class="panel-body">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 12.5%; text-align: right">ประเภทยานพาหนะ :</td>
                            <td style="width: 12.5%">
                             
                                <uc1:ddlMotorVehicleType ID="ddlMotorVehicleType" runat="server" OnSelectChanged="ddlMotorVehicleType1_SelectChanged" />
                             
                            </td>
                            <td style="width: 12.5%; text-align: right">&nbsp;</td>
                            <td style="width: 12.5%">&nbsp;</td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                        </tr>

                        <tr>
                            <td style="width: 12.5%; text-align: right">ขนาดยานพาหนะ :</td>
                            <td style="width: 12.5%">
                                <uc2:ddlMotorVehicleSegment ID="ddlMotorVehicleSegment" runat="server" />
                            </td>
                            <td style="width: 12.5%; text-align: right">&nbsp;</td>
                            <td style="width: 12.5%">&nbsp;</td>
                            <td style="width: 12.5%">
                                &nbsp;</td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                        </tr>

                        <tr>
                            <td style="width: 12.5%; text-align: right">ยี่ห้อพาหนะ :</td>
                            <td style="width: 12.5%">
                                <uc3:ddlMotorVehicleBrand ID="ddlMotorVehicleBrand" runat="server" />
                            </td>
                            <td style="width: 12.5%; text-align: center">
                                <asp:Button ID="btnShow" runat="server" SkinID="info" Text="แสดง" Width="80%" OnClick="btnShow_Click" />
                            </td>
                            <td style="width: 12.5%">&nbsp;</td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                        </tr>
                    </table>
                </div>

                <div class="panel panel-default input-md">
                    <div class="panel-heading"></div>
                    <div class="panel-body">
                        <table style="width: 100%">
                            <tr>
                                <td colspan="8">
                                    <asp:GridView ID="dgvMain" runat="server" Width="100%" AutoGenerateColumns="False" OnRowDataBound="OnRowDataBound" OnSelectedIndexChanged="OnSelectIndexChanged"  >
                                        <Columns>
                                            <asp:BoundField DataField="VehicleModel_ID" HeaderText="รหัส" />
                                            <asp:BoundField DataField="VehicleSegment_ID" HeaderText="ขนาด" />
                                            <asp:BoundField DataField="VehicleBrand_ID" HeaderText="ยี่ห้อ" />
                                            <asp:BoundField DataField="VehicleModelDetail" HeaderText="รุ่น" />                                         
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
                                    &nbsp;</td>
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
    </div>
</asp:Content>
