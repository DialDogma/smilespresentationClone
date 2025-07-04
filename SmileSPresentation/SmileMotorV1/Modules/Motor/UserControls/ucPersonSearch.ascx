<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPersonSearch.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucPersonSearch" %>
<script src="../../../Scripts/sss.js"></script>
<link href="../../../Content/meStyleSheet.css" rel="stylesheet" />

<asp:UpdatePanel ID="upd1" runat="server">
    <ContentTemplate>
        <table style="width: 100%">
            <tr>
                <td style="width: 25%; text-align: right;">
                   
                </td>
                <td style="width: 30%;">
                    <asp:TextBox ID="txtPersonID" runat="server" Enabled="false"></asp:TextBox>
                </td>
                <td style="width: 25%;">
                    <asp:Button ID="btnPopup" runat="server" Text="ค้นหา" SkinID="info"/>
                    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnPopup" PopupControlID="ModalPanel" BackgroundCssClass="modalBackground"></ajaxToolkit:ModalPopupExtender>
                </td>
                <td style="width: 20%;">&nbsp;</td>
            </tr>
        </table>
        <asp:Panel ID="ModalPanel" runat="server" Width="800px" CssClass="modalPopup" align="center">
            <div class="panel panel-default">
                <div class="panel-body">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 25%; text-align: right;">
                                <asp:Label ID="Label1" runat="server" Text="คำค้นหา :" CssClass="h4"></asp:Label>
                            </td>
                            <td style="width: 30%;">
                                <asp:TextBox ID="txtSearch" runat="server" ToolTip="ค้นหาโดยชื่อ-สกุล,ชื่อเล่น,เลขบัตรประชาชน" placeholder="ค้นหาโดยชื่อ-สกุล,ชื่อเล่น,เลขบัตรประชาชน"></asp:TextBox>
                            </td>
                            <td style="width: 25%;">
                                <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" OnClick="btnSearch_Click" />
                            </td>
                            <td style="width: 20%;">&nbsp;</td>
                        </tr>

                        <tr>
                            <td colspan="4">
                                <asp:GridView ID="dgvMain" runat="server" AutoGenerateColumns="false" OnSelectedIndexChanged="dgvMain_SelectedIndexChanged" OnRowDataBound="OnRowDataBound" AllowPaging="True" OnPageIndexChanging="OnPageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="1px">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>.
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FullName" HeaderText="ชื่อ - สกุล" />
                                        <asp:BoundField DataField="NickName" HeaderText="ชื่อเล่น" />
                                        <asp:BoundField DataField="PersonCardDetail" HeaderText="หมายเลขบัตร" />
                                        <asp:BoundField DataField="PersonCardTypeDetail" HeaderText="ประเภทบัตร" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="text-align: center">
                                <asp:Button ID="btnClose" runat="server" Text="ปิด" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            </Triggers>
        </asp:Panel>

    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
        <asp:PostBackTrigger ControlID="dgvMain" />
    </Triggers>
</asp:UpdatePanel>


