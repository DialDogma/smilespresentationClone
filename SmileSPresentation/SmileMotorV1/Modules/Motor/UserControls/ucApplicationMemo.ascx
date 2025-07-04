<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucApplicationMemo.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucApplicationMemo" %>
<%@ Register Src="DropdownUserControls/ddlMemoType.ascx" TagName="ddlMemoType" TagPrefix="uc1" %>
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
                <h4>บันทึกข้อความ</h4>
            </Header>
            <Content>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="table-responsive">
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 12.5%; text-align: right">ประเภท :</td>
                                    <td style="width: 12.5%">
                                        <uc1:ddlMemoType ID="ddlMemoType1" runat="server" />
                                    </td>
                                    <td style="width: 12.5%">
                                        <asp:Button ID="btnShow" runat="server" SkinID="info" Text="แสดง" Width="80%" OnClick="btnShow_Click" />
                                    </td>
                                    <td style="width: 12.5%"></td>
                                    <td style="width: 12.5%">
                                        <asp:HiddenField ID="hdfApp_id" runat="server" />
                                    </td>
                                    <td style="width: 12.5%"></td>
                                    <td style="width: 12.5%"></td>
                                    <td style="width: 12.5%"></td>
                                </tr>
                                <tr>
                                    <td colspan="8">
                                        <asp:GridView ID="dgvMain" runat="server" AutoGenerateColumns="false" Style="margin-right: 3px">
                                            <Columns>
                                                <asp:BoundField HeaderText="ID" DataField="MotorApplicationMemo_ID" />
                                                <asp:BoundField HeaderText="ประเภท" DataField="MotorApplicationMemoTypeDetail" />
                                                <asp:BoundField HeaderText="ข้อความ" DataField="MotorApplicationMemoDetail" />
                                                <asp:BoundField HeaderText="รหัสผู้บันทึก" DataField="EmployeeCode" />
                                                <asp:BoundField HeaderText="วันที่บันทึก" DataField="CreateDate" />
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Button ID="btnAdd" runat="server" SkinID="success" Text="เพิ่ม" />
                                        <ajaxToolkit:ModalPopupExtender ID="mpe" runat="server" TargetControlID="btnAdd" PopupControlID="ModalPanel"
                                            BackgroundCssClass="modalBackground" />
                                    </td>
                                    <td style="width: 12.5%"></td>
                                    <td style="width: 12.5%"></td>
                                    <td style="width: 12.5%"></td>
                                    <td style="width: 12.5%"></td>
                                    <td style="width: 12.5%"></td>
                                    <td style="width: 12.5%"></td>
                                </tr>
                                <tr>
                                    <td style="width: 12.5%"></td>
                                    <td style="width: 12.5%"></td>
                                    <td style="width: 12.5%"></td>
                                    <td style="width: 12.5%"></td>
                                    <td style="width: 12.5%"></td>
                                    <td style="width: 12.5%"></td>
                                    <td style="width: 12.5%"></td>
                                    <td style="width: 12.5%"></td>
                                </tr>
                            </table>

                        </div>

                        <asp:Panel ID="ModalPanel" runat="server" Width="87.5%" CssClass="modalPopup" align="center">

                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h4 class="panel-title">บันทึกข้อความ</h4>
                                </div>
                                <div class="panel-body">
                                    <div class="table-responsive">

                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 12.5%; text-align: right">ประเภท :</td>
                                                <td style="width: 12.5%">
                                                    <uc1:ddlMemoType ID="ddlMemoType2" runat="server" />
                                                </td>
                                                <td style="width: 12.5%"></td>
                                                <td style="width: 12.5%"></td>
                                                <td style="width: 12.5%"></td>
                                                <td style="width: 12.5%"></td>
                                                <td style="width: 12.5%"></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 12.5%; text-align: right">ข้อความ :</td>
                                                <td colspan="5">
                                                    <asp:TextBox ID="txtRemark" runat="server" Width="100%"></asp:TextBox>
                                                </td>
                                                <td style="width: 12.5%"></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 12.5%"></td>
                                                <td style="width: 12.5%"></td>
                                                <td style="width: 12.5%"></td>
                                                <td style="width: 12.5%"></td>
                                                <td style="width: 12.5%"></td>
                                                <td style="width: 12.5%"></td>
                                                <td style="width: 12.5%"></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 12.5%"></td>
                                                <td style="width: 12.5%"></td>
                                                <td colspan="3" style="text-align: center">
                                                    <asp:Label ID="lblWarning" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td style="width: 12.5%"></td>
                                                <td style="width: 12.5%"></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 12.5%"></td>
                                                <td style="width: 12.5%"></td>
                                                <td colspan="3" style="text-align: center">
                                                    <asp:Button ID="btnSave" runat="server" Text="บันทึก" Width="30%" OnClick="btnSave_Click" />
                                                    <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" Width="30%" OnClick="btnCancel_Click" />
                                                </td>
                                                <td style="width: 12.5%"></td>
                                                <td style="width: 12.5%"></td>
                                            </tr>
                                        </table>


                                    </div>
                                </div>
                            </div>

                        </asp:Panel>

                    </ContentTemplate>
                </asp:UpdatePanel>

            </Content>
        </ajaxToolkit:AccordionPane>
    </Panes>
</ajaxToolkit:Accordion>


