<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAppDetailPersonRelatedApplication.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucAppDetailPersonRelatedApplication" %>
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
                <h4>กรมธรรม์ที่เกี่ยวข้องกับบุคคลนี้</h4>
            </Header>
            <Content>
                <div class="table-responsive">
                    <asp:UpdatePanel ID="upd1" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="dgvMain" runat="server" AutoGenerateColumns="False" AllowPaging="true" OnPageIndexChanging="dgvMain_PageIndexChanging" PageSize="6">
                                <Columns>
                                     <asp:TemplateField HeaderText="" HeaderStyle-Width="1px">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>.
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    <asp:BoundField DataField="MotorApplication_Code" HeaderText="แอพพลิเคชั่น" />
                                    <asp:BoundField DataField="PersonRelatedApplicationTypeDetail" HeaderText="ความสัมพันธ์" />
                                    <asp:BoundField DataField="FullName" HeaderText="ชื่อ - สกุล" />
                                    <asp:BoundField DataField="ProductDetail" HeaderText="ผลิตภัณฑ์" />
                                    <asp:BoundField DataField="MotorApplicationStatusDetail" HeaderText="สถานะแอพพลิเคชั่น" />
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:HiddenField ID="hdfPerson_ID" runat="server" />
                </div>
            </Content>
        </ajaxToolkit:AccordionPane>
    </Panes>
</ajaxToolkit:Accordion>
