<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAppDetailDept.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucAppDetailDept" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucDebtRecieveDetail.ascx" TagPrefix="uc1" TagName="ucDebtRecieveDetail" %>

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
                <h4>ข้อมูลการชำระเงิน</h4>
            </Header>
            <Content>
                <asp:Button ID="btnLinkCashReceive" runat="server" SkinID="link" Text="เรียกดูยอดการชำระเงิน" Style="float: right" />
                <asp:UpdatePanel ID="updMain" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="dgvMain"
                            runat="server"
                            AutoGenerateColumns="false"
                            OnRowDataBound="dgvMain_RowDataBound"
                            OnSelectedIndexChanged="dgvMain_SelectedIndexChanged"
                            AllowPaging="false"
                            OnPageIndexChanging="dgvMain_PageIndexChanging" PageSize="6">
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="0">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>.
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Period" HeaderText="งวดชำระ" DataFormatString="{0:MM/yyyy}" />
                                <%-- <asp:BoundField DataField="InsuranceCompanyDetail" HeaderText="บริษัทประกัน" />--%>
                                <asp:BoundField DataField="ProductDetail" HeaderText="แผนความคุ้มครอง" />
                                <asp:BoundField DataField="Premium_Dept" HeaderText="ตั้งหนี้" />
                                <asp:BoundField DataField="Premium_Receive" HeaderText="ชำระแล้ว" />
                                <asp:BoundField DataField="PayMethodTypeDetail" HeaderText="วิธีการชำระ" />
                                <asp:BoundField DataField="PeriodTypeDetail" HeaderText="งวดชำระ" />
                                <asp:BoundField DataField="DebtStatus" HeaderText="สถานะการตั้งหนี้" />
                              <%--  <asp:BoundField DataField="Remark" HeaderText="หมายเหตุ" />--%>
                            </Columns>
                        </asp:GridView>
                        <asp:HiddenField ID="hdfApp_ID" runat="server" />
                        <ajaxToolkit:ModalPopupExtender ID="mpe"
                            runat="server"
                            TargetControlID="dgvMain"
                            PopupControlID="ModalPanel"
                            BackgroundCssClass="modalBackground" />

                        <asp:Panel ID="ModalPanel"
                            runat="server"
                            Width="900px"
                            Height="80%"
                            CssClass="modalPopup"
                            align="center"
                            Style="display: none"
                            ScrollBars="Vertical">
                            <%--Get UC Here For Modal Detail--%>
                            <uc1:ucDebtRecieveDetail runat="server" ID="ucDebtRecieveDetail" />
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Button ID="btnClose" runat="server" SkinID="danger" Text="ปิด" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="dgvMain" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </Content>
        </ajaxToolkit:AccordionPane>
    </Panes>
</ajaxToolkit:Accordion>