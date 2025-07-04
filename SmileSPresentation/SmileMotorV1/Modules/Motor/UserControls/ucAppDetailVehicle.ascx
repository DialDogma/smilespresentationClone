<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAppDetailVehicle.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucAppDetailVehicle" %>
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
                <h4>ข้อมูลรถ</h4>
            </Header>
            <Content>
                <table style="width: 100%">
                    <tr>
                        <td style="width: 12.5%; text-align: right">ยี่ห้อรถยนต์ :</td>
                        <td style="width: 12.5%">
                            <asp:Label ID="lblVehicleBrand" runat="server" SkinID="result"></asp:Label>
                        </td>
                        <td style="width: 12.5%; text-align: right">ประเภทรถ : </td>
                        <td style="width: 12.5%">
                            <asp:Label ID="lblVehicleType" runat="server" SkinID="result"></asp:Label>
                        </td>
                        <td style="width: 12.5%; text-align: right">ประเภทย่อย :</td>
                        <td style="width: 12.5%">
                            <asp:Label ID="lblVehicleSegment" runat="server" SkinID="result"></asp:Label>
                            <asp:Label ID="lblFuelType" runat="server" SkinID="result"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td style="width: 12.5%; text-align: right">รุ่นรถ :</td>
                        <td style="width: 12.5%">
                            <asp:Label ID="lblVehicleModel" runat="server" SkinID="result"></asp:Label>
                        </td>
                        <td style="width: 12.5%; text-align: right">ขนาดเครื่องยนต์ : </td>
                        <td style="width: 12.5%">
                            <asp:Label ID="lblVehicleCC" runat="server" SkinID="result"></asp:Label>
                        </td>
                        <td style="width: 12.5%; text-align: right">น้ำหนักรถ(kg) :</td>
                        <td style="width: 12.5%">
                            <asp:Label ID="lblWeightVehicle" runat="server" SkinID="result"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td style="width: 12.5%; text-align: right">เลขทะเบียน :</td>
                        <td style="width: 12.5%">
                            <asp:Label ID="lblLicensePlate" runat="server" SkinID="result"></asp:Label>
                        </td>
                        <td style="width: 12.5%; text-align: right">จังหวัด :</td>
                        <td style="width: 12.5%">
                            <asp:Label ID="lblVehicleProvince" runat="server" Text="" SkinID="result"></asp:Label>
                        </td>
                        <td style="width: 12.5%; text-align: right">ปีที่จดทะเบียน :</td>
                        <td style="width: 12.5%">
                            <asp:Label ID="lblVehicleYear" runat="server" SkinID="result"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td style="width: 12.5%; text-align: right">จำนวนที่นั่ง :</td>
                        <td style="width: 12.5%">
                            <asp:Label ID="lblSeat" runat="server" SkinID="result"></asp:Label>
                        </td>
                        <td style="width: 12.5%; text-align: right">เลขเครื่องยนต์ :</td>
                        <td style="width: 12.5%">
                            <asp:Label ID="lblEngineNumber" runat="server" SkinID="result"></asp:Label>
                        <td style="width: 12.5%; text-align: right">หมายเลขตัวถัง :</td>
                        <td style="width: 12.5%">
                            <asp:Label ID="lblChassisNumber" runat="server" SkinID="result"></asp:Label>
                        </td>
                    </tr>
                </table>
            </Content>
        </ajaxToolkit:AccordionPane>
    </Panes>
</ajaxToolkit:Accordion>