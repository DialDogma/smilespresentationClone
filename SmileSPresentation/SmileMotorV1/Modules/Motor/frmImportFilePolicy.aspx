<%@ Page Title="นำเข้าไฟล์กรมธรรม์" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmImportFilePolicy.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmImportFilePolicy" Theme="default" EnableEventValidation="false" %>

<%@ Register Src="UserControls/ucAppDetailApplication.ascx" TagName="ucAppDetailApplication" TagPrefix="uc1" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucDocument.ascx" TagPrefix="uc1" TagName="ucDocument" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4>นำเข้าไฟล์กรมธรรม์</h4>
                    </div>
                    <div class="panel-body">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 12.5%; text-align: right">ค้นหา Application :</td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" Width="100%" placeholder="พิมพ์คำค้นหาที่นี่"></asp:TextBox>
                                </td>
                                <td style="width: 12.5%">
                                    <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" SkinID="info" Width="90%" OnClick="btnSearch_Click" /></td>
                                <td style="width: 12.5%; text-align:right">&nbsp;</td>
                                <td style="width: 12.5%">
                                    &nbsp;</td>
                                <td style="width: 12.5%; text-align:right">ผลการค้นหา :</td>
                                <td style="width: 12.5%">
                                    <asp:Label ID="lblCountResult" runat="server" SkinID="result" Text="0"></asp:Label>
                                    &nbsp;รายการ</td>
                            </tr>
                            <tr>
                                <td colspan="8">
                                    <asp:GridView ID="dgvMain" runat="server" AutoGenerateColumns="false" AllowPaging="True" OnPageIndexChanging="dgvMain_PageIndexChanging" OnRowDataBound="dgvMain_RowDataBound" OnSelectedIndexChanged="dgvMain_SelectedIndexChanged" PageSize="5">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="0">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex +1 %>.
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="InsuranceCompanyDetail" HeaderText="บริษัทผู้รับประกัน" />
                                            <asp:BoundField DataField="ProductDetail" HeaderText="ผลิตภัณฑ์" />
                                            <asp:BoundField DataField="MotorApplication_Code" HeaderText="เลขแอพพลิเคชั่น" />
                                            <asp:BoundField DataField="CustName" HeaderText="ชื่อ-สกุล" />
                                            <asp:BoundField DataField="StartCover" HeaderText="วันที่เริ่มคุ้มครอง" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="VehicleBrandDetail" HeaderText="ยี่ห้อ" />
                                            <asp:BoundField DataField="VehicleModelDetail" HeaderText="รุ่น" />
                                            <asp:BoundField DataField="VehicleRegistrationNumber" HeaderText="เลขทะเบียน" />
                                            <asp:BoundField DataField="MotorApplicationStatusDetail" HeaderText="สถานะกรมธรรม์" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="8">
                                    <uc1:ucAppDetailApplication ID="ucAppDetailApplication1" runat="server" />
                                </td>
                            </tr>

                            <tr>
                                <td colspan="8">
                                    <uc1:ucDocument runat="server" ID="ucDocument" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
