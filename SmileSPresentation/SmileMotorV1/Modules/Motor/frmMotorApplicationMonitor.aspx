<%@ Page Title="การแจ้งเตือน" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmMotorApplicationMonitor.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmMotorApplicationMonitor" Theme="default" %>

<%@ Register TagPrefix="uc2" TagName="ucProgressbar" Src="~/CommonUserControls/ucProgressbar.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <uc2:ucProgressbar ID="ucProgressbar1" runat="server" />
            <div class="container">
                <div class="panel panel-default">
                    <div class="panel-heading input-md">
                        <h4>การแจ้งเตือน<asp:Label ID="lbTitle" runat="server"></asp:Label></h4>
                    </div>
                    <div class="panel-body">
                        <div class="table-responsive">
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:GridView ID="dgvMain"
                                            runat="server"
                                            AutoGenerateColumns="False"
                                            AllowPaging="True"
                                            OnPageIndexChanging="dgvMain_PageIndexChanging"
                                            PageSize="15"
                                            OnSelectedIndexChanged="dgvMain_SelectedIndexChanged"
                                            OnRowDataBound="dgvMain_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="0">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>.
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%-- <asp:BoundField DataField="insurancecompanydetail" HeaderText="บริษัทผู้รับประกัน" />--%>
                                                <asp:BoundField DataField="motorapplication_code" HeaderText="เลขแอพพลิเคชั่น" />
                                                <asp:BoundField DataField="productdetail" HeaderText="ผลิตภัณฑ์" />
                                                <asp:BoundField DataField="customer" HeaderText="ชื่อ-สกุล" />
                                                <asp:BoundField DataField="vehiclebranddetail" HeaderText="ยี่ห้อ" />
                                                <asp:BoundField DataField="vehiclemodeldetail" HeaderText="รุ่น" />
                                                <asp:BoundField DataField="vehicleregistrationnumber" HeaderText="เลขทะเบียน" />
                                                <asp:BoundField DataField="startcover" HeaderText="วันที่เริ่มคุ้มครอง" DataFormatString="{0:dd/MM/yyyy}" />
                                                <asp:BoundField DataField="EndCover" HeaderText="วันสิ้นสุดคุ้มครอง" DataFormatString="{0:dd/MM/yyyy}" />

                                                <asp:BoundField DataField="motorapplicationstatusdetail" HeaderText="สถานะกรมธรรม์" />
                                                <%--<asp:BoundField DataField="LastUpdateDate" HeaderText="วันที่ทำรายการล่าสุด" />--%>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="table-responsive">
                            <table class="table" style="width: 100%">
                                <tr>
                                    <td style="width: 12.5%; text-align: right;"></td>
                                    <td style="width: 12.5%; text-align: right;"></td>
                                    <td style="width: 12.5%; text-align: right;"></td>
                                    <td style="width: 12.5%; text-align: right;"></td>
                                    <td style="width: 12.5%; text-align: right;"></td>
                                    <td style="width: 12.5%; text-align: right;"></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>