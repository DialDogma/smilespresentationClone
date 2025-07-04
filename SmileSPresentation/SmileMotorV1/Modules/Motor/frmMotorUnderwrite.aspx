<%@ Page Title="ตรวจงาน" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmMotorUnderwrite.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmMotorUnderwrite" EnableEventValidation="false" Theme="default" %>

<%@ Register Src="UserControls/DropdownUserControls/ddlBranch.ascx" TagName="ddlBranch" TagPrefix="uc1" %>
<%@ Register Src="~/CommonUserControls/ucDatepicker.ascx" TagPrefix="uc1" TagName="ucDatepicker" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="panel-group">
            <div class="panel panel-default">
                <div class="panel-heading input-md">
                    <h4>ตรวจงาน</h4>
                </div>
                <div class="panel-body">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 12.5%; text-align: right">สาขา :</td>
                            <td colspan="2">
                                <uc1:ddlBranch ID="ddlBranch1" runat="server" AutoPostback="true" />
                            </td>
                            <td style="width: 12.5%; text-align: center">
                                <asp:Button ID="btnShow" runat="server" Width="80%" SkinID="info" Text="แสดง" OnClick="btnShow_Click" />
                            </td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%; text-align: right"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <asp:GridView ID="dgvMain" runat="server" Width="100%" AutoGenerateColumns="False" OnRowDataBound="OnRowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="0">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>.
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="MotorApplication_Code" HeaderText="Application Code" />
                                        <asp:BoundField DataField="ProductDetail" HeaderText="รายละเอียด" />
                                        <asp:BoundField DataField="CustName" HeaderText="ผู้เอาประกัน" />
                                        <asp:BoundField DataField="PayerName" HeaderText="ผู้ชำระเบี้ย" />
                                        <asp:BoundField DataField="BranchDetail" HeaderText="สาขา" />
                                        <asp:BoundField DataField="MotorApplicationStatusDetail" HeaderText="สถานะ" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
