<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmPaymentHistory.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmPaymentHistory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h2 class="panel-title">ประวัติการชำระเบี้ย</h2>
            </div>
            <div class="panel-body">
                <div class="table-responsive">
                        <table style="width: 100%;" class="table">
                        <tr>
                            <td style="width: 12.5%; text-align: right;">Application ID :</td>
                            <td style="width: 12.5%; text-align: right;">
                                
                                <asp:TextBox ID="txtSearchAppID" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                
                            </td>
                            <td style="width: 12.5%; text-align: right;">คำค้นหา :</td>
                            <td style="text-align: right;" colspan="3">
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" Width="100%" placeholder="พิมพ์คำค้นหาที่นี่"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 12.5%; text-align: right;">&nbsp;</td>
                            <td style="width: 12.5%; text-align: right;">
                                &nbsp;</td>
                            <td style="width: 12.5%; text-align: right;"></td>
                            <td style="width: 12.5%; text-align: right;"></td>
                            <td style="width: 12.5%; text-align: right;"></td>
                            <td style="width: 12.5%; text-align: right;"></td>
                        </tr>
                        <tr>
                            <td style="width: 12.5%; text-align: right;"></td>
                            <td style="width: 12.5%; text-align: right;"></td>
                            <td style="width: 12.5%; text-align: right;">
                                <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" CssClass="form-control btn btn-info" Width="100%" /></td>
                            <td style="width: 12.5%; text-align: right;">
                                <asp:Button ID="btnClear" runat="server" Text="ล้างข้อมูล" CssClass="form-control btn btn-default" Width="100%" /></td>
                            <td style="width: 12.5%; text-align: right;"></td>
                            <td style="width: 12.5%; text-align: right;"></td>
                        </tr>
                    </table>
                </div>

                <div class="table-responsive">
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <asp:GridView ID="dgvMain" runat="server" AutoGenerateColumns="False" CellPadding="4" DataKeyNames="ID" EmptyDataText="ไม่พบข้อมูล" CssClass="table" GridLines="Horizontal" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" ForeColor="Black">
                                    <Columns>
                                        <asp:ButtonField CommandName="Select" HeaderText="เลือก" Text="เลือก" />
                                        <asp:BoundField DataField="ID" HeaderText="ID" />
                                        <asp:BoundField DataField="Product" HeaderText="ผลิตภัณฑ์" />
                                        <asp:BoundField DataField="ClaimType" HeaderText="วิธีการเคลม" />
                                        <asp:BoundField DataField="CustomerName" HeaderText="ชื่อลูกค้า" />
                                        <asp:BoundField DataField="StartCoverDate" HeaderText="วันที่เริ่มคุ้มครอง" />
                                        <asp:BoundField DataField="EndCoverDate" HeaderText="วันที่สิ้นสุด" />
                                        <asp:BoundField DataField="VehicleLicensePlate" HeaderText="ทะเบียนรถ" />
                                    </Columns>
                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                    <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                                    <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                    <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                    <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                    <SortedDescendingHeaderStyle BackColor="#242121" />
                                </asp:GridView>
                            </td>

                        </tr>

                    </table>
                </div>
                <div class="table-responsive">
                    <table class="table" style="width:100%">
                        <tr>
                            <td style="width: 12.5%; text-align: right;"></td>
                            <td style="width: 12.5%; text-align: right;"></td>
                            <td style="width: 12.5%; text-align: right;"></td>
                            <td style="width: 12.5%; text-align: right;"></td>
                            <td style="width: 12.5%; text-align: right;"><asp:Button ID="btnEdit" runat="server" Text="ดูข้อมูล" CssClass="form-control btn btn-primary" Width="100%"/></td>
                            <td style="width: 12.5%; text-align: right;">&nbsp;</td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
