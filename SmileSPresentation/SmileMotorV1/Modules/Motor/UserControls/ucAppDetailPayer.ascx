<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAppDetailPayer.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucAppDetailPayer" %>
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
                <h4>ข้อมูลผู้ชำระเบี้ย <asp:Label ID="lblRelationTypeDetail" runat="server" SkinID="result" Font-Size="Small"></asp:Label></h4>
            </Header>
            <Content>

                <asp:Panel ID="PnlCustomer" runat="server">

                    <table style="width:100%" runat="server" id="tblCustomer">
                        <tr>
                        <td style="width: 12.5%; text-align: right">ชื่อ-นามสกุล :</td>
                        <td style="width: 12.5%">
                            <asp:Label ID="lblFullName" runat="server" SkinID="result"></asp:Label>
                        </td>
                        <td style="width: 12.5%; text-align: right">เพศ :
                        </td>
                        <td style="width: 12.5%">
                            <asp:Label ID="lblPayerSex" runat="server" SkinID="result"></asp:Label>
                        </td>
                        <td style="width: 12.5%; text-align: right">สถานภาพ :</td>
                        <td style="width: 12.5%">
                            <asp:Label ID="lblPayerStatus" runat="server" SkinID="result"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td style="width: 12.5%; text-align: right">เลขบัตรประชาชน :</td>
                        <td style="width: 12.5%">
                            <asp:Label ID="lblPayerCardNumber" runat="server" SkinID="result"></asp:Label>
                        </td>
                        <td style="width: 12.5%; text-align: right">Passport :</td>
                        <td style="width: 12.5%">
                            <asp:Label ID="lblPayerPassportNumber" runat="server" SkinID="result"></asp:Label>
                        </td>
                        <td style="width: 12.5%; text-align: right">วันเกิด :</td>
                        <td style="width: 12.5%">
                            <asp:Label ID="lblPayerBirthDate" runat="server" SkinID="result"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td style="width: 12.5%; text-align: right">ประเภทอาชีพ :</td>
                        <td style="width: 12.5%">
                            <asp:Label ID="lblPayerJobType" runat="server" SkinID="result"></asp:Label>
                        </td>
                        <td style="width: 12.5%; text-align: right">ลักษณะงาน :</td>
                        <td style="width: 12.5%">
                            <asp:Label ID="lblPayerJobDetail" runat="server" SkinID="result"></asp:Label>
                        </td>
                        <td style="width: 12.5%; text-align: right"></td>
                        <td style="width: 12.5%">
                           
                           <%--<asp:Label ID="lblPersonRelationTypeDetail" runat="server" SkinID="result"></asp:Label>--%>
                        </td>
                    </tr>
                    </table>

           </asp:Panel>



                <asp:Panel ID="PnlCorporate" runat="server">
                    <table style="width:100%" runat="server" id="tblCorporate">
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

                <table style="width: 100%">
                    
                    <tr>
                        <td style="width: 12.5%; text-align: right">&nbsp;</td>
                        <td style="width: 12.5%">&nbsp;</td>
                        <td style="width: 12.5%; text-align: right">&nbsp;</td>
                        <td style="width: 12.5%">&nbsp;</td>
                        <td style="width: 12.5%; text-align: right">
                             <asp:Button ID="btnshowmodalHistory" runat="server" Text="ประวัติการทำรายการ"  SkinID="link"/>
                            <ajaxToolkit:ModalPopupExtender ID="ModalHistory" runat="server" TargetControlID="btnshowmodalHistory" PopupControlID="ModaPanelHistory" BackgroundCssClass="modalBackground"></ajaxToolkit:ModalPopupExtender>
                        </td>
                        <td style="width: 12.5%">
                            <asp:Button ID="btnEditPerson" runat="server" Text="แก้ไขข้อมูลผู้ชำระเบี้ย" />
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

<asp:Panel ID="ModaPanelHistory" Width="800" runat="server" CssClass="modalPopup" align="center" style="display:contents">
    <%-- Gridview --%>
     <asp:GridView ID="dgvHistory" runat="server" AutoGenerateColumns="false" OnRowDataBound="dgvHistory_RowDataBound1" OnSelectedIndexChanged="dgvHistory_SelectedIndexChanged1">
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

