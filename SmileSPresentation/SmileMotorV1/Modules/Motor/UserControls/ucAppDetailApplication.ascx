<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAppDetailApplication.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucAppDetailApplication" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucBenefit.ascx" TagPrefix="uc1" TagName="ucBenefit" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucPolicyNumbersDetail.ascx" TagPrefix="uc1" TagName="ucPolicyNumbersDetail" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucPolicyNumbersAdd.ascx" TagPrefix="uc1" TagName="ucPolicyNumbersAdd" %>

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
                    <h4>ข้อมูลกรมธรรม์</h4>
                </h4>
            </Header>
            <Content>

                <table style="width: 100%">
                    <tr>
                        <td style="width: 12.5%; text-align: right">เลขที่อ้างอิงกรมธรรม์ :</td>
                        <td style="width: 12.5%;">
                            <asp:Label ID="lblMotorApplicationCode" runat="server" ForeColor="DarkRed"></asp:Label>
                        </td>
                        <td style="width: 12.5%; text-align: right">ประเภทสัญญา :</td>
                        <td style="width: 12.5%">
                            <asp:Label ID="lblContract" runat="server" ForeColor="DarkRed"></asp:Label>
                        </td>

                        <td style="width: 12.5%; text-align: right;">สถานะกรมธรรม์ :
                        </td>
                        <td style="width: 12.5%;">
                            <asp:Label ID="lblStatus" runat="server" ForeColor="White" Font-Size="Small" Width="100%"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td style="text-align: right">บริษัทผู้รับประกัน :</td>
                        <td style="width: 12.5%;">
                            <asp:Label ID="lblInsuranceCompany" runat="server" ForeColor="DarkRed"></asp:Label>
                        </td>
                        <td style="width: 12.5%; text-align: right">ประเภทการซ่อม :</td>
                        <td style="width: 12.5%;">
                            <asp:Label ID="lblClaimType" runat="server" ForeColor="DarkRed"></asp:Label>
                        </td>
                        <td style="width: 12.5%; text-align: right">เริ่มคุ้มครอง :</td>
                        <td style="width: 12.5%;">
                            <asp:Label ID="lblStartCoverDate" runat="server" ForeColor="DarkRed"></asp:Label>
                            &nbsp;สิ้นสุด:
                            <asp:Label ID="lblEndCoverDate" runat="server" ForeColor="DarkRed"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td style="text-align: right">ผลิตภัณฑ์ :</td>
                        <td style="width: 12.5%;">
                            <asp:Label ID="lblProduct" runat="server" ForeColor="DarkRed"></asp:Label>
                        </td>
                        <td style="width: 12.5%; text-align: right">
                            <asp:Button ID="btnshowmodal" runat="server" Text="สิทธิประโยชน์" SkinID="link" OnClick="btnshowmodal_Click" Style="text-align: right" />
                            <ajaxToolkit:ModalPopupExtender ID="ModalPopupBenefit" runat="server" TargetControlID="btnshowmodal" PopupControlID="ModalPanel" BackgroundCssClass="modalBackground"></ajaxToolkit:ModalPopupExtender>
                        </td>
                        <td style="width: 12.5%;">
                            <asp:Button ID="btnshowmodalHistory" runat="server" Text="ประวัติการทำรายการ" SkinID="link" OnClick="btnshowmodalHistory_Click" Style="text-align: left" />
                            <ajaxToolkit:ModalPopupExtender ID="ModalHistory" runat="server" TargetControlID="btnshowmodalHistory" PopupControlID="ModaPanelHistory" BackgroundCssClass="modalBackground"></ajaxToolkit:ModalPopupExtender>
                        </td>
                        <td style="width: 12.5%; text-align: right">
                            <asp:Button ID="btnPolicyNumbers" runat="server" Text="เลขที่กรมธรรม์" SkinID="link" Style="text-align: right" OnClick="btnPolicyNumbers_Click" />
                            <ajaxToolkit:ModalPopupExtender ID="ModalPolicyNumbersDetail" runat="server" TargetControlID="btnPolicyNumbers" PopupControlID="ModalPanelPolicyNumbersDetail" BackgroundCssClass="modalBackground"></ajaxToolkit:ModalPopupExtender>
                        </td>
                        <td style="width: 12.5%;">
                            <asp:Button ID="btnAddPolicyNumbers" runat="server" Text="เพิ่มเลขที่กรมธรรม์" SkinID="link" Style="text-align: left" OnClick="btnAddPolicyNumbers_Click" />
                            <ajaxToolkit:ModalPopupExtender ID="ModalPolicyAddNumbers" runat="server" TargetControlID="btnAddPolicyNumbers" PopupControlID="ModalPanelAddPolicyNumbers" BackgroundCssClass="modalBackground"></ajaxToolkit:ModalPopupExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 12.5%; text-align: right">ผู้รับผลประโยชน์ :</td>
                        <td colspan="2">
                            <asp:Label ID="lblHeir" runat="server" Text="" ForeColor="DarkRed"></asp:Label>
                        </td>
                        <td style="width: 12.5%;"></td>
                        <td style="width: 12.5%;"></td>
                        <td style="width: 12.5%;"></td>
                    </tr>
                </table>
            </Content>
        </ajaxToolkit:AccordionPane>
    </Panes>
</ajaxToolkit:Accordion>

<asp:Panel ID="ModalPanel" runat="server" Width="500px" CssClass="modalPopup" align="center" Style="display: none">
    <uc1:ucBenefit runat="server" ID="ucBenefit" />
    <table style="width: 100%">
        <tr>
            <td style="text-align: right">
                <asp:Button ID="btnClose" runat="server" Text="ปิด" />
            </td>
        </tr>
    </table>
</asp:Panel>

<asp:Panel ID="ModalPanelPolicyNumbersDetail" runat="server" Width="800px" Height="400" ScrollBars="Vertical" CssClass="modalPopup" align="center" Style="display: none">
    <uc1:ucPolicyNumbersDetail runat="server" ID="ucPolicyNumbersDetail" />
    <table style="width: 100%">
        <tr>
            <td>
                <asp:Button ID="btnCloseModalPolicyNumbers" runat="server" Text="ปิด" SkinID="danger" />
            </td>
        </tr>
    </table>
</asp:Panel>

<asp:Panel ID="ModalPanelAddPolicyNumbers" runat="server" Width="800px" CssClass="modalPopup" align="center" Style="display: none">
    <uc1:ucPolicyNumbersAdd runat="server" ID="ucPolicyNumbersAdd" />
    <table style="width: 100%">
        <tr>
            <td style="width: 12.5%"></td>
            <td style="width: 12.5%; text-align: center;">
                <asp:Button ID="btnSave" runat="server" SkinID="success" Text="บันทึก" Width="30%" OnClick="btnSave_Click" />
            </td>
            <td style="width: 12.5%; text-align: center;">
                <asp:Button ID="btnCancel" runat="server" SkinID="danger" Text="ยกเลิก" Width="30%" OnClick="btnCancel_Click" />
            </td>
            <td style="width: 12.5%"></td>
        </tr>
    </table>
</asp:Panel>

<asp:Panel ID="ModaPanelHistory" runat="server" Width="800px" CssClass="modalPopup" align="center" Style="display: none">
    <%--  Gridview --%>
    <asp:GridView ID="dgvMain" runat="server" AutoGenerateColumns="false" AllowPaging="True" PageSize="10" OnRowDataBound="dgvMain_RowDataBound" OnSelectedIndexChanged="dgvMain_SelectedIndexChanged" OnPageIndexChanging="dgvMain_OnPageIndexChanging">
        <Columns>
            <asp:TemplateField HeaderText="" HeaderStyle-Width="1px">
                <ItemTemplate>
                    <%# Container.DataItemIndex + 1 %>.
                </ItemTemplate>
                <HeaderStyle Width="1px" />
            </asp:TemplateField>
            <asp:BoundField DataField="MotorApplicationTransaction_Code" HeaderText="รหัสรายการ" />
            <asp:BoundField DataField="TransactionTypeDetail" HeaderText="รายละเอียด" />
            <asp:BoundField DataField="CreatedBy" HeaderText="สร้างโดย" />
            <asp:BoundField DataField="CreatedDate" HeaderText="วันที่สร้าง" />
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
<asp:HiddenField ID="hdfMotorAppStatusID" runat="server" />
<asp:HiddenField ID="hdfMotorApplicationID" runat="server" />