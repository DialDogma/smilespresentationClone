<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucMonitorForMotor.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucMonitorForMotor" %>
<%@ Register Src="~/CommonUserControls/ucDatepicker.ascx" TagPrefix="uc1" TagName="ucDatepicker" %>

<%--<link href="../../../Content/meStyleSheet.css" rel="stylesheet" />--%>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>

        <table style="width: 100%">
            <tr>
                <td style="text-align: right;">งวดความคุ้มครองวันที่ :</td>
                <td style="width: 12.5%">
                    <uc1:ucDatepicker runat="server" ID="ucDateStart" />
                </td>
                <td style="width: 12.5%; text-align: right;">ถึงวันที่ :</td>
                <td style="width: 12.5%">
                    <uc1:ucDatepicker runat="server" ID="ucDateEnd" />
                </td>
                <td style="width: 12.5%; text-align: center">
                    <asp:Button ID="btnShow" runat="server" Width="80%" SkinID="primary" Text="แสดง" />
                </td>
                <td style="width: 12.5%">
                    <asp:Button ID="btnExport" runat="server" Width="80%" SkinID="success" Text="Export" OnClick="btnExport_Click" /></td>
            </tr>
            <tr>
                <td colspan="6">
                    <asp:GridView ID="dgvMain" runat="server" OnRowDataBound="OnRowDataBound" OnSelectedIndexChanged="OnSelectedIndexChanged" AutoGenerateColumns="false" ShowFooter="True">
                        <Columns>
                            <asp:TemplateField HeaderStyle-Width="0">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>.
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="สาขา" DataField="BranchDetail" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="บันทึกลูกค้าใหม่" DataField="บันทึกลูกค้าใหม่" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField HeaderText="รอการอนุมัติ" DataField="รอการอนุมัติ" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField HeaderText="แก้ไขรอการอนุมัติ" DataField="แก้ไขรอการอนุมัติ" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField HeaderText="อนุมัติ" DataField="อนุมัติ" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField HeaderText="ไม่อนุมัติ" DataField="ไม่อนุมัติ" ItemStyle-HorizontalAlign="Center" />                            
                            <asp:BoundField HeaderText="รอการแก้ไข" DataField="รอการแก้ไข" ItemStyle-HorizontalAlign="Center" />                                                 
                            <asp:BoundField HeaderText="ยกเลิก" DataField="ยกเลิก" ItemStyle-HorizontalAlign="Center" />
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
