<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDocument.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucDocument" %>
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
                <h4>ไฟล์เอกสาร</h4>
            </Header>
            <Content>
                <div class="table-responsive">
                    <asp:GridView ID="dgvMain" runat="server" Width="100%" CssClass="table" GridLines="Horizontal" AutoGenerateColumns="False" CellPadding="4" ShowHeaderWhenEmpty="True" EmptyDataText="ไม่พบข้อมูล" RowStyle-CssClass="GvGrid" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" ForeColor="Black" Style="text-align: center" UseAccessibleHeader="False" OnRowDataBound="OnRowDataBound" DataKeyNames="CustomerDocument_ID">
                        <Columns>
                            <asp:BoundField HeaderText="รหัส" DataField="CustomerDocument_ID" />
                            <asp:BoundField DataField="CustomerDocument_Code" HeaderText="รหัสเอกสาร" />
                            <asp:BoundField HeaderText="ประเภท" DataField="DocumentTypeDetail" />
                            <asp:BoundField HeaderText="ประเภทID" DataField="DocumentType_ID" />
                            <asp:BoundField HeaderText="รายละเอียด" DataField="DocumentDetail" />
                            <asp:BoundField HeaderText="หมายเหตุ" DataField="Remark" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnScan" runat="server" Text="Scan"></asp:Button>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnOpenDoc" runat="server" Text="จำนวนเอกสาร"></asp:Button>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                        <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                        <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                        <RowStyle CssClass="GvGrid"></RowStyle>
                        <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#F7F7F7" />
                        <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                        <SortedDescendingCellStyle BackColor="#E5E5E5" />
                        <SortedDescendingHeaderStyle BackColor="#242121" />
                    </asp:GridView>
                </div>
                <asp:HiddenField runat="server" ID="hdfAppID" />
            </Content>
        </ajaxToolkit:AccordionPane>
    </Panes>
</ajaxToolkit:Accordion>