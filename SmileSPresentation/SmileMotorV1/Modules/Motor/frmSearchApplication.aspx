<%@ Page Title="ค้นหา Application" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmSearchApplication.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmSearchApplication" Theme="default" %>

<%@ Register Src="UserControls/DropdownUserControls/ddlMotorCompanyInsurance.ascx" TagName="ddlMotorCompanyInsurance" TagPrefix="uc1" %>

<%@ Register Src="../../CommonUserControls/ucProgressbar.ascx" TagName="ucProgressbar" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <uc2:ucProgressbar ID="ucProgressbar1" runat="server" />
            <div class="container">
                <div class="panel panel-default">
                    <div class="panel-heading input-md">
                        <h4>ค้นหา Application</h4>
                    </div>
                    <div class="panel-body">
                        <div class="table-responsive">
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 12.5%; text-align: right;">บริษัทผู้รับประกัน :</td>
                                    <td style="width: 12.5%; text-align: right;">
                                        <uc1:ddlMotorCompanyInsurance ID="ddlMotorCompanyInsurance" runat="server" />
                                    </td>
                                    <td style="width: 12.5%; text-align: right;"></td>
                                    <td style="text-align: right;" colspan="3"></td>
                                </tr>
                                <tr>
                                    <td style="width: 12.5%; text-align: right;">คำค้นหา :</td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" Width="100%" placeholder="พิมพ์คำค้นหาที่นี่"></asp:TextBox>
                                    </td>
                                    <td style="width: 12.5%; text-align: center;">
                                        <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" SkinID="info" Width="90%" OnClick="btnSearch_Click" /></td>
                                    <td style="width: 12.5%; text-align: right;"></td>
                                    <td style="width: 12.5%; text-align: right;"></td>
                                </tr>
                            </table>
                        </div>
                        <br />
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
                                                <asp:BoundField DataField="endcover" HeaderText="วันสิ้นสุดคุ้มครอง" DataFormatString="{0:dd/MM/yyyy}" />
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