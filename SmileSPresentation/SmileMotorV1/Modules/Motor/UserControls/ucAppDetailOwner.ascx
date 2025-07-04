<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAppDetailOwner.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucAppDetailOwner" %>
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
                <h4>ข้อมูลเจ้าของผลงาน</h4>
            </Header>
            <Content>

                <asp:GridView ID="dgvMain" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="MotorApplicationOwnerTypeDetail" HeaderText="ประเภทเจ้าของผลงาน" />
                        <asp:BoundField DataField="EmployeeCode" HeaderText="รหัสพนักงาน" />
                        <asp:BoundField DataField="FullName" HeaderText="ชื่อ - สกุล" />
                        <asp:BoundField DataField="BranchDetail" HeaderText="สาขา" />
                        <asp:BoundField DataField="EmployeeTeamDetail" HeaderText="ทีม" />
                        <asp:BoundField DataField="ZebraOwnerDetail" HeaderText="หมายเลขรถม้าลาย" />
                    </Columns>
                </asp:GridView>
            </Content>
        </ajaxToolkit:AccordionPane>
    </Panes>
</ajaxToolkit:Accordion>