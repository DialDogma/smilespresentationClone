<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAppDetailPerson.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucAppDetailPerson" %>
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
                <h4></h4>
            </Header>
            <Content>
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
                        <td style="width: 12.5%; text-align: right">เบอร์โทรศัพท์ :</td>
                        <td style="width: 12.5%">
                            <asp:Label ID="lblPhoneNumber" runat="server" SkinID="result"></asp:Label>
                        </td>
                        <td style="width: 12.5%; text-align: right">เบอร์มือถือ :</td>
                        <td style="width: 12.5%">
                            <asp:Label ID="lblMobileNumber" runat="server" SkinID="result"></asp:Label>
                        </td>
                        <td style="width: 12.5%; text-align: right"></td>
                        <td style="width: 12.5%"></td>
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
                        <td style="width: 12.5%; text-align: right">ประวัติการทำรายการ :</td>
                        <td style="width: 12.5%">
                            <asp:Button ID="btnshowmodalHistory" runat="server" Text="คลิกดูรายละเอียด" SkinID="link" />
                            <ajaxToolkit:ModalPopupExtender ID="ModalHistory" runat="server" TargetControlID="btnshowmodalHistory" PopupControlID="ModaPanelHistory" BackgroundCssClass="modalBackground"></ajaxToolkit:ModalPopupExtender>
                        </td>
                    </tr>

                    <tr>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%">
                            <asp:Button ID="btnEditPerson" runat="server" Text="แก้ไขข้อมูลลูกค้า" />
                        </td>
                    </tr>
                </table>
            </Content>
        </ajaxToolkit:AccordionPane>
    </Panes>
</ajaxToolkit:Accordion>

<asp:Panel ID="ModaPanelHistory" runat="server" CssClass="modalPopup" align="center" Style="display: none">
    <%-- TODO: Gridview Or UserControl--%>
    <h1>ทดสอบจ้า </h1>
    <table style="width: 100%">
        <tr>
            <td style="text-align: right">
                <asp:Button ID="btnCloseModalHistory" runat="server" Text="ปิด" SkinID="danger" />
            </td>
        </tr>
    </table>
</asp:Panel>

