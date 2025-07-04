<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAppDetailCustomer.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucAppDetailCustomer" %>
<link href="../../../Content/meStyleSheet.css" rel="stylesheet" />
<ajaxToolkit:Accordion ID="Accordion1"
    HeaderCssClass="accordionHeader"
    ContentCssClass="accordionContent"
    runat="server"
    FadeTransitions="true"
    TransitionDuration="500"
    AutoSize="None"
    SelectedIndex="0"
    RequireOpenedPane="false"
    SuppressHeaderPostbacks="false" Height="100%" Width="100%">
    <Panes>
        <ajaxToolkit:AccordionPane ID="Pane1" runat="server">
            <Header>
                <h4>
                    <asp:Label ID="lblTextHeader" runat="server" Text=""></asp:Label>
                </h4>
            </Header>
            <Content>
                <asp:Panel ID="panCustomer" runat="server">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 12.5%; text-align: right">ชื่อ-นามสกุล :</td>
                            <td style="width: 12.5%">
                                <asp:Label ID="lblFullname" runat="server" SkinID="result"></asp:Label>
                            </td>
                            <td style="width: 12.5%; text-align: right">เพศ :</td>
                            <td style="width: 12.5%">
                                <asp:Label ID="lblSex" runat="server" SkinID="result"></asp:Label>
                            </td>
                            <td style="width: 12.5%; text-align: right">สถานภาพ :</td>
                            <td style="width: 12.5%">
                                <asp:Label ID="lblMaritalStatusDetail" runat="server" SkinID="result"></asp:Label>
                            </td>
                        </tr>

                        <tr>
                            <td style="width: 12.5%; text-align: right">เลขบัตรประชาชน :</td>
                            <td style="width: 12.5%">
                                <asp:Label ID="lblCardNumber" runat="server" SkinID="result"></asp:Label>
                            </td>
                            <td style="width: 12.5%; text-align: right">Passport :</td>
                            <td style="width: 12.5%">
                                <asp:Label ID="lblPassportNumber" runat="server" SkinID="result"></asp:Label>
                            </td>
                            <td style="width: 12.5%; text-align: right">วันกิด :</td>
                            <td style="width: 12.5%">
                                <asp:Label ID="lblBirthDate" runat="server" SkinID="result"></asp:Label>
                            </td>
                        </tr>

                        <tr>
                            <td style="width: 12.5%; text-align: right">ประเภทอาชีพ :</td>
                            <td style="width: 12.5%">
                                <asp:Label ID="lblOccupationDetail" runat="server" SkinID="result"></asp:Label>
                            </td>
                            <td style="width: 12.5%; text-align: right">อาชีพ :</td>
                            <td style="width: 12.5%">
                                <asp:Label ID="lblOccupationLevelDetail" runat="server" SkinID="result"></asp:Label>
                            </td>
                            <td style="width: 12.5%; text-align: right"></td>
                            <td style="width: 12.5%">&nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>

                <%-- panel Corporate --%>
                <asp:Panel ID="panCorporate" runat="server">
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 12.5%; text-align: right;">ชื่อ :</td>
                            <td style="width: 37.5%;" colspan="3">
                                <asp:Label ID="lblFullCorporateName" runat="server" SkinID="result"></asp:Label>
                            </td>
                            <td style="width: 12.5%;"></td>
                            <td style="width: 12.5%;"></td>
                        </tr>
                        <tr>
                            <td style="width: 12.5%; text-align: right;">เลขประจำตัวผู้เสียภาษี :</td>
                            <td style="width: 12.5%;">
                                <asp:Label ID="lblTaxpayerNo" runat="server" SkinID="result"></asp:Label>
                            </td>
                            <td style="width: 12.5%; text-align: right;"></td>
                            <td style="width: 12.5%;"></td>
                            <td style="width: 12.5%;"></td>
                            <td style="width: 12.5%;"></td>
                        </tr>
                    </table>

                </asp:Panel>
                <table>
                    <tr>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%">&nbsp;</td>
                        <td style="width: 12.5%">
                            <asp:Button ID="btnshowmodalHistory" runat="server" SkinID="link" Text="ประวัติการทำรายการ" />
                            <ajaxToolkit:ModalPopupExtender ID="ModalHistory" runat="server" BackgroundCssClass="modalBackground" PopupControlID="ModaPanelHistory" TargetControlID="btnshowmodalHistory">
                            </ajaxToolkit:ModalPopupExtender>
                        </td>
                        <td style="width: 12.5%">
                            <asp:Button ID="btnEditPerson" runat="server" Text="แก้ไขข้อมูลลูกค้า"/>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="dgvMain" runat="server" AutoGenerateColumns="False" RowStyle-HorizontalAlign="Left">
                    <Columns>
                        <asp:BoundField HeaderText="ช่องทางการติดต่อ" DataField="ContactTypeDetail" HeaderStyle-Width="20%" />
                        <asp:BoundField HeaderText="รายละเอียด" DataField="ContactDetail" />
                    </Columns>
                </asp:GridView>
            </Content>
        </ajaxToolkit:AccordionPane>
    </Panes>
</ajaxToolkit:Accordion>

<asp:Panel ID="ModaPanelHistory" Width="800" runat="server" CssClass="modalPopup" align="center" Style="display: none">
    <%-- Gridview --%>
    <asp:GridView ID="dgvHistory" runat="server" AutoGenerateColumns="false" OnRowDataBound="dgvHistory_RowDataBound" OnSelectedIndexChanged="dgvHistory_SelectedIndexChanged">
        <Columns>
            <asp:TemplateField HeaderText="" HeaderStyle-Width="1px">
                <ItemTemplate>
                    <%# Container.DataItemIndex +1  %>.
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="PersonTransaction_Code" HeaderText="รหัสรายการ" />
            <asp:BoundField DataField="TransactionTypeDetail" HeaderText="รายละเอียด" />
            <asp:BoundField DataField="CreatedBy" HeaderText="สร้างโดย" />
            <asp:BoundField DataField="CreateDate" HeaderText="วันที่สร้าง" />
        </Columns>
    </asp:GridView>
    <table style="width: 100%">
        <tr>
            <td style="text-align: right">
                <asp:Button ID="btnCloseModalHistory" runat="server" Text="ปิด" SkinID="danger" />
            </td>
        </tr>
    </table>
</asp:Panel>


