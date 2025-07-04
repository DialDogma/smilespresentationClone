<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmManageBenefit.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmManageBenefit" Theme="default" EnableEventValidation="false"%>

<%@ Register Src="UserControls/DropdownUserControls/ddlMotorProductType.ascx" TagName="ddlMotorProductType" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function OpenPopup(id) {
            var newWindow = PopupCenter("frmManageBenefitAddEdit.aspx?id=" + id , 'popup', '900', '500');
            return false;
        }

        function OpenPopupAdd() {
            //  ใส่ Script window.open เพื่อให้ Popup.asp เปิดขึ้นมา
            var newWindow = PopupCenter("frmManageBenefitAddEdit.aspx?", 'popup', '900', '500');
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
                    <h2>Add/Edit&nbsp; Benefit</h2>
                </div>
                <div class="panel-body">
                    <table style="width: 100%">
                        <tr>
                            <td colspan="2" style="width: 12.5%; text-align: right">ประเภทผลิตภัณฑ์ :</td>
                            <td colspan="2" style="width: 12.5%">
                                <uc1:ddlMotorProductType ID="ddlMotorProductType1" runat="server" />
                            </td>

                            <td style="width: 12.5%">
                                <asp:Button ID="btnShow" runat="server" SkinID="info" Text ="แสดง" OnClick="btnShow_Click" />
                            </td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <asp:GridView ID="dgvMain" runat="server" Width="100%" AutoGenerateColumns="false" OnRowDataBound="dgvMain_RowDataBound" OnSelectedIndexChanged="dgvMain_SelectedIndexChanged">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="0">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>.
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="BenefitDetail" HeaderText="สิทธิประโยชน์" ItemStyle-HorizontalAlign="Left">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
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
                                <asp:Button ID="btnAdd" runat="server" SkinID="success" Text="เพิ่ม" Width="80%" />
                            </td>
                            <td style="width: 12.5%">
                                <asp:Button ID="btnEdit" runat="server" SkinID="warning" Text="แก้ไข" Width="80%" />
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
