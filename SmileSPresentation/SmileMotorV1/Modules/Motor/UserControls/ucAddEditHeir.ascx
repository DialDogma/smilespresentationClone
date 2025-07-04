<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAddEditHeir.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucAddEditHeir" %>
<%@ Register Src="~/CommonUserControls/ucDatepicker.ascx" TagPrefix="uc1" TagName="ucDatepicker" %>
<%@ Register Src="DropdownUserControls/ddlSex.ascx" TagName="ddlSex" TagPrefix="uc2" %>
<%@ Register Src="DropdownUserControls/ddlCustomerTitle.ascx" TagName="ddlCustomerTitle" TagPrefix="uc5" %>
<%@ Register Src="~/Modules/Motor/UserControls/DropdownUserControls/ddlMaritalStatus.ascx" TagPrefix="uc6" TagName="ddlMaritalStatus" %>

<div class="panel panel-default">
    <div class="panel-heading">
        <h4 class="panel-title">ผู้รับผลประโยชน์</h4>
    </div>
    <div class="panel-body">
        <div class="table-responsive">
            <table style="width: 100%;">
                <tr>
                    <td style="width: 8%; text-align: right;">ชื่อ :</td>
                    <td style="width: 8%; text-align: right;">
                        <uc5:ddlCustomerTitle ID="ddlHeirTitle" runat="server" />
                    </td>
                    <td style="width: 17%;">
                        <asp:TextBox ID="txtHeirName" runat="server" CssClass="form-control" placeholder="กรอกชื่อ" Width="100%"></asp:TextBox>
                    </td>
                    <td style="width: 12.5%; text-align: right;">นามสกุล :</td>
                    <td style="width: 12.5%; text-align: right;">
                        <asp:TextBox ID="txtHeirSurename" runat="server" CssClass="form-control" Width="100%" placeholder="กรอกนามสกุล"></asp:TextBox>
                    </td>
                    <td style="width: 12.5%; text-align: right;">เพศ :</td>
                    <td style="width: 12.5%; text-align: right;">

                        <uc2:ddlSex ID="ddlHeirSex" runat="server" />

                    </td>
                </tr>
                <tr>
                    <td style="width: 12.5%; text-align: right;" colspan="2">เลขบัตรประชาชน :</td>
                    <td style="width: 12.5%; text-align: right;">
                        <asp:TextBox ID="txtHeirIDCardNumber" runat="server" CssClass="form-control" Width="100%" placeholder="เลขบัตรประชาชน"></asp:TextBox>
                    </td>
                    <td style="width: 12.5%; text-align: right;">Passport :</td>
                    <td style="width: 12.5%; text-align: right;">
                        <asp:TextBox ID="txtHeirPassportNumber" runat="server" CssClass="form-control" Width="100%" placeholder="เลขบัตรPassport"></asp:TextBox></td>
                    <td style="width: 12.5%; text-align: right;">วันเกิด :</td>
                    <td style="width: 12.5%; text-align: left;">
                        <uc1:ucDatepicker runat="server" ID="dtHeirDateOfBirth" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 12.5%; text-align: right;" colspan="2">สถานภาพ :</td>
                    <td style="width: 12.5%; text-align: right;">
                        <uc6:ddlMaritalStatus ID="ddlMaritalStatus" runat="server" />
                    </td>
                    <td style="width: 12.5%; text-align: right;">เบอร์โทรศัพท์ :</td>
                    <td style="width: 12.5%; text-align: right;">
                        <asp:TextBox ID="txtHeirPhoneNumber" runat="server" CssClass="form-control" Width="100%" placeholder="เบอร์โทรศัพท์"></asp:TextBox>
                    </td>
                    <td style="width: 12.5%; text-align: right;">เบอร์โทรศัพท์มือถือ :</td>
                    <td style="width: 12.5%; text-align: right;">
                        <asp:TextBox ID="txtHeirPhoneNumber2" runat="server" CssClass="form-control" Width="100%" placeholder="เบอร์โทรศัพท์มือถือ"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
