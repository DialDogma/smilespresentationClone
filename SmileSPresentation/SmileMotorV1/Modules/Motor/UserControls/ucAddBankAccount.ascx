<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAddBankAccount.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucAddBankAccount" %>
<%@ Register Src="DropdownUserControls/ddlBankCompany.ascx" TagName="ddlBankCompany" TagPrefix="uc2" %>
<%@ Register Src="ucPersonDetail_mini.ascx" TagName="ucPersonDetail_mini" TagPrefix="uc3" %>

<asp:UpdatePanel runat="server" ID="updatepanel00">
    <ContentTemplate>
        <asp:Button ID="btnAdd" runat="server" Text="เพิ่มเลขที่บัญชี" />
        <ajaxToolkit:ModalPopupExtender ID="mpe" runat="server" TargetControlID="btnAdd" PopupControlID="ModalPanel" BackgroundCssClass="modalBackground" />
        <asp:Panel ID="ModalPanel" runat="server" Width="62.5%" CssClass="modalPopup" align="center">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4 style="text-align: center">เพิ่มเลขที่บัญชี</h4>
                </div>
                <div class="panel-body">
                    <div class="table-responsive">
                        <table style="width: 100%">
                            <tr>
                                <td colspan="5">
                                    <uc3:ucPersonDetail_mini ID="ucPersonDetail_mini1" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:GridView ID="dgvMain" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="5" OnPageIndexChanging="dgvMain_PageIndexChanging">
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="1px">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex+1 %>.
                                                </ItemTemplate>
                                                <HeaderStyle Width="1px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="BankDetail" HeaderText="ธนาคาร" />
                                            <asp:BoundField DataField="PersonBankAccountName" HeaderText="ชื่อบัญชี" />
                                            <asp:BoundField DataField="PersonBankAccountNo" HeaderText="เลขที่บัญชี" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 12.5%"></td>
                                <td style="width: 12.5%; text-align: right">ธนาคาร :</td>
                                <td style="width: 12.5%">
                                    <uc2:ddlBankCompany ID="ddlBankCompany1" runat="server" />
                                </td>
                                <td style="width: 12.5%"></td>
                                <td style="width: 12.5%">
                                    <asp:HiddenField ID="hdfPerson_ID" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 12.5%"></td>
                                <td style="width: 12.5%; text-align: right">ชื่อบัญชี :</td>
                                <td style="width: 12.5%">
                                    <asp:TextBox ID="txtBankAccountName" runat="server"></asp:TextBox>
                                </td>
                                <td style="width: 12.5%"></td>
                                <td style="width: 12.5%"></td>
                            </tr>
                            <tr>
                                <td style="width: 12.5%"></td>
                                <td style="width: 12.5%; text-align: right">เลขบัญชี :</td>
                                <td style="width: 12.5%">
                                    <asp:TextBox ID="txtBankAccountNo" runat="server"></asp:TextBox>
                                    <ajaxToolkit:FilteredTextBoxExtender runat="server" FilterType="Numbers" TargetControlID="txtBankAccountNo" />
                                </td>
                                <td style="width: 12.5%"></td>
                                <td style="width: 12.5%"></td>
                            </tr>
                            <tr>
                                <td style="width: 12.5%"></td>
                                <td colspan="3" style="text-align: center">
                                    <asp:Label ID="lblWarning" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="width: 12.5%"></td>
                            </tr>
                            <tr>
                                <td style="width: 12.5%"></td>
                                <td colspan="3" style="text-align: center">
                                    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="บันทึก" Width="30%" SkinID="primary" />
                                    <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="ยกเลิก" Width="30%" SkinID="danger" />
                                </td>
                                <td style="width: 12.5%"></td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>