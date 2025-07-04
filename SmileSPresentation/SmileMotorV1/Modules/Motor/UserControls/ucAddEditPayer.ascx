<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAddEditPayer.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucAddEditPayer" %>
<%@ Register Src="~/CommonUserControls/ucDatepicker.ascx" TagPrefix="uc1" TagName="ucDatepicker" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAddEditAddress.ascx" TagPrefix="uc1" TagName="ucAddEditAddress" %>
<%@ Register Src="DropdownUserControls/ddlCustomerTitle.ascx" TagName="ddlCustomerTitle" TagPrefix="uc2" %>
<%@ Register Src="DropdownUserControls/ddlJobType.ascx" TagName="ddlJobType" TagPrefix="uc5" %>
<%@ Register Src="DropdownUserControls/ddlSex.ascx" TagName="ddlSex" TagPrefix="uc3" %>
<%@ Register Src="DropdownUserControls/ddlMaritalStatus.ascx" TagName="ddlMaritalStatus" TagPrefix="uc4" %>

<%@ Register Src="DropdownUserControls/ddlJobTypeLevel.ascx" TagName="ddlJobTypeLevel" TagPrefix="uc3" %>

<div class="panel panel-default">
    <div class="panel-heading input-md">
        <h4 class="panel-title">ข้อมูลผู้ชำระเบี้ย</h4>
    </div>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div class="panel-body" id="bodyDetail">
                <div class="table-responsive">
                    <table style="width: 100%;">

                        <tr>
                            <td style="width: 8%; text-align: right;">ชื่อ :</td>
                            <td style="width: 8%; text-align: right;">
                                <uc2:ddlCustomerTitle ID="ddlPayerTitle" runat="server" />
                            </td>
                            <td style="width: 17%;">
                                <asp:TextBox ID="txtPayerName" runat="server" CssClass="form-control" placeholder="กรอกชื่อ" Width="100%"></asp:TextBox>
                            </td>
                            <td style="width: 12.5%; text-align: right;">นามสกุล :</td>
                            <td style="width: 12.5%; text-align: right;">
                                <asp:TextBox ID="txtPayerSurename" runat="server" CssClass="form-control" Width="100%" placeholder="กรอกนามสกุล"></asp:TextBox>
                            </td>
                            <td style="width: 12.5%; text-align: right;">เพศ :</td>
                            <td style="width: 12.5%; text-align: right;">
                                <uc3:ddlSex ID="ddlPayerSex" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 12.5%; text-align: right;" colspan="2">เลขบัตรประชาชน :</td>
                            <td style="width: 12.5%; text-align: right;">
                                <asp:TextBox ID="txtPayerIDCardNumber" runat="server" CssClass="form-control" Width="100%" placeholder="เลขบัตรประชาชน" MaxLength="13"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="txtPayerIDCardNumber_FilteredTextBoxExtender" runat="server" BehaviorID="txtPayerIDCardNumber_FilteredTextBoxExtender" TargetControlID="txtPayerIDCardNumber" FilterType="Numbers" />
                            </td>
                            <td style="width: 12.5%; text-align: right;">Passport :</td>
                            <td style="width: 12.5%; text-align: right;">
                                <asp:TextBox ID="txtPayerPassportNumber" runat="server" CssClass="form-control" Width="100%" placeholder="เลขบัตรPassport"></asp:TextBox></td>
                            <td style="width: 12.5%; text-align: right;">วันเกิด :</td>
                            <td style="width: 12.5%; text-align: left;">
                                <uc1:ucDatepicker runat="server" ID="dtPayerDateOfBirth" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 12.5%; text-align: right;" colspan="2">สถานภาพ :</td>
                            <td style="width: 12.5%; text-align: right;">
                                <uc4:ddlMaritalStatus ID="ddlMaritalStatus" runat="server" />
                            </td>
                            <td style="width: 12.5%; text-align: right;">เบอร์โทรศัพท์ :</td>
                            <td style="width: 12.5%; text-align: right;">
                                <asp:TextBox ID="txtPayerPhoneNumber" MaxLength="10" runat="server" CssClass="form-control" Width="100%" placeholder="เบอร์โทรศัพท์"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredCustomerPhoneNumber" runat="server" FilterType="Numbers" TargetControlID="txtPayerPhoneNumber" />
                            </td>
                            <td style="width: 12.5%; text-align: right;">เบอร์โทรศัพท์มือถือ :</td>
                            <td style="width: 12.5%; text-align: right;">
                                <asp:TextBox ID="txtPayerMobileNumber" MaxLength="10" runat="server" CssClass="form-control" Width="100%" placeholder="เบอร์โทรศัพท์มือถือ"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredMobileNumber" runat="server" FilterType="Numbers" TargetControlID="txtPayerMobileNumber" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 12.5%; text-align: right;" colspan="2">ประเภทอาชีพ :</td>
                            <td style="width: 12.5%; text-align: right;">
                                <uc5:ddlJobType ID="ddlPayerJobType" runat="server" OnSelectChanged="ddlPayerJobType_SelectChanged" />
                            </td>
                            <td style="width: 12.5%; text-align: right;">ระดับอาชีพ :</td>
                            <td style="width: 12.5%; text-align: right;">
                                <uc3:ddlJobTypeLevel ID="ddlJobTypeLevel" runat="server" />
                            </td>
                            <td style="width: 12.5%; text-align: right;">&nbsp;</td>
                            <td style="width: 12.5%; text-align: right;">
                                <asp:Button ID="btnEditPerson" runat="server" Text="แก้ไขข้อมูลผู้ชำระเบี้ย" Enabled="false" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>


        </ContentTemplate>
    </asp:UpdatePanel>




</div>


<script>
    $('#chkHide').change(function () {
        if (this.checked) {
            $('#bodyDetail').fadeOut('slow');
        }
        else {

            $('#bodyDetail').fadeIn('slow');
        }
    });
</script>
