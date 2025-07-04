<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAppDetailAppReNew.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucAppDetailAppReNew" %>
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
                    <asp:Label ID="lblHeader" runat="server">ประวัติการต่ออายุ</asp:Label>
                </h4>
            </Header>
            <Content>

                <asp:GridView ID="dgvMain" runat="server" AutoGenerateColumns="False" AllowPaging="false" OnRowDataBound="dgvMain_OnRowDataBound" OnSelectedIndexChanged="dgvMain_OnSelectedIndexChanged" EmptyDataText="ไม่พบข้อมูล">
                    <Columns>
                        <asp:BoundField HeaderText="เลขแอพพลิเคชั่น" DataField="MotorApplication_Code" />
                        <asp:BoundField HeaderText="ชื่อ-สกุล" DataField="Customer" />
                        <asp:BoundField HeaderText="ยี่ห้อ" DataField="VehicleBrandDetail" />
                        <asp:BoundField HeaderText="ยี่ห้อ" DataField="VehicleBrandDetail" />
                        <asp:BoundField HeaderText="รุ่น" DataField="VehicleModelDetail" />
                        <asp:BoundField HeaderText="เลขทะเบียน" DataField="VehicleRegistrationNumber" />
                        <asp:BoundField HeaderText="หมายเลขตัวถัง" DataField="VehicleChassisNumber" />
                        <asp:BoundField HeaderText="วันที่เริ่มคุ้มครอง" DataField="StartCover" />
                        <asp:BoundField HeaderText="วันที่สิ้นสุดคุ้มครอง" DataField="EndCover" />
                        <asp:BoundField HeaderText="สถานะกรมธรรม์" DataField="MotorApplicationStatusDetail" />
                    </Columns>
                </asp:GridView>
            </Content>
        </ajaxToolkit:AccordionPane>
    </Panes>
</ajaxToolkit:Accordion>