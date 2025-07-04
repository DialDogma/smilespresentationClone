<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmEndorseMotor.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmEndorseMotor" Theme="default" EnableEventValidation="false" %>

<%@ Register Src="../../CommonUserControls/ucProgressbar.ascx" TagName="ucProgressbar" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
            <div class="container">
                <div class="panel panel-default">
                    <div class="panel-heading input-md">
                        <h4>ค้นหา Application</h4>
                    </div>
                    <div class="panel-body">
                        <div class="table-responsive">
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 12.5%; text-align: right;">ประเภทการสลักหลัง :</td>
                                    <td style="width: 12.5%; text-align: right;">
                                        <asp:DropDownList ID="ddlEndorseType" AutoPostBack="True" runat="server"></asp:DropDownList>
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
                                            OnSelectedIndexChanged="dgvMain_OnSelectedIndexChanged"
                                            OnRowDataBound="dgvMain_OnRowDataBound"
                                            PageSize="15">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="0">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>.
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="insurancecompanydetail" HeaderText="บริษัทผู้รับประกัน" />
                                                <asp:BoundField DataField="productdetail" HeaderText="ผลิตภัณฑ์" />
                                                <asp:BoundField DataField="motorapplication_code" HeaderText="เลขแอพพลิเคชั่น" />
                                                <asp:BoundField DataField="customer" HeaderText="ชื่อ-สกุล" />
                                                <asp:BoundField DataField="startcover" HeaderText="วันที่เริ่มคุ้มครอง" DataFormatString="{0:dd/MM/yyyy}" />
                                                <asp:BoundField DataField="vehiclebranddetail" HeaderText="ยี่ห้อ" />
                                                <asp:BoundField DataField="vehiclemodeldetail" HeaderText="รุ่น" />
                                                <asp:BoundField DataField="vehicleregistrationnumber" HeaderText="เลขทะเบียน" />
                                                <asp:BoundField DataField="motorapplicationstatusdetail" HeaderText="สถานะกรมธรรม์" />
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
</asp:Content>
