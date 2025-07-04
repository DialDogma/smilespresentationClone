<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAddEditCustomer.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucAddEditCustomer" %>
<%@ Register Src="~/CommonUserControls/ucDatepicker.ascx" TagPrefix="uc1" TagName="ucDatepicker" %>
<%@ Register Src="DropdownUserControls/ddlSex.ascx" TagName="ddlSex" TagPrefix="uc2" %>
<%@ Register Src="DropdownUserControls/ddlCustomerTitle.ascx" TagName="ddlCustomerTitle" TagPrefix="uc5" %>
<%@ Register Src="DropdownUserControls/ddlMaritalStatus.ascx" TagName="ddlMaritalStatus" TagPrefix="uc6" %>
<%@ Register Src="DropdownUserControls/ddlJobType.ascx" TagName="ddlJobType" TagPrefix="uc7" %>
<%@ Register Src="DropdownUserControls/ddlJobTypeLevel.ascx" TagName="ddlJobTypeLevel" TagPrefix="uc3" %>

<div class="panel panel-default">
    <div class="panel-heading input-md">
        <h4>
            <asp:Label ID="lblTextHeader" runat="server" Text="ข้อมูลผู้เอาประกัน"></asp:Label>
        </h4>
    </div>
    <div class="panel-body">
        <asp:UpdatePanel ID="up1" runat="server">
            <ContentTemplate>
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 8%; text-align: right;">ชื่อ :</td>
                        <td style="width: 8%;">
                            <uc5:ddlCustomerTitle ID="ddlCustomerTitle" runat="server" />
                        </td>
                        <td style="width: 17%;">
                            <asp:TextBox ID="txtCustomerName" runat="server" placeholder="กรอกชื่อ" ValidateRequestMode="Enabled"></asp:TextBox>
                        </td>
                        <td style="width: 12.5%; text-align: right;">นามสกุล :</td>
                        <td style="width: 12.5%;">
                            <asp:TextBox ID="txtCustomerSurename" runat="server" placeholder="กรอกนามสกุล"></asp:TextBox>
                        </td>
                        <td style="width: 12.5%; text-align: right;">เพศ :</td>
                        <td style="width: 12.5%;">
                            <uc2:ddlSex ID="ddlCustomerSex" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 12.5%; text-align: right;" colspan="2">เลขบัตรประชาชน :</td>
                        <td style="width: 12.5%;">
                            <asp:TextBox ID="txtCustomerIDCardNumber" runat="server" placeholder="เลขบัตรประชาชน" MaxLength="13"></asp:TextBox>
                            <ajaxToolkit:FilteredTextBoxExtender ID="txtCustomerIDCardNumber_FilteredTextBoxExtender" runat="server" BehaviorID="txtCustomerIDCardNumber_FilteredTextBoxExtender" TargetControlID="txtCustomerIDCardNumber" FilterType="Numbers" />
                        </td>
                        <td style="width: 12.5%; text-align: right;">Passport :</td>
                        <td style="width: 12.5%;">
                            <asp:TextBox ID="txtCustomerPassportNumber" runat="server" placeholder="เลขบัตร Passport" MaxLength="20"></asp:TextBox>
                            <ajaxToolkit:FilteredTextBoxExtender ID="txtCustomerPassportNumber_FilteredTextBoxExtender" runat="server" BehaviorID="txtCustomerPassportNumber_FilteredTextBoxExtender" TargetControlID="txtCustomerPassportNumber" FilterType="Numbers, UppercaseLetters, LowercaseLetters" />
                        </td>
                        <td style="width: 12.5%; text-align: right;">วันเกิด :</td>
                        <td style="width: 12.5%;">
                            <uc1:ucDatepicker runat="server" ID="dtCustomerDateOfBirth" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 12.5%; text-align: right;" colspan="2">สถานภาพ :</td>
                        <td style="width: 12.5%;">
                            <uc6:ddlMaritalStatus ID="ddlMaritalStatus" runat="server" />
                        </td>
                        <td style="width: 12.5%; text-align: right;">เบอร์โทรศัพท์ :</td>
                        <td style="width: 12.5%;">
                            <asp:TextBox ID="txtCustomerPhoneNumber" runat="server" placeholder="เบอร์โทรศัพท์" MaxLength="10"></asp:TextBox>
                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredCustomerPhoneNumber" runat="server" FilterType="Numbers" TargetControlID="txtCustomerPhoneNumber" />
                        </td>
                        <td style="width: 12.5%; text-align: right;">เบอร์โทรศัพท์มือถือ :</td>
                        <td style="width: 12.5%;">
                            <asp:TextBox ID="txtCustomerMobileNumber" runat="server" placeholder="เบอร์โทรศัพท์มือถือ" MaxLength="10"></asp:TextBox>
                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredMobileNumber" runat="server" FilterType="Numbers" TargetControlID="txtCustomerMobileNumber" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 12.5%; text-align: right;" colspan="2">ประเภทอาชีพ :</td>
                        <td style="width: 12.5%;">
                            <uc7:ddlJobType ID="ddlJobType" runat="server" OnSelectChanged="ddlJobType_SelectChanged" />
                        </td>
                        <td style="width: 12.5%; text-align: right;">ระดับอาชีพ :</td>
                        <td style="width: 12.5%;">
                            <uc3:ddlJobTypeLevel ID="ddlJobTypeLevel" runat="server" />
                        </td>
                        <td style="width: 12.5%; text-align: right;">&nbsp;</td>
                        <td style="width: 12.5%; text-align: right;">
                            <asp:Button ID="btnEditPerson" runat="server" Text="แก้ไขข้อมูลลูกค้า" Enabled="false"/>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

</div>
<asp:HiddenField ID="hdfNickname" runat="server" />

