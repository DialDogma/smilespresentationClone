<%@ Page Title="บันทึกการคิดผลงาน" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmPortFolio.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmPortFolio" Theme="default" %>

<%@ Register Src="~/CommonUserControls/ucDatepicker.ascx" TagPrefix="uc1" TagName="ucDatepicker" %>
<%@ Register Src="~/CommonUserControls/ucAlert.ascx" TagPrefix="uc1" TagName="ucAlert" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--    <asp:UpdatePanel runat="server">
        <ContentTemplate>--%>
    <uc1:ucAlert runat="server" ID="ucAlert" />
    <div class="container">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h4>บันทึกการคิดผลงาน</h4>
            </div>
            <div class="panel-body">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 12.5%; text-align: right">เลือกวันที่คิดผลงาน :</td>
                        <td style="width: 12.5%">
                            <uc1:ucDatepicker runat="server" ID="ucDatepicker1" />
                        </td>
                        <td style="width: 12.5%">&nbsp;</td>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%"></td>
                    </tr>
                    <tr>
                        <td style="width: 12.5%; text-align: right">หมายเหตุ :</td>
                        <td colspan="2">
                            <asp:TextBox ID="txtRemark" runat="server"></asp:TextBox>
                        </td>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%"></td>
                    </tr>
                    <tr>
                        <td style="width: 12.5%">จำนวน Application :</td>
                        <td style="width: 12.5%">
                            <asp:Label ID="lblResult" runat="server" SkinID="result" Text="0"></asp:Label>
                            &nbsp;รายการ</td>
                        <td style="width: 12.5%">
                            <asp:Button ID="btnExport" runat="server" SkinID="link" Text="Export รายละเอียด" OnClick="btnExport_Click" />
                        </td>
                        <td style="width: 12.5%">
                            <asp:Button ID="btnSave" runat="server" Text="บันทึกคิดผลงาน" SkinID="success" OnClick="btnSave_Click" />
                            <ajaxToolkit:ConfirmButtonExtender ID="btnSave_ConfirmButtonExtender" runat="server" BehaviorID="btnSave_ConfirmButtonExtender" ConfirmText="ยืนยันการบันทึกคิดผลงาน?" TargetControlID="btnSave" />
                        </td>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%"></td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <%-- <asp:GridView ID="dgvMain" runat="server" AutoGenerateColumns="true">
                                        <Columns>
                                            <asp:TemplateField
                                                HeaderText="" HeaderStyle-Width="1px">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>.
                                                </ItemTemplate>
                                                <HeaderStyle Width="1px" />
                                        </Columns>
                                    </asp:GridView>--%>
                        </td>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%"></td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <%--        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>--%>
</asp:Content>
