<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAppDetailAddress.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucAppDetailAddressCustomer" %>
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
                    <asp:Label ID="lblHeader" runat="server"></asp:Label>
                </h4>
            </Header>
            <Content>

                <asp:GridView ID="dgvMain" runat="server" AutoGenerateColumns="False" AllowPaging="false">
                    <Columns>
                        <asp:BoundField HeaderText="ประเภทที่อยู่" DataField="AddressTypeDetail" HeaderStyle-Wrap="false" />
                        <asp:BoundField HeaderText="ที่อยู่" DataField="FullAddress" ItemStyle-HorizontalAlign="Left" />
                    </Columns>
                </asp:GridView>

            </Content>
        </ajaxToolkit:AccordionPane>
    </Panes>
</ajaxToolkit:Accordion>


