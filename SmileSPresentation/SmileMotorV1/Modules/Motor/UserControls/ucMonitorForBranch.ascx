<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucMonitorForBranch.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucMonitorForBranch" %>
<%@ Register Src="../../../CommonUserControls/ucDatepicker.ascx" TagName="ucDatepicker" TagPrefix="uc2" %>

<%@ Register Src="DropdownUserControls/ddlBranch.ascx" TagName="ddlBranch" TagPrefix="uc3" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <table style="width: 100%;">
            <tr>
                <td style="width: 12.5%">
                    <uc3:ddlBranch ID="ddlBranch" runat="server" />
                </td>
                <td style="text-align: right">งวดความคุ้มครองวันที่ :</td>

                <td style="width: 12.5%">
                    <uc2:ucDatepicker ID="ucDateStart" runat="server" />
                </td>
                <td style="width: 12.5%; text-align: right;">ถึงวันที่ :</td>
                <td style="width: 12.5%">
                    <uc2:ucDatepicker ID="ucDateEnd" runat="server" />
                </td>
                <td style="width: 12.5%; text-align: center">
                    <asp:Button ID="btnShow" runat="server" Width="80%" SkinID="primary" Text="แสดง"/>
                </td>
                <td style="width: 12.5%">
                    <asp:Button ID="btnExport" runat="server" Width="80%" SkinID="success" Text="Export" OnClick="btnExport_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="7">
                    <asp:GridView ID="dgvMain" runat="server" OnRowDataBound="OnRowDataBound" OnSelectedIndexChanged="OnSelectedIndexChanged" AutoGenerateColumns="false" Style="margin-right: 3px" ShowFooter="True">
                        <Columns>
                            <asp:TemplateField HeaderText="" HeaderStyle-Width="1px">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>.
                                </ItemTemplate>
                                <HeaderStyle Width="1px" />
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="รหัสพนักงาน" DataField="EmployeeCode" />
                            <asp:BoundField HeaderText="ชื่อ-สกุล" DataField="FullName" />
                            <asp:BoundField HeaderText="บันทึกลูกค้าใหม่" DataField="บันทึกลูกค้าใหม่" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="รอการอนุมัติ" DataField="รอการอนุมัติ" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="แก้ไขรอการอนุมัติ" DataField="แก้ไขรอการอนุมัติ" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="อนุมัติ" DataField="อนุมัติ" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="ไม่อนุมัติ" DataField="ไม่อนุมัติ" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="รอการแก้ไข" DataField="รอการแก้ไข" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="ยกเลิก" DataField="ยกเลิก" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnExport" />
    </Triggers>
</asp:UpdatePanel>
