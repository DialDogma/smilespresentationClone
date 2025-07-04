<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmShowDetailByEmpID.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmShowDetailByEmpID" EnableEventValidation="false" Theme="default" %>

<%@ Register src="UserControls/DropdownUserControls/ddlMotorApplicationStatus.ascx" tagname="ddlMotorApplicationStatus" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="panel-group">
            <div class="panel panel-default">
                <div class="panel-heading input-md">
                    <h4>ผู้ส่ง :
                        <asp:Label ID="lblName" runat="server" Text="EmpName"></asp:Label></h4>                    
                </div>
                <div class="panel-body">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 12.5%; text-align:right">สถานะ : 
                                 
                            </td>
                            <td style="width: 12.5%"><uc1:ddlMotorApplicationStatus ID="ddlMotorStatus1" runat="server" /></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <asp:GridView ID="dgvMain" runat="server" OnRowDataBound="OnRowDataBound" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="0">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>.
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="บริษัทประกัน" DataField="InsuranceCompanyDetail" />
                                        <asp:BoundField HeaderText="ApplicationCode" DataField="MotorApplication_Code" />
                                        <asp:BoundField HeaderText="รายละเอียดผลิตภัณฑ์" DataField="ProductDetail" />
                                        <asp:BoundField HeaderText="ชื่อ-สกุล ลูกค้า" DataField="FullName" />
                                        <asp:BoundField HeaderText="วันเริ่มคุ้มครอง" DataField="StartCover" DataFormatString="{0:dd/MM/yyy}"/>
                                        <asp:BoundField HeaderText="หมายเลขทะเบียนรถ" DataField="VehicleRegistrationNumber" />
                                        <asp:BoundField HeaderText="สถานะ" DataField="MotorApplicationStatusDetail" />
                                        <asp:BoundField HeaderText="สร้างโดย" DataField="EmployeeCode" />
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
